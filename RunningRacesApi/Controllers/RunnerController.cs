using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/team/{teamId}/[controller]")]
public class RunnerController : ControllerBase
{
    private readonly IRunnerService _runnerService;

    public RunnerController(IRunnerService runnerService)
    {
        _runnerService = runnerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Runner>>> GetByTeam(int teamId)
    {
        var runners = await _runnerService.GetByTeamAsync(teamId);
        return Ok(runners);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Runner>> GetById(int teamId, int id)
    {
        var runner = await _runnerService.GetByIdAsync(id);
        if (runner == null || runner.TeamId != teamId) return NotFound();
        return Ok(runner);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Runner>> Create(int teamId, Runner runner)
    {
        runner.TeamId = teamId;
        var created = await _runnerService.CreateAsync(runner);
        return CreatedAtAction(nameof(GetById), new { teamId, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int teamId, int id, Runner runner)
    {
        if (id != runner.Id || teamId != runner.TeamId) return BadRequest();
        await _runnerService.UpdateAsync(runner);
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