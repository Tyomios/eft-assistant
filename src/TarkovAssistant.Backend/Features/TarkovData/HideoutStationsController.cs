using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Provides access to hideout stations and their upgrade requirements.
/// </summary>
[ApiController]
[Route("api/tarkov/hideout-stations")]
[Produces("application/json")]
public sealed class HideoutStationsController : ControllerBase
{
    private readonly ITarkovDevClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="HideoutStationsController"/> class.
    /// </summary>
    /// <param name="client">The Tarkov.dev client.</param>
    public HideoutStationsController(ITarkovDevClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    /// <summary>Returns a page of hideout stations and their item requirements.</summary>
    /// <param name="parameters">The localization and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The requested page of hideout stations.</returns>
    [HttpGet]
    [ProducesResponseType<PageResponse<HideoutStationDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status502BadGateway)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PageResponse<HideoutStationDto>>> GetHideoutStationsAsync(
        [FromQuery] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var query = parameters.ToQuery();
        var stations = await _client.GetHideoutStationsAsync(query, cancellationToken);
        return Ok(PageResponseFactory.Create(query, stations));
    }
}
