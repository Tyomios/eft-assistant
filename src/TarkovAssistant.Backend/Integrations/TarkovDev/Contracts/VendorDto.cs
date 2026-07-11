namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Identifies a trader or the flea market in a price offer.
/// </summary>
public sealed class VendorDto
{
    /// <summary>Gets the localized vendor name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the normalized vendor name.</summary>
    public string NormalizedName { get; init; } = string.Empty;
}
