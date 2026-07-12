namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>
/// Represents the backend JSON payload that identifies an immutable catalog snapshot.
/// </summary>
internal sealed record CatalogVersionDto(long Version, DateTimeOffset PublishedAt, string ContentHash);
