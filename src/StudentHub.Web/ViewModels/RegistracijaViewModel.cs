using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentHub.Web.ViewModels;

/// <summary>
/// Podaci s obrasca za registraciju. Namjerno nije entitet Korisnik: obrazac smije
/// postaviti samo ova polja, pa korisnik ne moze u zahtjev ubaciti npr. Ulogu = Administrator.
/// </summary>
public class RegistracijaViewModel : IValidatableObject
{
    // bcrypt cita samo prvih 72 BAJTA lozinke, a znakovi poput č ili š u UTF-8 zauzimaju 2 bajta.
    private const int MaxBajtovaLozinke = 72;

    [Display(Name = "Ime")]
    [Required(ErrorMessage = "Unesite ime.")]
    [StringLength(50, ErrorMessage = "Ime može imati najviše {1} znakova.")]
    public string Ime { get; set; } = "";

    [Display(Name = "Prezime")]
    [Required(ErrorMessage = "Unesite prezime.")]
    [StringLength(50, ErrorMessage = "Prezime može imati najviše {1} znakova.")]
    public string Prezime { get; set; } = "";

    [Display(Name = "E-mail adresa")]
    [Required(ErrorMessage = "Unesite e-mail adresu.")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu e-mail adresu.")]
    [StringLength(255, ErrorMessage = "E-mail adresa može imati najviše {1} znakova.")]
    public string Email { get; set; } = "";

    [Display(Name = "Lozinka")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Unesite lozinku.")]
    [StringLength(72, MinimumLength = 8, ErrorMessage = "Lozinka mora imati od {2} do {1} znakova.")]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-ZČĆŽŠĐ]).*$",
        ErrorMessage = "Lozinka mora sadržavati barem jedan broj i jedno veliko slovo.")]
    public string Lozinka { get; set; } = "";

    // Izvrsava se na poslužitelju nakon provjere atributa. Klijentska provjera ne zna brojati bajtove.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Encoding.UTF8.GetByteCount(Lozinka) > MaxBajtovaLozinke)
            yield return new ValidationResult(
                "Lozinka je predugačka (znakovi poput č, š, ž zauzimaju više mjesta). Skratite je.",
                new[] { nameof(Lozinka) });
    }
}
