using RunningRacesApi.Models;

namespace RunningRacesApi.Repositories;

/// <summary>
/// Futóversenyek repository interface
/// </summary>
public interface IWayPointRepository
{
    Task<PagedResult<WayPoint>> GetAllByCategoryAsync(int categoryId, BaseSearchModel? searchModel);
    Task<WayPoint> CreateAsync(WayPoint wayPoint);
    Task<WayPoint?> GetByIdAsync(int? id);
    Task<WayPoint?> UpdateAsync(int id, WayPoint wayPoint);
    Task<bool> DeleteAsync(int id);
}