namespace RunningRacesApi.Models;

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public int? StartWayPointId { get; set; }
    public int? EndWayPointId { get; set; }
    public WayPoint? StartWayPoint { get; set; }
    public WayPoint? EndWayPoint { get; set; }
    public Guid? RaceId { get; set; }
    public Race? Race { get; set; }
}