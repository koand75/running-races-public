using Microsoft.Extensions.Caching.Memory;

namespace RunningRacesApi.Services;

public interface ITokenBlacklistService
{
    void AddToBlacklist(string token, DateTime expiresAt);
    bool IsBlacklisted(string token);
}

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly IMemoryCache _cache;

    public TokenBlacklistService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void AddToBlacklist(string token, DateTime expiresAt)
    {
        _cache.Set($"blacklist_{token}", true, expiresAt);
    }

    public bool IsBlacklisted(string token)
    {
        return _cache.TryGetValue($"blacklist_{token}", out _);
    }
}