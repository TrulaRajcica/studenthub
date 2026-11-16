using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Data;
using StudentHub.Web.Models;

namespace StudentHub.Web.Services;

/// <summary>
/// PRIVREMENA implementacija ITrenutniKorisnik dok TIM4-137 (prijava) nije gotov.
/// Korisnika cuva u sessionu pod kljucem "KorisnikId".
/// Kad prava prijava bude spremna, ovaj razred se brise i u Program.cs se
/// registrira implementacija koja cita User.Claims.
/// </summary>
public class SessionTrenutniKorisnik : ITrenutniKorisnik, IDemoPrijava
{
    private const string Kljuc = "KorisnikId";

    private readonly IHttpContextAccessor _http;
    private readonly StudentHubDbContext _db;

    public SessionTrenutniKorisnik(IHttpContextAccessor http, StudentHubDbContext db)
    {
        _http = http;
        _db = db;
    }

    public int? Id => _http.HttpContext?.Session.GetInt32(Kljuc);

    public async Task<Korisnik?> DohvatiAsync()
    {
        var id = Id;
        if (id is null) return null;

        return await _db.Korisnici.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id);
    }

    public void Prijavi(int korisnikId) => _http.HttpContext?.Session.SetInt32(Kljuc, korisnikId);

    public void Odjavi() => _http.HttpContext?.Session.Remove(Kljuc);
}
