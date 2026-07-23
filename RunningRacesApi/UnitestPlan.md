# Backend Unit Test Project Setup
## Projekt nevek:
- Solution mappa: RunningRaces
- API projekt: RunningRacesApi
- Test projekt: RunningRacesApi.Tests

## 1. Test projekt létrehozása

```bash
# Navigálj a solution mappába
cd RunningRaces

# xUnit test projekt létrehozása
dotnet new xunit -n RunningRacesApi.Tests

# Projekt hozzáadása a solution-höz
dotnet sln add RunningRacesApi.Tests/RunningRacesApi.Tests.csproj

# Referencia hozzáadása az API projekthez
cd RunningRacesApi.Tests
dotnet add reference ../RunningRacesApi/RunningRacesApi.csproj
```

## 2. NuGet csomagok telepítése

```bash
cd RunningRacesApi.Tests

# Moq - Mocking framework
dotnet add package Moq

# FluentAssertions - Olvashatóbb assertek
dotnet add package FluentAssertions

# Microsoft.EntityFrameworkCore.InMemory - InMemory DB tesztekhez
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

## 3. Projekt struktúra

```
RunningRaces/                           ← Solution mappa
├── RunningRaces.sln
├── RunningRacesApi/                    ← API projekt
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Repositories/
│   └── RunningRacesApi.csproj
└── RunningRacesApi.Tests/              ← Test projekt
    ├── Services/
    │   ├── RaceServiceTests.cs
    │   └── AuthServiceTests.cs
    ├── Repositories/
    │   └── RaceRepositoryTests.cs
    ├── Controllers/
    │   └── RaceControllerTests.cs
    ├── Helpers/
    │   └── TestDataGenerator.cs
    └── RunningRacesApi.Tests.csproj
```

## 4. Visual Studio lépések

### Test projekt létrehozása VS-ben:

1. **Solution Explorer → Jobb klikk a Solution-re**
   ```
   Add → New Project...
   ```

2. **Template választás:**
   ```
   xUnit Test Project (.NET Core)
   ```

3. **Projekt beállítások:**
   ```
   Project name:     RunningRacesApi.Tests
   Location:         C:\...\RunningRaces\  ← Solution gyökér!
   Solution:         Add to solution ✅
   ```

4. **Reference hozzáadása:**
   ```
   Solution Explorer → RunningRacesApi.Tests
   → Jobb klikk "Dependencies"
   → Add Project Reference...
   → Pipáld be: RunningRacesApi ✅
   → OK
   ```

5. **NuGet csomagok (Package Manager Console):**
   ```powershell
   Install-Package Moq
   Install-Package FluentAssertions
   Install-Package Microsoft.EntityFrameworkCore.InMemory
   ```

## 5. Namespace-ek

```csharp
// Test fájlokban:
using RunningRacesApi.Models;
using RunningRacesApi.Services;
using RunningRacesApi.Repositories;

namespace RunningRacesApi.Tests.Services;
namespace RunningRacesApi.Tests.Repositories;
namespace RunningRacesApi.Tests.Controllers;
namespace RunningRacesApi.Tests.Helpers;
```

## 6. Build és futtatás

```bash
# Build
dotnet build

# Összes teszt futtatása
dotnet test

# Verbose output
dotnet test --logger "console;verbosity=detailed"

# Code coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 7. Solution ellenőrzés

```bash
# Solution struktúra listázása
dotnet sln list

# Várható kimenet:
# Project(s)
# ----------
# RunningRacesApi/RunningRacesApi.csproj
# RunningRacesApi.Tests/RunningRacesApi.Tests.csproj
```