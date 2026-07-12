namespace TarkovAssistant.Backend.Features.Catalog;

/// <summary>
/// Contains the target catalog version and records required by a client with an older version.
/// </summary>
/// <param name="Version">The current published version.</param>
/// <param name="PublishedAt">The UTC time at which the target version was published.</param>
/// <param name="ContentHash">The target catalog content hash.</param>
/// <param name="Items">The item records required to reach the target version.</param>
public sealed record CatalogChangesResponse(
    long Version,
    DateTimeOffset PublishedAt,
    string ContentHash,
    IReadOnlyList<CatalogItemResponse> Items);
