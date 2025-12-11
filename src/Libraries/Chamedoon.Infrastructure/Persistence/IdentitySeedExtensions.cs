using System;
using System.Linq;
using System.Threading.Tasks;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Users;
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
        var userManager = services.GetRequiredService<UserManager<User>>();

        await EnsureRolesAsync(roleManager);
        await EnsureAdminAsync(userManager);
        await EnsureMemberAsync(userManager);
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

    private static async Task EnsureAdminAsync(UserManager<User> userManager)
    {
        var admin = await userManager.FindByNameAsync("Fara");
        if (admin == null)
        {
            admin = new User
            {
                UserName = "Fara",
                Email = "fara@chamedoon.local",
                EmailConfirmed = true,
                PhoneNumber = "0000000000",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            var createResult = await userManager.CreateAsync(admin, "Fara@123");
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            admin.LockoutEnabled = true;
            admin.EmailConfirmed = true;
            admin.LastModified = DateTime.UtcNow;
            var updateResult = await userManager.UpdateAsync(admin);
            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update admin user: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to assign admin role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    private static async Task EnsureMemberAsync(UserManager<User> userManager)
    {
        var member = await userManager.FindByNameAsync("MemberUser");
        if (member == null)
        {
            member = new User
            {
                UserName = "MemberUser",
                Email = "member@chamedoon.local",
                EmailConfirmed = true,
                PhoneNumber = "0000000001",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            var createResult = await userManager.CreateAsync(member, "Member@123");
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create member user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            member.LockoutEnabled = true;
            member.EmailConfirmed = true;
            member.LastModified = DateTime.UtcNow;
            var updateResult = await userManager.UpdateAsync(member);
            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update member user: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(member, "Member"))
        {
            var roleResult = await userManager.AddToRoleAsync(member, "Member");
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to assign member role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }
}
