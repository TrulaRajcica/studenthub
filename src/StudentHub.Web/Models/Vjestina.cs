using System.ComponentModel.DataAnnotations;

namespace StudentHub.Web.Models;

public class Vjestina
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Naziv { get; set; } = "";

    public ICollection<KorisnikVjestina> Korisnici { get; set; } = new List<KorisnikVjestina>();
    public ICollection<OglasVjestina> Oglasi { get; set; } = new List<OglasVjestina>();
}

public class KorisnikVjestina
{
    public int KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }

    public int VjestinaId { get; set; }
    public Vjestina? Vjestina { get; set; }
}
