using RunningRacesApi.Models.DTOs;

namespace RunningRacesApi.Services;

public interface ISectionImportService
{
    Task<SectionImportPreviewResultDto> PreviewAsync(IFormFile file);

    Task<int> ImportAsync(List<SectionImportDto> sectionsImport);
}