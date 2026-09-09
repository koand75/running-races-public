using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/race/{raceId}/category/{categoryId}/team/{teamId}/assignments")]
public class RunnerSectionController(IRunnerSectionService runnerSectionService) : ControllerBase
{
    private readonly IRunnerSectionService _runnerSectionService = runnerSectionService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RunnerSectionDto>>> GetByTeam(int categoryId, int teamId)
    {
        var assignments = await _runnerSectionService.GetByTeamAsync(categoryId, teamId);
        return Ok(assignments.Adapt<IEnumerable<RunnerSectionDto>>());
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveAll(int categoryId, int teamId, List<SaveRunnerSectionDto> assignmentsdto)
    {
        var assignments = assignmentsdto.Adapt<List<RunnerSection>>();
        assignments.ForEach(a => a.CategoryId = categoryId);
        await _runnerSectionService.SaveAllAsync(categoryId, teamId, assignments);
        return NoContent();
    }
}