using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Models;

namespace StudentHub.Web.Data;

public static class SeedData
{
    public static void Pokreni(StudentHubDbContext db)
    {
        if (db.Korisnici.Any()) return;   // seed ide samo u praznu bazu

        var ana    = new Korisnik { Ime = "Ana",  Prezime = "Marić",   Email = "ana@algebra.hr",   LozinkaHash = "demo" };
        var ivan   = new Korisnik { Ime = "Ivan", Prezime = "Novak",   Email = "ivan@algebra.hr",  LozinkaHash = "demo" };
        var admin  = new Korisnik { Ime = "Admin", Prezime = "Sustav", Email = "admin@algebra.hr", LozinkaHash = "demo",
                                    Uloga = UlogaKorisnika.Administrator };
        db.Korisnici.AddRange(ana, ivan, admin);

        var web  = new Kategorija { Naziv = "Web aplikacija" };
        var mob  = new Kategorija { Naziv = "Mobilna aplikacija" };
        var data = new Kategorija { Naziv = "Obrada podataka" };
        db.Kategorije.AddRange(web, mob, data);

        var tagovi = new[] { "Java", "C#", "React", "Python", "SQL", "Flutter" }
                        .Select(n => new Tag { Naziv = n }).ToArray();
        db.Tagovi.AddRange(tagovi);

        var vjestine = new[] { "Backend", "Frontend", "Baze podataka", "UI dizajn" }
                        .Select(n => new Vjestina { Naziv = n }).ToArray();
        db.Vjestine.AddRange(vjestine);
        db.SaveChanges();

        var projekti = new[]
        {
            new Projekt { Naziv = "Sustav za rezervaciju dvorana", Opis = "Web aplikacija za rezervaciju predavaonica na fakultetu.", Vlasnik = ana,  Kategorija = web,  JeObjavljen = true,  Status = StatusProjekta.Zavrsen, BrojPregleda = 1284, DatumObjave = DateTime.UtcNow.AddDays(-30) },
            new Projekt { Naziv = "Mobilna aplikacija za učenje", Opis = "Kartice za učenje stranih jezika s ponavljanjem u razmacima.", Vlasnik = ivan, Kategorija = mob,  JeObjavljen = true,  Status = StatusProjekta.UIzradi, BrojPregleda = 842,  DatumObjave = DateTime.UtcNow.AddDays(-21) },
            new Projekt { Naziv = "Analiza prometnih podataka",   Opis = "Obrada i vizualizacija podataka o gustoći prometa u gradu.",  Vlasnik = ana,  Kategorija = data, JeObjavljen = true,  Status = StatusProjekta.Zavrsen, BrojPregleda = 503,  DatumObjave = DateTime.UtcNow.AddDays(-14) },
            new Projekt { Naziv = "Generator rasporeda ispita",   Opis = "Algoritam koji raspoređuje ispite bez preklapanja.",          Vlasnik = ivan, Kategorija = web,  JeObjavljen = true,  Status = StatusProjekta.UIzradi, BrojPregleda = 219,  DatumObjave = DateTime.UtcNow.AddDays(-7) },
            new Projekt { Naziv = "Web shop za rukotvorine",      Opis = "Trgovina s košaricom i pregledom narudžbi.",                  Vlasnik = ana,  Kategorija = web,  JeObjavljen = true,  Status = StatusProjekta.Arhiviran, BrojPregleda = 97, DatumObjave = DateTime.UtcNow.AddDays(-60) },
            new Projekt { Naziv = "Prototip chat aplikacije",     Opis = "Skica aplikacije za razmjenu poruka u stvarnom vremenu.",     Vlasnik = ivan, Kategorija = mob,  JeObjavljen = false, Status = StatusProjekta.UIzradi },
        };
        db.Projekti.AddRange(projekti);
        db.SaveChanges();

        var komentari = new (int Projekt, Korisnik Autor, string Tekst, int Ocjena)[]
        {
            (0, ivan,  "Jako koristan projekt, sučelje je pregledno.", 5),
            (0, admin, "Dobro riješeno, nedostaje potvrda rezervacije mailom.", 4),
            (1, ana,   "Sviđa mi se ideja, ponavljanje u razmacima stvarno pomaže.", 5),
            (1, admin, "Aplikacija je spora pri učitavanju većih setova kartica.", 3),
            (2, ivan,  "Vizualizacije su jasne, podaci dobro odabrani.", 5),
            (2, admin, "Nedostaje opis izvora podataka.", 4),
            (3, ana,   "Algoritam radi, ali bi trebao podržati i dvorane.", 4),
            (3, admin, "Solidno za prvu verziju.", 3),
            (4, ivan,  "Košarica radi, dizajn bi mogao biti dotjeraniji.", 3),
            (4, ana,   "Dobra vježba, ali projekt je arhiviran prerano.", 4),
        };

        foreach (var k in komentari)
            db.Feedbackovi.Add(new Feedback
            {
                Projekt = projekti[k.Projekt], Autor = k.Autor,
                Komentar = k.Tekst, Ocjena = k.Ocjena,
                DatumKreiranja = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 20))
            });
        db.SaveChanges();

        // izracun prosjecne ocjene
        foreach (var p in db.Projekti.Include(x => x.Feedbackovi))
        {
            if (p.Feedbackovi.Count == 0) continue;
            p.BrojOcjena = p.Feedbackovi.Count;
            p.ProsjecnaOcjena = Math.Round((decimal)p.Feedbackovi.Average(f => f.Ocjena), 2);
        }
        db.SaveChanges();
    }
}
