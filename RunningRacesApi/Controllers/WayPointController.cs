using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

using System.Text;

[ApiController]
[Route("api/[controller]")]
public class WayPointController(IWayPointService service, IMapper mapper) : ControllerBase
{
    private readonly IWayPointService _service = service;
    private readonly IMapper _mapper = mapper;   

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WayPointDto>>> GetAll(int? pageSize)
    {
        var searchModel = new BaseSearchModel
        {
            PageSize = pageSize.HasValue ? pageSize.Value : 10
        };

        var result = await _service.GetAllAsync(searchModel);
        return Ok(_mapper.Map<PagedResult<WayPointDto>>(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<WayPointDto>> Create(WayPointDto wayPoint)
    {
        var created = await _service.CreateAsync(_mapper.Map<WayPoint>(wayPoint));
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, _mapper.Map<WayPointDto>(created));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, WayPointDto wayPoint)
    {
        var updated = await _service.UpdateAsync(id, _mapper.Map<WayPoint>(wayPoint));
        if (updated == null) return NotFound();
        return Ok(_mapper.Map<WayPointDto>(updated));
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