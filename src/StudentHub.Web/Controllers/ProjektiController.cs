using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Data;
using StudentHub.Web.Models;

namespace StudentHub.Web.Controllers;

public class ProjektiController : Controller
{
    private readonly StudentHubDbContext _db;

    public ProjektiController(StudentHubDbContext db) => _db = db;

    // GET /Projekti
    public async Task<IActionResult> Index()
    {
        var projekti = await _db.Projekti
            .AsNoTracking()
            .Where(p => p.JeObjavljen)              // skice se ne prikazuju javno
            .Include(p => p.Vlasnik)
            .Include(p => p.Kategorija)
            .Include(p => p.Tagovi).ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.DatumObjave)
            .ThenByDescending(p => p.Id)   // DatumObjave je nullable, Id jamci stabilan redoslijed
            .ToListAsync();

        return View(projekti);
    }

    // GET /Projekti/Detalji/5
    public async Task<IActionResult> Detalji(int id)
    {
        var projekt = await _db.Projekti
            .Include(p => p.Vlasnik)
            .Include(p => p.Kategorija)
            .Include(p => p.Tagovi).ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (projekt is null)
            return NotFound();

        // Skice nisu javne. Dok prijava (TIM4-137) nije gotova, nitko ne moze
        // dokazati da je vlasnik, pa se neobjavljeni projekt ne prikazuje nikome.
        if (!projekt.JeObjavljen)
            return NotFound();

        // TIM4-167: brojac pregleda. Jedna ciljana naredba umjesto ucitavanja
        // i spremanja cijelog entiteta.
        await _db.Projekti
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.BrojPregleda, p => p.BrojPregleda + 1));

        projekt.BrojPregleda += 1;   // da se povecanje odmah vidi na stranici

        return View(projekt);
    }
}
