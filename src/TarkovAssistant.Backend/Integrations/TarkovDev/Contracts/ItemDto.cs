namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents an item returned by the Tarkov.dev catalog.
/// </summary>
public sealed class ItemDto
{
    /// <summary>Gets the Tarkov.dev item identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the localized item name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets the normalized item name.</summary>
    public string? NormalizedName { get; init; }

    /// <summary>Gets the localized short name.</summary>
    public string? ShortName { get; init; }

    /// <summary>Gets the localized description.</summary>
    public string? Description { get; init; }

    /// <summary>Gets the base price in roubles.</summary>
    public int BasePrice { get; init; }

    /// <summary>Gets the upstream update timestamp.</summary>
    public string? Updated { get; init; }

    /// <summary>Gets the inventory width in cells.</summary>
    public int Width { get; init; }

    /// <summary>Gets the inventory height in cells.</summary>
    public int Height { get; init; }

    /// <summary>Gets the inventory icon background color.</summary>
    public string BackgroundColor { get; init; } = string.Empty;

    /// <summary>Gets the compact icon URL.</summary>
    public string? IconLink { get; init; }

    /// <summary>Gets the inventory-grid image URL.</summary>
    public string? GridImageLink { get; init; }

    /// <summary>Gets the base image URL.</summary>
    public string? BaseImageLink { get; init; }

    /// <summary>Gets the inspection image URL.</summary>
    public string? InspectImageLink { get; init; }

    /// <summary>Gets the 512-pixel image URL.</summary>
    public string? Image512pxLink { get; init; }

    /// <summary>Gets the high-resolution image URL.</summary>
    public string? Image8xLink { get; init; }

    /// <summary>Gets the Tarkov wiki URL.</summary>
    public string? WikiLink { get; init; }

    /// <summary>Gets the item type classifications.</summary>
    public IReadOnlyList<string> Types { get; init; } = [];

    /// <summary>Gets the average flea-market price during the last 24 hours.</summary>
    public int? Avg24hPrice { get; init; }

    /// <summary>Gets the latest recorded low price.</summary>
    public int? LastLowPrice { get; init; }

    /// <summary>Gets the lowest price recorded during the last 24 hours.</summary>
    public int? Low24hPrice { get; init; }

    /// <summary>Gets the highest price recorded during the last 24 hours.</summary>
    public int? High24hPrice { get; init; }

    /// <summary>Gets the absolute price change during the last 48 hours.</summary>
    public double? ChangeLast48h { get; init; }

    /// <summary>Gets the percentage price change during the last 48 hours.</summary>
    public double? ChangeLast48hPercent { get; init; }

    /// <summary>Gets the latest flea-market offer count.</summary>
    public int? LastOfferCount { get; init; }

    /// <summary>Gets the item weight in kilograms.</summary>
    public double? Weight { get; init; }

    /// <summary>Gets the minimum player level required for flea-market access.</summary>
    public int? MinLevelForFlea { get; init; }

    /// <summary>Gets the estimated flea-market listing fee.</summary>
    public int? FleaMarketFee { get; init; }

    /// <summary>Gets the catalog categories assigned to the item.</summary>
    public IReadOnlyList<ItemCategoryDto> Categories { get; init; } = [];

    /// <summary>Gets the available sale offers.</summary>
    public IReadOnlyList<ItemPriceDto> SellFor { get; init; } = [];

    /// <summary>Gets the available purchase offers.</summary>
    public IReadOnlyList<ItemPriceDto> BuyFor { get; init; } = [];

    /// <summary>Gets type-specific item properties.</summary>
    public ItemPropertiesDto? Properties { get; init; }
}
