using System.ComponentModel.DataAnnotations;

namespace StudentHub.Web.Models;

public class Projekt
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Naziv { get; set; } = "";

    [Required]
    public string Opis { get; set; } = "";

    public int VlasnikId { get; set; }
    public Korisnik? Vlasnik { get; set; }

    public int KategorijaId { get; set; }
    public Kategorija? Kategorija { get; set; }

    public StatusProjekta Status { get; set; } = StatusProjekta.UIzradi;
    public bool JeObjavljen { get; set; }

    public DateTime DatumKreiranja { get; set; } = DateTime.UtcNow;
    public DateTime? DatumObjave { get; set; }

    public int BrojPregleda { get; set; }
    public decimal? ProsjecnaOcjena { get; set; }
    public int BrojOcjena { get; set; }

    public ICollection<ProjektTag> Tagovi { get; set; } = new List<ProjektTag>();
    public ICollection<Feedback> Feedbackovi { get; set; } = new List<Feedback>();
    public ICollection<Oglas> Oglasi { get; set; } = new List<Oglas>();
    public ICollection<ClanTima> Tim { get; set; } = new List<ClanTima>();
}
