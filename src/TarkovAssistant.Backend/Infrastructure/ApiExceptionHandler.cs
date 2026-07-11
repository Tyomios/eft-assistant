using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Integrations.TarkovDev;

namespace TarkovAssistant.Backend.Infrastructure;

public sealed class ApiExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title, detail, level) = exception switch
        {
            RequestValidationException validation => (
                StatusCodes.Status400BadRequest,
                "Invalid request",
                validation.Message,
                LogLevel.Information),
            TarkovDevGraphQlException graphQl => (
                StatusCodes.Status502BadGateway,
                "Tarkov.dev rejected the query",
                graphQl.Message,
                LogLevel.Warning),
            TarkovDevException upstream => (
                StatusCodes.Status503ServiceUnavailable,
                "Tarkov.dev is unavailable",
                upstream.Message,
                LogLevel.Warning),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Unexpected server error",
                "An unexpected error occurred.",
                LogLevel.Error)
        };

        if (level == LogLevel.Information)
        {
            logger.LogInformation("Request rejected with status code {StatusCode}: {Message}", status, exception.Message);
        }
        else
        {
            logger.Log(level, exception, "Request failed with status code {StatusCode}", status);
        }
        httpContext.Response.StatusCode = status;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
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
}

public sealed class RequestValidationException(string message) : Exception(message);
