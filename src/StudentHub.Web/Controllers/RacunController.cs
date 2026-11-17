using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Data;
using StudentHub.Web.Models;
using StudentHub.Web.Services;
using StudentHub.Web.ViewModels;

namespace StudentHub.Web.Controllers;

/// <summary>Korisnicki racun: registracija (ovdje), a prijava i odjava dolaze u zasebnom PR-u.</summary>
public class RacunController : Controller
{
    private readonly StudentHubDbContext _db;

    public RacunController(StudentHubDbContext db) => _db = db;

    // GET /Racun/Registracija
    [HttpGet]
    public IActionResult Registracija() => View(new RegistracijaViewModel());

    // POST /Racun/Registracija
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registracija(RegistracijaViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);   // ime, prezime i e-mail ostaju u obrascu; lozinka se iz sigurnosnih razloga ne vraca

        // SQLite usporeduje tekst osjetljivo na velika/mala slova, pa e-mail uvijek spremamo malim slovima.
        // Prijava (sljedeci PR) mora raditi istu normalizaciju.
        var email = model.Email.Trim().ToLowerInvariant();

        if (await _db.Korisnici.AnyAsync(k => k.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "Račun s ovom e-mail adresom već postoji.");
            return View(model);
        }

        _db.Korisnici.Add(new Korisnik
        {
            Ime = model.Ime.Trim(),
            Prezime = model.Prezime.Trim(),
            Email = email,
            LozinkaHash = Lozinka.Hash(model.Lozinka),
            Uloga = UlogaKorisnika.Student   // uloga se nikad ne uzima iz obrasca
        });

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Dva istovremena zahtjeva s istim e-mailom: provjera iznad ih oba propusti,
            // a jedinstveni indeks u bazi odbije drugi.
            ModelState.AddModelError(nameof(model.Email), "Račun s ovom e-mail adresom već postoji.");
            return View(model);
        }

        // Post-Redirect-Get: osvjezavanje stranice ne salje obrazac ponovno
        return RedirectToAction(nameof(RegistracijaUspjesna));
    }

    // GET /Racun/RegistracijaUspjesna
    [HttpGet]
    public IActionResult RegistracijaUspjesna() => View();
}
