namespace RunningRacesApi.Services;

public interface ICsvExportService
{
    string Export<T>(IEnumerable<T> data, IEnumerable<string> columns);
}
