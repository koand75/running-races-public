using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/team/{teamId}/assignments")]
public class RunnerSectionController(IRunnerSectionService runnerSectionService, IMapper mapper) : ControllerBase
{
    private readonly IRunnerSectionService _runnerSectionService = runnerSectionService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RunnerSectionDto>>> GetByTeam(int teamId)
    {
        var assignments = await _runnerSectionService.GetByTeamAsync(teamId);
        return Ok(_mapper.Map<IEnumerable<RunnerSectionDto>>(assignments));
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveAll(int teamId, List<SaveRunnerSectionDto> assignmentsdto)
    {
        var assignments = _mapper.Map<List<RunnerSection>>(assignmentsdto);

        await _runnerSectionService.SaveAllAsync(teamId,assignments);
        return NoContent();
    }
}