using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Integrations.TarkovDev;

namespace TarkovAssistant.Backend.Infrastructure;

/// <summary>
/// Converts application and upstream exceptions into RFC 9457 problem responses.
/// </summary>
public sealed partial class ApiExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<ApiExceptionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiExceptionHandler"/> class.
    /// </summary>
    /// <param name="problemDetailsService">The service used to write problem responses.</param>
    /// <param name="logger">The exception logger.</param>
    public ApiExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<ApiExceptionHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(problemDetailsService);
        ArgumentNullException.ThrowIfNull(logger);

        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);

        var (status, title, detail) = exception switch
        {
            RequestValidationException validation => (
                StatusCodes.Status400BadRequest,
                "Invalid request",
                validation.Message),
            BadHttpRequestException badRequest => (
                StatusCodes.Status400BadRequest,
                "Invalid request",
                badRequest.Message),
            TarkovDevGraphQlException graphQl => (
                StatusCodes.Status502BadGateway,
                "Tarkov.dev rejected the query",
                graphQl.Message),
            TarkovDevException upstream => (
                StatusCodes.Status503ServiceUnavailable,
                "Tarkov.dev is unavailable",
                upstream.Message),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Unexpected server error",
                "An unexpected error occurred.")
        };

        LogException(status, exception);
        httpContext.Response.StatusCode = status;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path,
                Extensions = { ["traceId"] = httpContext.TraceIdentifier }
            }
        });
    }

    private void LogException(int status, Exception exception)
    {
        if (exception is RequestValidationException or BadHttpRequestException)
        {
            LogRequestRejected(_logger, status, exception.Message);
            return;
        }

        if (status == StatusCodes.Status500InternalServerError)
        {
            LogUnexpectedFailure(_logger, status, exception);
            return;
        }

        LogUpstreamFailure(_logger, status, exception);
    }

    [LoggerMessage(
        EventId = 2000,
        Level = LogLevel.Information,
        Message = "Request rejected with status code {StatusCode}: {Message}")]
    private static partial void LogRequestRejected(ILogger logger, int statusCode, string message);

    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Warning,
        Message = "Upstream request failed with status code {StatusCode}")]
    private static partial void LogUpstreamFailure(ILogger logger, int statusCode, Exception exception);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Error,
        Message = "Request failed with status code {StatusCode}")]
    private static partial void LogUnexpectedFailure(ILogger logger, int statusCode, Exception exception);
}
