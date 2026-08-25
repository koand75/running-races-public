using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Authorize]
[Route("api/section-import")]
public class SectionImportController : ControllerBase
{
    private readonly ISectionImportService _importService;

    public SectionImportController(ISectionImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("preview")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Preview(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nincs fájl feltöltve");

        if (!file.FileName.EndsWith(".csv"))
            return BadRequest("Csak CSV fájl engedélyezett");

        var result = await _importService.PreviewAsync(file);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Import(List<SectionImportDto> sectionImport)
    {
        try
        {
            var result = await _importService.ImportAsync(sectionImport);
            return Ok(new { imported = result });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}