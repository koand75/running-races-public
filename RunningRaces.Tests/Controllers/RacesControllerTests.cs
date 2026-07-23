using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using RunningRacesApi.Controllers;
using RunningRacesApi.Services;
using RunningRacesApi.Models;

namespace RunningRaces.Tests.Controllers;

/// <summary>
/// RacesController unit tesztek
/// Mock service-szel, csak HTTP válaszokat teszteljük
/// </summary>
public class RacesControllerTests
{
    private readonly Mock<IRaceService> _mockService;
    private readonly RacesController _controller;

    public RacesControllerTests()
    {
        // Mock service létrehozása
        _mockService = new Mock<IRaceService>();

        // Controller példány a mock-kal
        _controller = new RacesController(_mockService.Object);
    }

    [Fact]
    public async Task GetRaceById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var raceId = Guid.NewGuid();
        var expectedRace = new Race
        {
            Id = raceId,
            Name = "Test Race",
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today
        };

        _mockService
            .Setup(s => s.GetRaceByIdAsync(raceId))
            .ReturnsAsync(expectedRace);  

        // Act
        var result = await _controller.GetRaceById(raceId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetRaceById_WithValidId_ReturnsCorrectRace()
    {
        // Arrange
        var raceId = Guid.NewGuid();
        var expectedRace = new Race
        {
            Id = raceId,
            Name = "Budapest Marathon",
            Location = "Budapest",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(1),
            IsActive = true
        };

        _mockService
            .Setup(s => s.GetRaceByIdAsync(raceId))
            .ReturnsAsync(expectedRace);

        // Act
        var result = await _controller.GetRaceById(raceId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        var returnedRace = okResult!.Value as Race;
        returnedRace.Should().NotBeNull();
        returnedRace!.Id.Should().Be(expectedRace.Id);
        returnedRace.Name.Should().Be("Budapest Marathon");
        returnedRace.Location.Should().Be("Budapest");
        returnedRace.Distance.Should().Be(42.2);
    }

    [Fact]
    public async Task GetRaceById_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        _mockService
            .Setup(s => s.GetRaceByIdAsync(nonExistentId))
            .ReturnsAsync((Race?)null);  

        // Act
        var result = await _controller.GetRaceById(nonExistentId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetPublicRaces_ReturnsOkResultWithRaces()
    {
        // Arrange
        var expectedRaces = new PagedResult<Race>
        {
            Items = new List<Race> {
                new Race { Id = Guid.NewGuid(), Name = "Race 1", Location = "Budapest", Distance = 10, Date = DateTime.Today, IsActive = true },
                new Race { Id = Guid.NewGuid(), Name = "Race 2", Location = "Debrecen", Distance = 21.1, Date = DateTime.Today, IsActive = true }
            }
        };

        _mockService
            .Setup(s => s.GetPublicRacesAsync(It.IsAny<RaceSearchModel>()))
            .ReturnsAsync(expectedRaces);

        // Act
        var result = await _controller.GetPublicRaces(null);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        var returnedRaces = okResult!.Value as PagedResult<Race>;
        returnedRaces.Should().NotBeNull();
        returnedRaces.Items.Should().HaveCount(2);
        returnedRaces.Items![0].Name.Should().Be("Race 1");
        returnedRaces.Items[1].Name.Should().Be("Race 2");
    }

    [Fact]
    public async Task CreateRace_WithValidRace_ReturnsCreatedResult()
    {
        // Arrange
        var newRace = new Race
        {
            Name = "New Race",
            Location = "Szeged",
            Distance = 15,
            Date = DateTime.Today.AddMonths(2)
        };

        var createdRace = new Race
        {
            Id = Guid.NewGuid(),  // Service generálta
            Name = newRace.Name,
            Location = newRace.Location,
            Distance = newRace.Distance,
            Date = newRace.Date,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _mockService
            .Setup(s => s.CreateRaceAsync(It.IsAny<Race>()))
            .ReturnsAsync(createdRace);

        // Act
        var result = await _controller.CreateRace(newRace);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task CreateRace_WithValidRace_ReturnsCreatedRace()
    {
        // Arrange
        var newRace = new Race
        {
            Name = "Marathon",
            Location = "Pécs",
            Distance = 42.2,
            Date = DateTime.Today.AddMonths(3)
        };

        var createdRace = new Race
        {
            Id = Guid.NewGuid(),
            Name = newRace.Name,
            Location = newRace.Location,
            Distance = newRace.Distance,
            Date = newRace.Date,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _mockService
            .Setup(s => s.CreateRaceAsync(It.IsAny<Race>()))
            .ReturnsAsync(createdRace);

        // Act
        var result = await _controller.CreateRace(newRace);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();

        var returnedRace = createdResult!.Value as Race;
        returnedRace.Should().NotBeNull();
        returnedRace!.Name.Should().Be("Marathon");
        returnedRace.Location.Should().Be("Pécs");
        returnedRace.Distance.Should().Be(42.2);
        returnedRace.Id.Should().NotBe(Guid.Empty);  // Service generálta!
    }

    [Fact]
    public async Task CreateRace_WithValidRace_ReturnsLocationHeader()
    {
        // Arrange
        var newRace = new Race
        {
            Name = "Trail Run",
            Location = "Visegrád",
            Distance = 25,
            Date = DateTime.Today.AddMonths(1)
        };

        var createdRace = new Race
        {
            Id = Guid.NewGuid(),
            Name = newRace.Name,
            Location = newRace.Location,
            Distance = newRace.Distance,
            Date = newRace.Date,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _mockService
            .Setup(s => s.CreateRaceAsync(It.IsAny<Race>()))
            .ReturnsAsync(createdRace);

        // Act
        var result = await _controller.CreateRace(newRace);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.ActionName.Should().Be(nameof(RacesController.GetRaceById));

        var routeValues = createdResult.RouteValues;
        routeValues.Should().ContainKey("id");
        routeValues!["id"].Should().Be(createdRace.Id);
    }

    [Fact]
    public async Task UpdateRace_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var raceId = Guid.NewGuid();
        var updateData = new Race
        {
            Id = raceId,
            Name = "Updated Race Name",
            Location = "Updated Location",
            Distance = 25.5,
            Date = DateTime.Today.AddMonths(2)
        };

        var updatedRace = new Race
        {
            Id = raceId,
            Name = updateData.Name,
            Location = updateData.Location,
            Distance = updateData.Distance,
            Date = updateData.Date,
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            ModifiedAt = DateTime.UtcNow
        };

        _mockService
            .Setup(s => s.UpdateRaceAsync(raceId, It.IsAny<Race>()))
            .ReturnsAsync(updatedRace);

        // Act
        var result = await _controller.UpdateRace(raceId, updateData);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task DeleteRace_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var raceId = Guid.NewGuid();

        _mockService
            .Setup(s => s.DeleteRaceAsync(raceId))
            .ReturnsAsync(true);  

        // Act
        var result = await _controller.DeleteRace(raceId);

        // Assert
        result.Should().BeOfType<NoContentResult>();  
    }

    [Fact]
    public async Task DeleteRace_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        _mockService
            .Setup(s => s.DeleteRaceAsync(nonExistentId))
            .ReturnsAsync(false);  

        // Act
        var result = await _controller.DeleteRace(nonExistentId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();  
    }
}