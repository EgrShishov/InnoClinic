using Microsoft.AspNetCore.Http.Extensions;

namespace Appointments.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var originalBodyStream = httpContext.Response.Body;

            using var responseBody = new MemoryStream();

            httpContext.Response.Body = responseBody;

            await _next(httpContext);

            if (httpContext.Response.StatusCode < 200 || httpContext.Response.StatusCode >= 300)
            {
                Log.Error($"request {httpContext.Request.GetDisplayUrl} returns {httpContext.Response.StatusCode}");
            }

            await responseBody.CopyToAsync(originalBodyStream);
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
