using RunningRacesApi.Models;
using RunningRacesApi.Repositories;

namespace RunningRacesApi.Services;

public class WayPointService : IWayPointService
{
    private readonly IWayPointRepository _repository;

    public WayPointService(IWayPointRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<WayPoint>> GetAllAsync(BaseSearchModel? searchModel = null)
    {
        return await _repository.GetAllAsync(searchModel);
    }

    public async Task<WayPoint> CreateAsync(WayPoint wayPoint)
    {
        return await _repository.CreateAsync(wayPoint);
    }

    public async Task<WayPoint?> UpdateAsync(int id, WayPoint wayPoint)
    {
        return await _repository.UpdateAsync(id, wayPoint);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}