using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/race/{raceId}/category/{categoryId}/registration")]
public class RaceCategoryTeamController(IRaceCategoryTeamService service) : ControllerBase
{
    private readonly IRaceCategoryTeamService _service = service;

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<RaceCategoryTeamDto>>> GetByCategory(int categoryId)
    {
        var result = await _service.GetByCategoryAsync(categoryId);
        return Ok(result.Adapt<IEnumerable<RaceCategoryTeamDto>>());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RaceCategoryTeamDto>> Create(int categoryId, RaceCategoryTeamDto dto)
    {
        dto.CategoryId = categoryId;
        var created = await _service.CreateAsync(dto.Adapt<RaceCategoryTeam>());
        return CreatedAtAction(nameof(GetByCategory), new { categoryId }, created.Adapt<RaceCategoryTeamDto>());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}