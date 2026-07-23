namespace RunningRacesApi.Models;

public class RaceSearchModel
{
    public string? SearchTerm { get; set; }
    public string? SearchField { get; set; }  // "name", "location", "all"
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public bool? IsActive { get; set; }  // null = csak aktív, true = aktív, false = inaktív
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

}