namespace RunningRacesApi.Services;

public interface ISectionImportService
{
    Task<int> ImportAsync(IFormFile file);
}