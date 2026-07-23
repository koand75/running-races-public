using Xunit;
using Moq;
using FluentAssertions;
using RunningRacesApi.Services;
using RunningRacesApi.Repositories;
using RunningRacesApi.Models;

namespace RunningRaces.Tests.Services;

/// <summary>
/// RaceService unit tesztek
/// Mock repository-val, csak az üzleti logikát teszteljük
/// </summary>
public class RaceServiceTests
{
    private readonly Mock<IRaceRepository> _mockRepository;
    private readonly RaceService _service;

    public RaceServiceTests()
    {
        // Mock repository létrehozása
        _mockRepository = new Mock<IRaceRepository>();

        // Service példány a mock-kal
        _service = new RaceService(_mockRepository.Object);
    }

    [Theory]
    [InlineData(true)]   // User true-t küld → Service true-ra írja (változatlan)
    [InlineData(false)]  // User false-t küld → Service true-ra írja (felülír!)
    [InlineData(null)]   // User null-t küld → Service true-ra írja (beállít!)
    public async Task GetPublicRacesAsync_AlwaysForcesIsActiveToTrue(bool? userInputIsActive)
    {
        // Arrange
        var searchModel = new RaceSearchModel { IsActive = userInputIsActive };

        _mockRepository
            .Setup(r => r.GetRacesAsync(It.IsAny<RaceSearchModel>()))
            .ReturnsAsync(new PagedResult<Race>());

        // Act
        await _service.GetPublicRacesAsync(searchModel);

        // Assert - MINDIG true kell legyen!
        _mockRepository.Verify(
            r => r.GetRacesAsync(It.Is<RaceSearchModel>(m => m.IsActive == true)),
            Times.Once
        );
    }

    [Theory]
    [InlineData(true)]   // Admin true-t küld → Service true-n hagyja
    [InlineData(false)]  // Admin false-t küld → Service false-on hagyja
    [InlineData(null)]   // Admin null-t küld → Service null-on hagyja
    public async Task GetAdminRacesAsync_DoesNotModifyIsActive(bool? adminInputIsActive)
    {
        // Arrange
        var searchModel = new RaceSearchModel { IsActive = adminInputIsActive };

        _mockRepository
            .Setup(r => r.GetRacesAsync(It.IsAny<RaceSearchModel>()))
            .ReturnsAsync(new PagedResult<Race>());

        // Act
        await _service.GetAdminRacesAsync(searchModel);

        // Assert - Pontosan ugyanaz marad!
        _mockRepository.Verify(
            r => r.GetRacesAsync(It.Is<RaceSearchModel>(m => m.IsActive == adminInputIsActive)),
            Times.Once
        );
    }

    [Theory]
    [InlineData("")]           // Üres string
    [InlineData("   ")]        // Csak whitespace
    [InlineData(null)]         // Null
    public async Task CreateRaceAsync_WithInvalidName_ThrowsArgumentException(string? invalidName)
    {
        // Arrange
        var race = new Race
        {
            Name = invalidName!,
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1)
        };

        // Act
        var act = async () => await _service.CreateRaceAsync(race);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*name*");  
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CreateRaceAsync_WithInvalidLocation_ThrowsArgumentException(string? invalidLocation)
    {
        // Arrange
        var race = new Race
        {
            Name = "Test Race",
            Location = invalidLocation!,
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1)
        };

