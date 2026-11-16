using System.ComponentModel.DataAnnotations;

namespace StudentHub.Web.Models;

public class Kategorija
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Naziv { get; set; } = "";

    public ICollection<Projekt> Projekti { get; set; } = new List<Projekt>();
}

public class Tag
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Naziv { get; set; } = "";

    public ICollection<ProjektTag> Projekti { get; set; } = new List<ProjektTag>();
}

public class ProjektTag
{
    public int ProjektId { get; set; }
    public Projekt? Projekt { get; set; }

    public int TagId { get; set; }
    public Tag? Tag { get; set; }
}
