# Changelog

Az összes jelentős változás ebben a projektben dokumentálva van ebben a fájlban.

A formátum alapja: [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
és ez a projekt követi a [Semantic Versioning](https://semver.org/spec/v2.0.0.html) szabványt.

---

## [Unreleased]

### Tervezett - Backend
- Refresh Token mechanizmus
- Error Handling Middleware
- Structured Logging (Serilog)
- Rate Limiting (Anti-brute force)
- API Versioning
- Health Checks endpoint
- Docker support (Dockerfile + docker-compose)

### Tervezett - Frontend
- Component unit tesztek (Jasmine)
- E2E tesztek (Cypress/Playwright)
- Toast notifications (success/error messages)
- Loading spinners
- Form validation improvements
- Accessibility (a11y) improvements
- Internationalization (i18n - multi-language)
- Progressive Web App (PWA) support

### Tervezett - DevOps
- CI/CD pipeline (GitHub Actions)
- Code coverage reports (Codecov)
- Automated testing on PR
- Deployment pipeline

---
## [0.8.0] - 2026-07-21

### Hozzáadva - Backend
- **Ultra Balaton (UB) modul** - új funkcionális egység
  - `Team` entitás (csapatok, rajtidővel)
  - `Runner` entitás (futók, alap-tempóval)
  - `Section` entitás (szakaszok, útvonal-bontás)
  - `WayPoint` entitás (szakaszhatár-pontok, koordinátákkal)
  - `RunnerSection` entitás (futó-szakasz hozzárendelés, egyéni tempóval)
  - `TeamController`, `RunnerController`, `SectionController`, `RunnerSectionController`
  - `SectionImportController` - szakaszok tömeges importálása
  - `WayPointController` teljes CRUD + törlés védi a használatban lévő pontokat
- **Adatbázis migrációk**
  - `AddUltraBalatonTables` - Team/Runner/Section/WayPoint/RunnerSection táblák
  - `AddTeamStartTime`, `UpdateTeamStartTimeToDateTime` - csapat rajtidő mező
  - `AddWayPointTable`, `AddWayPointsToSection` - útvonalpontok
  - `MakeWayPointCoordinatesNullable`, `RemoveStartEndPointFromSection` - séma finomítás

### Hozzáadva - Frontend
- **UB navigáció átszervezés**
  - MatSidenav hamburger menü public és admin layoutban
  - Almenü: UltraBalaton → Szakaszok, Csapatok
  - Be/Kijelentkezés a sidenav-ban
  - Felhasználó ikon public headerben (admin jelzéssel)
- **Váltópontok oldal** (`/ub/waypoints`) - lista, szerkesztés, új, törlés
- **authGuard** az UB route-on
- **isAdminMode** javítva - route + role alapján dönt
- Jelszó megjelenítés ikon cserélve
- Vissza gombok egységesítve (`mat-stroked-button`)
- Szerepkör-alapú UI az UB modulban (section-list, team-list, waypoints) - admin-only gombok elrejtése

### Változott - Frontend
- **UI redesign** a meglévő komponenseken
  - `admin-header`, `public-header`, `login` - stílus és layout frissítés
  - `race-list`, `race-form` - vizuális finomítás
  - `delete-confirmation-dialog` - stílus egységesítés

### Hozzáadva - Tesztek (Frontend)
- `WaypointDialog` - create, save, cancel, prefill tesztek
- `Waypoints` - create, load, delete tesztek
- `WayPoint` service - CRUD HTTP tesztek
- `SectionInsertDialog` - create, save, cancel tesztek
- `DeleteConfirmationDialog` - create, confirm, cancel tesztek
- `Login` - create, form validation, submit, error handling tesztek
- `RaceList` - create, load, sort, clear tesztek
- `RaceForm` - create, form validation, submit tesztek
- `PublicHeader` - create, menu toggle, logout tesztek
- `AdminHeader` - create, menu toggle, logout tesztek
- `AdminLayout` - create, menu toggle, logout tesztek
- `authGuard` - allow/deny tesztek
- `adminGuard` - allow/deny tesztek

### Hozzáadva - Tesztek (Backend)
- `WayPointControllerTests` - GetAll, Create, Update, Delete, Delete in use tesztek

---
## [0.7.0] - 2025-12-11

### Hozzáadva - Backend
- **Server-side pagination**
  - `PagedResult<T>` wrapper osztály (Items, TotalCount, Page, PageSize)
  - `RaceSearchModel` bővítve Page és PageSize paraméterekkel
  - Repository, Service, Controller átállítva lapozott válaszra

### Hozzáadva - Frontend
- **MatPaginator integráció**
  - `PagedResult<T>` interface
  - Lapozó komponens a verseny listához
  - Oldalméret választó (5, 10, 20, 50)
  - Paginator styling (barna/bézs színvilág)

### Javítva
- CSS layout tisztítás (globális container stílus)
- AuthController teszt - GetRolesAsync mock hozzáadva

### Technikai
- Angular Material Paginator
- Backend pagination: Skip/Take + CountAsync

## [0.6.1] - 2025-12-11

### Refaktorálás
- **Layout komponensek bevezetése**
  - `PublicLayoutComponent` - public fejléc + router-outlet
  - `AdminLayoutComponent` - admin fejléc + router-outlet
  - Nested routing az admin útvonalakhoz
- **RaceListComponent egyszerűsítés**
  - Fejléc logika eltávolítva (layout kezeli)
  - `updateAdminMode()` metódus törölve
- **RaceFormComponent egyszerűsítés**
  - Fejléc logika eltávolítva
  - `isAdminMode` property törölve
  - `onCancel()` AuthService-t használ localStorage helyett
- **TokenBlacklistService** - Scoped → Singleton (egy instance az app lifetime-ra)

### Javítva
- Login hint mindkét teszt fiókot mutatja (Admin + User)

### UI/UX
- Fejléc hozzáadva a verseny szerkesztő oldalhoz
- Reszponzív mobil nézet (race-list, race-form)
- Színek finomhangolása (admin header gradient)
- Lista háttér (melegebb, kevésbé rideg megjelenés)

## [0.6.0] - 2025-12-11

### Hozzáadva - Backend
- **Szerepkör-alapú Jogosultságkezelés**
  - Admin és User szerepkörök ASP.NET Identity-ben
  - DatabaseSeeder automatikusan létrehozza a szerepköröket
  - Szerepkör információk a JWT tokenben
  - `[Authorize(Roles = "Admin")]` attribútum Create/Update/Delete endpoint-okon
  - Csak Admin felhasználók módosíthatnak versenyeket
  - Sima felhasználók csak olvasási joggal rendelkeznek

### Hozzáadva - Frontend
- **Szerepkör-alapú felhasználói felület**
  - jwt-decode library token elemzéshez
  - `getUserRole()` és `isAdmin()` metódusok AuthService-ben
  - Feltételes megjelenítés: Admin gombok csak Admin szerepkörrel
  - Material Icons integráció (login/logout ikonok)
  - Public header dinamikus Belépés/Kijelentkezés gombbal
  - Admin header kijelentkezés funkcióval

### Változott - Backend
- JWT token generálás aszinkron (`GenerateJwtTokenAsync`)
- Szerepkör információk automatikusan hozzáadva UserManager-ből

### Változott - Frontend
- Kijelentkezés után `/races` (publikus) oldalra navigál `/login` helyett
- Verseny űrlap mégse gomb kontextus-függő routing (admin vs public)
- Material Icons használata Material Buttons nélkül (egyedi styling megőrizve)

### Biztonság
- Admin szerepkör szükséges minden CRUD művelethez versenyeken
- User szerepkör csak olvasási jogot biztosít
- JWT tokenek tartalmazzák a szerepkör információt kliens oldali döntésekhez

### Adatbázis
- Admin felhasználó: admin@runningraceandi.com / Admin123!
- Test felhasználó: test@runningraceandi.com / Test123!
- Szerepkörök automatikus seed alkalmazás indításkor
- Meglévő felhasználók frissítve megfelelő szerepkörökkel

### Technikai
- jwt-decode: ^4.0.0 (npm package)
- Material Icons font integráció
- Szerepkör detektálás JWT claims alapján

---

## [0.5.0] - 2025-12-08

### Hozzáadva - Backend
- **ASP.NET Identity integráció**
  - ApplicationUser model (IdentityUser extension)
  - IdentityDbContext használata
  - Automatikus jelszó hashing
  - Felhasználó validáció (unique email, password requirements)
- **Token Blacklist rendszer**
  - ITokenBlacklistService interface
  - TokenBlacklistService (MemoryCache alapú)
  - JwtBlacklistMiddleware a tokenek ellenőrzéséhez
  - Biztonságos logout (token érvénytelenítés)
- **DatabaseSeeder**
  - Automatikus szerepkör létrehozás (Admin, User)
  - Seed felhasználók létrehozása indításkor
  - Meglévő felhasználók szerepkör frissítése

### Hozzáadva - Backend Testing
- **AuthControllerTests** (3 teszt)
  - Login with valid credentials
  - Login with invalid credentials (Theory)
  - Logout with valid token

### Változott - Backend
- AuthController átírva ASP.NET Identity-re
- Program.cs: Identity és MemoryCache konfiguráció
- JWT token tartalmazza a felhasználó szerepköreit

### Adatbázis Migration
- `20251208103052_AddIdentity.cs` - Identity táblák hozzáadása

---

## [0.4.0] - 2025-11-20

### Hozzáadva - Backend Testing
- **RaceServiceTests** (16 teszt)
  - GetPublicRacesAsync - IsActive always forced to true
  - GetAdminRacesAsync - IsActive not modified
  - CreateRaceAsync - validation tests (name, location, distance)
  - CreateRaceAsync - default values (Id, CreatedAt, IsActive)
  - UpdateRaceAsync - validation and existence checks
  - DeleteRaceAsync - repository delegation
  - GetRaceByIdAsync - empty Guid validation
- **RacesControllerTests** (11 teszt)
  - GetRaceById - valid/invalid ID scenarios
  - GetPublicRaces - returns OK with races
  - CreateRace - returns CreatedAtAction with location header
  - UpdateRace - returns OK with updated race
  - DeleteRace - returns NoContent/NotFound

### Hozzáadva - Frontend Testing
- **AuthService tesztek** (6 teszt)
  - Login successfully stores token
  - Logout clears token
  - getToken returns token/null
  - isAuthenticated returns true/false

### Technikai
- Moq használata service és repository mock-oláshoz
- FluentAssertions minden assertion-höz
- AAA pattern (Arrange-Act-Assert) következetes használata

---

## [0.3.0] - 2025-11-17

### Hozzáadva - Backend

#### Service Layer (3-tier Architecture)
- **IRaceService** interface
  - `GetPublicRacesAsync()` - Public endpoint logic
  - `GetAdminRacesAsync()` - Admin endpoint logic
  - `GetRaceByIdAsync()` - Single race retrieval
  - `CreateRaceAsync()` - Create with validation
  - `UpdateRaceAsync()` - Update with validation
  - `DeleteRaceAsync()` - Soft delete
- **RaceService** implementation
  - Business logic validations:
    - Name required (not null/empty/whitespace)
    - Location required
    - Distance: min 0.1 km, max 500 km
  - Public endpoint: **Forces IsActive = true** (security)
  - Admin endpoint: IsActive opcionális (full control)
  - Automatic field population:
    - Id generation (Guid.NewGuid())
    - CreatedAt = DateTime.UtcNow
    - IsActive = true (default)

#### Repository Simplification
- Removed default IsActive filtering
- Repository now filters **only when explicitly provided**
- Cleaner separation of concerns (logic in Service, not Repository)

#### Public/Admin Endpoint Separation
- **GET /api/races/public**
  - No authentication required
  - **Always returns only active races** (IsActive forced to true)
  - Default sorting by date (ascending)
- **GET /api/races/admin**
  - JWT authentication required
  - Returns all races by default (IsActive = null)
  - Admin can explicitly filter by IsActive (true/false)

### Változott - Backend
- **RacesController** refactored
  - Dependency changed: `IRaceRepository` → `IRaceService`
  - Controller now delegates to Service layer
  - Exception handling improved (ArgumentException, InvalidOperationException)

### Hozzáadva - Frontend

#### Angular Material Integration
- **DeleteConfirmationDialogComponent**
  - Material Dialog for delete confirmation
  - Shows race name in confirmation message
  - Cancel/Confirm buttons with Material styling
  - Gradient red header
- Material Button module
- Material Dialog module

#### Component Enhancements
- **race-list.component.ts**
  - Dynamic endpoint selection: `getRaces(endpoint, searchModel)`
  - `isAdminMode` detection based on route URL
  - Admin mode: Shows admin header, IsActive filter, Edit/Delete buttons
  - Public mode: Shows public header, no admin actions
  - IsActive filter dropdown (Admin only)
  - Delete confirmation with Material Dialog

#### Service Updates
- **race.service.ts**
  - New method signature: `getRaces(endpoint: 'public' | 'admin', searchModel)`
  - Endpoint parameter for dynamic public/admin switching
  - Query parameter builder for all search options

#### UI/UX Improvements
- Admin header with logout button
- Public header with login button
- Gradient backgrounds (purple for public, brown for admin)
- Responsive design for mobile
- Atma custom font integration

### Javítva
- IsActive filtering logic (moved from Repository to Service)
- Public endpoint security (no way to access inactive races)

---

## [0.2.0] - 2025-11-17

### Hozzáadva - Backend Testing

#### Repository Test Coverage (18 teszt)
- **GetRacesAsync tests**
  - Active filtering (IsActive = true/false/null)
  - Search functionality (by name, location, all)
  - Sorting (name, date, location, distance, asc/desc)
- **GetByIdAsync tests**
  - Valid ID returns race
  - Invalid ID returns null
- **CreateAsync tests**
  - Basic creation
  - Theory tests: various valid race data
  - Theory tests: invalid names (null, empty, whitespace)
  - Theory tests: various distances
  - Theory tests: negative distances (validation)
  - Database persistence verification
- **UpdateAsync tests**
  - Success scenario with valid data
  - Theory tests: different valid data combinations
  - Non-existent ID throws InvalidOperationException
- **DeleteAsync tests**
  - Soft delete functionality (IsActive = false)
  - ModifiedAt timestamp update
  - Race still exists in database
  - Non-existent ID returns false

#### Repository Validation Logic
- Race name validation (required)
- Distance validation (must be positive)

#### Testing Infrastructure
- xUnit test framework
- FluentAssertions for readable assertions
- InMemory Database for test isolation
- Theory tests with InlineData
- IDisposable cleanup pattern

### Adatbázis Migration
- `20251117170945_AddAuditFields.cs`
  - Added `CreatedAt` (DateTime, required)
  - Added `IsActive` (bool, default true)
  - Added `ModifiedAt` (DateTime?, nullable)

---

## [0.1.0] - 2025-11-17

### Hozzáadva - Backend Architecture

#### Repository Pattern Implementation
- **IRaceRepository** interface
  - `GetRacesAsync(RaceSearchModel)` - Query with filters
  - `GetByIdAsync(Guid)` - Single race retrieval
  - `CreateAsync(Race)` - Insert new race
  - `UpdateAsync(Guid, Race)` - Update existing race
  - `DeleteAsync(Guid)` - Soft delete (IsActive = false)
- **RaceRepository** class
  - EF Core LINQ queries
  - Search (name, location, all)
  - Sort (name, date, location, distance)
  - Filter (IsActive)
- **Dependency Injection** setup in Program.cs

#### RacesController
- CRUD endpoints
- JWT authentication
- Swagger documentation

### Hozzáadva - Frontend
- Angular 20 Standalone Components
- RaceListComponent (public + admin mode)
- RaceFormComponent (create + edit)
- LoginComponent
- AuthService (JWT handling)
- RaceService (HTTP calls)
- Admin guard
- Auth interceptor

### Hozzáadva - Testing Setup
- RunningRacesApi.Tests project
- xUnit framework
- Moq for mocking
- FluentAssertions
- InMemory database testing

### Hozzáadva - Git Version Control
- GitHub repository setup
- .gitignore configured

### Technikai Stack
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQLite database
- JWT Bearer Authentication
- Swagger/OpenAPI documentation
- Angular 20 (Standalone Components)

### Adatbázis
- Initial Migration: `20251117125614_InitialCreate.cs`
- Race table with 4 seed records

---

## Verziókezelés

A projekt követi a **Semantic Versioning (SemVer)** szabványt:

### Formátum: MAJOR.MINOR.PATCH

- **MAJOR** (1.x.x) - Breaking changes
- **MINOR** (x.1.x) - Új funkciók (backward compatible)
- **PATCH** (x.x.1) - Bug fix-ek

### Pre-release (0.x.x)
- Instabil API
- Breaking changes bármikor előfordulhatnak
- Production használatra **NEM javasolt**

---

## Jelenlegi Fejlesztési Fázis

🚧 **Pre-release (0.8.0)** 🚧

**Kész:**
- ✅ Backend architektúra (3-layer)
- ✅ ASP.NET Identity
- ✅ JWT autentikáció + token blacklist
- ✅ Szerepkör-alapú jogosultságkezelés
- ✅ Backend tesztek (48 teszt)
- ✅ Frontend alapfunkciók
- ✅ AuthService tesztek (6 teszt)
- ✅ Server-side pagination
- ✅ Ultra Balaton modul (Team/Runner/Section/WayPoint)

**Következő célok:**
1. Frontend komponens tesztek
2. E2E tesztek
3. CI/CD pipeline
4. Docker support

---

**Last Updated:** 2026-03-29  
**Maintained by:** Kovács Andrea ([@koand75](https://github.com/koand75))
