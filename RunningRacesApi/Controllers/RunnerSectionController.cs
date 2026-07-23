using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/team/{teamId}/assignments")]
public class RunnerSectionController : ControllerBase
{
    private readonly IRunnerSectionService _runnerSectionService;

    public RunnerSectionController(IRunnerSectionService runnerSectionService)
    {
        _runnerSectionService = runnerSectionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RunnerSection>>> GetByTeam(int teamId)
    {
        var assignments = await _runnerSectionService.GetByTeamAsync(teamId);
        return Ok(assignments);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveAll(int teamId, List<SaveRunnerSectionDto> assignmentsdto)
    {
        var assignments = assignmentsdto.Select(a => new RunnerSection
        {
            SectionId = a.SectionId,
            RunnerId = a.RunnerId,
            CustomPace = a.CustomPace
        }).ToList();

        await _runnerSectionService.SaveAllAsync(teamId, assignments);
        return NoContent();
    }
}