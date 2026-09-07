using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface IRunnerSectionService
{
    Task<IEnumerable<RunnerSection>> GetByTeamAsync(Guid raceId, int teamId);
    Task SaveAllAsync(Guid raceId, int teamId, List<RunnerSection> assignments);
}