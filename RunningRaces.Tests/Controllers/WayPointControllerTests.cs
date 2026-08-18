using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using RunningRacesApi.Models;
using RunningRacesApi.Services;

namespace RunningRaces.Tests.Controllers;

public class WayPointControllerTests
{
    private readonly Mock<IWayPointService> _mockService;
    private readonly WayPointController _controller;

    public WayPointControllerTests()
    {
        _mockService = new Mock<IWayPointService>();
        _controller = new WayPointController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var pagedResult = new PagedResult<WayPoint>
        {
            Items = new List<WayPoint> { new WayPoint { Name = "Balatonfüred" } },
            TotalCount = 1
        };
        _mockService.Setup(s => s.GetAllAsync(It.IsAny<BaseSearchModel>()))
            .ReturnsAsync(pagedResult);

        var result = await _controller.GetAll();

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        var wp = new WayPoint { Id = 1, Name = "Keszthely" };
        _mockService.Setup(s => s.CreateAsync(It.IsAny<WayPoint>())).ReturnsAsync(wp);

        var result = await _controller.Create(wp);

        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Update_ExistingWayPoint_ReturnsOk()
    {
        var wp = new WayPoint { Id = 1, Name = "Siófok Updated" };
        _mockService.Setup(s => s.UpdateAsync(1, It.IsAny<WayPoint>())).ReturnsAsync(wp);

        var result = await _controller.Update(1, wp);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_NonExistingWayPoint_ReturnsNotFound()
    {
        _mockService.Setup(s => s.UpdateAsync(999, It.IsAny<WayPoint>())).ReturnsAsync((WayPoint?)null);

        var result = await _controller.Update(999, new WayPoint());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_NotInUse_ReturnsNoContent()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_InUse_ReturnsBadRequest()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}