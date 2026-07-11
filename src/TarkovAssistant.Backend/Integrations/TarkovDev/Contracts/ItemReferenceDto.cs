namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Provides a compact reference to an item.
/// </summary>
public sealed class ItemReferenceDto
{
    /// <summary>Gets the Tarkov.dev item identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the localized item name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets the normalized item name.</summary>
    public string? NormalizedName { get; init; }

    /// <summary>Gets the localized short name.</summary>
    public string? ShortName { get; init; }

    /// <summary>Gets the compact icon URL.</summary>
    public string? IconLink { get; init; }

    /// <summary>Gets the inventory-grid image URL.</summary>
    public string? GridImageLink { get; init; }
}
