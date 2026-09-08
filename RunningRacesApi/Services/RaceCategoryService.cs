using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public class RaceCategoryService(AppDbContext context) : IRaceCategoryService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<RaceCategory>> GetAllByRaceAsync(Guid raceId)
    {
        return await _context.RaceCategory
            .Where(c => c.RaceId == raceId)
            .ToListAsync();
    }

    public async Task<RaceCategory> CreateAsync(RaceCategory category)
    {
        _context.RaceCategory.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(int id, RaceCategory category)
    {
        var existing = await _context.RaceCategory.FindAsync(id);
        if (existing == null) return;
        existing.Name = category.Name;
        existing.RaceType = category.RaceType;
        existing.Measurement = category.Measurement;
        existing.Distance = category.Distance;
        existing.Duration = category.Duration;
        existing.StartDateTime = category.StartDateTime;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.RaceCategory.FindAsync(id);
        if (category == null) return;
        _context.RaceCategory.Remove(category);
        await _context.SaveChangesAsync();
    }
}