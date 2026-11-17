using BCryptNet = BCrypt.Net.BCrypt;

namespace StudentHub.Web.Services;

/// <summary>
/// Jedino mjesto u aplikaciji koje zna kako se lozinke hashiraju i provjeravaju.
/// Registracija, prijava i seed koriste ovu klasu, pa se algoritam i cost faktor
/// mijenjaju na jednom mjestu.
/// </summary>
public static class Lozinka
{
    /// <summary>Zahtjev iz kartice TIM4-181: bcrypt s cost faktorom najmanje 12.</summary>
    public const int CostFaktor = 12;

    /// <summary>Vraca bcrypt hash; salt je ugraden u hash i svaki poziv daje drugaciji rezultat.</summary>
    public static string Hash(string lozinka) => BCryptNet.HashPassword(lozinka, CostFaktor);

    /// <summary>
    /// Provjerava lozinku prema pohranjenom hashu. Neispravan hash (npr. stari "demo"
    /// iz ranijeg seeda) tretira se kao pogresna lozinka, a ne kao pad aplikacije.
    /// </summary>
    public static bool Provjeri(string lozinka, string hash)
    {
        try
        {
            return BCryptNet.Verify(lozinka, hash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
    }
}
