using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Infrastructure;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Provides access to the Tarkov.dev item catalog.
/// </summary>
[ApiController]
[Route("api/tarkov/items")]
[Produces("application/json")]
public sealed class ItemsController : ControllerBase
{
    private readonly ITarkovDevClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemsController"/> class.
    /// </summary>
    /// <param name="client">The Tarkov.dev client.</param>
    public ItemsController(ITarkovDevClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    /// <summary>Returns a page of items from Tarkov.dev.</summary>
    /// <param name="parameters">The localization, paging, and item filtering parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The requested page of items.</returns>
    [HttpGet]
    [ProducesResponseType<PageResponse<ItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status502BadGateway)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PageResponse<ItemDto>>> GetItemsAsync(
        [FromQuery] TarkovQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var query = parameters.ToQuery(allowItemType: true);
        var items = await _client.GetItemsAsync(query, cancellationToken);
        return Ok(PageResponseFactory.Create(query, items));
    }

    /// <summary>Returns an item by its Tarkov.dev identifier.</summary>
    /// <param name="id">The Tarkov.dev item identifier.</param>
    /// <param name="parameters">The localization parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching item, or HTTP 404 when no item exists.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<ItemDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status502BadGateway)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<ItemDto>> GetItemAsync(
        string id,
        [FromQuery] TarkovLocaleParameters parameters,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(parameters);

        var item = await _client.GetItemAsync(id, parameters.ToQuery(), cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }
}
