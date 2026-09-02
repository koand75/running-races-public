namespace RunningRacesApi.Models.DTOs;

public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateTime? StartTime { get; set; }
    public List<RunnerDto> Runners { get; set; } = new();
}