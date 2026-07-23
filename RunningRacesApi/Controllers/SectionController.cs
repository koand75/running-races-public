using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Section>>> GetAll()
    {
        var sections = await _sectionService.GetAllAsync();
        return Ok(sections);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Section>> GetById(int id)
    {
        var section = await _sectionService.GetByIdAsync(id);
        if (section == null) return NotFound();
        return Ok(section);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Section>> Create(Section section)
    {
        var created = await _sectionService.CreateAsync(section);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Section section)
    {
        if (id != section.Id) return BadRequest();
        await _sectionService.UpdateAsync(section);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _sectionService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("insert-after/{afterOrder}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Section>> InsertAfter(int afterOrder, Section section)
    {
        var created = await _sectionService.InsertAfterAsync(afterOrder, section);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}