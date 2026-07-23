# Visual Studio - Test Projekt Létrehozás
## Végleges projekt nevek

## 📂 Struktúra

```
RunningRaces/                           ← Solution mappa
├── RunningRaces.sln                    ← Solution file
├── RunningRacesApi/                    ← API projekt
└── RunningRacesApi.Tests/              ← Test projekt (ide kerül!)
```

---

## 🔧 Lépések Visual Studio-ban

### 1. Test projekt létrehozása

**Solution Explorer → Jobb klikk "Solution 'RunningRaces'"**
```
Add → New Project...
```

### 2. Template választás

**Keresés:**
```
xUnit Test Project
```

**Válaszd ki:**
```
✅ xUnit Test Project (.NET Core)
   C#
```

**Next →**

### 3. Projekt konfiguráció

```
Project name:     RunningRacesApi.Tests
Location:         C:\Users\YourName\source\repos\RunningRaces\
                  ↑ Solution gyökér! (NE az Api alatt!)
```

**⚠️ KRITIKUS:** A Location mezőben **csak a solution gyökér** legyen!
```
✅ HELYES:  C:\...\RunningRaces\
❌ ROSSZ:   C:\...\RunningRaces\RunningRacesApi\
```

**Solution:**
```
🔘 Add to solution  ← Ez legyen bepipálva!
```

**Create →**

---

### 4. Project Reference hozzáadása

**Solution Explorer → RunningRacesApi.Tests**
```
Jobb klikk "Dependencies"
→ Add Project Reference...
```

**Reference Manager ablakban:**
```
✅ RunningRacesApi    ← Pipáld be!
```

**OK**

---

### 5. NuGet csomagok telepítése

**Option A - Package Manager Console:**

```
View → Other Windows → Package Manager Console
```

**Futtasd:**
```powershell
# Váltsd át a projektet:
Default project: RunningRacesApi.Tests  ← Dropdown

# Telepítsd a csomagokat:
Install-Package Moq
Install-Package FluentAssertions
Install-Package Microsoft.EntityFrameworkCore.InMemory
```

---

**Option B - NuGet Package Manager UI:**

```
Tools → NuGet Package Manager → Manage NuGet Packages for Solution...
```

**Browse tab:**
1. Keress rá: `Moq`
   - Válaszd ki: **RunningRacesApi.Tests** projekt
   - **Install**

2. Keress rá: `FluentAssertions`
   - Válaszd ki: **RunningRacesApi.Tests** projekt
   - **Install**

3. Keress rá: `Microsoft.EntityFrameworkCore.InMemory`
   - Válaszd ki: **RunningRacesApi.Tests** projekt
   - **Install**

---

### 6. Ellenőrzés

**Solution Explorer struktúra:**

```
Solution 'RunningRaces' (2 of 2 projects)
├── RunningRacesApi
│   ├── Dependencies
│   ├── Controllers
│   ├── Models
│   ├── Services
│   └── Program.cs
└── RunningRacesApi.Tests                    ← Ezen a szinten!
    ├── Dependencies
    │   ├── Projects
    │   │   └── RunningRacesApi              ← Referencia OK!
    │   └── Packages
    │       ├── FluentAssertions
    │       ├── Moq
    │       └── xunit
    └── UnitTest1.cs
```

**⚠️ Ha a RunningRacesApi.Tests az API alatt van → ROSSZ hely!**

---

### 7. Fájl rendszer ellenőrzés

**File Explorer:**
```
C:\...\RunningRaces\
├── RunningRaces.sln
├── RunningRacesApi\
│   └── RunningRacesApi.csproj
└── RunningRacesApi.Tests\                   ← Itt kell lennie!
    └── RunningRacesApi.Tests.csproj
```

**NE legyen itt:**
```
❌ C:\...\RunningRaces\RunningRacesApi\RunningRacesApi.Tests\
```

---

### 8. Build & Test

**Build:**
```
Build → Rebuild Solution
(Ctrl + Shift + B)
```

**Test Explorer:**
```
Test → Test Explorer
(Ctrl + E, T)
```

**Tesztek futtatása:**
```
Test → Run All Tests
(Ctrl + R, A)
```

---

## ❌ Ha rossz helyre került

### Gyors javítás:

1. **Solution Explorer → RunningRacesApi.Tests**
   - Jobb klikk → **Remove** (vagy **Delete**)

2. **File Explorer-ben töröld:**
   ```
   C:\...\RunningRaces\RunningRacesApi\RunningRacesApi.Tests\  ← Töröld!
   ```

3. **Kezdd újra a "Test projekt létrehozása" lépéstől**
   - Ügyelj a **Location** mezőre!

---

## ✅ Sikeres telepítés jele

**Solution Explorer:**
- 2 projekt látható
- RunningRacesApi.Tests **nem** az Api alatt van
- Dependencies → Projects → RunningRacesApi referencia OK

**Test Explorer:**
- Legalább 1 teszt látható (UnitTest1)

**Build output:**
```
Build succeeded.
    2 Project(s) succeeded
```

---

## 🎯 Következő lépés

Másold be a teszt fájlokat:
- Services/RaceServiceTests.cs
- Helpers/TestDataGenerator.cs

És töröld a generált:
- UnitTest1.cs