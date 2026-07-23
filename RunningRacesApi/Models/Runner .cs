namespace RunningRacesApi.Models;

public class Runner : BaseEntity
{
    public int TeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int BasePace { get; set; }
    public string? Notes { get; set; }

    public ICollection<RunnerSection> Assignments { get; set; } = new List<RunnerSection>();
}