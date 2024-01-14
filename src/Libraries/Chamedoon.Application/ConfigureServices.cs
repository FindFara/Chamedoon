using Chamedoon.Application.Common.Utilities.Security;
using Chamedoon.Application.Services.Email.Query;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Chamedoon.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient<IEmailService, SendGridMailService>();
        services.AddScoped<ISecurityService, SecurityService>();

        return services;
    }
}

