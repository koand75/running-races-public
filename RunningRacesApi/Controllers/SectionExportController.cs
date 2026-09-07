using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/race/{raceId}/section-export")]
public class SectionExportController(ISectionService sectionService,
    ICsvExportService csvExportService) : ControllerBase
{
    private readonly ISectionService _sectionService = sectionService;
    private readonly ICsvExportService _csvExportService = csvExportService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Export(Guid raceId, [FromQuery] bool includeId = false)
    {
        var sections = await _sectionService.GetAllByRaceAsync(raceId);

        var sectiosnToExport = sections.Adapt<IEnumerable<SectionExportDto>>();

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
