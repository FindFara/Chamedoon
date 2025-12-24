using Chamedoon.Application.Services.Email.Query;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Chamedoon.Application.Common.Behaviors;
using Chamedoon.Application.Services.Admin.Blogs;
using Chamedoon.Application.Services.Admin.Dashboard;
using Chamedoon.Application.Services.Admin.Discounts;
using Chamedoon.Application.Services.Admin.Payments;
using Chamedoon.Application.Services.Admin.Roles;
using Chamedoon.Application.Services.Admin.Subscriptions;
using Chamedoon.Application.Services.Admin.Users;
using Chamedoon.Application.Services.Admin.Countries;
using Chamedoon.Application.Services.Immigration;
using Chamedoon.Application.Services.Payments;
using Chamedoon.Application.Services.Subscription;


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
        services.AddScoped<IAdminPaymentService, AdminPaymentService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();
        services.AddScoped<IAdminDiscountCodeService, AdminDiscountCodeService>();
        services.AddScoped<IAdminSubscriptionPlanService, AdminSubscriptionPlanService>();
        services.AddScoped<IAdminCountryService, AdminCountryService>();
        services.AddScoped<IImmigrationEvaluationService, ImmigrationEvaluationService>();
        services.AddScoped<ICountryDataCache, CountryDataCache>();
        services.AddScoped<ImmigrationScoringService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<SubscriptionService>();
        services.AddMemoryCache();

        return services;
    }
}
