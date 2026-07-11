namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Describes a hideout station upgrade level.
/// </summary>
public sealed class HideoutStationLevelDto
{
    /// <summary>Gets the station-level identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the station level.</summary>
    public int Level { get; init; }

    /// <summary>Gets the construction duration in seconds.</summary>
    public int ConstructionTime { get; init; }

    /// <summary>Gets the localized level description.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Gets the numeric identifier from the upstream Tarkov dataset.</summary>
    public int? TarkovDataId { get; init; }

    /// <summary>Gets the items required to construct the level.</summary>
    public IReadOnlyList<ItemRequirementDto> ItemRequirements { get; init; } = [];
}
