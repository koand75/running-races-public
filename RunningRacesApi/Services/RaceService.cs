using RunningRacesApi.Models;
using RunningRacesApi.Repositories;

namespace RunningRacesApi.Services;

public class RaceService : IRaceService
{
    private readonly IRaceRepository _repository;

    public RaceService(IRaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<Race>> GetPublicRacesAsync(RaceSearchModel searchModel)
    {
        searchModel.SearchField ??= "all";
        searchModel.SortBy ??= "date";
        searchModel.SortDirection ??= "asc";

        searchModel.IsActive = true;

        return await _repository.GetRacesAsync(searchModel);
    }

    public async Task<PagedResult<Race>> GetAdminRacesAsync(RaceSearchModel searchModel)
    {
        searchModel.SearchField ??= "all";
        searchModel.SortBy ??= "date";
        searchModel.SortDirection ??= "asc";

        return await _repository.GetRacesAsync(searchModel);
    }

    public async Task<Race?> GetRaceByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid race ID.", nameof(id));
        }

        return await _repository.GetByIdAsync(id);
    }

    public async Task<Race> CreateRaceAsync(Race race)
    {
        ValidateRace(race);

        race.Id = Guid.NewGuid();
        race.CreatedAt = DateTime.UtcNow;
        race.IsActive = true;

        return await _repository.CreateAsync(race);
    }

    public async Task<Race> UpdateRaceAsync(Guid id, Race race)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid race ID.", nameof(id));
        }

        ValidateRace(race);

        var existingRace = await _repository.GetByIdAsync(id);
        if (existingRace == null)
        {
            throw new InvalidOperationException($"Race with ID {id} not found.");
        }

        return await _repository.UpdateAsync(id, race);
    }

    public async Task<bool> DeleteRaceAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid race ID.", nameof(id));
        }

        return await _repository.DeleteAsync(id);
    }

    private void ValidateRace(Race race)
    {
        if (string.IsNullOrWhiteSpace(race.Name))
        {
            throw new ArgumentException("Race name is required.", nameof(race.Name));
        }

        if (string.IsNullOrWhiteSpace(race.Location))
        {
            throw new ArgumentException("Race location is required.", nameof(race.Location));
        }

        if (race.Distance <= 0)
        {
            throw new ArgumentException("Distance must be positive.", nameof(race.Distance));
        }

        if (race.Distance > 500)
        {
            throw new ArgumentException("Distance cannot exceed 500 km.", nameof(race.Distance));
        }
    }

    public async Task RestoreRaceAsync(Guid id)
    {
        var race = await _repository.GetByIdAsync(id);
        if (race == null) throw new InvalidOperationException("Verseny nem található");
        race.IsActive = true;
        await _repository.UpdateAsync(id, race);
    }
}