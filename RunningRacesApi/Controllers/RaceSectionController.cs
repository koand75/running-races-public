using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/race/{raceId}/section")]
public class RaceSectionController(ISectionService sectionService) : ControllerBase
{
    private readonly ISectionService _sectionService = sectionService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SectionDto>>> GetAll(Guid raceId)
    {       
        var sections = await _sectionService.GetAllByRaceAsync(raceId);
        return Ok(sections.Adapt<IEnumerable<SectionDto>>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SectionDto>> GetSectionByRaceAsync(Guid raceId, int id)
    {
        var section = await _sectionService.GetSectionByRaceAsync(raceId, id);
        if (section == null) return NotFound();
        return Ok(section.Adapt<SectionDto>());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SectionDto>> Create(Guid raceId, SectionDto section)
    {
        section.RaceId = raceId;
        var created = await _sectionService.CreateAsync(section.Adapt<Section>());
        return CreatedAtAction(nameof(GetSectionByRaceAsync), new { raceId, id = created.Id }, created.Adapt<SectionDto>());
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid raceId, int id, SectionDto section)
    {
        if (id != section.Id) return BadRequest();
        section.RaceId = raceId;
        await _sectionService.UpdateAsync(section.Adapt<Section>());
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid raceId, int id)
    {
        await _sectionService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("insert-after/{afterOrder}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SectionDto>> InsertAfter(Guid raceId, int afterOrder, SectionDto section)
    {
        section.RaceId = raceId;
        var created = await _sectionService.InsertAfterAsync(afterOrder, section.Adapt<Section>());
        return CreatedAtAction(nameof(GetSectionByRaceAsync), new { id = created.Id }, created.Adapt<SectionDto>());
    }
}