using Microsoft.AspNetCore.Identity;

namespace RunningRacesApi.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}