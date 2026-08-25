using RunningRacesApi.Models.DTOs;

namespace RunningRacesApi.Services;

public interface ISectionImportService
{  
    Task<IEnumerable<SectionImportPreviewDto>> PreviewAsync(IFormFile file);

    Task<int> ImportAsync(List<SectionImportDto> sectionsImport);
}