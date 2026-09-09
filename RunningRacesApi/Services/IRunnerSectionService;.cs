using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface IRunnerSectionService
{
    Task<IEnumerable<RunnerSection>> GetByTeamAsync(int categoryId, int teamId);
    Task SaveAllAsync(int categoryId, int teamId, List<RunnerSection> assignments);
}