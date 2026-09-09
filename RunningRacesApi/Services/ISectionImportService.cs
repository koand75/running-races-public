using RunningRacesApi.Models.DTOs;

namespace RunningRacesApi.Services;

public interface ISectionImportService
{
    Task<SectionImportPreviewResultDto> PreviewAsync(int raceId, IFormFile file);

    Task<int> ImportAsync(Guid raceId, List<SectionImportDto> sectionsImport);
}