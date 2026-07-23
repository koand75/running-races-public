using Microsoft.EntityFrameworkCore;

using RunningRacesApi.Data;
using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public class RunnerService : IRunnerService
{
    private readonly AppDbContext _context;

    public RunnerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Runner>> GetByTeamAsync(int teamId)
    {
        return await _context.Runners
            .Where(r => r.TeamId == teamId && r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    public async Task<Runner?> GetByIdAsync(int id)
    {
        return await _context.Runners
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
    }

    public async Task<Runner> CreateAsync(Runner runner)
    {
        runner.CreatedAt = DateTime.UtcNow;
        _context.Runners.Add(runner);
        await _context.SaveChangesAsync();
        return runner;
    }

    public async Task UpdateAsync(Runner runner)
    {
        runner.ModifiedAt = DateTime.UtcNow;
        _context.Runners.Update(runner);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var runner = await _context.Runners.FindAsync(id);
        if (runner != null)
        {
            runner.IsActive = false;
            runner.ModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}