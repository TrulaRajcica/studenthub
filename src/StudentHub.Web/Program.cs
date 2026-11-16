using Microsoft.EntityFrameworkCore;
using StudentHub.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// SQLite - baza je obicna datoteka pokraj aplikacije, ne treba instalacija
var conn = builder.Configuration.GetConnectionString("Default")
           ?? "Data Source=studenthub.db";
builder.Services.AddDbContext<StudentHubDbContext>(o => o.UseSqlite(conn));

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
