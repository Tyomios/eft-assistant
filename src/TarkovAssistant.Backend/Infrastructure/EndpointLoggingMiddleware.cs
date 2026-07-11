using System.Diagnostics;

namespace TarkovAssistant.Backend.Infrastructure;

/// <summary>
/// Writes one structured completion event for every HTTP request.
/// </summary>
public sealed partial class EndpointLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EndpointLoggingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EndpointLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next request delegate in the pipeline.</param>
    /// <param name="logger">The request-completion logger.</param>
    public EndpointLoggingMiddleware(
        RequestDelegate next,
        ILogger<EndpointLoggingMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(logger);

        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the remaining request pipeline and logs the final response status.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task that represents request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var stopwatch = Stopwatch.StartNew();
        var endpoint = context.GetEndpoint()?.DisplayName ?? "unmatched";

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            LogRequestCompleted(
                _logger,
                context.Request.Method,
                context.Request.Path.Value ?? string.Empty,
                endpoint,
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds,
                context.TraceIdentifier);
        }
    }

    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} matched {Endpoint} responded {StatusCode} in {ElapsedMilliseconds:F1} ms TraceId={TraceId}")]
    private static partial void LogRequestCompleted(
        ILogger logger,
        string method,
        string path,
        string endpoint,
        int statusCode,
        double elapsedMilliseconds,
        string traceId);
}
