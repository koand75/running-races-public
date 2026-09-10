using RunningRacesApi.Models;

namespace RunningRacesApi.Services;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
