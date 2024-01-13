using Microsoft.AspNetCore.Authentication.Cookies;

namespace Chamedoon.WebAPI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
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
        services.AddHttpContextAccessor();
        return services;
    }
}
