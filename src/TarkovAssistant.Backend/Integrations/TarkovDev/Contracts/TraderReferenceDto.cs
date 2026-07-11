namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Provides a compact reference to a trader.
/// </summary>
public sealed class TraderReferenceDto
{
    /// <summary>Gets the trader identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the localized trader name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the normalized trader name.</summary>
    public string NormalizedName { get; init; } = string.Empty;
}
