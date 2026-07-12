using Microsoft.AspNetCore.Mvc;

namespace TarkovAssistant.Backend.Features.DataImport;

/// <summary>
/// Starts and reports local Tarkov.dev catalog imports used by the Windows client.
/// </summary>
[ApiController]
[Route("api/data-import")]
[Produces("application/json")]
public sealed class DataImportController : ControllerBase
{
    private readonly ImportCatalogHandler _handler;

    /// <summary>
    /// Initializes the import API controller.
    /// </summary>
    /// <param name="services">The request service provider containing the catalog-import use case.</param>
    public DataImportController(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _handler = services.GetRequiredService<ImportCatalogHandler>();
    }

    /// <summary>
    /// Downloads, stores, and publishes the current recognition catalog.
    /// </summary>
    /// <param name="cancellationToken">Stops the import when the HTTP request is aborted.</param>
    /// <returns>The completed import outcome, or HTTP 409 when an import is already in progress.</returns>
    [HttpPost("run")]
    [ProducesResponseType<DataImportRunResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DataImportRunResponse>> RunAsync(CancellationToken cancellationToken)
    {
        var result = await _handler.RunAsync(cancellationToken).ConfigureAwait(false);
        return result is null
            ? Conflict(new ProblemDetails { Detail = "A catalog import is already in progress." })
            : Ok(result);
    }

    /// <summary>
    /// Gets the latest recorded state of the catalog import.
    /// </summary>
    /// <param name="cancellationToken">Stops the database operation when the HTTP request is aborted.</param>
    /// <returns>The current import state, or HTTP 404 when no import has started.</returns>
    [HttpGet("status")]
    [ProducesResponseType<DataImportStatusResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DataImportStatusResponse>> GetStatusAsync(CancellationToken cancellationToken)
    {
        var result = await _handler.GetStatusAsync(cancellationToken).ConfigureAwait(false);
        return result is null ? NotFound() : Ok(result);
    }
}
