using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Domin.Entity.Customers;
using System;

namespace Chamedoon.Infrastructure.FluentConfigs.Users;

public class UserConfigs : IEntityTypeConfiguration<User>
{
    private static readonly DateTime SeedTimestamp = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder
            .HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.Id);

        var adminUser = new User
        {
            Id = 1,
            UserName = "Fara",
            NormalizedUserName = "FARA",
            Email = "fara@chamedoon.local",
            NormalizedEmail = "FARA@CHAMEDOON.LOCAL",
            EmailConfirmed = true,
            PhoneNumber = "0000000000",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            Created = SeedTimestamp,
            LastModified = SeedTimestamp,
            SecurityStamp = "A7E549A7-3F8B-451E-91FA-1796BB7D35DD",
            ConcurrencyStamp = "7C4049E0-28D8-4F73-8B3B-521C8AA86E01",
            // Default password: Fara@123
            PasswordHash = "AQEAAAAQJwAADwAAACAAAAAvb3VjaGFtZWRvb24AAADwGfC0Y4T7PTPHl5K5BozOUhniKY8lf2LaALSFDyNS+g=="
        };

        var memberUser = new User
        {
            Id = 2,
            UserName = "MemberUser",
            NormalizedUserName = "MEMBERUSER",
            Email = "member@chamedoon.local",
            NormalizedEmail = "MEMBER@CHAMEDOON.LOCAL",
            EmailConfirmed = true,
            PhoneNumber = "0000000001",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            Created = SeedTimestamp,
            LastModified = SeedTimestamp,
            SecurityStamp = "B2C44D4B-6A05-4940-9F35-8840AD54ABCE",
            ConcurrencyStamp = "8A1F656C-5E34-47A9-9F77-EDBE4F400C53",
            // Default password: Member@123
            PasswordHash = "AQEAAAAQJwAADwAAACAAAABtZW1iZXJzYWx0MDAwMDDIAETRsvsP9Xirew+tTpAHhWVbhKuWDfkVWKfs6VtaEw=="
        };

        builder.HasData(adminUser, memberUser);
    }
}
