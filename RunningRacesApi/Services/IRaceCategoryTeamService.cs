using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface IRaceCategoryTeamService
{
    Task<IEnumerable<RaceCategoryTeam>> GetByCategoryAsync(int categoryId);
    Task<RaceCategoryTeam> CreateAsync(RaceCategoryTeam entry);
    Task DeleteAsync(int id);
}
