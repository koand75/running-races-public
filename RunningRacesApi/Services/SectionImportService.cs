using AutoMapper;

using RunningRacesApi.Enums;
using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;

using System.Text;

namespace RunningRacesApi.Services;

public class SectionImportService : ISectionImportService
{
    private readonly ISectionService _sectionService;
    private readonly IWayPointService _wayPointService;
    private readonly IMapper _mapper;

    public SectionImportService(ISectionService sectionService, IWayPointService wayPointService, IMapper mapper)
    {
        _sectionService = sectionService;
        _wayPointService = wayPointService;
        _mapper = mapper;
    }

    public async Task<int> ImportAsync(List<SectionImportDto> sectionsImport)
    {
        var sections = new List<Section>();

        foreach (var item in sectionsImport)
        {
            var section = _mapper.Map<Section>(item);       
            var startWp = await _wayPointService.GetByIdAsync(section.StartWayPointId);
            var endWp = await _wayPointService.GetByIdAsync(section.EndWayPointId);

            if (startWp == null || endWp == null)
                throw new InvalidOperationException($"WayPoint not found for section {item.Order}");
            section.Name = $"{startWp.Name} - {endWp.Name}";
            sections.Add(section);
        }

        await _sectionService.ReplaceAllAsync(sections);

        return sections.Count;
    }

    public async Task<IEnumerable<SectionImportPreviewDto>> PreviewAsync(IFormFile file)
    {
        var sections = new List<SectionImportPreviewDto>();

        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);

        await reader.ReadLineAsync();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(';');

            var section = new SectionImportPreviewDto
            {
                Order = int.Parse(values[0]),
                Distance = double.Parse(values[1]),
                StartWayPointName = values[2],
                StartLat = double.Parse(values[3]),
                StartLng = double.Parse(values[4]),
                EndWayPointName = values[5],
                EndLat = double.Parse(values[6]),
                EndLng = double.Parse(values[7]),
                Description = values.Length > 8 ? values[8] : null
            };

            sections.Add(section);
        }

        var searchModel = new BaseSearchModel()
        {
            PageSize = int.MaxValue
        };

        var wayPoints = await _wayPointService.GetAllAsync(searchModel);
        var matched = Match(sections, wayPoints.Items);

        return matched;
    }

    public IEnumerable<SectionImportPreviewDto> Match(IEnumerable<SectionImportPreviewDto> sections, IEnumerable<WayPoint> wayPoints)
    {
        foreach (var section in sections)
        {
            var statusStart = WayPointMatchStatus.NotFound;
            var statusEnd = WayPointMatchStatus.NotFound;
            List<int> startWayPointIds = new List<int>();
            List<int> endWayPointIds = new List<int>();
            var oldStatusStart = WayPointMatchStatus.NotFound;
            var oldStatusEnd = WayPointMatchStatus.NotFound;

            foreach (var wayPoint in wayPoints)
            {
                statusStart = MatchPoint(section.StartWayPointName, section.StartLat, section.StartLng, wayPoint);

                if (statusStart == WayPointMatchStatus.Exact)
                {
                    startWayPointIds.Clear();
                    startWayPointIds.Add(wayPoint.Id);
                    break;
                }

                if (statusStart == WayPointMatchStatus.Partial)
                {
                    startWayPointIds.Add(wayPoint.Id);
                }

                if (oldStatusStart < statusStart)
                {
                    statusStart = oldStatusStart;
                }

                oldStatusStart = statusStart;
            }

            foreach (var wayPoint in wayPoints)
            {

                statusEnd = MatchPoint(section.EndWayPointName, section.EndLat, section.EndLng, wayPoint);

                if (statusEnd == WayPointMatchStatus.Exact)
                {
                    endWayPointIds.Clear();
                    endWayPointIds.Add(wayPoint.Id);
                    break;
                }

                if (statusEnd == WayPointMatchStatus.Partial)
                {
                    endWayPointIds.Add(wayPoint.Id);
                }

                if (oldStatusEnd < statusEnd)
                {
                    statusEnd = oldStatusEnd;
                }

                oldStatusEnd = statusEnd;
            }

            section.StartWayPointStatus = statusStart;
            section.EndWayPointStatus = statusEnd;
            section.MatchedStartWayPointIds = startWayPointIds;
            section.MatchedEndWayPointIds = endWayPointIds;
        }

        return sections;
    }

    private WayPointMatchStatus MatchPoint(string? name, double? lat, double? lng, WayPoint wayPoint)
    {
        bool nameMatch = wayPoint.Name == name;
        bool coordMatch = Math.Abs((wayPoint.Lat ?? 0) - (lat ?? 0)) < 0.0001 &&
                  Math.Abs((wayPoint.Lng ?? 0) - (lng ?? 0)) < 0.0001;

        if (nameMatch && coordMatch) return WayPointMatchStatus.Exact;
        if (nameMatch || coordMatch) return WayPointMatchStatus.Partial;
        return WayPointMatchStatus.NotFound;
    }
}