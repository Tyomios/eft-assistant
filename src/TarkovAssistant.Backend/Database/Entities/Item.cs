namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Item : SourceEntity
{
    internal string Name { get; set; } = string.Empty;

    internal string NormalizedName { get; set; } = string.Empty;

    internal string ShortName { get; set; } = string.Empty;

    internal string? Description { get; set; }

    internal int BasePrice { get; set; }

    internal int Width { get; set; }

    internal int Height { get; set; }

    internal string? BackgroundColor { get; set; }

    internal string? WikiUrl { get; set; }

    internal decimal? WeightKilograms { get; set; }

    internal int? MinimumFleaLevel { get; set; }

    internal int? FleaMarketFee { get; set; }
}
