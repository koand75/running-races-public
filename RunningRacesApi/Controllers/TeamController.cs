using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController(ITeamService teamService, IMapper mapper) : ControllerBase
{
    private readonly ITeamService _teamService = teamService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll()
    {
        var teams = await _teamService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TeamDto>>(teams));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDto>> GetById(int id)
    {
        var team = await _teamService.GetByIdAsync(id);
        if (team == null) return NotFound();
        return Ok(_mapper.Map<TeamDto>(team));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TeamDto>> Create([FromBody] TeamDto team)
    {
        var created = await _teamService.CreateAsync(_mapper.Map<Team>(team));
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<TeamDto>(created));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, TeamDto team)
    {
        if (id != team.Id) return BadRequest();
        await _teamService.UpdateAsync(_mapper.Map<Team>(team));
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _teamService.DeleteAsync(id);
        return NoContent();
    }
}