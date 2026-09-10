namespace RunningRacesApi.Models;

public class RaceCategoryTeam
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public RaceCategory? Category { get; set; }
    public int TeamId { get; set; }
    public Team? Team { get; set; }
    public DateTime? StartTime { get; set; }
}
