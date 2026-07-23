using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

[ApiController]
[Route("api/[controller]")]
public class WayPointController : ControllerBase
{
    private readonly AppDbContext _context;

    public WayPointController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WayPoint>>> GetAll()
    {
        return Ok(await _context.WayPoints.OrderBy(w => w.Name).ToListAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<WayPoint>> Create(WayPoint wayPoint)
    {
        _context.WayPoints.Add(wayPoint);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = wayPoint.Id }, wayPoint);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, WayPoint wayPoint)
    {
        var existing = await _context.WayPoints.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Name = wayPoint.Name;
        existing.Lat = wayPoint.Lat;
        existing.Lng = wayPoint.Lng;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var inUse = await _context.Sections
            .AnyAsync(s => s.StartWayPointId == id || s.EndWayPointId == id);
        if (inUse) return BadRequest("Váltópont használatban van!");
        var wp = await _context.WayPoints.FindAsync(id);
        if (wp == null) return NotFound();
        _context.WayPoints.Remove(wp);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}