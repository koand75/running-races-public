namespace RunningRacesApi.Models.DTOs;

public class RaceCategoryTeamDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public DateTime? StartTime { get; set; }
}
