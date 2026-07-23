# 🏃 Running Races 3.0 - Management Application

Full-stack futóversenyek kezelő alkalmazás modern technológiákkal. ASP.NET Core backend JWT autentikációval + Angular 20 standalone frontend.

---

## 🚀 Tech Stack

### Backend
- **ASP.NET Core 8.0** - Web API
- **Entity Framework Core 8.0** - ORM
- **ASP.NET Identity** - Felhasználókezelés & jelszó hashing
- **SQLite** - Database (Development)
- **JWT Bearer Authentication** - Biztonság
- **Swagger/OpenAPI** - API dokumentáció
- **xUnit 2.6.5** - Testing framework
- **Moq 4.20.70** - Mocking library
- **FluentAssertions 6.12.0** - Readable assertions

### Frontend
- **Angular 20.3** - SPA Framework (Standalone Components)
- **TypeScript 5.9** - Type safety
- **RxJS 7.8** - Reactive programming
- **Angular Material 20.2** - UI Components
- **Angular Router** - Routing + Guards
- **jwt-decode** - Token parsing
- **Atma Font** - Custom typography

---

## 📂 Projekt Struktúra

```
RunningRaces/
├── RunningRacesApi/                  # 🔙 Backend API
│   ├── Controllers/
│   │   ├── RacesController.cs        # CRUD + keresés/szűrés
│   │   └── AuthController.cs         # JWT login/logout endpoint
│   │
│   ├── Services/                     # 📋 Üzleti logika réteg
│   │   ├── IRaceService.cs
│   │   ├── RaceService.cs            # Validáció + public/admin logika
│   │   ├── ITokenBlacklistService.cs
│   │   └── TokenBlacklistService.cs  # JWT token blacklist (logout)
│   │
│   ├── Repositories/                 # 💾 Adatelérési réteg
│   │   ├── IRaceRepository.cs
│   │   └── RaceRepository.cs         # EF Core LINQ queries
│   │
│   ├── Data/
│   │   ├── AppDbContext.cs           # EF Core DbContext + seed
│   │   └── DatabaseSeeder.cs         # Szerepkörök és felhasználók seed
│   │
│   ├── Middleware/
│   │   └── JwtBlacklistMiddleware.cs # Token blacklist ellenőrzés
│   │
│   ├── Migrations/
│   │   ├── 20251117125614_InitialCreate.cs
│   │   ├── 20251117170945_AddAuditFields.cs
│   │   ├── 20251208103052_AddIdentity.cs
│   │   ├── 20260227100214_AddUltraBalatonTables.cs
│   │   ├── 20260328131132_AddTeamStartTime.cs
│   │   ├── 20260328131654_UpdateTeamStartTimeToDateTime.cs
│   │   ├── 20260410085812_AddWayPointTable.cs
│   │   ├── 20260410090229_AddWayPointsToSection.cs
│   │   └── 20260410123705_RemoveStartEndPointFromSection.cs
│   │
│   ├── Models/
│   │   ├── Race.cs                   # Domain model
│   │   ├── ApplicationUser.cs        # Identity user extension
│   │   ├── RaceSearchModel.cs        # DTO - keresési paraméterek
│   │   └── DTOs/
│   │       ├── LoginDto.cs
│   │       └── RegisterDto.cs
│   │
│   ├── Properties/
│   │   └── launchSettings.json       # Ports: 7156 (HTTPS), 5066 (HTTP)
│   │
│   ├── Program.cs                    # DI container + middleware + CORS
│   ├── appsettings.json              # JWT config + connection strings
│   └── RunningRacesApi.csproj
│
├── RunningRacesApi.Tests/            # 🧪 Unit & Integration Tests
│   ├── Controllers/
│   │   ├── RacesControllerTests.cs   # 11 teszt
│   │   └── AuthControllerTests.cs    # 3 teszt
│   ├── Services/
│   │   └── RaceServiceTests.cs       # 16 teszt
│   ├── Repositories/
│   │   └── RaceRepositoryTests.cs    # 18 teszt
│   └── RunningRacesApi.Tests.csproj
│
├── running-races-ui/                 # 🌐 Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   │   ├── race-list/        # Public + Admin lista
│   │   │   │   ├── race-form/        # Create + Edit form
│   │   │   │   ├── login/            # JWT bejelentkezés
│   │   │   │   ├── admin-header/     # Admin fejléc (logout)
│   │   │   │   ├── public-header/    # Public fejléc (login gomb)
│   │   │   │   ├── admin-layout/     # Admin oldalak layout wrappere
│   │   │   │   ├── public-layout/    # Public oldalak layout wrappere
│   │   │   │   └── delete-confirmation-dialog/
│   │   │   │
│   │   │   ├── services/
│   │   │   │   ├── auth.ts           # JWT kezelés + localStorage
│   │   │   │   └── race.ts           # HTTP API calls
│   │   │   │
│   │   │   ├── models/
│   │   │   │   ├── race.model.ts
│   │   │   │   └── race-search.model.ts
│   │   │   │
│   │   │   ├── guards/
│   │   │   │   └── admin.guard.ts    # CanActivate - JWT check
│   │   │   │
│   │   │   ├── interceptors/
│   │   │   │   └── auth.interceptor.ts  # Auto Bearer token
│   │   │   │
│   │   │   ├── app.ts                # Root component
│   │   │   ├── app.config.ts         # Providers
│   │   │   └── app.routes.ts         # Routing config
│   │   │
│   │   ├── main.ts
│   │   ├── index.html
│   │   ├── styles.css                # Global styles + Atma font
│   │   └── custom-theme.scss         # Material theme
│   │
│   ├── angular.json
│   ├── package.json
│   └── tsconfig.json
│
├── .gitignore
├── RunningRaces.sln
├── README.md                         # Ez a fájl
└── CHANGELOG.md                      # Verziókezelés
```

