namespace RunningRacesApi.Models;

public class Team : BaseEntity
{
    /// <summary>
    /// Team name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public int Year { get; set; }

    /// <summary>
    /// List of member
    /// </summary>
    public ICollection<Runner> Runners { get; set; } = new List<Runner>();
}