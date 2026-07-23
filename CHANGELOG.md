# Changelog

All notable changes to this project are documented in this file.

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
this project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Planned - Backend
- Refresh Token mechanism
- Error Handling Middleware
- Structured Logging (Serilog)
- Rate Limiting (Anti-brute force)
- API Versioning
- Health Checks endpoint
- Docker support (Dockerfile + docker-compose)

### Planned - Frontend
- Component unit tests (Jasmine)
- E2E tests (Cypress/Playwright)
- Toast notifications (success/error messages)
- Loading spinners
- Form validation improvements
- Accessibility (a11y) improvements
- Internationalization (i18n - multi-language)
- Progressive Web App (PWA) support

### Planned - DevOps
- CI/CD pipeline (GitHub Actions)
- Code coverage reports (Codecov)
- Automated testing on PR
- Deployment pipeline

---

## [0.8.1] - 2026-07-21

### Added
- Race restore functionality (restore from soft delete)

### Changed
- Replaced `DeleteConfirmationDialog` with generic `ConfirmationDialogComponent` across all delete actions
- Warning message when deleting a runner that planner assignments will also be removed

## [0.8.0] - 2026-07-21

### Added - Backend
- **Ultra Balaton (UB) module** - new functional unit
  - `Team` entity (teams with start time)
  - `Runner` entity (runners with base pace)
  - `Section` entity (race sections)
  - `WayPoint` entity (waypoints with coordinates)
  - `RunnerSection` entity (runner-section assignments with custom pace)
  - `TeamController`, `RunnerController`, `SectionController`, `RunnerSectionController`
  - `SectionImportController` - bulk section import
  - `WayPointController` full CRUD + delete protection for waypoints in use
- **Database migrations**
  - `AddUltraBalatonTables` - Team/Runner/Section/WayPoint/RunnerSection tables
  - `AddTeamStartTime`, `UpdateTeamStartTimeToDateTime` - team start time field
  - `AddWayPointTable`, `AddWayPointsToSection` - waypoints
  - `MakeWayPointCoordinatesNullable`, `RemoveStartEndPointFromSection` - schema cleanup

### Added - Frontend
- **UB navigation restructure**
  - MatSidenav hamburger menu in public and admin layouts
  - Submenu: UltraBalaton → Sections, Teams
  - Login/Logout in sidenav
  - User icon in public header (with admin indicator)
- **Waypoints page** (`/ub/waypoints`) - list, edit, create, delete
- **authGuard** on UB routes
- **isAdminMode** fixed - based on route + role
- Password visibility icon replaced
- Back buttons standardized (`mat-stroked-button`)
- Role-based UI in UB module (section-list, team-list, waypoints) - admin-only buttons hidden

### Changed - Frontend
- **UI redesign** on existing components
  - `admin-header`, `public-header`, `login` - style and layout updates
  - `race-list`, `race-form` - visual improvements
  - `delete-confirmation-dialog` - style consistency

### Added - Tests (Frontend)
- `WaypointDialog` - create, save, cancel, prefill tests
- `Waypoints` - create, load, delete tests
- `WayPoint` service - CRUD HTTP tests
- `SectionInsertDialog` - create, save, cancel tests
- `DeleteConfirmationDialog` - create, confirm, cancel tests
- `Login` - create, form validation, submit, error handling tests
- `RaceList` - create, load, sort, clear tests
- `RaceForm` - create, form validation, submit tests
- `PublicHeader` - create, menu toggle, logout tests
- `AdminHeader` - create, menu toggle, logout tests
- `AdminLayout` - create, menu toggle, logout tests
- `authGuard` - allow/deny tests
- `adminGuard` - allow/deny tests
- `Planner` - create, load, cumulative km, block swap, assignment tests
- `SectionList` - create, load, delete, full distance tests

### Added - Tests (Backend)
- `WayPointControllerTests` - GetAll, Create, Update, Delete, Delete in use tests

---

## [0.7.0] - 2025-12-11

