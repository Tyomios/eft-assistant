namespace TarkovAssistant.Backend.Features.Catalog;

/// <summary>
/// Identifies one immutable catalog snapshot that clients can synchronize.
/// </summary>
/// <param name="Version">The monotonically increasing catalog version.</param>
/// <param name="PublishedAt">The UTC instant at which the snapshot was published.</param>
/// <param name="ContentHash">The backend-computed content hash for diagnostics and cache verification.</param>
public sealed record CatalogVersionResponse(long Version, DateTimeOffset PublishedAt, string ContentHash);
