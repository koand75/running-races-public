using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface ISectionService
{
    Task<IEnumerable<Section>> GetAllByRaceAsync(Guid raceId);
    Task<Section?> GetSectionByRaceAsync(Guid raceId, int sectionId);
    Task<Section> CreateAsync(Section section);
    Task UpdateAsync(Section section);
    Task DeleteAsync(int id);
    Task<Section> InsertAfterAsync(int afterOrder, Section section);
    Task ReplaceAllAsync(List<Section> sections);
}