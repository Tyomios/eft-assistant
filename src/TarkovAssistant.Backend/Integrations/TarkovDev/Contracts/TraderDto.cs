namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents a trader and the trader's loyalty levels.
/// </summary>
public sealed class TraderDto
{
    /// <summary>Gets the trader identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the localized trader name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the normalized trader name.</summary>
    public string NormalizedName { get; init; } = string.Empty;

    /// <summary>Gets the localized trader description.</summary>
    public string? Description { get; init; }

    /// <summary>Gets the next inventory reset timestamp.</summary>
    public string? ResetTime { get; init; }

    /// <summary>Gets the trader discount.</summary>
    public double Discount { get; init; }

    /// <summary>Gets the trader image URL.</summary>
    public string? ImageLink { get; init; }

    /// <summary>Gets the high-resolution trader image URL.</summary>
    public string? Image4xLink { get; init; }

    /// <summary>Gets the numeric identifier from the upstream Tarkov dataset.</summary>
    public int? TarkovDataId { get; init; }

    /// <summary>Gets the trader's primary currency item.</summary>
    public ItemReferenceDto Currency { get; init; } = new();

    /// <summary>Gets the trader loyalty levels.</summary>
    public IReadOnlyList<TraderLevelDto> Levels { get; init; } = [];
}
