namespace RunningRacesApi.Models.DTOs;
public class RunnerDto
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int BasePace { get; set; }
    public string? Notes { get; set; }
}
