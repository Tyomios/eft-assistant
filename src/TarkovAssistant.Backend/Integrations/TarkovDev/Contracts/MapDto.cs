namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Provides a compact reference to an Escape from Tarkov map.
/// </summary>
public sealed class MapDto
{
    /// <summary>Gets the map identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the localized map name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the normalized map name.</summary>
    public string NormalizedName { get; init; } = string.Empty;
}
