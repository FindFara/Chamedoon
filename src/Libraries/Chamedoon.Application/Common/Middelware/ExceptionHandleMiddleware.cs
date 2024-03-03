using Chamedoon.Application.Common.Exeption;
using Chamedoon.Application.Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog.Context;
using Serilog.Core;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Chamedoon.Application.Common.Middelware
{
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

        public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(ex, httpContext);

            }
        }
        private async Task HandleException(Exception ex, HttpContext httpContext)
        {
            var responseModel = new ResponseModel
            {
                Message = ex.Message,
                Success = false,

            };
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //Encode Persian characters 
            };

            if (ex is SecurityTokenExpiredException)
            {
                //ToDo
            }
            if (ex is UnauthorizedAccessException)
            {
                //ToDo
            }
            if (ex is AppValidationException)
            {
                //ToDo
            }

            if (ex is ThrowException exception)
            {
                using (LogContext.PushProperty("MethodName", exception.MethodName ?? ""))
                {
                    _logger.LogError(ex, "{Message}", ex.Message);
                }
                responseModel.StatusCode = StatusCodes.Status500InternalServerError;
                var json = JsonSerializer.Serialize(responseModel, options);
                await httpContext.Response.WriteAsync(json);
            }
            else
            {
                _logger.LogError(
               ex, "Exception occurred: {Message}", ex.Message);
                responseModel.StatusCode = 520;
            }
        }
    }

}
