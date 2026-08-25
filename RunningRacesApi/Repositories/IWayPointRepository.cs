using RunningRacesApi.Models;

namespace RunningRacesApi.Repositories;

/// <summary>
/// Futóversenyek repository interface
/// </summary>
public interface IWayPointRepository
{
    /// <summary>
    /// Összes váltópont lekérdezése keresési paraméterekkel
    /// </summary>
    Task<PagedResult<WayPoint>> GetAllAsync(BaseSearchModel? searchModel = null);
    Task<WayPoint> CreateAsync(WayPoint wayPoint);

    Task<WayPoint?> GetByIdAsync(int? id);
    Task<WayPoint?> UpdateAsync(int id, WayPoint wayPoint);
    Task<bool> DeleteAsync(int id);
}