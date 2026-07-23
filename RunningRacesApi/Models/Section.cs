using System.ComponentModel.DataAnnotations.Schema;

namespace RunningRacesApi.Models;

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }

    [ForeignKey("StartWayPoint")]
    public int? StartWayPointId { get; set; }

    [ForeignKey("EndWayPoint")]
    public int? EndWayPointId { get; set; }
    public WayPoint? StartWayPoint { get; set; }
    public WayPoint? EndWayPoint { get; set; }
}