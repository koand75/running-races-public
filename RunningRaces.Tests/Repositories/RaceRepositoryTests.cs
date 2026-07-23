using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;
using RunningRacesApi.Repositories;

namespace RunningRacesApi.Tests.Repositories;

/// <summary>
/// RaceRepository integration tesztek (InMemory DB-vel)
/// </summary>
public class RaceRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly RaceRepository _repository;

    public RaceRepositoryTests()
    {
        // InMemory database létrehozása minden teszthez
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Egyedi DB név
            .Options;

        _context = new AppDbContext(options);
        _repository = new RaceRepository(_context);

        // Seed adatok
        SeedTestData();
    }

    [Fact]
    public async Task GetRacesAsync_WithoutIsActiveFilter_ReturnsAllRaces()
    {
        // Arrange
        var searchModel = new RaceSearchModel();  // IsActive = null

        // Act
        var result = await _repository.GetRacesAsync(searchModel);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3); // Minden verseny
        result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetRacesAsync_WithIsActiveTrue_ReturnsOnlyActiveRaces()
    {
        // Arrange
        var searchModel = new RaceSearchModel { IsActive = true };

        // Act
        var result = await _repository.GetRacesAsync(searchModel);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2); // Csak aktívak
        result.TotalCount.Should().Be(2);
        result.Items.All(r => r.IsActive).Should().BeTrue();
    }

    [Fact]
    public async Task GetRacesAsync_WithIsActiveFalse_ReturnsOnlyInactiveRaces()
    {
        // Arrange
        var searchModel = new RaceSearchModel { IsActive = false };

        // Act
        var result = await _repository.GetRacesAsync(searchModel);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1); // Csak inaktívak
        result.Items.All(r => !r.IsActive).Should().BeTrue();
        result.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsRace()
    {
        // Arrange
        var races = _context.Races.ToList();
        var firstRace = races.First();

        // Act
        var result = await _repository.GetByIdAsync(firstRace.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(firstRace.Id);
        result.Name.Should().Be(firstRace.Name);
        result.Location.Should().Be(firstRace.Location);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var invalidId = Guid.NewGuid(); // Nem létező ID

        // Act
        var result = await _repository.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidRace_CreatesRaceInDatabase()
    {
        // Arrange
        var newRace = new Race
        {
            Id = Guid.NewGuid(),
            Name = "Debrecen Maraton",
            Date = new DateTime(2025, 11, 15),
            Location = "Debrecen",
            Distance = 42.195,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _repository.CreateAsync(newRace);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(newRace.Id);
        result.Name.Should().Be(newRace.Name);

        // Ellenőrizzük hogy tényleg bekerült a DB-be
        var fromDb = await _context.Races.FindAsync(newRace.Id);
        fromDb.Should().NotBeNull();
        fromDb!.Name.Should().Be("Debrecen Maraton");
    }

    [Theory]
    [InlineData("Budapest Maraton", "Budapest", 42.195)]
    [InlineData("Balaton Félmaraton", "Balatonfüred", 21.1)]
    [InlineData("Szeged 10K", "Szeged", 10)]
    [InlineData("Ultra Trail", "Visegrád", 100)]
    public async Task CreateAsync_WithVariousValidData_CreatesRaceSuccessfully(
         string name,
         string location,
         double distance)
    {
        // Arrange
        var newRace = new Race
        {
            Id = Guid.NewGuid(),
            Name = name,
            Location = location,
            Distance = distance,
            Date = DateTime.Today.AddMonths(1),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _repository.CreateAsync(newRace);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Location.Should().Be(location);
        result.Distance.Should().Be(distance);

        // DB ellenőrzés
        var fromDb = await _context.Races.FindAsync(result.Id);
        fromDb.Should().NotBeNull();
        fromDb!.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("")]           // Üres név
    [InlineData("   ")]        // Csak whitespace
    [InlineData(null)]         // Null
    public async Task CreateAsync_WithInvalidName_ThrowsException(string? invalidName)
    {
        // Arrange
        var race = new Race
        {
            Id = Guid.NewGuid(),
            Name = invalidName!,
            Date = DateTime.Today,
            Location = "Test",
            Distance = 10,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        var act = async () => await _repository.CreateAsync(race);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*name*");
    }

    [Theory]
    [InlineData(0.1)]       // Minimum távolság
    [InlineData(5)]         // Rövid futás
    [InlineData(42.195)]    // Maraton
    [InlineData(100)]       // Ultra
    [InlineData(250)]       // Spartathlon
    public async Task CreateAsync_WithVariousDistances_CreatesRace(double distance)
    {
        // Arrange
        var race = new Race
        {
            Id = Guid.NewGuid(),
            Name = $"Test Race {distance}km",
            Date = DateTime.Today.AddMonths(1),
            Location = "Test",
            Distance = distance,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _repository.CreateAsync(race);

        // Assert
        result.Distance.Should().Be(distance);

        var fromDb = await _context.Races.FindAsync(result.Id);
        fromDb!.Distance.Should().Be(distance);
    }

    [Theory]
    [InlineData(-1)]        // Negatív
    [InlineData(-100)]      // Nagy negatív
    [InlineData(0)]         // Nulla (lehet érvényes?)
    public async Task CreateAsync_WithNegativeDistance_ThrowsException(double invalidDistance)
    {
        // Arrange
        var race = new Race
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Date = DateTime.Today,
            Location = "Test",
            Distance = invalidDistance,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        var act = async () => await _repository.CreateAsync(race);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*distance*");
    }

    [Fact]
    public async Task CreateAsync_AddsRaceToContext()
    {
        // Arrange
        var initialCount = await _context.Races.CountAsync();

        var newRace = new Race
        {
            Id = Guid.NewGuid(),
            Name = "Test Race",
            Date = DateTime.Today.AddMonths(1),
            Location = "Test City",
            Distance = 10,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _repository.CreateAsync(newRace);

        // Assert
        var finalCount = await _context.Races.CountAsync();
        finalCount.Should().Be(initialCount + 1);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_UpdatesRaceSuccessfully()
    {
        // Arrange
        var races = _context.Races.ToList();
        var existingRace = races.First();
        var originalName = existingRace.Name;

        var updatedRace = new Race
        {
            Id = existingRace.Id,
            Name = "Updated Race Name",
            Date = existingRace.Date,
            Location = "Updated Location",
            Distance = 50,
            IsActive = existingRace.IsActive,
            CreatedAt = existingRace.CreatedAt
        };

        // Act
        var result = await _repository.UpdateAsync(existingRace.Id, updatedRace);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Updated Race Name");
        result.Location.Should().Be("Updated Location");
        result.Distance.Should().Be(50);

        // DB ellenőrzés
        var fromDb = await _context.Races.FindAsync(existingRace.Id);
        fromDb!.Name.Should().Be("Updated Race Name");
        fromDb.Name.Should().NotBe(originalName);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentId_ThrowsInvalidOperationException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var race = new Race
        {
            Id = nonExistentId,
            Name = "Test",
            Date = DateTime.Today,
            Location = "Test",
            Distance = 10,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var act = async () => await _repository.UpdateAsync(nonExistentId, race);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not found*");
    }

    [Theory]
    [InlineData("Modified Name 1", "Modified Location 1", 25.5)]
    [InlineData("Modified Name 2", "Modified Location 2", 15)]
    [InlineData("Modified Name 3", "Modified Location 3", 100)]
    public async Task UpdateAsync_WithDifferentValidData_UpdatesSuccessfully(
        string newName,
        string newLocation,
        double newDistance)
    {
        // Arrange
        var races = _context.Races.ToList();
        var existingRace = races.First();

        var updatedRace = new Race
        {
            Id = existingRace.Id,
            Name = newName,
            Date = existingRace.Date,
            Location = newLocation,
            Distance = newDistance,
            IsActive = existingRace.IsActive,
            CreatedAt = existingRace.CreatedAt
        };

        // Act
        var result = await _repository.UpdateAsync(existingRace.Id, updatedRace);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(newName);
        result.Location.Should().Be(newLocation);
        result.Distance.Should().Be(newDistance);

        // DB ellenőrzés
        var fromDb = await _context.Races.FindAsync(existingRace.Id);
        fromDb!.Name.Should().Be(newName);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_SoftDeletesRace()
    {
        // Arrange
        var races = _context.Races.ToList();
        var raceToDelete = races.First(r => r.IsActive);
        var raceId = raceToDelete.Id;

        // Act
        var result = await _repository.DeleteAsync(raceId);

        // Assert
        result.Should().BeTrue();

        // DB ellenőrzés - verseny még létezik, de inaktív
        var fromDb = await _context.Races.FindAsync(raceId);
        fromDb.Should().NotBeNull();
        fromDb!.IsActive.Should().BeFalse();
        fromDb.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_DoesNotRemoveFromDatabase()
    {
        // Arrange
        var initialCount = await _context.Races.CountAsync();
        var races = _context.Races.ToList();
        var raceToDelete = races.First(r => r.IsActive);

        // Act
        await _repository.DeleteAsync(raceToDelete.Id);

        // Assert
        var finalCount = await _context.Races.CountAsync();
        finalCount.Should().Be(initialCount); // Soft delete - count nem változik!
    }

    [Fact]
    public async Task DeleteAsync_DeletedRace_NotReturnedByGetRacesAsync()
    {
        // Arrange
        var races = _context.Races.ToList();
        var raceToDelete = races.First(r => r.IsActive);

        // Act
        await _repository.DeleteAsync(raceToDelete.Id);

        // GetRacesAsync alapértelmezetten csak aktívakat ad vissza
        var activeRaces = await _repository.GetRacesAsync(new RaceSearchModel() { IsActive = true});

        // Assert
        activeRaces.Items.Should().NotContain(r => r.Id == raceToDelete.Id);
    }

    /// <summary>
    /// Teszt adatok feltöltése
    /// </summary>
    private void SeedTestData()
    {
        _context.Races.AddRange(
            new Race
            {
                Id = Guid.NewGuid(),
                Name = "Budapest Maraton",
                Date = new DateTime(2025, 10, 1),
                Location = "Budapest",
                Distance = 42.195,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Race
            {
                Id = Guid.NewGuid(),
                Name = "Balaton Félmaraton",
                Date = new DateTime(2025, 6, 15),
                Location = "Balatonfüred",
                Distance = 21.1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Race
            {
                Id = Guid.NewGuid(),
                Name = "Szeged 10K",
                Date = new DateTime(2025, 4, 20),
                Location = "Szeged",
                Distance = 10,
                IsActive = false, // INAKTÍV!
                CreatedAt = DateTime.UtcNow
            }
        );

        _context.SaveChanges();
    }

    /// <summary>
    /// Cleanup - InMemory DB törlése
    /// </summary>
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}