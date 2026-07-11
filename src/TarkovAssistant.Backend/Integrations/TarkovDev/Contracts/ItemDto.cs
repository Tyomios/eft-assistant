using System.Text.Json.Serialization;

namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

public sealed class ItemDto
{
    public string Id { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? NormalizedName { get; init; }
    public string? ShortName { get; init; }
    public string? Description { get; init; }
    public int BasePrice { get; init; }
    public string? Updated { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public string BackgroundColor { get; init; } = string.Empty;
    public string? IconLink { get; init; }
    public string? GridImageLink { get; init; }
    public string? BaseImageLink { get; init; }
    public string? InspectImageLink { get; init; }
    public string? Image512pxLink { get; init; }
    public string? Image8xLink { get; init; }
    public string? WikiLink { get; init; }
    public IReadOnlyList<string> Types { get; init; } = [];
    public int? Avg24hPrice { get; init; }
    public int? LastLowPrice { get; init; }
    public int? Low24hPrice { get; init; }
    public int? High24hPrice { get; init; }
    public double? ChangeLast48h { get; init; }
    public double? ChangeLast48hPercent { get; init; }
    public int? LastOfferCount { get; init; }
    public double? Weight { get; init; }
    public int? MinLevelForFlea { get; init; }
    public int? FleaMarketFee { get; init; }
    public IReadOnlyList<ItemCategoryDto> Categories { get; init; } = [];
    public IReadOnlyList<ItemPriceDto> SellFor { get; init; } = [];
    public IReadOnlyList<ItemPriceDto> BuyFor { get; init; } = [];
    public ItemPropertiesDto? Properties { get; init; }
}

public sealed class ItemCategoryDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
    public string? ImageLink { get; init; }
    public int? MinLevelForFlea { get; init; }
}

public sealed class ItemPriceDto
{
    public VendorDto Vendor { get; init; } = new();
    public int? Price { get; init; }
    public string? Currency { get; init; }
    public int? PriceRUB { get; init; }
}

public sealed class VendorDto
{
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
}

public sealed class ItemPropertiesDto
{
    [JsonPropertyName("__typename")]
    public string TypeName { get; init; } = string.Empty;

    public string? Caliber { get; init; }
    public ItemReferenceDto? DefaultAmmo { get; init; }
    public int? EffectiveDistance { get; init; }
    public double? Ergonomics { get; init; }
    public IReadOnlyList<string>? FireModes { get; init; }
    public int? FireRate { get; init; }
    public int? MaxDurability { get; init; }
    public int? RecoilVertical { get; init; }
    public int? RecoilHorizontal { get; init; }
    public int? SightingRange { get; init; }
    public int? DefaultWidth { get; init; }
    public int? DefaultHeight { get; init; }
    public double? DefaultErgonomics { get; init; }
    public int? DefaultRecoilVertical { get; init; }
    public int? DefaultRecoilHorizontal { get; init; }
    public double? DefaultWeight { get; init; }
    public double? RecoilModifier { get; init; }
    public int? Capacity { get; init; }
    public double? LoadModifier { get; init; }
    public double? AmmoCheckModifier { get; init; }
    public double? MalfunctionChance { get; init; }
    public IReadOnlyList<ItemReferenceDto>? AllowedAmmo { get; init; }
}

public sealed class ItemReferenceDto
{
    public string Id { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? NormalizedName { get; init; }
    public string? ShortName { get; init; }
    public string? IconLink { get; init; }
    public string? GridImageLink { get; init; }
}
