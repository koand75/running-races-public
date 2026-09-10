namespace RunningRacesApi.Models.DTOs;

public class RaceCategoryTeamDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public DateTime? StartTime { get; set; }
    public string RaceName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public DateTime? RaceStartDate { get; set; }
}
