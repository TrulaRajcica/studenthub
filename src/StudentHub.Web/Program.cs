using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Data;
using StudentHub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// SQLite - baza je obicna datoteka pokraj aplikacije, ne treba instalacija
var conn = builder.Configuration.GetConnectionString("Default")
           ?? "Data Source=studenthub.db";
builder.Services.AddDbContext<StudentHubDbContext>(o => o.UseSqlite(conn));

// --- feedback na projektima (TIM4-41, TIM4-187) ---
builder.Services.AddScoped<FeedbackServis>();

// PRIVREMENO: dok prijava (TIM4-137) nije gotova, trenutni korisnik se cuva u sessionu.
// Kad prijava bude spojena, mijenja se samo implementacija iza ITrenutniKorisnik.
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.IdleTimeout = TimeSpan.FromHours(8);
});
builder.Services.AddScoped<SessionTrenutniKorisnik>();
builder.Services.AddScoped<ITrenutniKorisnik>(sp => sp.GetRequiredService<SessionTrenutniKorisnik>());
builder.Services.AddScoped<IDemoPrijava>(sp => sp.GetRequiredService<SessionTrenutniKorisnik>());

var app = builder.Build();

// migracije i seed pri svakom pokretanju - "baza jednom naredbom"
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StudentHubDbContext>();
    db.Database.Migrate();
    SeedData.Pokreni(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();   // u razvoju ostaje cisti http na portu 5080
}
app.UseStaticFiles();
app.UseRouting();
app.UseSession();      // PRIVREMENO: nosi odabranog korisnika dok prijava nije gotova
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
