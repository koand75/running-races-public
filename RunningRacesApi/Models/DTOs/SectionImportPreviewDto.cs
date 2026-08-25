using RunningRacesApi.Enums;

namespace RunningRacesApi.Models.DTOs;
public class SectionImportPreviewDto :SectionExportDto
{
    public WayPointMatchStatus StartWayPointStatus { get; set; } 
    public WayPointMatchStatus EndWayPointStatus { get; set; } 
    public List<int> MatchedStartWayPointIds { get; set; } = new();
    public List<int> MatchedEndWayPointIds { get; set; } = new();
}
