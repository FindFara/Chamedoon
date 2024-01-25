using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;

namespace Chamedoon.WebAPI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        //services.AddAuthentication(op =>
        //{
        //    op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //    op.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //    op.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //    op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //}).AddCookie(op =>
        //{
        //    op.LoginPath = "/auth/login";
        //    op.LogoutPath = "/auth/logout";
        //    op.ExpireTimeSpan = TimeSpan.FromMinutes(143200);
        //});
        //services.AddSession(options =>
        //{
        //    options.IdleTimeout = TimeSpan.FromMinutes(20);
        //    options.Cookie.HttpOnly = true;
        //});
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Example API",
                Version = "v1",
                Description = "An example of an ASP.NET Core Web API",
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Email = "example@example.com",
                    Url = new Uri("https://example.com/contact"),
                },
            });

        });
        services.AddHttpContextAccessor();
        return services;
    }
}
