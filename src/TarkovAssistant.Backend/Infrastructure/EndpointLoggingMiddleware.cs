using System.Diagnostics;

namespace TarkovAssistant.Backend.Infrastructure;

public sealed class EndpointLoggingMiddleware(
    RequestDelegate next,
    ILogger<EndpointLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var endpoint = context.GetEndpoint()?.DisplayName ?? "unmatched";

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            logger.LogInformation(
                "HTTP {Method} {Path} matched {Endpoint} responded {StatusCode} in {ElapsedMilliseconds:F1} ms TraceId={TraceId}",
                context.Request.Method,
                context.Request.Path,
                endpoint,
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds,
                context.TraceIdentifier);
        }
    }
}
