namespace RunningRacesApi.Models;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }

    public ICollection<Runner> Runners { get; set; } = new List<Runner>();

    public DateTime? StartTime { get; set; }
}