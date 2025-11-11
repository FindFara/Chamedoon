using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Domin.Entity.Permissions;
using Microsoft.AspNetCore.Identity;
using Chamedoon.Infrastructure.Persistence;
using Chamedoon.Application.Common.Utilities.CustomizIdentity;
using Chamedoon.Infrastructure.Repositories.Admin;

namespace Chamedoon.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        /*

         Add-Migration AddUser -OutputDir Migrations -Context ApplicationDbContext -Project Chamedoon.Infrastructure -Args '--environment Production'

         Update-Database -Args '--environment Production'

         dotnet ef migrations add migrationsname --verbose --project "src/Libraries/Chamedoon.Infrastructure/Chamedoon.Infrastructure.csproj" --startup-project "src/Presentation/Chamedoon.WebAPI/Chamedoon.WebAPI.csproj" --context ApplicationDbContext -- --environment Chamedoon

        dotnet ef database update -- --environment Production

         */

        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("ChamedoonConnection"));
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IAdminBlogRepository, AdminBlogRepository>();
        services.AddScoped<IAdminRoleRepository, AdminRoleRepository>();

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<PersianIdentityErrorDescriber>();
        return services;

    }
}
