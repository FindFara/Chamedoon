using System;
using System.Linq;
using System.Threading.Tasks;
using Chamedoon.Infrastructure.Persistence;
using Chamedoon.Application.Services.Payments;
using Chamedoon.Application.Services.Subscription;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChamedoonWebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
    {
        var appUrl = configuration.GetSection("Urls")?.GetValue<string>("AppUrl");

        services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = "122973351692-2fgb7h8v7qff9qnehugl7fio831lnvi8.apps.googleusercontent.com";
            options.ClientSecret = "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";
            options.CallbackPath = "/signin-google";
            options.SignInScheme = IdentityConstants.ExternalScheme;

            if (!string.IsNullOrWhiteSpace(appUrl) && Uri.TryCreate(appUrl, UriKind.Absolute, out var appUri))
            {
                options.Events ??= new OAuthEvents();
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    var callbackUri = new Uri(appUri, options.CallbackPath.Value);
                    var originalRedirectUri = new Uri(context.RedirectUri);
                    var query = QueryHelpers.ParseQuery(originalRedirectUri.Query);
                    var parameters = query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

                    parameters["redirect_uri"] = callbackUri.ToString();

                    var updatedRedirectUri = QueryHelpers.AddQueryString(
                        originalRedirectUri.GetLeftPart(UriPartial.Path),
                        parameters);

                    context.Response.Redirect(updatedRedirectUri);
                    return Task.CompletedTask;
                };
            }
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/auth/phone";
            options.LogoutPath = "/auth/logout";
            options.AccessDeniedPath = "/auth/AccessDenied";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromDays(2);
            options.SlidingExpiration = true;
        });

        services.AddScoped<SubscriptionService>();
        services.AddScoped<PaymentService>();
        services.Configure<PaymentGatewayOptions>(configuration.GetSection(PaymentGatewayOptions.SectionName));
        services.AddHttpClient(PaymentGatewayOptions.HttpClientName, client =>
        {
            var baseUrl = configuration.GetSection(PaymentGatewayOptions.SectionName)?.GetValue<string>(nameof(PaymentGatewayOptions.BaseUrl));
            if (!string.IsNullOrWhiteSpace(baseUrl) && Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
            {
                client.BaseAddress = uri;
            }
        });

        return services;
    }
}
