using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Utilities.CustomizIdentity;
using Chamedoon.Domin.Configs;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Infrastructure.Persistence;
using Chamedoon.Infrastructure.Repositories.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.Configure<MelipayamakConfig>(configuration.GetSection(MelipayamakConfig.SectionName));
        services.AddHttpClient("MelipayamakSms", client =>
        {
            var melipayamakSection = configuration.GetSection(MelipayamakConfig.SectionName);
            var baseUrl = melipayamakSection?.GetValue<string>(nameof(MelipayamakConfig.OtpApiBaseAddress));
            baseUrl ??= melipayamakSection?.GetValue<string>(nameof(MelipayamakConfig.BaseUrl));
            if (!string.IsNullOrWhiteSpace(baseUrl) && Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri))
            {
                client.BaseAddress = baseUri;
            }
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IAdminBlogRepository, AdminBlogRepository>();
        services.AddScoped<IAdminRoleRepository, AdminRoleRepository>();
        services.AddScoped<IAdminPaymentRepository, AdminPaymentRepository>();
        services.AddScoped<IAdminDiscountCodeRepository, AdminDiscountCodeRepository>();
        services.AddScoped<IAdminSubscriptionPlanRepository, AdminSubscriptionPlanRepository>();
        services.AddScoped<IAdminCountryRepository, AdminCountryRepository>();
        services.AddScoped<ISmsService, MelipayamakSmsService>();

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<PersianIdentityErrorDescriber>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
        return services;

    }
}
