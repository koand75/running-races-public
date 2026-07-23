using System.Text.Json.Serialization;

namespace RunningRacesApi.Models;

public class RunnerSection
{
    public int Id { get; set; }
    public int SectionId { get; set; }

    [JsonIgnore]
    public Section? Section { get; set; } = null!;
    
    public int RunnerId { get; set; }

    [JsonIgnore]
    public Runner? Runner { get; set; } = null!;
    
    public int CustomPace { get; set; }
}


public class SaveRunnerSectionDto
{
    public int SectionId { get; set; }
    public int RunnerId { get; set; }
    public int CustomPace { get; set; }
}