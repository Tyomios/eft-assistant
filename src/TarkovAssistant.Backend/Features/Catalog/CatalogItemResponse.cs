namespace TarkovAssistant.Backend.Features.Catalog;

/// <summary>
/// Contains the recognition-relevant item data and public image locations for one catalog snapshot.
/// </summary>
/// <param name="Id">The stable internal item identifier.</param>
/// <param name="ExternalId">The source-system identifier retained for reference.</param>
/// <param name="Name">The localized display name.</param>
/// <param name="NormalizedName">The normalized name used for deterministic matching.</param>
/// <param name="ShortName">The compact display name.</param>
/// <param name="Width">The item width in inventory cells.</param>
/// <param name="Height">The item height in inventory cells.</param>
/// <param name="IconUrl">The optional icon URL.</param>
/// <param name="GridImageUrl">The optional grid-image URL.</param>
public sealed record CatalogItemResponse(
    Guid Id,
    string ExternalId,
    string Name,
    string NormalizedName,
    string ShortName,
    int Width,
    int Height,
    string? IconUrl,
    string? GridImageUrl);
