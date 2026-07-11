using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Provides access to weapons and their ammunition compatibility.
/// </summary>
[ApiController]
[Route("api/tarkov/weapons")]
[Produces("application/json")]
public sealed class WeaponsController : ControllerBase
{
    private readonly ITarkovDevClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeaponsController"/> class.
    /// </summary>
    /// <param name="client">The Tarkov.dev client.</param>
    public WeaponsController(ITarkovDevClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    /// <summary>Returns a page of weapons and their compatible ammunition.</summary>
    /// <param name="parameters">The localization and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The requested page of weapons.</returns>
    [HttpGet]
    [ProducesResponseType<PageResponse<ItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status502BadGateway)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PageResponse<ItemDto>>> GetWeaponsAsync(
        [FromQuery] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var query = parameters.ToQuery();
        var weapons = await _client.GetWeaponsAsync(query, cancellationToken);
        return Ok(PageResponseFactory.Create(query, weapons));
    }
}
