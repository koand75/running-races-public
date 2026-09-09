using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

[ApiController]
[Route("api/race/{raceId}/category/{categoryId}/waypoint")]
public class WayPointController(IWayPointService service) : ControllerBase
{
    private readonly IWayPointService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WayPointDto>>> GetAll(int categoryId, int? pageSize)
    {
        var searchModel = new BaseSearchModel
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10
        };

        var result = await _service.GetAllByCategoryAsync(categoryId, searchModel);
        return Ok(result.Adapt<PagedResult<WayPointDto>>());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<WayPointDto>> Create(int categoryId, WayPointDto wayPoint)
    {
        wayPoint.CategoryId = categoryId;
        var created = await _service.CreateAsync(wayPoint.Adapt<WayPoint>());
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created.Adapt<WayPointDto>());
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int categoryId, int id, WayPointDto wayPoint)
    {
        wayPoint.CategoryId = categoryId;
        var updated = await _service.UpdateAsync(id, wayPoint.Adapt<WayPoint>());
        if (updated == null) return NotFound();
        return Ok(updated.Adapt<WayPointDto>());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return BadRequest("It is a used waypoint!");
        return NoContent();
    }
}