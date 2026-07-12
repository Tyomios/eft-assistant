namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>Represents the backend response containing catalog records changed since a client version.</summary>
internal sealed record CatalogChangesDto(long Version, DateTimeOffset PublishedAt, string ContentHash, IReadOnlyList<CatalogItemDto> Items);
