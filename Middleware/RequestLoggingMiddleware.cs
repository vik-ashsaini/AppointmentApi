namespace AppointmentApi.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogInformation("Request: {method} {path}", context.Request.Method, context.Request.Path);
            await next(context);
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
            => builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
