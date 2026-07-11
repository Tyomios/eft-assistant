using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Provides access to traders and their loyalty levels.
/// </summary>
[ApiController]
[Route("api/tarkov/traders")]
[Produces("application/json")]
public sealed class TradersController : ControllerBase
{
    private readonly ITarkovDevClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="TradersController"/> class.
    /// </summary>
    /// <param name="client">The Tarkov.dev client.</param>
    public TradersController(ITarkovDevClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    /// <summary>Returns a page of traders and their loyalty levels.</summary>
    /// <param name="parameters">The localization and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The requested page of traders.</returns>
    [HttpGet]
    [ProducesResponseType<PageResponse<TraderDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status502BadGateway)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PageResponse<TraderDto>>> GetTradersAsync(
        [FromQuery] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var query = parameters.ToQuery();
        var traders = await _client.GetTradersAsync(query, cancellationToken);
        return Ok(PageResponseFactory.Create(query, traders));
    }
}