---

## 🏗️ Architektúra

### Backend - 3-Layer Pattern

```
┌─────────────────┐
│   Controller    │  ← HTTP Endpoints (ASP.NET Core)
└────────┬────────┘
         │
┌────────▼────────┐
│    Service      │  ← Business Logic (Validations, Rules)
└────────┬────────┘
         │
┌────────▼────────┐
│   Repository    │  ← Data Access (EF Core)
└────────┬────────┘
         │
┌────────▼────────┐
│   DbContext     │  ← Database (SQLite)
└─────────────────┘
```

### Frontend - Component-Based Architecture

```
┌──────────────────────────────────────────┐
│           Router                         │
├──────────────┬───────────────────────────┤
│              │                           │
│  /races      │  /admin/*                 │
│  (Public)    │  (Protected)              │
│              │                           │
│  ┌───────────▼───────────┐               │
│  │  RaceListComponent    │               │
│  │  ┌──────────────────┐ │               │
│  │  │ PublicHeader     │ │               │
│  │  │ AdminHeader      │ │               │
│  │  └──────────────────┘ │               │
│  └───────────────────────┘               │
│              │                           │
│  ┌───────────▼───────────┐               │
│  │  RaceService          │               │
│  │  AuthService          │               │
│  └───────────┬───────────┘               │
│              │                           │
│  ┌───────────▼───────────┐               │
│  │  HttpClient           │               │
│  │  (AuthInterceptor)    │               │
│  └───────────┬───────────┘               │
│              │                           │
│              ▼                           │
│      Backend API (JWT)                   │
└──────────────────────────────────────────┘
```

---

## 🔑 API Endpoints

### Base URL
- **Development:** `https://localhost:7156/api`
- **Swagger UI:** `https://localhost:7156/swagger`

