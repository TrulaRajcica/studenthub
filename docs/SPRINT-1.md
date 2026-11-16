# Sprint 1 — plan

Projekt u Jiri: **TIM4 — Tim4-AplikacijaZaFeedbackISuradnju** · ploča `boards/2`

---

## Stanje backloga — provjereno 16. 11. 2026. navečer

| | Stanje |
|---|---|
| Epici | 6 ✔ (TIM4-1, 3, 4, 40, 51, 80) |
| Storyji | **37** ✔ (poraslo s 26 — dodani su i nefunkcionalni zahtjevi kao priče) |
| Taskovi i subtaskovi | ~140, dodano u velikoj seriji 16. 11. u 17:25 |
| Bugovi | **0 ✘ — profesor traži barem jedan na ploči** |
| Sprint 1 | kreiran, ali i dalje **prazan (0 stavki) i nije pokrenut** |
| Assignee | **sve neraspoređeno** |
| Stupci ploče | zadani To Do / In Progress / Done — treba dodati Code Review i Testing |

Backlog je sada jako detaljan. Priča za prijavu u međuvremenu je dodana (**TIM4-137**),
pa je više ne treba izmišljati. Ostaje jedna stvarna rupa: nema priče za obično
pregledavanje liste objavljenih projekata.

## Sprint Goal

> **Posjetitelj se može registrirati i prijaviti, pregledati listu objavljenih projekata s detaljima i ostaviti komentar s ocjenom.**

Izvan opsega Sprinta 1: upload datoteka, potvrda e-maila, pretraga i filtri, preporuke
sličnih projekata, oglasi, ponude, chat, administracija.

---

## Sadržaj Sprinta 1 — 21 SP

| Kartica | Story | SP | Tko |
|---|---|---|---|
| TIM4-5 | Kao posjetitelj želim unijeti registracijske podatke uz validaciju… | 3 | Kristijan |
| TIM4-137 | Kao registrirani korisnik želim se prijaviti i obnoviti zaboravljenu lozinku… | 5 | Kristijan |
| **nova** | Kao korisnik želim vidjeti listu objavljenih projekata i otvoriti detalje… | 5 | Bruno |
| TIM4-7 | Kao student želim unijeti naziv, opis i kategoriju projekta… | 3 | Bruno |
| TIM4-41 | Kao student želim ostaviti komentar i ocjenu na tuđi projekt… | 5 | Tin |

**Raspodjela: Kristijan 8 SP · Bruno 8 SP · Tin 5 SP** uz zajednički infrastrukturni rad.

> **TIM4-137 suzi na prijavu i odjavu.** Obnova zaboravljene lozinke traži slanje
> e-maila i ne stane u prvi tjedan — izdvoji je u zasebnu priču i vrati u backlog.

> Postojeći task **TIM4-166 „Stranica detalja projekta"** poveži kao dijete nove priče
> umjesto da radiš novi task.

Uz to tri infrastrukturna taska i jedna Bug kartica — sve je u `Jira_Sprint1_dopuna.csv`
(12 redaka, samo ono čega stvarno nema).

## Trajanje i događaji

Sprint traje **1 tjedan**.

| Događaj | Dan i sat | Trajanje | Tko |
|---|---|---|---|
| Sprint Planning | [UPIŠI dan vježbi], [UPIŠI sat] | 45 min | svi |
| Daily Scrum | ponedjeljak, srijeda, petak u [UPIŠI sat] | 15 min | svi, online |
| Sprint Review | [UPIŠI dan vježbi], [UPIŠI sat] | 20 min | svi + nastavnik |
| Sprint Retrospective | odmah nakon Reviewa | 20 min | svi |

> Na Reviewu demo pokreće **drugi član tima, ne autor funkcionalnosti**.

---

## Definition of Done

Kartica je Done tek kad su ispunjena **sva** pravila:

1. Kod je spojen u `main` kroz pull request koji je odobrio barem jedan drugi član tima.
2. **Aplikacija se pokreće svježim klonom po README-u na računalima sva tri člana tima.**
3. Baza se postavlja bez ručnog rada — migracije i seed izvršavaju se pri pokretanju.
4. U repozitoriju nema tajni ni apsolutnih putanja; `.env.example` je ažuriran.
5. Prihvatni kriteriji iz opisa kartice su ispunjeni i ručno provjereni.
6. Smoke test glavnog toka prolazi (`dotnet test` je zelen).
7. Kartica u Jiri je u statusu Done i sadrži poveznicu na pull request.

---

## Kanban ploča

### Stupci i izlazni kriteriji

| Stupac | Kartica smije izaći iz stupca kad |
|---|---|
| **TO DO** | ima opis, prihvatne kriterije i procjenu u story pointima |
| **IN PROGRESS** | postoji grana nazvana po kartici i kod je pushan na remote |
| **CODE REVIEW** | pull request je otvoren, opisano je kako se testira, recenzent je odobrio |
| **TESTING** | **drugi član tima** povukao granu i prošao scenarij **na svom računalu** |
| **DONE** | ispunjen cijeli Definition of Done i kod je spojen u `main` |

### WIP limiti

| Stupac | Limit |
|---|---|
| IN PROGRESS | najviše 3 kartice |
| CODE REVIEW | najviše 2 kartice |
| TESTING | najviše 2 kartice |

Kad je limit dosegnut, ne otvara se nova kartica — pomaže se dovršiti postojeću.

---

## Što napraviti u Jiri, redom

1. **Uvezi `Jira_Sprint1_dopuna.csv`** (1 priča, 5 taskova, 5 subtaskova, 1 bug).
2. **Poveži novu priču s epicom TIM4-4** (Upravljanje projektima) i pod nju prebaci postojeći task TIM4-166.
3. **Napuni Sprint 1** — u Backlogu povuci TIM4-5, TIM4-137, TIM4-7, TIM4-41 i novu priču u `TIM4 Sprint 1`.
4. **Dodaj datume sprinta** („Add dates") i klikni **Start sprint**.
5. **Dodijeli Assignee** po tablici gore. Bez toga profesor ne vidi tko što radi.
6. **Dodaj stupce ploči**: Board → ••• → Configure board → Columns → dodaj
   `CODE REVIEW` i `TESTING` između In Progress i Done.
7. **Postavi WIP limite** na ta tri stupca (Column settings → Set column limit).
8. **Upiši izlazne kriterije** iz tablice gore u opis svakog stupca.

---

## Podjela posla

| Član | Modul | Zaduženje u Sprintu 1 |
|---|---|---|
| Kristijan Horvatić | 1 — Korisnici i profili | registracija, prijava, odjava, seed korisnika |
| Bruno Bebek | 2 — Projekti | EF modeli i migracije, lista projekata, detalji, unos projekta |
| Tin Ramić | 3 — Feedback i suradnja | unos komentara i ocjene, prikaz komentara i prosjeka |

Moduli 4, 5 i 6 ostaju u Product Backlogu za kasnije sprintove.

---

## Rizici

| Rizik | Mjera |
|---|---|
| Aplikacija radi samo autoru | Svježi klon na sva tri računala **prvi dan**, ne zadnji |
| Različite verzije .NET SDK-a | `global.json` zaključava verziju |
| Baza postoji samo na jednom računalu | SQLite + migracije i seed pri pokretanju |
| Tin čeka Brunine modele | Bruno prvi dan pushne samo entitete i migraciju, poglede radi poslije |
| Konflikti u istim datotekama | Zajednički `DbContext` mijenja samo Bruno |
