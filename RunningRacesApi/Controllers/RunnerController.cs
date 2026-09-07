using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/team/{teamId}/[controller]")]
public class RunnerController(IRunnerService runnerService) : ControllerBase
{
    private readonly IRunnerService _runnerService = runnerService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RunnerDto>>> GetByTeam(int teamId)
    {
        var result = await _runnerService.GetByTeamAsync(teamId);
        return Ok(result.Adapt<IEnumerable<RunnerDto>>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RunnerDto>> GetById(int teamId, int id)
    {
        var runner = await _runnerService.GetByIdAsync(id);
        if (runner == null || runner.TeamId != teamId) return NotFound();
        return Ok(runner.Adapt<RunnerDto>());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RunnerDto>> Create(int teamId, RunnerDto runner)
    {
        runner.TeamId = teamId;
        var created = await _runnerService.CreateAsync(runner.Adapt<Runner>());
        return CreatedAtAction(nameof(GetById), new { teamId, id = created.Id }, created.Adapt<RunnerDto>());
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int teamId, int id, RunnerDto runner)
    {
        if (id != runner.Id || teamId != runner.TeamId) return BadRequest();
        await _runnerService.UpdateAsync(runner.Adapt<Runner>());
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