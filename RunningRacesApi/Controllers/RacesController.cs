using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RacesController(IRaceService service, IMapper mapper) : ControllerBase
{
    private readonly IRaceService _service = service;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Get active races
    /// </summary>
    [HttpGet("public")]
    public async Task<ActionResult<PagedResult<RaceDto>>> GetPublicRaces([FromQuery] RaceSearchModel searchModel)
    {
        var result = await _service.GetPublicRacesAsync(searchModel);

        return Ok(_mapper.Map<PagedResult<RaceDto>>(result));
    }

    /// <summary>
    /// Get racelist with filter
    /// </summary>
    [HttpGet("admin")]
    [Authorize]
    public async Task<ActionResult<PagedResult<RaceDto>>> GetAdminRaces([FromQuery] RaceSearchModel searchModel)
    {
        var result = await _service.GetAdminRacesAsync(searchModel);
        return Ok(_mapper.Map<PagedResult<RaceDto>>(result));
    }

    /// <summary>
    /// Get race details
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RaceDto>> GetRaceById(Guid id)
    {
        try
        {
            var race = await _service.GetRaceByIdAsync(id);

            if (race == null)
            {
                return NotFound(new { message = "Race not found." });
            }

            return Ok(_mapper.Map<RaceDto>(race));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create new race (AdminOnly)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RaceDto>> CreateRace([FromBody] RaceDto race)
    {
        try
        {
            var createdRace = await _service.CreateRaceAsync(_mapper.Map<Race>(race));
            return CreatedAtAction(nameof(GetRaceById), new { id = createdRace.Id }, createdRace);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Modify race data
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RaceDto>> UpdateRace(Guid id, [FromBody] RaceDto race)
    {
        try
        {
            var updatedRace = await _service.UpdateRaceAsync(id, _mapper.Map<Race>(race));
            return Ok(updatedRace);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete race (Admin Only, soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRace(Guid id)
    {
        try
        {
            var success = await _service.DeleteRaceAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Race not found." });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Reactivate race
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(Guid id)
    {
        await _service.RestoreRaceAsync(id);
        return NoContent();
    }
}