### Added - Backend
- **Server-side pagination**
  - `PagedResult<T>` wrapper class (Items, TotalCount, Page, PageSize)
  - `RaceSearchModel` extended with Page and PageSize parameters
  - Repository, Service, Controller updated for paginated responses

### Added - Frontend
- **MatPaginator integration**
  - `PagedResult<T>` interface
  - Paginator component for race list
  - Page size selector (5, 10, 20, 50)
  - Paginator styling

### Fixed
- CSS layout cleanup (global container style)
- AuthController test - GetRolesAsync mock added

### Technical
- Angular Material Paginator
- Backend pagination: Skip/Take + CountAsync

---

## [0.6.1] - 2025-12-11

### Refactored
- **Layout components introduced**
  - `PublicLayoutComponent` - public header + router-outlet
  - `AdminLayoutComponent` - admin header + router-outlet
  - Nested routing for admin routes
- **RaceListComponent simplified**
  - Header logic removed (handled by layout)
  - `updateAdminMode()` method removed
- **RaceFormComponent simplified**
  - Header logic removed
  - `isAdminMode` property removed
  - `onCancel()` uses AuthService instead of localStorage
- **TokenBlacklistService** - Scoped → Singleton

### Fixed
- Login hint shows both test accounts (Admin + User)

### UI/UX
- Header added to race form page
- Responsive mobile view (race-list, race-form)
- Color fine-tuning (admin header gradient)
- Warmer list background

---

## [0.6.0] - 2025-12-11

### Added - Backend
- **Role-based authorization**
  - Admin and User roles in ASP.NET Identity
  - DatabaseSeeder creates roles automatically
  - Role information in JWT token
  - `[Authorize(Roles = "Admin")]` on Create/Update/Delete endpoints
  - Only Admin users can modify races
  - Regular users have read-only access

### Added - Frontend
- **Role-based UI**
  - jwt-decode library for token parsing
  - `getUserRole()` and `isAdmin()` methods in AuthService
  - Conditional display: Admin buttons only for Admin role
  - Material Icons integration (login/logout icons)
  - Dynamic Login/Logout button in public header
  - Logout in admin header

### Changed - Backend
- JWT token generation async (`GenerateJwtTokenAsync`)
- Role information automatically added from UserManager

### Changed - Frontend
- After logout navigates to `/races` (public) instead of `/login`
- Race form cancel button context-dependent routing (admin vs public)
- Material Icons used without Material Buttons (preserving custom styling)

### Security
- Admin role required for all CRUD operations on races
- User role has read-only access
- JWT tokens contain role information for client-side decisions

### Database
- Admin user: admin@runningraceandi.com / Admin123!
- Test user: test@runningraceandi.com / Test123!
- Roles automatically seeded on startup
- Existing users updated with appropriate roles

### Technical
- jwt-decode: ^4.0.0 (npm package)
- Material Icons font integration
- Role detection via JWT claims

---

## [0.5.0] - 2025-12-08

### Added - Backend
- **ASP.NET Identity integration**
  - ApplicationUser model (IdentityUser extension)
  - IdentityDbContext usage
  - Automatic password hashing
  - User validation (unique email, password requirements)
- **Token Blacklist system**
  - ITokenBlacklistService interface
  - TokenBlacklistService (MemoryCache based)
  - JwtBlacklistMiddleware for token validation
  - Secure logout (token invalidation)
- **DatabaseSeeder**
  - Automatic role creation (Admin, User)
  - Seed users created on startup
  - Existing users updated with roles

### Added - Backend Testing
- **AuthControllerTests** (3 tests)
  - Login with valid credentials
  - Login with invalid credentials (Theory)
  - Logout with valid token

### Changed - Backend
- AuthController rewritten with ASP.NET Identity
- Program.cs: Identity and MemoryCache configuration
- JWT token includes user roles

### Database Migration
- `20251208103052_AddIdentity.cs` - Identity tables added

---

## [0.4.0] - 2025-11-20

