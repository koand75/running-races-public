namespace RunningRacesApi.Models.DTOs;
public class SectionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public int? StartWayPointId { get; set; }
    public int? EndWayPointId { get; set; }
    public WayPointDto? StartWayPoint { get; set; }
    public WayPointDto? EndWayPoint { get; set; }
}
