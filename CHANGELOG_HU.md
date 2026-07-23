# Változásnapló

Az összes jelentős változás ebben a projektben dokumentálva van ebben a fájlban.

A formátum alapja: [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
ez a projekt követi a [Semantic Versioning](https://semver.org/spec/v2.0.0.html) szabványt.

---


## [Kiadatlan]

### Tervezett - Backend
- Refresh Token mechanizmus
- Hibakezelő Middleware
- Strukturált naplózás (Serilog)
- Rate Limiting (brute force védelem)
- API verziókezelés
- Health Checks endpoint
- Docker támogatás (Dockerfile + docker-compose)

### Tervezett - Frontend
- Komponens unit tesztek (Jasmine)
- E2E tesztek (Cypress/Playwright)
- Toast értesítések (siker/hiba üzenetek)
- Töltési jelzők
- Űrlap validáció fejlesztése
- Akadálymentesítés (a11y)
- Többnyelvűség (i18n)
- Progressive Web App (PWA) támogatás

### Tervezett - DevOps
- CI/CD pipeline (GitHub Actions)
- Kódlefedettségi riportok (Codecov)
- Automatizált tesztelés PR-nál
- Telepítési pipeline

---
## [0.8.1] - 2026-07-21

### Hozzáadva
- Verseny visszaállítás funkció (soft delete után restore)

### Változott
- `DeleteConfirmationDialog` helyett általános `ConfirmationDialogComponent` minden törlés dialognál
- Futó törlésekor figyelmeztető üzenet a tervező beosztások elvesztéséről


## [0.8.0] - 2026-07-21

### Hozzáadva - Backend
- **Ultra Balaton (UB) modul** - új funkcionális egység
  - `Team` entitás (csapatok rajtidővel)
  - `Runner` entitás (futók alap-tempóval)
  - `Section` entitás (szakaszok)
  - `WayPoint` entitás (váltópontok koordinátákkal)
  - `RunnerSection` entitás (futó-szakasz hozzárendelés egyéni tempóval)
  - `TeamController`, `RunnerController`, `SectionController`, `RunnerSectionController`
  - `SectionImportController` - szakaszok tömeges importálása
  - `WayPointController` teljes CRUD + törlés védi a használatban lévő váltópontokat
- **Adatbázis migrációk**
  - `AddUltraBalatonTables` - Team/Runner/Section/WayPoint/RunnerSection táblák
  - `AddTeamStartTime`, `UpdateTeamStartTimeToDateTime` - csapat rajtidő mező
  - `AddWayPointTable`, `AddWayPointsToSection` - váltópontok
  - `MakeWayPointCoordinatesNullable`, `RemoveStartEndPointFromSection` - séma tisztítás

### Hozzáadva - Frontend
- **UB navigáció átszervezés**
  - MatSidenav hamburger menü a publikus és admin layoutban
  - Almenü: UltraBalaton → Szakaszok, Csapatok
  - Be/Kijelentkezés a sidenav-ban
  - Felhasználó ikon a publikus fejlécben (admin jelzéssel)
- **Váltópontok oldal** (`/ub/waypoints`) - lista, szerkesztés, új, törlés
- **authGuard** az UB route-on
- **isAdminMode** javítva - route + szerepkör alapján dönt
- Jelszó megjelenítés ikon cserélve
- Vissza gombok egységesítve (`mat-stroked-button`)
- Szerepkör-alapú UI az UB modulban (szakaszlista, csapatlista, váltópontok) - admin-only gombok elrejtése

### Változott - Frontend
- **UI áttervezés** a meglévő komponenseken
  - `admin-header`, `public-header`, `login` - stílus és elrendezés frissítés
  - `race-list`, `race-form` - vizuális finomítás
  - `delete-confirmation-dialog` - stílus egységesítés

### Hozzáadva - Tesztek (Frontend)
- `WaypointDialog` - létrehozás, mentés, mégse, előtöltés tesztek
- `Waypoints` - létrehozás, betöltés, törlés tesztek
- `WayPoint` service - CRUD HTTP tesztek
- `SectionInsertDialog` - létrehozás, mentés, mégse tesztek
- `DeleteConfirmationDialog` - létrehozás, megerősítés, mégse tesztek
- `Login` - létrehozás, űrlap validáció, küldés, hibakezelés tesztek
- `RaceList` - létrehozás, betöltés, rendezés, törlés tesztek
- `RaceForm` - létrehozás, űrlap validáció, küldés tesztek
- `PublicHeader` - létrehozás, menü toggle, kijelentkezés tesztek
- `AdminHeader` - létrehozás, menü toggle, kijelentkezés tesztek
- `AdminLayout` - létrehozás, menü toggle, kijelentkezés tesztek
- `authGuard` - engedélyezés/tiltás tesztek
- `adminGuard` - engedélyezés/tiltás tesztek
- `Planner` - létrehozás, betöltés, kumulatív km, blokkcsere, hozzárendelés tesztek
- `SectionList` - létrehozás, betöltés, törlés, teljes távolság tesztek

### Hozzáadva - Tesztek (Backend)
- `WayPointControllerTests` - GetAll, Create, Update, Delete, Delete használatban tesztek

---

## [0.7.0] - 2025-12-11

### Hozzáadva - Backend
- **Szerveroldali lapozás**
  - `PagedResult<T>` wrapper osztály (Items, TotalCount, Page, PageSize)
  - `RaceSearchModel` bővítve Page és PageSize paraméterekkel
  - Repository, Service, Controller átállítva lapozott válaszra

### Hozzáadva - Frontend
- **MatPaginator integráció**
  - `PagedResult<T>` interface
  - Lapozó komponens a versenylistához
  - Oldalméret választó (5, 10, 20, 50)
  - Lapozó stílusozás

### Javítva
- CSS elrendezés tisztítás (globális container stílus)
- AuthController teszt - GetRolesAsync mock hozzáadva

### Technikai
- Angular Material Paginator
- Backend lapozás: Skip/Take + CountAsync

---

## [0.6.1] - 2025-12-11

### Refaktorálás
- **Layout komponensek bevezetése**
  - `PublicLayoutComponent` - publikus fejléc + router-outlet
  - `AdminLayoutComponent` - admin fejléc + router-outlet
  - Beágyazott routing az admin útvonalakhoz
- **RaceListComponent egyszerűsítés**
  - Fejléc logika eltávolítva (a layout kezeli)
  - `updateAdminMode()` metódus törölve
- **RaceFormComponent egyszerűsítés**
  - Fejléc logika eltávolítva
  - `isAdminMode` property törölve
  - `onCancel()` AuthService-t használ localStorage helyett
- **TokenBlacklistService** - Scoped → Singleton

### Javítva
- Belépési hint mindkét teszt fiókot mutatja (Admin + User)

### UI/UX
- Fejléc hozzáadva a verseny szerkesztő oldalhoz
- Reszponzív mobil nézet (race-list, race-form)
- Színek finomhangolása (admin fejléc gradiens)
- Melegebb lista háttér

---

## [0.6.0] - 2025-12-11

### Hozzáadva - Backend
- **Szerepkör-alapú jogosultságkezelés**
  - Admin és User szerepkörök ASP.NET Identity-ben
  - DatabaseSeeder automatikusan létrehozza a szerepköröket
  - Szerepkör információk a JWT tokenben
  - `[Authorize(Roles = "Admin")]` Create/Update/Delete endpointokon
  - Csak Admin felhasználók módosíthatnak versenyeket
  - Sima felhasználóknak csak olvasási joguk van

### Hozzáadva - Frontend
- **Szerepkör-alapú felhasználói felület**
  - jwt-decode könyvtár token elemzéshez
  - `getUserRole()` és `isAdmin()` metódusok az AuthService-ben
  - Feltételes megjelenítés: Admin gombok csak Admin szerepkörrel
  - Material Icons integráció (belépés/kijelentkezés ikonok)
  - Dinamikus Belépés/Kijelentkezés gomb a publikus fejlécben
  - Kijelentkezés az admin fejlécben

### Változott - Backend
- JWT token generálás aszinkron (`GenerateJwtTokenAsync`)
- Szerepkör információk automatikusan hozzáadva UserManager-ből

### Változott - Frontend
- Kijelentkezés után `/races` (publikus) oldalra navigál `/login` helyett
- Verseny űrlap mégse gomb kontextus-függő routing (admin vs publikus)
- Material Icons használata Material Buttons nélkül (egyedi stílusozás megőrizve)

### Biztonság
- Admin szerepkör szükséges minden CRUD művelethez versenyeken
- User szerepkör csak olvasási jogot biztosít
- JWT tokenek tartalmazzák a szerepkör információt kliens oldali döntésekhez

### Adatbázis
- Admin felhasználó: admin@runningraceandi.com / Admin123!
- Teszt felhasználó: test@runningraceandi.com / Test123!
- Szerepkörök automatikus seed alkalmazás indításkor
- Meglévő felhasználók frissítve megfelelő szerepkörökkel

### Technikai
- jwt-decode: ^4.0.0 (npm csomag)
- Material Icons font integráció
- Szerepkör detektálás JWT claims alapján

---

## [0.5.0] - 2025-12-08

### Hozzáadva - Backend
- **ASP.NET Identity integráció**
  - ApplicationUser modell (IdentityUser kiterjesztés)
  - IdentityDbContext használata
  - Automatikus jelszó hash-elés
  - Felhasználó validáció (egyedi email, jelszó követelmények)
- **Token Blacklist rendszer**
  - ITokenBlacklistService interface
  - TokenBlacklistService (MemoryCache alapú)
  - JwtBlacklistMiddleware a tokenek ellenőrzéséhez
  - Biztonságos kijelentkezés (token érvénytelenítés)
- **DatabaseSeeder**
  - Automatikus szerepkör létrehozás (Admin, User)
  - Seed felhasználók létrehozása indításkor
  - Meglévő felhasználók szerepkör frissítése

### Hozzáadva - Backend tesztek
- **AuthControllerTests** (3 teszt)
  - Bejelentkezés érvényes adatokkal
  - Bejelentkezés érvénytelen adatokkal (Theory)
  - Kijelentkezés érvényes tokennel

### Változott - Backend
- AuthController átírva ASP.NET Identity-re
- Program.cs: Identity és MemoryCache konfiguráció
- JWT token tartalmazza a felhasználó szerepköreit

### Adatbázis migráció
- `20251208103052_AddIdentity.cs` - Identity táblák hozzáadása

---

## [0.4.0] - 2025-11-20

### Hozzáadva - Backend tesztek
- **RaceServiceTests** (16 teszt)
  - GetPublicRacesAsync - IsActive mindig true-ra kényszerítve
  - GetAdminRacesAsync - IsActive nem módosított
  - CreateRaceAsync - validációs tesztek (név, helyszín, távolság)
  - CreateRaceAsync - alapértelmezett értékek (Id, CreatedAt, IsActive)
  - UpdateRaceAsync - validáció és létezés ellenőrzés
  - DeleteRaceAsync - repository delegálás
  - GetRaceByIdAsync - üres Guid validáció
- **RacesControllerTests** (11 teszt)
  - GetRaceById - érvényes/érvénytelen ID esetek
  - GetPublicRaces - OK választ ad versenyekkel
  - CreateRace - CreatedAtAction választ ad location fejléccel
  - UpdateRace - OK választ ad frissített versennyel
  - DeleteRace - NoContent/NotFound választ ad

### Hozzáadva - Frontend tesztek
- **AuthService tesztek** (6 teszt)
  - Sikeres bejelentkezés elmenti a tokent
  - Kijelentkezés törli a tokent
  - getToken visszaad tokent/null-t
  - isAuthenticated igaz/hamis értéket ad

### Technikai
- Moq használata service és repository mock-oláshoz
- FluentAssertions minden ellenőrzéshez
- AAA minta (Arrange-Act-Assert) következetes használata

---

## [0.3.0] - 2025-11-17

### Hozzáadva - Backend

#### Service réteg (3-tier architektúra)
- **IRaceService** interface
  - `GetPublicRacesAsync()` - Publikus endpoint logika
  - `GetAdminRacesAsync()` - Admin endpoint logika
  - `GetRaceByIdAsync()` - Egy verseny lekérése
  - `CreateRaceAsync()` - Létrehozás validációval
  - `UpdateRaceAsync()` - Módosítás validációval
  - `DeleteRaceAsync()` - Soft delete
- **RaceService** implementáció
  - Üzleti logika validációk:
    - Név kötelező (nem null/üres/whitespace)
    - Helyszín kötelező
    - Távolság: min 0.1 km, max 500 km
  - Publikus endpoint: **IsActive = true kényszerítve** (biztonság)
  - Admin endpoint: IsActive opcionális (teljes kontroll)
  - Automatikus mezőkitöltés:
    - Id generálás (Guid.NewGuid())
    - CreatedAt = DateTime.UtcNow
    - IsActive = true (alapértelmezett)

#### Repository egyszerűsítés
- Alapértelmezett IsActive szűrés eltávolítva
- Repository csak **explicit megadáskor** szűr
- Tisztább felelősség-szétválasztás (logika Service-ben, nem Repository-ban)

#### Publikus/Admin endpoint szétválasztás
- **GET /api/races/public**
  - Nincs authentikáció szükséges
  - **Mindig csak aktív versenyeket ad vissza** (IsActive true-ra kényszerítve)
  - Alapértelmezett rendezés dátum szerint (növekvő)
- **GET /api/races/admin**
  - JWT authentikáció szükséges
  - Alapértelmezetten minden versenyt visszaad (IsActive = null)
  - Admin szűrhet IsActive szerint (true/false)

### Változott - Backend
- **RacesController** refaktorálva
  - Függőség változott: `IRaceRepository` → `IRaceService`
  - Controller delegál a Service rétegnek
  - Hibakezelés javítva (ArgumentException, InvalidOperationException)

### Hozzáadva - Frontend

#### Angular Material integráció
- **DeleteConfirmationDialogComponent**
  - Material Dialog törlés megerősítéshez
  - Verseny nevét mutatja a megerősítő üzenetben
  - Mégse/Megerősítés gombok Material stílussal
  - Gradiens piros fejléc
- Material Button modul
- Material Dialog modul

#### Komponens fejlesztések
- **race-list.component.ts**
  - Dinamikus endpoint választás: `getRaces(endpoint, searchModel)`
  - `isAdminMode` detektálás route URL alapján
  - Admin mód: Admin fejléc, IsActive szűrő, Szerkesztés/Törlés gombok
  - Publikus mód: Publikus fejléc, nincs admin funkció
  - IsActive szűrő legördülő (csak Adminnak)
  - Törlés megerősítés Material Dialog-gal

#### Service frissítések
- **race.service.ts**
  - Új metódus aláírás: `getRaces(endpoint: 'public' | 'admin', searchModel)`
  - Endpoint paraméter dinamikus publikus/admin váltáshoz
  - Query paraméter builder minden keresési opcióhoz

#### UI/UX fejlesztések
- Admin fejléc kijelentkezés gombbal
- Publikus fejléc belépés gombbal
- Gradiens háttér (lila publikusnak, barna adminnak)
- Reszponzív mobil design
- Atma egyedi betűtípus integráció

### Javítva
- IsActive szűrési logika (Repository-ból Service-be áthelyezve)
- Publikus endpoint biztonság (inaktív versenyekhez nem lehet hozzáférni)

---

## [0.2.0] - 2025-11-17

### Hozzáadva - Backend tesztek

#### Repository tesztlefedettség (18 teszt)
- **GetRacesAsync tesztek**
  - Aktív szűrés (IsActive = true/false/null)
  - Keresés funkció (név, helyszín, összes szerint)
  - Rendezés (név, dátum, helyszín, távolság, növekvő/csökkenő)
- **GetByIdAsync tesztek**
  - Érvényes ID visszaad versenyt
  - Érvénytelen ID null-t ad vissza
- **CreateAsync tesztek**
  - Alap létrehozás
  - Theory tesztek: különböző érvényes verseny adatok
  - Theory tesztek: érvénytelen nevek (null, üres, whitespace)
  - Theory tesztek: különböző távolságok
  - Theory tesztek: negatív távolságok (validáció)
  - Adatbázis perzisztencia ellenőrzés
- **UpdateAsync tesztek**
  - Sikeres eset érvényes adatokkal
  - Theory tesztek: különböző érvényes adatkombinációk
  - Nem létező ID InvalidOperationException-t dob
- **DeleteAsync tesztek**
  - Soft delete funkció (IsActive = false)
  - ModifiedAt timestamp frissítés
  - Verseny még megvan az adatbázisban
  - Nem létező ID false-t ad vissza

#### Repository validációs logika
- Verseny név validáció (kötelező)
- Távolság validáció (pozitív szám szükséges)

#### Tesztelési infrastruktúra
- xUnit test keretrendszer
- FluentAssertions olvasható ellenőrzésekhez
- InMemory adatbázis teszt izoláció
- Theory tesztek InlineData-val
- IDisposable cleanup minta

### Adatbázis migráció
- `20251117170945_AddAuditFields.cs`
  - `CreatedAt` hozzáadva (DateTime, kötelező)
  - `IsActive` hozzáadva (bool, alapértelmezett true)
  - `ModifiedAt` hozzáadva (DateTime?, nullable)

---

## [0.1.0] - 2025-11-17

### Hozzáadva - Backend architektúra

#### Repository minta implementáció
- **IRaceRepository** interface
  - `GetRacesAsync(RaceSearchModel)` - Szűréses lekérdezés
  - `GetByIdAsync(Guid)` - Egy verseny lekérése
  - `CreateAsync(Race)` - Új verseny létrehozása
  - `UpdateAsync(Guid, Race)` - Meglévő verseny módosítása
  - `DeleteAsync(Guid)` - Soft delete (IsActive = false)
- **RaceRepository** osztály
  - EF Core LINQ lekérdezések
  - Keresés (név, helyszín, összes)
  - Rendezés (név, dátum, helyszín, távolság)
  - Szűrés (IsActive)
- **Dependency Injection** beállítás Program.cs-ben

#### RacesController
- CRUD endpointok
- JWT authentikáció
- Swagger dokumentáció

### Hozzáadva - Frontend
- Angular 20 Standalone Components
- RaceListComponent (publikus + admin mód)
- RaceFormComponent (létrehozás + szerkesztés)
- LoginComponent
- AuthService (JWT kezelés)
- RaceService (HTTP hívások)
- Admin guard
- Auth interceptor

### Hozzáadva - Tesztelési infrastruktúra
- RunningRacesApi.Tests projekt
- xUnit keretrendszer
- Moq mock-oláshoz
- FluentAssertions
- InMemory adatbázis tesztelés

### Hozzáadva - Git verziókezelés
- GitHub repository beállítva
- .gitignore konfigurálva

### Technikai stack
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQLite adatbázis
- JWT Bearer authentikáció
- Swagger/OpenAPI dokumentáció
- Angular 20 (Standalone Components)

### Adatbázis
- Kezdeti migráció: `20251117125614_InitialCreate.cs`
- Verseny tábla 4 seed rekorddal

---

## Verziókezelés

A projekt a **Semantic Versioning (SemVer)** szabványt követi:

### Formátum: MAJOR.MINOR.PATCH

- **MAJOR** (1.x.x) - Visszamenőleg nem kompatibilis változások
- **MINOR** (x.1.x) - Új funkciók (visszamenőleg kompatibilis)
- **PATCH** (x.x.1) - Hibajavítások

### Pre-release (0.x.x)
- Instabil API
- Visszamenőleg nem kompatibilis változások bármikor előfordulhatnak
- Éles használatra **NEM ajánlott**

---

## Jelenlegi fejlesztési fázis

🚧 **Pre-release (0.8.0)** 🚧

**Kész:**
- ✅ Backend architektúra (3-layer)
- ✅ ASP.NET Identity
- ✅ JWT authentikáció + token blacklist
- ✅ Szerepkör-alapú jogosultságkezelés
- ✅ Backend tesztek (48 teszt)
- ✅ Frontend alapfunkciók
- ✅ AuthService tesztek (6 teszt)
- ✅ Szerveroldali lapozás
- ✅ Ultra Balaton modul (Team/Runner/Section/WayPoint)
- ✅ Frontend komponens tesztek
- ✅ UB navigáció átszervezés + váltópontok oldal

**Következő célok:**
1. E2E tesztek
2. CI/CD pipeline
3. Docker támogatás
4. Térkép nézet (Leaflet)

---

**Utoljára frissítve:** 2026-07-21  
**Karbantartó:** Kovács Andrea ([@koand75](https://github.com/koand75))
