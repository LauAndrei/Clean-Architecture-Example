using System.Diagnostics;

namespace NET8_CleanArchitecture_Azure.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();

        await next.Invoke(context);

        stopWatch.Stop();

        if (stopWatch.Elapsed > TimeSpan.FromSeconds(4))
        {
            logger.LogInformation("Request {RequestMethod} at {RequestPath} took {ElapsedSeconds} seconds",
                context.Request.Method, context.Request.Path, stopWatch.Elapsed.Seconds);
        }
    }
}