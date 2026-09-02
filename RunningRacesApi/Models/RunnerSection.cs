namespace RunningRacesApi.Models;

public class RunnerSection
{
    public int Id { get; set; }
    public int SectionId { get; set; }

    public Section? Section { get; set; } = null!;
    
    public int RunnerId { get; set; }

    public Runner? Runner { get; set; } = null!;
    
    public int CustomPace { get; set; }
}
