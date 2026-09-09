namespace RunningRacesApi.Models.DTOs;

public class RaceCategoryDropdownDto
{
    public Guid RaceId { get; set; }
    public string RaceName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
