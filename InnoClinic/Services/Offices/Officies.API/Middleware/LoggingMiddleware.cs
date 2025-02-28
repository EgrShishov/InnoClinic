using Microsoft.AspNetCore.Http.Extensions;

namespace Officies.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger;

        public LoggingMiddleware(RequestDelegate next, ILog logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var originalResponseBody = httpContext.Response.Body;

            using var responseBody = new MemoryStream();

            httpContext.Response.Body = responseBody;

            await _next(httpContext);

            if (httpContext.Response.StatusCode < 200 || httpContext.Response.StatusCode >=300)
            {
                _logger.Error($"request {httpContext.Request.GetDisplayUrl} returns {httpContext.Response.StatusCode}");
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
