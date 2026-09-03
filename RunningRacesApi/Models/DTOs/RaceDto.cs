using RunningRacesApi.Enums;

namespace RunningRacesApi.Models.DTOs;
public class RaceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double Distance { get; set; }
    public bool IsActive { get; set; }
    public RaceType RaceType { get; set; }
}