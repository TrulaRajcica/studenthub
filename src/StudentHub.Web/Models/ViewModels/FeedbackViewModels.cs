using System.ComponentModel.DataAnnotations;
using StudentHub.Web.Models;

namespace StudentHub.Web.Models.ViewModels;

/// <summary>Podaci obrasca za unos komentara i ocjene (TIM4-99).</summary>
public class UnosFeedbackaVm
{
    public int ProjektId { get; set; }

    [Required(ErrorMessage = "Komentar ne smije biti prazan.")]
    [MaxLength(2000, ErrorMessage = "Komentar smije imati najvise 2000 znakova.")]
    [Display(Name = "Komentar")]
    public string Komentar { get; set; } = "";

    [Range(1, 5, ErrorMessage = "Ocjena mora biti cijeli broj od 1 do 5.")]
    [Display(Name = "Ocjena")]
    public int Ocjena { get; set; }
}

/// <summary>Sve sto stranica projekta treba za prikaz feedbacka (TIM4-187).</summary>
public class FeedbackProjektaVm
{
    public int ProjektId { get; set; }
    public string ProjektNaziv { get; set; } = "";

    /// <summary>Prosjecna ocjena projekta, null ako jos nema nijedne ocjene.</summary>
    public decimal? ProsjecnaOcjena { get; set; }

    public int BrojOcjena { get; set; }

    /// <summary>Komentari nakon primjene filtra, najnoviji prvi.</summary>
    public IReadOnlyList<Feedback> Komentari { get; set; } = [];

    /// <summary>Ukupan broj komentara prije filtriranja.</summary>
    public int UkupnoKomentara { get; set; }

    /// <summary>Odabrana ocjena u filtru (TIM4-104), null = prikazi sve.</summary>
    public int? FiltarOcjena { get; set; }

    public bool KorisnikJePrijavljen { get; set; }
    public bool VecOcijenio { get; set; }
    public bool JeVlasnikProjekta { get; set; }

    /// <summary>Prazan ili ponovno popunjen obrazac, null kad se obrazac ne smije prikazati.</summary>
    public UnosFeedbackaVm? Obrazac { get; set; }

    /// <summary>Smije li trenutni korisnik ostaviti feedback na ovaj projekt.</summary>
    public bool SmijeOstaviti => KorisnikJePrijavljen && !VecOcijenio && !JeVlasnikProjekta;

    /// <summary>Razlog zasto obrazac nije prikazan, za poruku korisniku.</summary>
    public string? RazlogBezObrasca =>
        !KorisnikJePrijavljen ? "Za ostavljanje komentara potrebno je prijaviti se."
        : JeVlasnikProjekta   ? "Na vlastiti projekt ne mozes ostaviti ocjenu."
        : VecOcijenio         ? "Ovaj projekt si vec ocijenio - jedan feedback po korisniku."
        : null;
}
