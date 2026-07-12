using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Backend.Database;
using TarkovAssistant.Backend.Database.Entities;

namespace TarkovAssistant.Backend.Features.Catalog;

/// <summary>
/// Exposes immutable, bulk-oriented catalog snapshots to the local Windows client.
/// </summary>
[ApiController]
[Route("api/catalog")]
[Produces("application/json")]
public sealed class CatalogController : ControllerBase
{
    private readonly TarkovAssistantDbContext _database;

    /// <summary>
    /// Initializes a catalog endpoint controller.
    /// </summary>
    /// <param name="services">The request service provider containing the local imported-catalog database.</param>
    public CatalogController(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _database = services.GetRequiredService<TarkovAssistantDbContext>();
    }

    /// <summary>
    /// Gets the latest fully published catalog version.
    /// </summary>
    /// <param name="cancellationToken">Stops the database operation when the request is aborted.</param>
    /// <returns>The current version, or HTTP 404 before the first import publishes a catalog.</returns>
    [HttpGet("version")]
    [ProducesResponseType<CatalogVersionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CatalogVersionResponse>> GetVersionAsync(CancellationToken cancellationToken)
    {
        var version = await LatestVersionAsync(cancellationToken).ConfigureAwait(false);
        return version is null
            ? NotFound()
            : Ok(new CatalogVersionResponse(version.Version, version.PublishedAt, version.ContentHash));
    }

    /// <summary>
    /// Gets all items belonging to the requested current catalog version.
    /// </summary>
    /// <param name="version">The version returned by <c>GET /api/catalog/version</c>.</param>
    /// <param name="cancellationToken">Stops the database operation when the request is aborted.</param>
    /// <returns>The complete local matching catalog, or HTTP 409 if a newer version was published.</returns>
    [HttpGet("items")]
    [ProducesResponseType<IReadOnlyList<CatalogItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IReadOnlyList<CatalogItemResponse>>> GetItemsAsync(
        [FromQuery] long version,
        CancellationToken cancellationToken)
    {
        if (version <= 0)
        {
            return BadRequest(new ProblemDetails { Detail = "A positive catalog version is required." });
        }

        var current = await LatestVersionAsync(cancellationToken).ConfigureAwait(false);
        if (current is null || current.Version != version)
        {
            return Conflict(new ProblemDetails { Detail = "The requested catalog version is no longer current." });
        }

        var items = await (
            from item in _database.Items.AsNoTracking()
            join icon in _database.ItemImages.AsNoTracking().Where(image => image.Kind == ItemImageKind.Icon)
                on item.Id equals icon.ItemId into icons
            from icon in icons.DefaultIfEmpty()
            join grid in _database.ItemImages.AsNoTracking().Where(image => image.Kind == ItemImageKind.Grid)
                on item.Id equals grid.ItemId into grids
            from grid in grids.DefaultIfEmpty()
            orderby item.Id
            select new CatalogItemResponse(
                item.Id,
                item.ExternalId,
                item.Name,
                item.NormalizedName,
                item.ShortName,
                item.Width,
                item.Height,
                icon == null ? null : icon.SourceUrl,
                grid == null ? null : grid.SourceUrl))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Ok(items);
    }

    /// <summary>
    /// Gets catalog records required by a client that has an older catalog version.
    /// </summary>
    /// <param name="sinceVersion">The version currently stored by the client, or zero for its first synchronization.</param>
    /// <param name="cancellationToken">Stops the database operation when the request is aborted.</param>
    /// <returns>The current version and records needed by the client.</returns>
    [HttpGet("changes")]
    [ProducesResponseType<CatalogChangesResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CatalogChangesResponse>> GetChangesAsync(
        [FromQuery] long sinceVersion,
        CancellationToken cancellationToken)
    {
        if (sinceVersion < 0)
        {
            return BadRequest(new ProblemDetails { Detail = "The catalog version cannot be negative." });
        }

        var current = await LatestVersionAsync(cancellationToken).ConfigureAwait(false);
        if (current is null)
        {
            return NotFound();
        }

        if (sinceVersion == current.Version)
        {
            return Ok(new CatalogChangesResponse(current.Version, current.PublishedAt, current.ContentHash, []));
        }

        var items = await GetCatalogItemsAsync(cancellationToken).ConfigureAwait(false);
        return Ok(new CatalogChangesResponse(current.Version, current.PublishedAt, current.ContentHash, items));
    }

    private Task<List<CatalogItemResponse>> GetCatalogItemsAsync(CancellationToken cancellationToken)
    {
        return (
            from item in _database.Items.AsNoTracking()
            join icon in _database.ItemImages.AsNoTracking().Where(image => image.Kind == ItemImageKind.Icon)
                on item.Id equals icon.ItemId into icons
            from icon in icons.DefaultIfEmpty()
            join grid in _database.ItemImages.AsNoTracking().Where(image => image.Kind == ItemImageKind.Grid)
                on item.Id equals grid.ItemId into grids
            from grid in grids.DefaultIfEmpty()
            orderby item.Id
            select new CatalogItemResponse(item.Id, item.ExternalId, item.Name, item.NormalizedName, item.ShortName,
                item.Width, item.Height, icon == null ? null : icon.SourceUrl, grid == null ? null : grid.SourceUrl))
            .ToListAsync(cancellationToken);
    }

    private Task<CatalogVersion?> LatestVersionAsync(CancellationToken cancellationToken)
    {
        return _database.CatalogVersions
            .AsNoTracking()
            .OrderByDescending(version => version.Version)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
