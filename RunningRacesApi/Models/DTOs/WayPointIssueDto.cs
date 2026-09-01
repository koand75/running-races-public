using RunningRacesApi.Enums;

namespace RunningRacesApi.Models.DTOs;

public class WayPointIssueDto
{
    public string Name { get; set; } = string.Empty;
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public WayPointMatchStatus Status { get; set; }
    public List<int> MatchedIds { get; set; } = new();
}