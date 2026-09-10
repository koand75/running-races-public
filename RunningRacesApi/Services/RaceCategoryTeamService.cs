using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public class RaceCategoryTeamService(AppDbContext context) : IRaceCategoryTeamService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<RaceCategoryTeam>> GetByCategoryAsync(int categoryId)
    {
        return await _context.RaceCategoryTeams
            .Include(x => x.Team)
            .Include(x => x.Category)
                .ThenInclude(c => c.Race)
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<RaceCategoryTeam> CreateAsync(RaceCategoryTeam entry)
    {
        _context.RaceCategoryTeams.Add(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task DeleteAsync(int id)
    {
        var entry = await _context.RaceCategoryTeams.FindAsync(id);
        if (entry != null)
        {
            _context.RaceCategoryTeams.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
