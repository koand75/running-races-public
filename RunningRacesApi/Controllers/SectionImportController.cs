using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nincs fájl feltöltve");

        if (!file.FileName.EndsWith(".csv"))
            return BadRequest("Csak CSV fájl engedélyezett");

        var result = await _importService.ImportAsync(file);
        return Ok(new { imported = result });
    }
}