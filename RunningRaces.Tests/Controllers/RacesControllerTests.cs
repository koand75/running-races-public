using AutoMapper;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using RunningRacesApi.Controllers;
using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

using Xunit;

namespace RunningRaces.Tests.Controllers;

/// <summary>
/// RacesController unit tesztek
/// Mock service-szel, csak HTTP válaszokat teszteljük
/// </summary>
public class RacesControllerTests
{
    private readonly Mock<IRaceService> _mockService;
    private readonly RacesController _controller;
    private readonly Mock<IMapper> _mockMapper;

    public RacesControllerTests()
    {
        // Mock service létrehozása
        _mockService = new Mock<IRaceService>();
        _mockMapper = new Mock<IMapper>();

        // Controller példány a mock-kal
        _controller = new RacesController(_mockService.Object, _mockMapper.Object);
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

        _mockMapper.Setup(m => m.Map<RaceDto>(It.IsAny<Race>())).Returns(new RaceDto { Id = raceId });

        // Act
        var result = await _controller.GetRaceById(raceId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        var returnedRace = okResult!.Value as RaceDto;
        returnedRace.Should().NotBeNull();
        returnedRace!.Id.Should().Be(raceId);
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

        var pagedDto = new PagedResult<RaceDto>
        {
            Items = new List<RaceDto> {
                new RaceDto { Name = "Race 1" },
                new RaceDto { Name = "Race 2" }
    },
            TotalCount = 2
        };
        _mockMapper.Setup(m => m.Map<PagedResult<RaceDto>>(It.IsAny<PagedResult<Race>>())).Returns(pagedDto);

        // Act
        var result = await _controller.GetPublicRaces(new RaceSearchModel());

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var returnedRaces = okResult!.Value as PagedResult<RaceDto>;
        returnedRaces.Should().NotBeNull();
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
        var raceDto = new RaceDto { Id = createdRace.Id, Name = createdRace.Name };
        _mockMapper.Setup(m => m.Map<RaceDto>(It.IsAny<Race>())).Returns(raceDto);

        // Act
        var result = await _controller.CreateRace(raceDto);

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

        var raceDto = new RaceDto { Id = createdRace.Id, Name = createdRace.Name };
        _mockMapper.Setup(m => m.Map<RaceDto>(It.IsAny<Race>())).Returns(raceDto);

        // Act
        var result = await _controller.CreateRace(raceDto);

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

        var raceDto = new RaceDto { Id = createdRace.Id, Name = createdRace.Name };
        _mockMapper.Setup(m => m.Map<RaceDto>(It.IsAny<Race>())).Returns(raceDto);

        // Act
        var result = await _controller.CreateRace(raceDto);

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

        var raceDto = new RaceDto { Id = raceId, Name = updatedRace.Name };
        _mockMapper.Setup(m => m.Map<RaceDto>(It.IsAny<Race>())).Returns(raceDto);

        // Act
        var result = await _controller.UpdateRace(raceId, raceDto);

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