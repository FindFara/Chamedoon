using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ChamedoonWebUI.Middleware
{
    public class XssProtectionMiddleware
    {
        private static readonly Regex SuspiciousInputPattern = new("(<script|</script>|onerror\\s*=|onload\\s*=|javascript:|eval\\s*\\(|alert\\s*\\()", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly RequestDelegate _next;

        public XssProtectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (await HasSuspiciousInputAsync(context))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Bad request.");
                return;
            }

            await _next(context);
        }

        private static async Task<bool> HasSuspiciousInputAsync(HttpContext context)
        {
            if (context.Request.Query.Any(q => q.Value.Any(ContainsSuspiciousInput)))
            {
                return true;
            }

            if (context.Request.HasFormContentType)
            {
                context.Request.EnableBuffering();
                var form = await context.Request.ReadFormAsync();
                context.Request.Body.Position = 0;

                if (form.SelectMany(f => f.Value).Any(ContainsSuspiciousInput))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsSuspiciousInput(string? value)
        {
            return !string.IsNullOrWhiteSpace(value) && SuspiciousInputPattern.IsMatch(value);
        }
    }

    public static class XssProtectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseXssProtection(this IApplicationBuilder app)
        {
            return app.UseMiddleware<XssProtectionMiddleware>();
        }
    }
}
