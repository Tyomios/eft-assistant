namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Trader : SourceEntity
{
    internal Guid? CurrencyItemId { get; set; }

    internal string Name { get; set; } = string.Empty;

    internal string NormalizedName { get; set; } = string.Empty;

    internal string? Description { get; set; }

    internal DateTimeOffset? ResetAt { get; set; }

    internal decimal Discount { get; set; }

    internal string? ImageUrl { get; set; }

    internal string? HighResolutionImageUrl { get; set; }

    internal int? TarkovDataId { get; set; }
}
