using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface IRunnerService
{
    Task<IEnumerable<Runner>> GetByTeamAsync(int teamId);
    Task<Runner?> GetByIdAsync(int id);
    Task<Runner> CreateAsync(Runner runner);
    Task UpdateAsync(Runner runner);
    Task DeleteAsync(int id);
}