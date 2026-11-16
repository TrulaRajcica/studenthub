using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Data;
using StudentHub.Web.Models;
using StudentHub.Web.Models.ViewModels;

namespace StudentHub.Web.Services;

/// <summary>
/// Poslovna pravila za feedback na projektima.
/// Pokriva TIM4-100 (jedan feedback po korisniku), TIM4-101 i TIM4-102
/// (izracun i azuriranje prosjeka) te citanje za TIM4-187.
/// </summary>
public class FeedbackServis
{
    private readonly StudentHubDbContext _db;

    public FeedbackServis(StudentHubDbContext db) => _db = db;

    /// <summary>Rezultat pokusaja objave feedbacka.</summary>
    public record Rezultat(bool Uspjeh, string? Greska)
    {
        public static Rezultat Ok() => new(true, null);
        public static Rezultat Problem(string poruka) => new(false, poruka);
    }

    /// <summary>
    /// Objavljuje komentar s ocjenom. Vraca gresku umjesto iznimke kad pravilo nije ispunjeno,
    /// da kontroler moze poruku prikazati uz obrazac.
    /// </summary>
    public async Task<Rezultat> ObjaviAsync(int projektId, int autorId, string komentar, int ocjena)
    {
        if (ocjena is < 1 or > 5)
            return Rezultat.Problem("Ocjena mora biti cijeli broj od 1 do 5.");

        komentar = (komentar ?? "").Trim();
        if (komentar.Length == 0)
            return Rezultat.Problem("Komentar ne smije biti prazan.");
        if (komentar.Length > 2000)
            return Rezultat.Problem("Komentar smije imati najvise 2000 znakova.");

        var projekt = await _db.Projekti.FirstOrDefaultAsync(p => p.Id == projektId);
        if (projekt is null)
            return Rezultat.Problem("Projekt ne postoji.");
        if (!projekt.JeObjavljen)
            return Rezultat.Problem("Projekt nije objavljen pa se na njega ne moze ostaviti feedback.");
        if (projekt.VlasnikId == autorId)
            return Rezultat.Problem("Na vlastiti projekt ne mozes ostaviti ocjenu.");

        // TIM4-100: jedan feedback po korisniku po projektu.
        // Baza ima i jedinstveni indeks (ProjektId, AutorId) - ovo je provjera
        // koja korisniku daje razumljivu poruku umjesto greske baze.
        if (await VecOcijenioAsync(projektId, autorId))
            return Rezultat.Problem("Ovaj projekt si vec ocijenio - jedan feedback po korisniku.");

        _db.Feedbackovi.Add(new Feedback
        {
            ProjektId = projektId,
            AutorId = autorId,
            Komentar = komentar,
            Ocjena = ocjena,
            DatumKreiranja = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();

        // TIM4-102: prosjek se osvjezava odmah nakon svakog novog feedbacka,
        // da ga stranica projekta procita bez dodatnog racunanja (TIM4-182).
        await OsvjeziProsjekAsync(projektId);

        return Rezultat.Ok();
    }

    public Task<bool> VecOcijenioAsync(int projektId, int autorId) =>
        _db.Feedbackovi.AnyAsync(f => f.ProjektId == projektId && f.AutorId == autorId);

    /// <summary>
    /// TIM4-101: ponovno izracunava prosjecnu ocjenu i broj ocjena i sprema ih na projekt.
    /// Racuna se u bazi (jedna GROUP BY naredba), ne ucitavanjem svih feedbackova.
    /// </summary>
    public async Task OsvjeziProsjekAsync(int projektId)
    {
        var projekt = await _db.Projekti.FirstOrDefaultAsync(p => p.Id == projektId);
        if (projekt is null) return;

        var statistika = await _db.Feedbackovi
            .Where(f => f.ProjektId == projektId)
            .GroupBy(f => f.ProjektId)
            .Select(g => new { Broj = g.Count(), Prosjek = g.Average(f => (double)f.Ocjena) })
            .FirstOrDefaultAsync();

        if (statistika is null)
        {
            projekt.BrojOcjena = 0;
            projekt.ProsjecnaOcjena = null;
        }
        else
        {
            projekt.BrojOcjena = statistika.Broj;
            projekt.ProsjecnaOcjena = Math.Round((decimal)statistika.Prosjek, 2);
        }

        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Sve za prikaz feedbacka na stranici projekta (TIM4-103, TIM4-104, TIM4-168).
    /// </summary>
    public async Task<FeedbackProjektaVm?> DohvatiZaProjektAsync(
        int projektId, int? trenutniKorisnikId, int? filtarOcjena)
    {
        var projekt = await _db.Projekti
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projektId);

        if (projekt is null) return null;

        var upit = _db.Feedbackovi.AsNoTracking().Where(f => f.ProjektId == projektId);

        var ukupno = await upit.CountAsync();

        // TIM4-104: filtar po visini ocjene
        if (filtarOcjena is >= 1 and <= 5)
            upit = upit.Where(f => f.Ocjena == filtarOcjena);

        // TIM4-103: komentari s autorom i odgovorima, najnoviji prvi
        var komentari = await upit
            .Include(f => f.Autor)
            .Include(f => f.Odgovori).ThenInclude(o => o.Autor)
            .OrderByDescending(f => f.DatumKreiranja)
            .ToListAsync();

        var vecOcijenio = trenutniKorisnikId is int kid
                          && await VecOcijenioAsync(projektId, kid);

        var vm = new FeedbackProjektaVm
        {
            ProjektId = projekt.Id,
            ProjektNaziv = projekt.Naziv,
            ProsjecnaOcjena = projekt.ProsjecnaOcjena,
            BrojOcjena = projekt.BrojOcjena,
            Komentari = komentari,
            UkupnoKomentara = ukupno,
            FiltarOcjena = filtarOcjena is >= 1 and <= 5 ? filtarOcjena : null,
            KorisnikJePrijavljen = trenutniKorisnikId.HasValue,
            VecOcijenio = vecOcijenio,
            JeVlasnikProjekta = trenutniKorisnikId == projekt.VlasnikId
        };

        if (vm.SmijeOstaviti)
            vm.Obrazac = new UnosFeedbackaVm { ProjektId = projektId };

        return vm;
    }
}
