using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/race/{raceId}/team/{teamId}/assignments")]
public class RunnerSectionController(IRunnerSectionService runnerSectionService) : ControllerBase
{
    private readonly IRunnerSectionService _runnerSectionService = runnerSectionService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RunnerSectionDto>>> GetByTeam(Guid raceId, int teamId)
    {
        var assignments = await _runnerSectionService.GetByTeamAsync(raceId, teamId);
        return Ok(assignments.Adapt<IEnumerable<RunnerSectionDto>>());
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveAll(Guid raceId, int teamId, List<SaveRunnerSectionDto> assignmentsdto)
    {
        var assignments = assignmentsdto.Adapt<List<RunnerSection>>();
        assignments.ForEach(a => a.RaceId = raceId);
        await _runnerSectionService.SaveAllAsync(raceId, teamId, assignments);
        return NoContent();
    }
}