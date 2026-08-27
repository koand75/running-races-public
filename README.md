# ?? Running Races - Management Application

Full-stack running race management application with modern technologies. ASP.NET Core backend with JWT authentication + Angular 20 standalone frontend.

---

## ?? Tech Stack

### Backend
- **ASP.NET Core 8.0** - Web API
- **Entity Framework Core 8.0** - ORM
- **ASP.NET Identity** - User management & password hashing
- **SQLite** - Database (Development)
- **JWT Bearer Authentication** - Security
- **Swagger/OpenAPI** - API documentation
- **xUnit 2.6.5** - Testing framework
- **Moq 4.20.70** - Mocking library
- **FluentAssertions 6.12.0** - Readable assertions

### Frontend
- **Angular 20** - SPA Framework (Standalone Components)
- **TypeScript 5.9** - Type safety
- **RxJS 7.8** - Reactive programming
- **Angular Material 20** - UI Components
- **Angular CDK** - Drag & drop
- **Leaflet** - Interactive maps
- **Angular Router** - Routing + Guards
- **jwt-decode** - Token parsing
- **Atma Font** - Custom typography

---

## ?? Project Structure

```
RunningRaces/
©À©¤©¤ RunningRacesApi/                  # ?? Backend API
©¦   ©À©¤©¤ Controllers/
©¦   ©¦   ©À©¤©¤ RacesController.cs
©¦   ©¦   ©À©¤©¤ AuthController.cs
©¦   ©¦   ©À©¤©¤ TeamController.cs
©¦   ©¦   ©À©¤©¤ RunnerController.cs
©¦   ©¦   ©À©¤©¤ SectionController.cs
©¦   ©¦   ©À©¤©¤ SectionImportController.cs
©¦   ©¦   ©À©¤©¤ SectionExportController.cs
©¦   ©¦   ©À©¤©¤ RunnerSectionController.cs
©¦   ©¦   ©¸©¤©¤ WayPointController.cs
©¦   ©¦
©¦   ©À©¤©¤ Services/                     # Business logic layer
©¦   ©À©¤©¤ Repositories/                 # Data access layer
©¦   ©À©¤©¤ Models/                       # Domain models + DTOs
©¦   ©À©¤©¤ Data/                         # DbContext + Seeder
©¦   ©À©¤©¤ Middleware/                   # JWT blacklist
©¦   ©À©¤©¤ Helpers/                      # CSV export
©¦   ©¸©¤©¤ Enums/                        # WayPointMatchStatus
©¦
©À©¤©¤ RunningRacesApi.Tests/            # ?? Unit Tests
©¦
©À©¤©¤ running-races-ui/                 # ?? Angular Frontend
©¦   ©À©¤©¤ src/app/
©¦   ©¦   ©À©¤©¤ components/               # Shared components
©¦   ©¦   ©À©¤©¤ features/
©¦   ©¦   ©¦   ©¸©¤©¤ ub/                   # UltraBalaton module
©¦   ©¦   ©¦       ©À©¤©¤ components/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ section-list/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ section-import/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ section-import-dialog/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ team-list/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ team-detail/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ team-edit/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ planner/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ waypoints/
©¦   ©¦   ©¦       ©¦   ©À©¤©¤ map/
©¦   ©¦   ©¦       ©¦   ©¸©¤©¤ team-map/
©¦   ©¦   ©¦       ©À©¤©¤ services/
©¦   ©¦   ©¦       ©¸©¤©¤ models/
©¦   ©¦   ©À©¤©¤ guards/
©¦   ©¦   ©À©¤©¤ interceptors/
©¦   ©¦   ©¸©¤©¤ services/
©¦   ©À©¤©¤ public/
©¦   ©¦   ©¸©¤©¤ samples/
©¦   ©¦       ©¸©¤©¤ sampleSections.csv    # Sample import file
©¦   ©¸©¤©¤ src/environments/             # API URL configuration
©¦
©À©¤©¤ Dockerfile                        # Backend Docker
©À©¤©¤ docker-compose.yml                # Full stack orchestration
©À©¤©¤ .gitignore
©À©¤©¤ RunningRaces.sln
©À©¤©¤ CHANGELOG.md
©¸©¤©¤ CHANGELOG_HU.md
```

---

## ??? Architecture

### Backend - 3-Layer Pattern

```
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦   Controller    ©¦  ¡û HTTP Endpoints
©¸©¤©¤©¤©¤©¤©¤©¤©¤©Ð©¤©¤©¤©¤©¤©¤©¤©¤©¼
         ©¦
©°©¤©¤©¤©¤©¤©¤©¤©¤¨‹©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦    Service      ©¦  ¡û Business Logic
©¸©¤©¤©¤©¤©¤©¤©¤©¤©Ð©¤©¤©¤©¤©¤©¤©¤©¤©¼
         ©¦
©°©¤©¤©¤©¤©¤©¤©¤©¤¨‹©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦   Repository    ©¦  ¡û Data Access (EF Core)
©¸©¤©¤©¤©¤©¤©¤©¤©¤©Ð©¤©¤©¤©¤©¤©¤©¤©¤©¼
         ©¦
©°©¤©¤©¤©¤©¤©¤©¤©¤¨‹©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦   DbContext     ©¦  ¡û SQLite Database
©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
```

---

## ?? API Endpoints

### Base URL
- **Development:** `https://localhost:7156/api`
- **Swagger UI:** `https://localhost:7156/swagger`

### Race Endpoints

| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| `GET` | `/races/public` | Public | Active races only |
| `GET` | `/races/admin` | Any | All races |
| `POST` | `/races` | Admin | Create race |
| `PUT` | `/races/{id}` | Admin | Update race |
| `DELETE` | `/races/{id}` | Admin | Soft delete |
| `PATCH` | `/races/{id}/restore` | Admin | Restore deleted race |

### UltraBalaton Endpoints

| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| `GET` | `/section` | Public | All sections with waypoints |
| `POST` | `/section-import/preview` | Admin | CSV preview with waypoint matching |
| `POST` | `/section-import` | Admin | Import sections from DTO list |
| `GET` | `/section-export` | Auth | Export sections as CSV |
| `GET` | `/waypoint` | Public | All waypoints |
| `POST` | `/waypoint` | Admin | Create waypoint |
| `PUT` | `/waypoint/{id}` | Admin | Update waypoint |
| `DELETE` | `/waypoint/{id}` | Admin | Delete waypoint (if not in use) |
| `GET` | `/team` | Auth | All teams |
| `GET` | `/runner` | Auth | Runners by team |
| `PUT` | `/team/{id}/assignments` | Auth | Save planner assignments |

---

## ?? Authentication & Authorization

### Test Accounts

| Role | Email | Password | Permissions |
|------|-------|----------|-------------|
| **Admin** | admin@runningraceandi.com | Admin123! | Full CRUD access |
| **User** | test@runningraceandi.com | Test123! | Read-only |

### Security Features

- ? ASP.NET Identity integration
- ? Automatic password hashing
- ? JWT token authentication
- ? Token blacklist (secure logout)
- ? Role-based authorization
- ? Route guards (frontend protection)
- ? Auth interceptor (auto Bearer token)

---

## ??? UltraBalaton Module

The UB module manages team-based ultra marathon planning:

- **Sections** ¨C 58 race sections with waypoints and distances
- **Waypoints** ¨C GPS coordinates for each transition point
- **Teams** ¨C Teams with start time
- **Runners** ¨C Team members with base pace
- **Planner** ¨C Drag & drop runner-section assignment
- **Map** ¨C Interactive Leaflet map with section routes
- **Team Map** ¨C Runner assignments visualized per team
- **Import/Export** ¨C CSV import with waypoint matching preview

---

## ??? Development Setup

### Prerequisites

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Node.js 20+** - [Download](https://nodejs.org/)
- **Angular CLI 20+** - `npm install -g @angular/cli`
- **Docker** (optional) - [Download](https://www.docker.com/)

### Backend Setup

```bash
cd RunningRacesApi
dotnet restore
dotnet ef database update
dotnet run
# API: https://localhost:7156
# Swagger: https://localhost:7156/swagger
```

### Frontend Setup

```bash
cd running-races-ui
npm install
ng serve
# App: http://localhost:4200
```

### Docker Setup

```bash
# Build and start all services
docker-compose up --build

# Backend: http://localhost:7156
# Frontend: http://localhost:4200
```

---

## ?? Tests

### Backend Tests

```bash
cd RunningRacesApi.Tests
dotnet test
```

| Layer | Tests | Status |
|-------|-------|--------|
| Repository | 18 | ? |
| Service | 16 | ? |
| Controller | 17 | ? |
| Auth | 3 | ? |

### Frontend Tests

```bash
cd running-races-ui
ng test --watch=false
```

| Layer | Tests | Status |
|-------|-------|--------|
| Services | 30+ | ? |
| Components | 60+ | ? |
| Guards | 4 | ? |

---

## ? Implemented Features

### Backend
- 3-layer architecture (Controller ¡ú Service ¡ú Repository)
- ASP.NET Identity user management
- JWT authentication with token blacklist
- Role-based authorization (Admin/User)
- CRUD for races, sections, teams, runners, waypoints
- CSV import with waypoint matching (Exact/Partial/NotFound)
- CSV export for sections
- Generic `CsvExportService`
- `BaseSearchModel` for shared pagination
- Soft delete + restore for races
- Docker support

### Frontend
- Angular 20 Standalone Components
- UltraBalaton module (sections, teams, planner, map)
- Interactive Leaflet maps (global + team-specific)
- Drag & drop planner with block swapping
- Section import with two-step preview flow
- CSV export with optional ID
- Environment-based API URL configuration
- MatSidenav hamburger menu
- Role-based UI
- 90+ unit tests

---

## ?? Known Limitations

- JWT secret is a placeholder ¡ª **replace before production deployment**
- CORS open to localhost:4200
- SQLite (use PostgreSQL/SQL Server for production)
- JWT stored in localStorage (XSS risk; HttpOnly cookie recommended)
- No refresh token mechanism
- No rate limiting

---

## ?? Documentation

- [CHANGELOG.md](CHANGELOG.md) - Version history (English)
- [CHANGELOG_HU.md](CHANGELOG_HU.md) - Version history (Hungarian)
- [Swagger](https://localhost:7156/swagger) - API docs (while running)

---

## ?? Author

**Kov¨¢cs Andrea**
- GitHub: [@koand75](https://github.com/koand75)

---

## ?? License

This project is not under an open source license.
The code is viewable but may not be used, modified, or distributed
without written permission from the author.

? 2026 Kov¨¢cs Andrea ¡ª All Rights Reserved

---

| Metric | Value |
|--------|-------|
| Backend code | ~12,000 lines (C#) |
| Frontend code | ~6,000 lines (TypeScript/HTML/CSS) |
| Backend tests | 54 unit tests |
| Frontend tests | 90+ unit tests |
| Angular components | 20+ standalone components |
| API endpoints | 20+ endpoints |
| **Version** | **0.9.0** |
| **Last updated** | **2026-08-27** |