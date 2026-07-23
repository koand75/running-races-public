using RunningRacesApi.Models;

namespace RunningRacesApi.Repositories;

/// <summary>
/// Futóversenyek repository interface
/// </summary>
public interface IRaceRepository
{
    /// <summary>
    /// Összes verseny lekérdezése keresési paraméterekkel
    /// </summary>
    Task<PagedResult<Race>> GetRacesAsync(RaceSearchModel searchModel);

    /// <summary>
    /// Egy konkrét verseny lekérdezése ID alapján
    /// </summary>
    Task<Race?> GetByIdAsync(Guid id);

    /// <summary>
    /// Új verseny létrehozása
    /// </summary>
    Task<Race> CreateAsync(Race race);

    /// <summary>
    /// Verseny módosítása
    /// </summary>
    Task<Race> UpdateAsync(Guid id, Race race);

    /// <summary>
    /// Verseny törlése (soft delete)
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}