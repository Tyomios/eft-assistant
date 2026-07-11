namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents a Tarkov.dev item category.
/// </summary>
public sealed class ItemCategoryDto
{
    /// <summary>Gets the category identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the localized category name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the normalized category name.</summary>
    public string NormalizedName { get; init; } = string.Empty;

    /// <summary>Gets the category image URL.</summary>
    public string? ImageLink { get; init; }

    /// <summary>Gets the category-specific minimum flea-market level.</summary>
    public int? MinLevelForFlea { get; init; }
}
