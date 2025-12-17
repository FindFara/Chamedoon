using System;
using System.Linq;
using System.Threading.Tasks;
using Chamedoon.Domin.Entity.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chamedoon.Infrastructure.Persistence;

public static class IdentitySeedExtensions
{
    private static readonly string[] DefaultRoles = { "Admin", "Member", "Manager" };

    public static async Task SeedIdentityDataAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<Role>>();

        await EnsureRolesAsync(roleManager);
    }

    private static async Task EnsureRolesAsync(RoleManager<Role> roleManager)
    {
        foreach (var roleName in DefaultRoles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var role = new Role
            {
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant()
            };

            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }

}
