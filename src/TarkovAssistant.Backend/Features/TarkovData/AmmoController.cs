using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Provides access to ammunition statistics.
/// </summary>
[ApiController]
[Route("api/tarkov/ammo")]
[Produces("application/json")]
public sealed class AmmoController : ControllerBase
{
    private readonly ITarkovDevClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="AmmoController"/> class.
    /// </summary>
    /// <param name="client">The Tarkov.dev client.</param>
    public AmmoController(ITarkovDevClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    /// <summary>Returns a page of ammunition statistics.</summary>
    /// <param name="parameters">The localization and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The requested page of ammunition records.</returns>
    [HttpGet]
    [ProducesResponseType<PageResponse<AmmoDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status502BadGateway)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PageResponse<AmmoDto>>> GetAmmoAsync(
        [FromQuery] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var query = parameters.ToQuery();
        var ammo = await _client.GetAmmoAsync(query, cancellationToken);
        return Ok(PageResponseFactory.Create(query, ammo));
    }
}
