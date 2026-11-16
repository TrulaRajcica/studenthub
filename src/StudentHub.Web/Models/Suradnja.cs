using System.ComponentModel.DataAnnotations;

namespace StudentHub.Web.Models;

public class Oglas
{
    public int Id { get; set; }

    public int ProjektId { get; set; }
    public Projekt? Projekt { get; set; }

    [Required, MaxLength(100)]
    public string NazivUloge { get; set; } = "";

    [Required]
    public string Opis { get; set; } = "";

    public int BrojMjesta { get; set; } = 1;
    public int BrojSlobodnihMjesta { get; set; } = 1;

    public StatusOglasa Status { get; set; } = StatusOglasa.Otvoren;
    public DateTime DatumObjave { get; set; } = DateTime.UtcNow;
    public DateTime? DatumZatvaranja { get; set; }

    public ICollection<OglasVjestina> TrazeneVjestine { get; set; } = new List<OglasVjestina>();
    public ICollection<Ponuda> Ponude { get; set; } = new List<Ponuda>();
}

public class OglasVjestina
{
    public int OglasId { get; set; }
    public Oglas? Oglas { get; set; }

    public int VjestinaId { get; set; }
    public Vjestina? Vjestina { get; set; }
}

public class Ponuda
{
    public int Id { get; set; }

    public int OglasId { get; set; }
    public Oglas? Oglas { get; set; }

    public int KandidatId { get; set; }
    public Korisnik? Kandidat { get; set; }

    [MaxLength(1000)]
    public string? Poruka { get; set; }

    public StatusPonude Status { get; set; } = StatusPonude.NaCekanju;
    public DateTime DatumSlanja { get; set; } = DateTime.UtcNow;
    public DateTime? DatumOdluke { get; set; }
}

public class ClanTima
{
    public int Id { get; set; }

    public int ProjektId { get; set; }
    public Projekt? Projekt { get; set; }

    public int KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }

    [MaxLength(100)]
    public string Uloga { get; set; } = "Član tima";

    public DateTime DatumPridruzivanja { get; set; } = DateTime.UtcNow;
}
