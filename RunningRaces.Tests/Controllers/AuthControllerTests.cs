using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Moq;

using RunningRacesApi.Controllers;
using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRaces.Tests.Controllers;

/// <summary>
/// AuthController unit tests
/// Test login endpoint with various scenarios
/// </summary>
public class AuthControllerTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly AuthController _controller;
    private readonly Mock<ITokenBlacklistService> _mockBlacklistService;

    public AuthControllerTests()
    {
        // Mock configuration (JWT settings)
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup JWT configuration values
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("SuperSecretKeyForTestingPurposesOnly123456");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
        _mockConfiguration.Setup(c => c["Jwt:ExpireMinutes"]).Returns("60");
        _mockBlacklistService = new Mock<ITokenBlacklistService>();

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
           userStore.Object, null, null, null, null, null, null, null, null
       );

        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
           _mockUserManager.Object,
           new Mock<IHttpContextAccessor>().Object,
           new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
           null, null, null, null
       );

        // Controller with mocked configuration
        _controller = new AuthController(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockConfiguration.Object,
            _mockBlacklistService.Object);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "admin",
            Password = "admin123"
        };

        // Mock user
        var user = new ApplicationUser { Email = "admin", UserName = "admin" };

        _mockUserManager
            .Setup(x => x.FindByEmailAsync("admin"))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Admin" });

        // Mock successful password check
        _mockSignInManager
            .Setup(x => x.CheckPasswordSignInAsync(user, "admin123", false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        // Token objektum ellenőrzése
        var response = okResult.Value;
        response.Should().NotBeNull();

        // Token property létezik
        var tokenProperty = response!.GetType().GetProperty("token");
        tokenProperty.Should().NotBeNull();

        var token = tokenProperty!.GetValue(response) as string;
        token.Should().NotBeNullOrEmpty();
        token.Should().StartWith("eyJ");  // JWT token mindig ezzel kezdődik
    }

    [Theory]
    [InlineData("wronguser", "Test123!")]
    [InlineData("test@runningraceandi.com", "wrongpassword")]
    [InlineData(null, "Test123!")]
    [InlineData("", "Test123!")]
    [InlineData("   ", "Test123!")]
    [InlineData("test@runningraceandi.com", null)]
    [InlineData("test@runningraceandi.com", "")]
    [InlineData("test@runningraceandi.com", "   ")]
    public async Task Login_WithInvalidUsernameOrPassword_ReturnsUnauthorized(string? email, string? password)
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = email,
            Password = password
        };

        var user = new ApplicationUser { Email = "test@runningraceandi.com", UserName = "Test123!" };

        // Ezen a ponton a _mockUserManager már inicializált
        _mockUserManager
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) =>
                email == "test@runningraceandi.com" ? user : null);

        _mockSignInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public void Logout_WithValidToken_ReturnsOk()
    {
        // Arrange
        var token = "fake-jwt-token";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        _controller.HttpContext.Request.Headers["Authorization"] = $"Bearer {token}";

        _mockConfiguration.Setup(c => c["Jwt:ExpireMinutes"]).Returns("60");

        // Act
        var result = _controller.Logout();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockBlacklistService.Verify(
            x => x.AddToBlacklist(token, It.IsAny<DateTime>()),
            Times.Once
        );
    }
}