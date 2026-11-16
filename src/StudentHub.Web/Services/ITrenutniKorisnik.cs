using StudentHub.Web.Models;

namespace StudentHub.Web.Services;

/// <summary>
/// Tko je trenutno prijavljen korisnik.
///
/// PRIVREMENO: prijava (TIM4-137) jos nije gotova, pa implementacija cuva
/// odabranog korisnika u sessionu. Kad prijava bude spojena, zamjenjuje se
/// samo registracija u Program.cs - kod koji ovo sucelje koristi ostaje isti.
/// </summary>
public interface ITrenutniKorisnik
{
    /// <summary>Id prijavljenog korisnika, ili null ako nitko nije prijavljen.</summary>
    int? Id { get; }

    bool JePrijavljen => Id.HasValue;

    /// <summary>Cijeli zapis korisnika, ili null ako nitko nije prijavljen.</summary>
    Task<Korisnik?> DohvatiAsync();
}

/// <summary>Privremeni dodatak: rucni odabir korisnika dok prijava ne proradi.</summary>
public interface IDemoPrijava
{
    void Prijavi(int korisnikId);
    void Odjavi();
}