### Public Endpoints (No Authentication)

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/auth/login` | Bejelentkezés → JWT token |
| `GET` | `/races/public` | **Csak aktív** versenyek (IsActive forced to true) |
| `GET` | `/races/{id}` | Egy verseny részletei |

### Protected Endpoints (JWT Required)

| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| `POST` | `/auth/logout` | Any | Kijelentkezés (token blacklist) |
| `GET` | `/races/admin` | Any | **Összes** verseny (IsActive opcionális) |
| `POST` | `/races` | Admin | Új verseny létrehozása |
| `PUT` | `/races/{id}` | Admin | Verseny módosítása |
| `DELETE` | `/races/{id}` | Admin | Verseny törlése (soft delete) |

**Authentication Header:**
```
Authorization: Bearer <JWT_TOKEN>
```

---

## 🔐 Autentikáció & Jogosultságkezelés

### Teszt Fiókok

| Szerepkör | Email | Jelszó | Jogosultságok |
|-----------|-------|--------|---------------|
| **Admin** | admin@runningraceandi.com | Admin123! | Teljes CRUD hozzáférés |
| **User** | test@runningraceandi.com | Test123! | Csak olvasási jog |

### Szerepkörök

- **Admin**: Versenyek létrehozása, módosítása, törlése + inaktív versenyek elérése
- **User**: Aktív versenyek megtekintése (authentikált)
- **Public**: Aktív versenyek listázása (névtelen)

### Biztonsági Funkciók

- ✅ ASP.NET Identity integráció
- ✅ Automatikus jelszó hashing
- ✅ JWT token alapú autentikáció
- ✅ Token blacklist (biztonságos logout)
- ✅ Szerepkör-alapú jogosultságkezelés
- ✅ Route guards (frontend védelem)

---

## 🔍 Keresés & Szűrés

### Query Parameters

```typescript
{
  searchTerm?: string;      // "budapest", "maraton", etc.
  searchField?: string;     // "name" | "location" | "all"
  sortBy?: string;          // "name" | "date" | "location" | "distance"
  sortDirection?: string;   // "asc" | "desc"
  isActive?: boolean;       // true | false | undefined (admin only)
}
```

### Példa Lekérdezések

```http
# Public - Aktív versenyek Budapesten, dátum szerint
GET /api/races/public?searchTerm=budapest&searchField=location&sortBy=date

# Admin - Inaktív versenyek
GET /api/races/admin?isActive=false

# Admin - Összes verseny név szerint rendezve
GET /api/races/admin?sortBy=name&sortDirection=desc
```

---

## 📊 Adatbázis

### Race Tábla

| Mező | Típus | Nullable | Leírás |
|------|-------|----------|--------|
| `Id` | `Guid` | ❌ | Primary Key |
| `Name` | `string` | ❌ | Verseny neve |
| `Date` | `DateTime` | ❌ | Verseny dátuma |
| `Location` | `string` | ❌ | Helyszín városa |
| `Distance` | `double` | ❌ | Távolság (km) |
| `IsActive` | `bool` | ❌ | Soft delete flag (default: true) |
| `CreatedAt` | `DateTime` | ❌ | Létrehozás időpontja (UTC) |
| `ModifiedAt` | `DateTime` | ✅ | Módosítás időpontja |

### Seed Data (4 verseny)

```
1. Budapest Marathon        - 2025.10.05 - 42.2 km
2. Balaton Supermarathon    - 2025.07.12 - 195 km
3. SPAR Budapest Half       - 2025.09.14 - 21.1 km
4. Telekom Vivicittá        - 2025.04.06 - 10 km
```

---

## 🛠️ Fejlesztés

### Előfeltételek

- **.NET 8 SDK** - [Letöltés](https://dotnet.microsoft.com/download)
- **Node.js 18+** - [Letöltés](https://nodejs.org/)
- **Angular CLI 20+** - `npm install -g @angular/cli`
- **Visual Studio 2022** vagy **VS Code**
- **Git**

### 🚀 Backend Indítás

```bash
# 1. Navigate to backend
cd RunningRacesApi

# 2. Restore packages
dotnet restore

# 3. Database migration (SQLite)
dotnet ef database update

# 4. Run API
dotnet run

# ✅ API elérhető: https://localhost:7156
# ✅ Swagger UI: https://localhost:7156/swagger
```

### 🌐 Frontend Indítás

```bash
# 1. Navigate to frontend
cd running-races-ui

