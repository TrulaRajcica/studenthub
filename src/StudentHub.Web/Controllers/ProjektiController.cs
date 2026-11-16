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
}
