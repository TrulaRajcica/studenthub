using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Models;

namespace StudentHub.Web.Data;

public class StudentHubDbContext : DbContext
{
    public StudentHubDbContext(DbContextOptions<StudentHubDbContext> options) : base(options) { }

    public DbSet<Korisnik> Korisnici => Set<Korisnik>();
    public DbSet<Vjestina> Vjestine => Set<Vjestina>();
    public DbSet<KorisnikVjestina> KorisnikVjestine => Set<KorisnikVjestina>();
    public DbSet<Kategorija> Kategorije => Set<Kategorija>();
    public DbSet<Tag> Tagovi => Set<Tag>();
    public DbSet<ProjektTag> ProjektTagovi => Set<ProjektTag>();
    public DbSet<Projekt> Projekti => Set<Projekt>();
    public DbSet<Feedback> Feedbackovi => Set<Feedback>();
    public DbSet<OdgovorNaFeedback> OdgovoriNaFeedback => Set<OdgovorNaFeedback>();
    public DbSet<Oglas> Oglasi => Set<Oglas>();
    public DbSet<OglasVjestina> OglasVjestine => Set<OglasVjestina>();
    public DbSet<Ponuda> Ponude => Set<Ponuda>();
    public DbSet<ClanTima> ClanoviTima => Set<ClanTima>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // --- jedinstvenost ---
        b.Entity<Korisnik>().HasIndex(k => k.Email).IsUnique();
        b.Entity<Vjestina>().HasIndex(v => v.Naziv).IsUnique();
        b.Entity<Kategorija>().HasIndex(k => k.Naziv).IsUnique();
        b.Entity<Tag>().HasIndex(t => t.Naziv).IsUnique();

        // jedan korisnik = jedan feedback po projektu
        b.Entity<Feedback>().HasIndex(f => new { f.ProjektId, f.AutorId }).IsUnique();

        // isti kandidat ne moze dvaput na isti oglas
        b.Entity<Ponuda>().HasIndex(p => new { p.OglasId, p.KandidatId }).IsUnique();

        // isti korisnik je clan tima projekta samo jednom
        b.Entity<ClanTima>().HasIndex(c => new { c.ProjektId, c.KorisnikId }).IsUnique();

        // --- vezne tablice M:N ---
        b.Entity<KorisnikVjestina>().HasKey(x => new { x.KorisnikId, x.VjestinaId });
        b.Entity<ProjektTag>().HasKey(x => new { x.ProjektId, x.TagId });
        b.Entity<OglasVjestina>().HasKey(x => new { x.OglasId, x.VjestinaId });

        // --- ponasanje pri brisanju ---
        b.Entity<Projekt>()
            .HasOne(p => p.Vlasnik).WithMany(k => k.Projekti)
            .HasForeignKey(p => p.VlasnikId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<Feedback>()
            .HasOne(f => f.Projekt).WithMany(p => p.Feedbackovi)
            .HasForeignKey(f => f.ProjektId).OnDelete(DeleteBehavior.Cascade);

        b.Entity<Feedback>()
            .HasOne(f => f.Autor).WithMany(k => k.Feedbackovi)
            .HasForeignKey(f => f.AutorId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<OdgovorNaFeedback>()
            .HasOne(o => o.Feedback).WithMany(f => f.Odgovori)
            .HasForeignKey(o => o.FeedbackId).OnDelete(DeleteBehavior.Cascade);

        b.Entity<OdgovorNaFeedback>()
            .HasOne(o => o.Autor).WithMany()
            .HasForeignKey(o => o.AutorId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<Ponuda>()
            .HasOne(p => p.Kandidat).WithMany(k => k.Ponude)
            .HasForeignKey(p => p.KandidatId).OnDelete(DeleteBehavior.Restrict);

        b.Entity<ClanTima>()
            .HasOne(c => c.Korisnik).WithMany(k => k.Clanstva)
            .HasForeignKey(c => c.KorisnikId).OnDelete(DeleteBehavior.Restrict);

        // --- preciznost ---
        b.Entity<Projekt>().Property(p => p.ProsjecnaOcjena).HasPrecision(3, 2);

        // --- enumi kao tekst u bazi, citljivije pri pregledu ---
        b.Entity<Korisnik>().Property(k => k.Uloga).HasConversion<string>().HasMaxLength(20);
        b.Entity<Projekt>().Property(p => p.Status).HasConversion<string>().HasMaxLength(20);
        b.Entity<Oglas>().Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
        b.Entity<Ponuda>().Property(p => p.Status).HasConversion<string>().HasMaxLength(20);

        base.OnModelCreating(b);
    }
}
