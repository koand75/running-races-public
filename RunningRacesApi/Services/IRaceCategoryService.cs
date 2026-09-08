using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface IRaceCategoryService
{
    Task<IEnumerable<RaceCategory>> GetAllByRaceAsync(Guid raceId);
    Task<RaceCategory> CreateAsync(RaceCategory category);
    Task UpdateAsync(int id, RaceCategory category);
    Task DeleteAsync(int id);
}
