using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/race/{raceId}/category")]
public class RaceCategoryController(IRaceCategoryService categoryService) : ControllerBase
{
    private readonly IRaceCategoryService _categoryService = categoryService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RaceCategoryDto>>> GetAll(Guid raceId)
    {
        var categories = await _categoryService.GetAllByRaceAsync(raceId);
        return Ok(categories.Adapt<IEnumerable<RaceCategoryDto>>());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RaceCategoryDto>> Create(Guid raceId, RaceCategoryDto dto)
    {
        dto.RaceId = raceId;
        var created = await _categoryService.CreateAsync(dto.Adapt<RaceCategory>());
        return CreatedAtAction(nameof(GetAll), new { raceId }, created.Adapt<RaceCategoryDto>());
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid raceId, int id, RaceCategoryDto dto)
    {
        dto.RaceId = raceId;
        await _categoryService.UpdateAsync(id, dto.Adapt<RaceCategory>());
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
}