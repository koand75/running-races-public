using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Services;

[ApiController]
[Route("api/[controller]")]
public class WayPointController : ControllerBase
{
    private readonly IWayPointService _service;

    public WayPointController(IWayPointService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WayPoint>>> GetAll(int? pageSize)
    {
        var searchModel = new BaseSearchModel
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10
        };

        return Ok(await _service.GetAllAsync(searchModel));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<WayPoint>> Create(WayPoint wayPoint)
    {
        var created = await _service.CreateAsync(wayPoint);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, WayPoint wayPoint)
    {
        var updated = await _service.UpdateAsync(id, wayPoint);
        if (updated == null) return NotFound();
        return Ok(updated);
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