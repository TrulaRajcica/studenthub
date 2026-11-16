# StudentHub — Platforma za feedback i suradnju na studentskim projektima

Projektni zadatak iz kolegija Programsko inženjerstvo.
Tim: Bruno Bebek · Tin Ramić · Kristijan Horvatić

---

## Preduvjeti

| Alat | Verzija | Provjera |
|---|---|---|
| .NET SDK | 10.0 (vidi `global.json`) | `dotnet --version` |
| Git | bilo koja novija | `git --version` |

Baza je **SQLite** i ne treba je instalirati — nastaje kao datoteka pri prvom pokretanju.

Ako `dotnet --version` javi drugu verziju od one u `global.json`, preuzmi točan SDK
s https://dotnet.microsoft.com/download i pokreni ponovno.

---

## Pokretanje (svježi klon)

```bash
git clone https://github.com/<organizacija>/studenthub.git
cd studenthub
cp .env.example .env          # Windows PowerShell: copy .env.example .env
dotnet restore
dotnet run --project src/StudentHub.Web
```

Aplikacija se otvara na **http://localhost:5080**.

To je sve. Pri pokretanju se automatski:
1. primjenjuju EF Core migracije i stvara `studenthub.db`
2. upisuju početni (seed) podaci ako je baza prazna

Nema ručnog kreiranja baze ni pokretanja skripti.

---

## Testni računi iz seed podataka

| E-mail | Lozinka | Uloga |
|---|---|---|
| ana@algebra.hr | Test123! | student |
| ivan@algebra.hr | Test123! | student |
| admin@algebra.hr | Test123! | administrator |

---

## Struktura repozitorija

```
src/StudentHub.Web/        ASP.NET Core MVC aplikacija
  Models/                  EF Core entiteti (Korisnik, Projekt, Feedback, ...)
  Data/                    DbContext, migracije, seed podaci
  Controllers/             kontroleri po modulima
  Views/                   Razor pogledi
tests/StudentHub.Tests/    smoke testovi
docs/                      dijagrami i dokumentacija
```

---

## Resetiranje baze

Ako baza dođe u neispravno stanje, obriši datoteku i pokreni ponovno:

```bash
rm src/StudentHub.Web/studenthub.db        # Windows: del ...\studenthub.db
dotnet run --project src/StudentHub.Web
```

---

## Testovi

```bash
dotnet test
```

---

## Pravila rada s repozitorijem

- Radi se na granama, nikad izravno na `main`
- Naziv grane po kartici iz Jire: `feature/SH-12-prikaz-liste-projekata`
- Spajanje isključivo kroz pull request koji je odobrio drugi član tima
- U repozitorij **ne idu**: `.env`, `*.db`, `bin/`, `obj/`
- U kodu **nema apsolutnih putanja** — koristi `Path.Combine` i relativne putanje
