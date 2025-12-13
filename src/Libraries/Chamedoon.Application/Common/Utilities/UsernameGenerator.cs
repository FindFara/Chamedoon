using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Domin.Entity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Common.Utilities;

public static class UsernameGenerator
{
    public static async Task<string> GenerateUniqueAsync(UserManager<User> userManager, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userManager);

        string username;
        do
        {
            username = $"U{RandomNumberGenerator.GetInt32(0, 100_000_000):D8}";
        }
        while (await userManager.Users.AnyAsync(u => u.UserName == username, cancellationToken));

        return username;
    }
}
