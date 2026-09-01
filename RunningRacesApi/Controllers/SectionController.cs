using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController(ISectionService sectionService, IMapper mapper) : ControllerBase
{
    private readonly ISectionService _sectionService = sectionService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SectionDto>>> GetAll()
    {
        var sections = await _sectionService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SectionDto>>(sections));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SectionDto>> GetById(int id)
    {
        var section = await _sectionService.GetByIdAsync(id);
        if (section == null) return NotFound();
        return Ok(_mapper.Map<SectionDto>(section));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SectionDto>> Create(SectionDto section)
    {
        var created = await _sectionService.CreateAsync(_mapper.Map<Section>(section));
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SectionDto>(created));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, SectionDto section)
    {
        if (id != section.Id) return BadRequest();
        await _sectionService.UpdateAsync(_mapper.Map<Section>(section));
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
    public async Task<ActionResult<SectionDto>> InsertAfter(int afterOrder, SectionDto section)
    {
        var created = await _sectionService.InsertAfterAsync(afterOrder, _mapper.Map<Section>(section));
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SectionDto>(created));
    }
}