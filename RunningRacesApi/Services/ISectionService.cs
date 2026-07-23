using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface ISectionService
{
    Task<IEnumerable<Section>> GetAllAsync();
    Task<Section?> GetByIdAsync(int id);
    Task<Section> CreateAsync(Section section);
    Task UpdateAsync(Section section);
    Task DeleteAsync(int id);
    Task<Section> InsertAfterAsync(int afterOrder, Section section);
}