### Added - Backend Testing
- **RaceServiceTests** (16 tests)
  - GetPublicRacesAsync - IsActive always forced to true
  - GetAdminRacesAsync - IsActive not modified
  - CreateRaceAsync - validation tests (name, location, distance)
  - CreateRaceAsync - default values (Id, CreatedAt, IsActive)
  - UpdateRaceAsync - validation and existence checks
  - DeleteRaceAsync - repository delegation
  - GetRaceByIdAsync - empty Guid validation
- **RacesControllerTests** (11 tests)
  - GetRaceById - valid/invalid ID scenarios
  - GetPublicRaces - returns OK with races
  - CreateRace - returns CreatedAtAction with location header
  - UpdateRace - returns OK with updated race
  - DeleteRace - returns NoContent/NotFound

### Added - Frontend Testing
- **AuthService tests** (6 tests)
  - Login successfully stores token
  - Logout clears token
  - getToken returns token/null
  - isAuthenticated returns true/false

### Technical
- Moq for service and repository mocking
- FluentAssertions for all assertions
- AAA pattern (Arrange-Act-Assert) used consistently

---

## [0.3.0] - 2025-11-17

### Added - Backend

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
  - Admin endpoint: IsActive optional (full control)
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

### Changed - Backend
- **RacesController** refactored
  - Dependency changed: `IRaceRepository` → `IRaceService`
  - Controller now delegates to Service layer
  - Exception handling improved (ArgumentException, InvalidOperationException)

### Added - Frontend

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

### Fixed
- IsActive filtering logic (moved from Repository to Service)
- Public endpoint security (no way to access inactive races)

---

## [0.2.0] - 2025-11-17

### Added - Backend Testing

#### Repository Test Coverage (18 tests)
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

### Database Migration
- `20251117170945_AddAuditFields.cs`
  - Added `CreatedAt` (DateTime, required)
  - Added `IsActive` (bool, default true)
  - Added `ModifiedAt` (DateTime?, nullable)

---

## [0.1.0] - 2025-11-17

### Added - Backend Architecture

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

### Added - Frontend
- Angular 20 Standalone Components
- RaceListComponent (public + admin mode)
- RaceFormComponent (create + edit)
- LoginComponent
- AuthService (JWT handling)
- RaceService (HTTP calls)
- Admin guard
- Auth interceptor

### Added - Testing Setup
- RunningRacesApi.Tests project
- xUnit framework
- Moq for mocking
- FluentAssertions
- InMemory database testing

### Added - Git Version Control
- GitHub repository setup
- .gitignore configured

### Technical Stack
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQLite database
- JWT Bearer Authentication
- Swagger/OpenAPI documentation
- Angular 20 (Standalone Components)

### Database
- Initial Migration: `20251117125614_InitialCreate.cs`
- Race table with 4 seed records

---

## Versioning

This project follows **Semantic Versioning (SemVer)**:

### Format: MAJOR.MINOR.PATCH

- **MAJOR** (1.x.x) - Breaking changes
- **MINOR** (x.1.x) - New features (backward compatible)
- **PATCH** (x.x.1) - Bug fixes

### Pre-release (0.x.x)
- Unstable API
- Breaking changes may occur at any time
- **NOT recommended** for production use

---

## Current Development Phase

🚧 **Pre-release (0.8.0)** 🚧

**Done:**
- ✅ Backend architecture (3-layer)
- ✅ ASP.NET Identity
- ✅ JWT authentication + token blacklist
- ✅ Role-based authorization
- ✅ Backend tests (48 tests)
- ✅ Frontend core features
- ✅ AuthService tests (6 tests)
- ✅ Server-side pagination
- ✅ Ultra Balaton module (Team/Runner/Section/WayPoint)
- ✅ Frontend component tests
- ✅ UB navigation restructure + waypoints page

**Next goals:**
1. E2E tests
2. CI/CD pipeline
3. Docker support
4. Map view (Leaflet)

---

**Last Updated:** 2026-07-21  
**Maintained by:** Kovács Andrea ([@koand75](https://github.com/koand75))
