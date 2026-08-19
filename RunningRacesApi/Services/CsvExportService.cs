
using System.Text;

namespace RunningRacesApi.Services;

public class CsvExportService : ICsvExportService
{
    public string Export<T>(IEnumerable<T> data, IEnumerable<string> columns)
    {
        var sb = new StringBuilder();
        var columnList = columns.ToList();
        var properties = typeof(T).GetProperties()
            .Where(x => columnList.Contains(x.Name))
            .OrderBy(x => columnList.IndexOf(x.Name))
            .ToList();

        sb.AppendLine(string.Join(";", columnList));

        foreach (var item in data)
        {
            var values = properties.Select(x => x.GetValue(item)?.ToString() ?? "");
            sb.AppendLine(string.Join(";", values));
        }
        
        return sb.ToString();
    }
}
