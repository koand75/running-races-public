using RunningRacesApi.Data;
using RunningRacesApi.Models;

using System.Text;

namespace RunningRacesApi.Services;

public class SectionImportService : ISectionImportService
{
    private readonly AppDbContext _context;

    public SectionImportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> ImportAsync(IFormFile file)
    {
        var sections = new List<Section>();

        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);

        await reader.ReadLineAsync();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(';');

            var section = new Section
            {
                Order = int.Parse(values[0]),
                Distance = double.Parse(values[1]),
                //StartPoint = values[2],
                //EndPoint = values[3],
                Description = values.Length > 4 ? values[4] : null,
                Name = $"{values[2]} - {values[3]}"
            };

            sections.Add(section);
        }

        _context.Sections.RemoveRange(_context.Sections);
        _context.Sections.AddRange(sections);
        await _context.SaveChangesAsync();

        return sections.Count;
    }
}