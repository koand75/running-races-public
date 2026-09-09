using Mapster;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/race/{raceId}/category/{categoryId}/section")]
public class RaceSectionController(ISectionService sectionService) : ControllerBase
{
    private readonly ISectionService _sectionService = sectionService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SectionDto>>> GetAll(int categoryId)
    {       
        var sections = await _sectionService.GetAllByCategoryAsync(categoryId);
        return Ok(sections.Adapt<IEnumerable<SectionDto>>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SectionDto>> GetSectionByRaceAsync(int categoryId, int id)
    {
        var section = await _sectionService.GetSectionByCategoryAsync(categoryId, id);
        if (section == null) return NotFound();
        return Ok(section.Adapt<SectionDto>());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SectionDto>> Create(int categoryId, SectionDto section)
    {
        section.CategoryId = categoryId;
        var created = await _sectionService.CreateAsync(section.Adapt<Section>());
        return CreatedAtAction(nameof(GetSectionByRaceAsync), new { categoryId, id = created.Id }, created.Adapt<SectionDto>());
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int categoryId, int id, SectionDto section)
    {
        if (id != section.Id) return BadRequest();
        section.CategoryId = categoryId;
        await _sectionService.UpdateAsync(section.Adapt<Section>());
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int categoryId, int id)
    {
        await _sectionService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("insert-after/{afterOrder}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SectionDto>> InsertAfter(int categoryId, int afterOrder, SectionDto section)
    {
        section.CategoryId = categoryId;
        var created = await _sectionService.InsertAfterAsync(afterOrder, section.Adapt<Section>());
        return CreatedAtAction(nameof(GetSectionByRaceAsync), new { id = created.Id }, created.Adapt<SectionDto>());
    }
}