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

         Add-Migration AddUser -OutputDir Migrations -Context ApplicationDbContext -Project Chamedoon.Infrastructure -Args '--environment Production'

         Update-Database -Args '--environment Production'

         dotnet ef migrations add migrationsname 
        --verbose 
        --project "src/Libraries/Chamedoon.Infrastructure/Chamedoon.Infrastructure.csproj" 
        --startup-project "src/Presentation/Chamedoon.WebAPI/Chamedoon.WebAPI.csproj" 
        --context ApplicationDbContext 
        -- --environment Chamedoon

        dotnet ef database update -- --environment Production

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