        // Act
        var act = async () => await _service.CreateRaceAsync(race);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*location*");
    }

    [Theory]
    [InlineData(0)]        // Nulla
    [InlineData(-1)]       // Negatív
    [InlineData(-100)]     // Nagy negatív
    public async Task CreateRaceAsync_WithInvalidDistance_ThrowsArgumentException(double invalidDistance)
    {
        // Arrange
        var race = new Race
        {
            Name = "Test Race",
            Location = "Budapest",
            Distance = invalidDistance,
            Date = DateTime.Today.AddMonths(1)
        };

        // Act
        var act = async () => await _service.CreateRaceAsync(race);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*positive*"); 
    }

    [Theory]
    [InlineData(501)]      
    [InlineData(600)]     
    [InlineData(1000)]     
    public async Task CreateRaceAsync_WithDistanceOver500_ThrowsArgumentException(double tooLargeDistance)
    {
        // Arrange
        var race = new Race
        {
            Name = "Test Race",
            Location = "Budapest",
            Distance = tooLargeDistance,
            Date = DateTime.Today.AddMonths(1)
        };

        // Act
        var act = async () => await _service.CreateRaceAsync(race);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*cannot exceed 500*");
    }

    [Fact]
    public async Task CreateRaceAsync_WithValidRace_SetsDefaultValues()
    {
        // Arrange
        var race = new Race
        {
            // User NEM ad meg Id, CreatedAt, IsActive-et!
            Name = "Test Race",
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1)
        };

        _mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Race>()))
            .ReturnsAsync((Race r) => r);  

        // Act
        var result = await _service.CreateRaceAsync(race);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);  
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));  
        result.IsActive.Should().BeTrue();  
    }

    [Fact]
    public async Task CreateRaceAsync_WithValidRace_CallsRepository()
    {
        // Arrange
        var race = new Race
        {
            Name = "Test Race",
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1)
        };

        _mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Race>()))
            .ReturnsAsync((Race r) => r);

        // Act
        await _service.CreateRaceAsync(race);

        // Assert
        _mockRepository.Verify(
            r => r.CreateAsync(It.Is<Race>(race =>
                race.Name == "Test Race" &&
                race.Location == "Budapest" &&
                race.Distance == 42.2 &&
                race.Id != Guid.Empty && 
                race.IsActive == true     
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateRaceAsync_WithEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var race = new Race
        {
            Name = "Test Race",
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1)
        };

        // Act
        var act = async () => await _service.UpdateRaceAsync(Guid.Empty, race);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid race ID*");
    }

    [Fact]
    public async Task UpdateRaceAsync_WithNonExistentId_ThrowsInvalidOperationException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var race = new Race
        {
            Name = "Test Race",
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1)
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(nonExistentId))
            .ReturnsAsync((Race?)null);

        // Act
        var act = async () => await _service.UpdateRaceAsync(nonExistentId, race);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not found*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task UpdateRaceAsync_WithInvalidName_ThrowsArgumentException(string? invalidName)
    {
        // Arrange
        var id = Guid.NewGuid();
        var race = new Race
        {
            Name = invalidName!,
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1)
        };

        // Act
        var act = async () => await _service.UpdateRaceAsync(id, race);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*name*");
    }

    [Fact]
    public async Task UpdateRaceAsync_WithValidData_CallsRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingRace = new Race
        {
            Id = id,
            Name = "Old Name",
            Location = "Old Location",
            Distance = 10,
            Date = DateTime.Today
        };

        var updatedRace = new Race
        {
            Name = "New Name",
            Location = "New Location",
            Distance = 21.1,
            Date = DateTime.Today.AddMonths(1)
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existingRace);  

        _mockRepository
            .Setup(r => r.UpdateAsync(id, It.IsAny<Race>()))
            .ReturnsAsync(updatedRace);

        // Act
        var result = await _service.UpdateRaceAsync(id, updatedRace);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(id, updatedRace), Times.Once);
        result.Should().Be(updatedRace);
    }

    [Fact]
    public async Task DeleteRaceAsync_WithEmptyGuid_ThrowsArgumentException()
    {
        // Act
        var act = async () => await _service.DeleteRaceAsync(Guid.Empty);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid race ID*");
    }

    [Fact]
    public async Task DeleteRaceAsync_WithValidId_CallsRepositoryAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true); 

        // Act
        var result = await _service.DeleteRaceAsync(id);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteRaceAsync_WithNonExistentId_CallsRepositoryAndReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(false);  

        // Act
        var result = await _service.DeleteRaceAsync(id);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetRaceByIdAsync_WithEmptyGuid_ThrowsArgumentException()
    {
        // Act
        var act = async () => await _service.GetRaceByIdAsync(Guid.Empty);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid race ID*");
    }

    [Fact]
    public async Task GetRaceByIdAsync_WithValidId_ReturnsRace()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedRace = new Race
        {
            Id = id,
            Name = "Test Race",
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(expectedRace);

        // Act
        var result = await _service.GetRaceByIdAsync(id);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        result.Should().Be(expectedRace);
    }

    [Fact]
    public async Task GetRaceByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Race?)null);

        // Act
        var result = await _service.GetRaceByIdAsync(id);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        result.Should().BeNull();
    }

}