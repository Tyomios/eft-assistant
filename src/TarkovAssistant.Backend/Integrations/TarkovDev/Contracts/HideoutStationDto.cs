namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents a hideout station and its upgrade levels.
/// </summary>
public sealed class HideoutStationDto
{
    /// <summary>Gets the station identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the localized station name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the normalized station name.</summary>
    public string NormalizedName { get; init; } = string.Empty;

    /// <summary>Gets the station image URL.</summary>
    public string? ImageLink { get; init; }

    /// <summary>Gets the numeric identifier from the upstream Tarkov dataset.</summary>
    public int? TarkovDataId { get; init; }

    /// <summary>Gets the available station levels.</summary>
    public IReadOnlyList<HideoutStationLevelDto> Levels { get; init; } = [];
}
