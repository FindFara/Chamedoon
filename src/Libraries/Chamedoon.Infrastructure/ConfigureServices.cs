using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Entity.User;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Application.Utilities.CustomizIdentity;
using Microsoft.AspNetCore.Identity;
using Chamedoon.Infrastructure.Persistence;

namespace Chamedoon.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        /*   

         Add-Migration Name -OutputDir Migrations -Context ApplicationDbContext -Project Chamedoon.Infrastructure
         Update-Database  -Context ApplicationDbContext 
         dotnet ef migrations add migrationsname -p Chamedoon.Infrastructure -s Chamedoon.WebUI --context ApplicationDbContext

         */

        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("ChamedoonConnection"));
        });
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<PersianIdentityErrorDescriber>();

        return services;

    }
}
