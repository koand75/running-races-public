using RunningRacesApi.Enums;

namespace RunningRacesApi.Models;

public class RaceCategory
{
    public int Id { get; set; }
    public Guid RaceId { get; set; }
    public Race? Race { get; set; }
    public string Name { get; set; } = string.Empty;
    public RaceType RaceType { get; set; }
    public MeasurementType Measurement { get; set; }
    public double? Distance { get; set; }
    public int? Duration { get; set; }
    public DateTime? StartDateTime { get; set; }
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}
