namespace RunningRacesApi.Models;

public class RaceSearchModel : BaseSearchModel
{
    public bool? IsActive { get; set; }  // null = csak aktív, true = aktív, false = inaktív
}