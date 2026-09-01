using AutoMapper;

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
    private readonly IMapper _mapper;

    public SectionExportController(ISectionService sectionService,
        ICsvExportService csvExportService,
        IMapper mapper)
    {
        _sectionService = sectionService;
        _csvExportService = csvExportService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Export([FromQuery] bool includeId = false)
    {
        var sections = await _sectionService.GetAllAsync();

        var sectiosnToExport = _mapper.Map<IEnumerable<SectionExportDto>>(sections);

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
