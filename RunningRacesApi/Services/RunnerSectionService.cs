using Microsoft.EntityFrameworkCore;
using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public class RunnerSectionService : IRunnerSectionService
{
    private readonly AppDbContext _context;

    public RunnerSectionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RunnerSection>> GetByTeamAsync(int teamId)
    {
        return await _context.RunnerSections
            .Include(rs => rs.Section)
            .Include(rs => rs.Runner)
            .Where(rs => rs.Runner.TeamId == teamId)
            .OrderBy(rs => rs.Section.Order)
            .ToListAsync();
    }

    public async Task SaveAllAsync(int teamId, List<RunnerSection> assignments)
    {
        var oldAssignments = await _context.RunnerSections
            .Where(rs => rs.Runner.TeamId == teamId)
            .ToListAsync();
        
        _context.RunnerSections.RemoveRange(oldAssignments);
        _context.RunnerSections.AddRange(assignments);
        
        await _context.SaveChangesAsync();
    }
}