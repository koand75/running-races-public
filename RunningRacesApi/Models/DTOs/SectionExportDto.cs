namespace RunningRacesApi.Models.DTOs;
public class SectionExportDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public double Distance { get; set; }
    public string? StartWayPointName { get; set; }
    public double? StartLat { get; set; }
    public double? StartLng { get; set; }
    public string? EndWayPointName { get; set; }
    public double? EndLat { get; set; }
    public double? EndLng { get; set; }
    public string? Description { get; set; }
}
