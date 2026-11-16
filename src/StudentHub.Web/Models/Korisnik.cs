using System.ComponentModel.DataAnnotations;

namespace StudentHub.Web.Models;

public class Korisnik
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Ime { get; set; } = "";

    [Required, MaxLength(50)]
    public string Prezime { get; set; } = "";

    [Required, MaxLength(255), EmailAddress]
    public string Email { get; set; } = "";

    [Required, MaxLength(255)]
    public string LozinkaHash { get; set; } = "";

    public UlogaKorisnika Uloga { get; set; } = UlogaKorisnika.Student;

    [MaxLength(500)]
    public string? Biografija { get; set; }

    public DateTime DatumRegistracije { get; set; } = DateTime.UtcNow;

    public string PunoIme => $"{Ime} {Prezime}";

    public ICollection<Projekt> Projekti { get; set; } = new List<Projekt>();
    public ICollection<Feedback> Feedbackovi { get; set; } = new List<Feedback>();
    public ICollection<KorisnikVjestina> Vjestine { get; set; } = new List<KorisnikVjestina>();
    public ICollection<Ponuda> Ponude { get; set; } = new List<Ponuda>();
    public ICollection<ClanTima> Clanstva { get; set; } = new List<ClanTima>();
}
