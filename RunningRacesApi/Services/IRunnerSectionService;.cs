using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface IRunnerSectionService
{
    Task<IEnumerable<RunnerSection>> GetByTeamAsync(int teamId);
    Task SaveAllAsync(int teamId, List<RunnerSection> assignments);
}