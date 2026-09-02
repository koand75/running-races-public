namespace RunningRacesApi.Models.DTOs;

public class RunnerSectionDto
{
    public int? Id { get; set; }
    public int SectionId { get; set; }
    public int RunnerId { get; set; }
    public int CustomPace { get; set; }
    public SectionDto? Section { get; set; }
    public RunnerDto? Runner { get; set; }
}
