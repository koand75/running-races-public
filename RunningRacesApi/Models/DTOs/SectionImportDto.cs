namespace RunningRacesApi.Models.DTOs;

public class SectionImportDto
{
    public int Order { get; set; }
    public double Distance { get; set; }
    public string? Description { get; set; }
    public int? StartWayPointId { get; set; }
    public int? EndWayPointId { get; set; }
}
