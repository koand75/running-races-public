using Microsoft.AspNetCore.Identity;

using RunningRacesApi.Models;

using System.Data;

namespace RunningRacesApi.Data;

public class DatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(ILogger<DatabaseSeeder> logger)
    {
        _logger = logger;
    }

    public async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 1. Create Roles
        await SeedRoles(roleManager);

        // 2. Create Users with Roles
        var usersToSeed = new[]
        {
            new { Email = "admin@runningraceandi.com", UserName = "Admin", FullName = "Administrator", Password = "Admin123!", Role = "Admin" },
            new { Email = "test@runningraceandi.com", UserName = "Test_User", FullName = "Test User", Password = "Test123!", Role = "User" }
        };

        foreach (var userData in usersToSeed)
        {
            await SeedUser(userManager, userData.Email, userData.UserName, userData.FullName, userData.Password, userData.Role);
        }
    }

    private async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                _logger.LogInformation($"✅ Role '{roleName}' created");
            }
        }
    }

    private async Task SeedUser(
        UserManager<ApplicationUser> userManager, string email, string userName, string fullName, string password, string role)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        ApplicationUser user;

        if (existingUser == null)
        {
            // Create new user
            user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                _logger.LogError($"❌ Failed to create user '{email}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return;
            }

            _logger.LogInformation($"✅ User '{email}' created");
        }
        else
        {
            user = existingUser;
        }

        // User exists - check and add role if missing
        await AddRoleSeedUserWithLogs(userManager, user, role, email);
    }

    private async Task AddRoleSeedUserWithLogs(UserManager<ApplicationUser> userManager, ApplicationUser user, string role, string email)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        if (!userRoles.Contains(role))
        {
            await userManager.AddToRoleAsync(user, role);
            _logger.LogInformation($"✅ Role '{role}' added to existing user '{email}'");
        }
        else
        {
            _logger.LogInformation($"ℹ️ User '{email}' already has role '{role}'");
        }
    }
}