using Chamedoon.Application.Services.Email.Query;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Chamedoon.Application.Common.Behaviors;
using Chamedoon.Application.Services.Admin.Blogs;
using Chamedoon.Application.Services.Admin.Dashboard;
using Chamedoon.Application.Services.Admin.Roles;
using Chamedoon.Application.Services.Admin.Users;


namespace Chamedoon.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        services.AddScoped<IEmailService, SendGridMailService>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IAdminBlogService, AdminBlogService>();
        services.AddScoped<IAdminRoleService, AdminRoleService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();

        return services;
    }
}

