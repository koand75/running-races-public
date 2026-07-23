using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public class SectionService : ISectionService
{
    private readonly AppDbContext _context;

    public SectionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Section>> GetAllAsync()
    {
        return await _context.Sections
            .Include(x => x.EndWayPoint)
            .Include(x => x.StartWayPoint)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<Section?> GetByIdAsync(int id)
    {
        return await _context.Sections
                    .Include(x => x.StartWayPoint)
                    .Include(x => x.EndWayPoint)
                    .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Section> CreateAsync(Section section)
    {
        var startWp = await _context.WayPoints.FindAsync(section.StartWayPointId);
        var endWp = await _context.WayPoints.FindAsync(section.EndWayPointId);
        section.Name = $"{startWp?.Name} - {endWp?.Name}";
        _context.Sections.Add(section);
        await _context.SaveChangesAsync();
        return section;
    }

    public async Task UpdateAsync(Section section)
    {
        var startWp = await _context.WayPoints.FindAsync(section.StartWayPointId);
        var endWp = await _context.WayPoints.FindAsync(section.EndWayPointId);
        section.Name = $"{startWp?.Name} - {endWp?.Name}";
        _context.Sections.Update(section);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var section = await _context.Sections.FindAsync(id);
        if (section != null)
        {
            var sectionsToShift = await _context.Sections
                .Where(s => s.Order > section.Order)
                .ToListAsync();

            foreach (var s in sectionsToShift)
                s.Order--;

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Section> InsertAfterAsync(int afterOrder, Section section)
    {
        var sectionsToShift = await _context.Sections
            .Where(s => s.Order > afterOrder)
            .ToListAsync();

        foreach (var s in sectionsToShift)
            s.Order++;

        section.Order = afterOrder + 1;
        section.Name = $"{section.StartWayPoint?.Name} - {section.EndWayPoint?.Name}";
        _context.Sections.Add(section);

        await _context.SaveChangesAsync();
        return section;
    }
}