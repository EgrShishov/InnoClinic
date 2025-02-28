using Microsoft.AspNetCore.Http.Extensions;

namespace Profiles.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var originalResponseBody = httpContext.Response.Body;

            using var originalBody = new MemoryStream();

            httpContext.Response.Body = originalBody;

            await _next(httpContext);

            if (httpContext.Response.StatusCode < 200 || httpContext.Response.StatusCode >= 300)
            {
                _logger.LogError($"request {httpContext.Request.GetDisplayUrl} returns {httpContext.Response.StatusCode}", httpContext.Response.Body);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
