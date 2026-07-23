using RunningRacesApi.Services;

namespace RunningRacesApi.Middleware;

public class JwtBlacklistMiddleware
{
    private readonly RequestDelegate _next;

    public JwtBlacklistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token) && blacklistService.IsBlacklisted(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Token has been revoked");
            return;
        }

        await _next(context);
    }
}