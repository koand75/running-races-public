namespace RunningRacesApi.Models.DTOs;

public class SectionImportPreviewResultDto
{
    public IEnumerable<SectionImportPreviewDto> Sections { get; set; } = new List<SectionImportPreviewDto>();
    public IEnumerable<WayPointIssueDto> WayPointIssues { get; set; } = new List<WayPointIssueDto>();
}
