using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Data;
using StudentHub.Web.Models.ViewModels;
using StudentHub.Web.Services;

namespace StudentHub.Web.Controllers;

/// <summary>
/// Feedback na projektima - TIM4-41 (unos) i TIM4-187 (prikaz).
///
/// Stranica /Feedback/Projekt/5 postoji da se tok moze pokazati dok stranica
/// detalja projekta (TIM4-166) nije gotova. Kad bude, u nju se ubacuju partiali
/// _OcjenaProjekta i _FeedbackSekcija, a ova stranica se moze maknuti.
/// </summary>
public class FeedbackController : Controller
{
    private readonly StudentHubDbContext _db;
    private readonly FeedbackServis _servis;
    private readonly ITrenutniKorisnik _korisnik;
    private readonly IDemoPrijava _demoPrijava;

    public FeedbackController(
        StudentHubDbContext db,
        FeedbackServis servis,
        ITrenutniKorisnik korisnik,
        IDemoPrijava demoPrijava)
    {
        _db = db;
        _servis = servis;
        _korisnik = korisnik;
        _demoPrijava = demoPrijava;
    }

    // GET /Feedback/Projekt/5?ocjena=4
    public async Task<IActionResult> Projekt(int id, int? ocjena)
    {
        var vm = await _servis.DohvatiZaProjektAsync(id, _korisnik.Id, ocjena);
        if (vm is null) return NotFound();

        await PripremiDemoPrijavuAsync();
        return View(vm);
    }

    // POST /Feedback/Objavi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Objavi(UnosFeedbackaVm unos)
    {
        if (_korisnik.Id is not int autorId)
            return RedirectToAction(nameof(Projekt), new { id = unos.ProjektId });

        if (ModelState.IsValid)
        {
            var rezultat = await _servis.ObjaviAsync(unos.ProjektId, autorId, unos.Komentar, unos.Ocjena);

            if (rezultat.Uspjeh)
            {
                TempData["Poruka"] = "Komentar je objavljen.";
                return RedirectToAction(nameof(Projekt), new { id = unos.ProjektId });
            }

            ModelState.AddModelError(string.Empty, rezultat.Greska!);
        }

        // Obrazac se vraca s unesenim tekstom da korisnik ne pise ponovno.
        var vm = await _servis.DohvatiZaProjektAsync(unos.ProjektId, autorId, null);
        if (vm is null) return NotFound();

        vm.Obrazac = unos;
        await PripremiDemoPrijavuAsync();
        return View(nameof(Projekt), vm);
    }

    // --- PRIVREMENO: rucni odabir korisnika dok prijava (TIM4-137) nije gotova ---

    // POST /Feedback/DemoPrijava
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DemoPrijava(int korisnikId, int projektId)
    {
        if (korisnikId > 0) _demoPrijava.Prijavi(korisnikId);
        else _demoPrijava.Odjavi();

        return RedirectToAction(nameof(Projekt), new { id = projektId });
    }

    private async Task PripremiDemoPrijavuAsync()
    {
        ViewBag.Korisnici = await _db.Korisnici
            .AsNoTracking()
            .OrderBy(k => k.Ime)
            .Select(k => new { k.Id, Ime = k.Ime + " " + k.Prezime })
            .ToListAsync();

        ViewBag.TrenutniKorisnikId = _korisnik.Id ?? 0;
    }
}
