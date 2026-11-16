using System.ComponentModel.DataAnnotations;

namespace StudentHub.Web.Models;

public class Feedback
{
    public int Id { get; set; }

    public int ProjektId { get; set; }
    public Projekt? Projekt { get; set; }

    public int AutorId { get; set; }
    public Korisnik? Autor { get; set; }

    [Required, MaxLength(2000)]
    public string Komentar { get; set; } = "";

    [Range(1, 5)]
    public int Ocjena { get; set; }

    public DateTime DatumKreiranja { get; set; } = DateTime.UtcNow;
    public DateTime? DatumAzuriranja { get; set; }

    public ICollection<OdgovorNaFeedback> Odgovori { get; set; } = new List<OdgovorNaFeedback>();
}

public class OdgovorNaFeedback
{
    public int Id { get; set; }

    public int FeedbackId { get; set; }
    public Feedback? Feedback { get; set; }

    public int AutorId { get; set; }
    public Korisnik? Autor { get; set; }

    [Required, MaxLength(2000)]
    public string Tekst { get; set; } = "";

    public DateTime DatumKreiranja { get; set; } = DateTime.UtcNow;
}
