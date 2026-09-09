using RunningRacesApi.Models.DTOs;

namespace RunningRacesApi.Services;

public interface ISectionImportService
{
    Task<SectionImportPreviewResultDto> PreviewAsync(int categoryId, IFormFile file);

    Task<int> ImportAsync(int categoryId, List<SectionImportDto> sectionsImport);
}