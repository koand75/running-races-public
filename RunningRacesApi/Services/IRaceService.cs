using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

/// <summary>
/// Race szolgáltatás interface - üzleti logika
/// </summary>
public interface IRaceService
{
    /// <summary>
    /// Public versenyek lekérdezése (csak aktívak)
    /// </summary>
    Task<PagedResult<Race>> GetPublicRacesAsync(RaceSearchModel searchModel);

    /// <summary>
    /// Admin versenyek lekérdezése (összes: aktív + inaktív)
    /// </summary>
    Task<PagedResult<Race>> GetAdminRacesAsync(RaceSearchModel searchModel);

    /// <summary>
    /// Verseny lekérdezése ID alapján
    /// </summary>
    Task<Race?> GetRaceByIdAsync(Guid id);

    /// <summary>
    /// Új verseny létrehozása
    /// </summary>
    Task<Race> CreateRaceAsync(Race race);

    /// <summary>
    /// Verseny módosítása
    /// </summary>
    Task<Race> UpdateRaceAsync(Guid id, Race race);

    /// <summary>
    /// Verseny törlése (soft delete)
    /// </summary>
    Task<bool> DeleteRaceAsync(Guid id);

    Task RestoreRaceAsync(Guid id);
}