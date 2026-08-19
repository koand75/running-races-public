using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/section-export")]
public class SectionExportController : ControllerBase
{
    private readonly ISectionService _sectionService;
    private readonly ICsvExportService _csvExportService;

    public SectionExportController(ISectionService sectionService, ICsvExportService csvExportService)
    {
        _sectionService = sectionService;
        _csvExportService = csvExportService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Export([FromQuery] bool includeId = false)
    {
        var sections = await _sectionService.GetAllAsync();

        var sectiosnToExport = sections.Select(s => new SectionExportDto
        {
            Id = s.Id,
            Order = s.Order,
            Distance = s.Distance,
            StartWayPointName = s.StartWayPoint?.Name,
            StartLat = s.StartWayPoint?.Lat,
            StartLng = s.StartWayPoint?.Lng,
            EndWayPointName = s.EndWayPoint?.Name,
            EndLat = s.EndWayPoint?.Lat,
            EndLng = s.EndWayPoint?.Lng,
            Description = s.Description
        });

        var columns = new List<string> { 
            "Order", 
            "Distance",
            "StartWayPointName",
            "StartLat",
            "StartLng",
            "EndWayPointName",
            "EndLat",
            "EndLng",
            "Description"
        };
        if (includeId)
        {
            columns.Insert(0, "Id");
        }

        var csv = _csvExportService.Export(sectiosnToExport, columns);
        return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "section.csv");
    }
}
