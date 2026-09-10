using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

using System.Security.Claims;

namespace RunningRacesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ITokenBlacklistService blacklistService,
        ITokenService tokenService) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly ITokenBlacklistService _blacklistService = blacklistService;
    private readonly ITokenService _tokenService = tokenService;

    /// <summary>
    /// Login and get JWT token
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        // Find user by email
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Check password
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Generate JWT token
        var token = await GenerateJwtTokenAsync(user);

        return Ok(new { token });
    }

    /// <summary>
    /// Generate JWT token for user
    /// </summary>
    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        return await _tokenService.GenerateTokenAsync(user);
    }

    /// <summary>
    /// Logout - invalidate token
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var expireMinutes = Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]);
        var expiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);

        _blacklistService.AddToBlacklist(token, expiresAt);

        return Ok(new { message = "Successfully logged out" });
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return Unauthorized();

        var newToken = await _tokenService.GenerateTokenAsync(user); ;
        return Ok(new { token = newToken });
    }
}