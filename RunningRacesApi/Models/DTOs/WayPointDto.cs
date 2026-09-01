namespace RunningRacesApi.Models.DTOs;

public class WayPointDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double? Lat { get; set; }
    public double? Lng { get; set; }
}
