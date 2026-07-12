namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>
/// Represents one backend catalog item before it is normalized into the local SQLite index.
/// </summary>
internal sealed record CatalogItemDto(
    Guid Id,
    string ExternalId,
    string Name,
    string NormalizedName,
    string ShortName,
    int Width,
    int Height,
    string? IconUrl,
    string? GridImageUrl);
