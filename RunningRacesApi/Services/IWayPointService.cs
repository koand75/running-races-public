using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface IWayPointService
{
    Task<PagedResult<WayPoint>> GetAllAsync(BaseSearchModel? searchModel = null);
    Task<WayPoint> CreateAsync(WayPoint wayPoint);
    Task<WayPoint?> UpdateAsync(int id, WayPoint wayPoint);
    Task<bool> DeleteAsync(int id);
}