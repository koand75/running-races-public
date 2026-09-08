namespace RunningRacesApi.Models.DTOs;
public class RaceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string CategorySummary { get; set; } = string.Empty;
}