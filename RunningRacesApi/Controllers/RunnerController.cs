using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

using System.Diagnostics;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/team/{teamId}/[controller]")]
public class RunnerControllre(IRunnerService runnerService, IMapper mapper) : ControllerBase
{
    private readonly IRunnerService _runnerService = runnerService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RunnerDto>>> GetByTeam(int teamId)
    {
        var result = await _runnerService.GetByTeamAsync(teamId);
        return Ok(_mapper.Map<PagedResult<RunnerDto>>(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RunnerDto>> GetById(int teamId, int id)
    {
        var runner = await _runnerService.GetByIdAsync(id);
        if (runner == null || runner.TeamId != teamId) return NotFound();
        return Ok(_mapper.Map<RunnerDto>(runner));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RunnerDto>> Create(int teamId, RunnerDto runner)
    {
        runner.TeamId = teamId;
        var created = await _runnerService.CreateAsync(_mapper.Map<Runner>(runner));
        return CreatedAtAction(nameof(GetById), new { teamId, id = created.Id }, _mapper.Map<RunnerDto>(created));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int teamId, int id, RunnerDto runner)
    {
        if (id != runner.Id || teamId != runner.TeamId) return BadRequest();
        await _runnerService.UpdateAsync(_mapper.Map<Runner>(runner));
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int teamId, int id)
    {
        await _runnerService.DeleteAsync(id);
        return NoContent();
    }
}