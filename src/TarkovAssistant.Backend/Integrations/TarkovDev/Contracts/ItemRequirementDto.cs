namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Describes an item required for a hideout upgrade.
/// </summary>
public sealed class ItemRequirementDto
{
    /// <summary>Gets the requirement identifier.</summary>
    public string? Id { get; init; }

    /// <summary>Gets the required item count.</summary>
    public int Count { get; init; }

    /// <summary>Gets the upstream quantity value.</summary>
    public int Quantity { get; init; }

    /// <summary>Gets the required item.</summary>
    public ItemReferenceDto Item { get; init; } = new();
}