# 2. Install dependencies
npm install

# 3. Start dev server
ng serve

# ✅ App elérhető: http://localhost:4200
```

### 🧪 Tesztek Futtatása

#### Backend Tests (48 teszt)
```bash
cd RunningRacesApi.Tests
dotnet test

# Verbose output
dotnet test --logger "console;verbosity=detailed"
```

**Teszt lefedettség:**
| Réteg | Tesztek | Státusz |
|-------|---------|---------|
| Repository | 18 | ✅ |
| Service | 16 | ✅ |
| Controller | 11 | ✅ |
| Auth | 3 | ✅ |

#### Frontend Tests
```bash
cd running-races-ui
ng test

# Single run (CI/CD)
ng test --watch=false --code-coverage
```

**Teszt lefedettség:**
| Réteg | Tesztek | Státusz |
|-------|---------|---------|
| AuthService | 6 | ✅ |
| Components | - | 🔄 Tervezett |

---

## 📝 Implementált Funkciók

### ✅ Backend
- 3-layer architektúra (Controller → Service → Repository)
- ASP.NET Identity felhasználókezelés
- JWT autentikáció token blacklist-tel
- Szerepkör-alapú jogosultságkezelés (Admin/User)
- CRUD műveletek versenyekhez
- Keresés, szűrés, rendezés
- Soft delete (IsActive flag)
- Public/Admin endpoint szétválasztás
- Swagger API dokumentáció
- Unit tesztek (48 teszt)

### ✅ Frontend
- Angular 20 Standalone Components
- JWT interceptor (auto Bearer token)
- Route guards (admin védelem)
- Szerepkör-alapú UI (feltételes megjelenítés)
- Material Dialog (törlés megerősítés)
- Reszponzív design
- AuthService tesztek

### 🔄 Tervezett
- E2E tesztek (Cypress/Playwright)
- CI/CD pipeline (GitHub Actions)
- Docker support
- Refresh tokens
- Email notifications
- Structured logging (Serilog)

---

## 🐛 Ismert Limitációk

### Fejlesztői Környezet
- ⚠️ JWT secret az appsettings.json-ben placeholder érték — **nyilvánosan elérhető szerverre telepítés előtt cseréld le saját, egyedi, min. 32 karakteres kulcsra** (env variable vagy user secrets ajánlott, ne kerüljön repóba)
- ⚠️ CORS nyitva localhost:4200-ra
- ⚠️ SQLite (production: SQL Server/PostgreSQL)

### Technikai Debt
- ❌ Nincs refresh token mechanizmus
- ❌ Nincs rate limiting
- ❌ Nincs Error Handling Middleware
- ❌ Frontend komponens tesztek hiányoznak
- ❌ JWT token `localStorage`-ban tárolva (XSS-nek kitett; HttpOnly cookie lenne biztonságosabb)

---

## 📚 Dokumentáció

- [CHANGELOG.md](CHANGELOG.md) - Verziókezelés
- [Swagger API Docs](https://localhost:7156/swagger) - API dokumentáció (futtatás közben)

---

## 👤 Szerző

**Kovács Andrea**
- GitHub: [@koand75](https://github.com/koand75)

---

## 📈 Statisztikák

| Metrika | Érték |
|---------|-------|
| Backend kód | ~8,000 sor (C#) |
| Frontend kód | ~2,500 sor (TypeScript/HTML/CSS) |
| Backend tesztek | 48 unit teszt |
| Frontend tesztek | 6 unit teszt |
| Angular komponensek | 8 standalone component |
| API végpontok | 8 endpoint |
| **Verzió** | **0.8.0** |
| **Utolsó frissítés** | **2026-03-29** |

---

## 📄 License

Ez a projekt jelenleg nincs nyílt forráskódú licenc alatt.
A kód megtekinthető, de nem használható fel, nem módosítható,
nem terjeszthető a szerző írásos engedélye nélkül.

© 2026 Kovács Andrea — All Rights Reserved

---

**⭐ Ha tetszik a projekt, adj egy star-ot a GitHub-on!** ⭐
