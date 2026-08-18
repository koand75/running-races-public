namespace RunningRacesApi.Models;

public class BaseSearchModel
{
    public string? SearchTerm { get; set; }
    public string? SearchField { get; set; }  
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}