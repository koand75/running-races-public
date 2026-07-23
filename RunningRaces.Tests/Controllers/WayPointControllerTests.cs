using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRaces.Tests.Controllers;

public class WayPointControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly WayPointController _controller;

    public WayPointControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _controller = new WayPointController(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task GetAll_ReturnsAllWayPoints()
    {
        // Arrange
        _context.WayPoints.AddRange(
            new WayPoint { Name = "Balatonfüred", Lat = 46.95, Lng = 17.89 },
            new WayPoint { Name = "Tihany", Lat = 46.91, Lng = 17.88 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll();

        // Assert
        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var list = ok!.Value as IEnumerable<WayPoint>;
        list.Should().HaveCount(2);
    }

    [Fact]
    public async Task Create_AddsWayPoint_ReturnsCreated()
    {
        // Arrange
        var wp = new WayPoint { Name = "Keszthely", Lat = 46.76, Lng = 17.24 };

        // Act
        var result = await _controller.Create(wp);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        _context.WayPoints.Should().HaveCount(1);
    }

    [Fact]
    public async Task Update_ExistingWayPoint_ReturnsOk()
    {
        // Arrange
        var wp = new WayPoint { Name = "Siófok", Lat = 46.90, Lng = 18.05 };
        _context.WayPoints.Add(wp);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Update(wp.Id, new WayPoint { Name = "Siófok Updated", Lat = 46.91, Lng = 18.06 });

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _context.WayPoints.First().Name.Should().Be("Siófok Updated");
    }

    [Fact]
    public async Task Update_NonExistingWayPoint_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Update(999, new WayPoint { Name = "X" });

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_WayPointNotInUse_ReturnsNoContent()
    {
        // Arrange
        var wp = new WayPoint { Name = "Zamárdi" };
        _context.WayPoints.Add(wp);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Delete(wp.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _context.WayPoints.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_WayPointInUse_ReturnsBadRequest()
    {
        // Arrange
        var wp = new WayPoint { Name = "Balatonalmádi" };
        _context.WayPoints.Add(wp);
        await _context.SaveChangesAsync();

        var section = new Section { Name = "Test", Order = 1, Distance = 5, StartWayPointId = wp.Id };
        _context.Sections.Add(section);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Delete(wp.Id);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}