using Chamedoon.Application.Services.Email.Query;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Chamedoon.Application.Common.Behaviors;
using Chamedoon.UI.Services;


namespace Chamedoon.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddBlazorUIServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountOperation, AccountOperation>();

        return services;
    }
}

