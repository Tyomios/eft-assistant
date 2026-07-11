namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Describes the requirements and rates for a trader loyalty level.
/// </summary>
public sealed class TraderLevelDto
{
    /// <summary>Gets the loyalty-level identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the loyalty-level number.</summary>
    public int Level { get; init; }

    /// <summary>Gets the required player level.</summary>
    public int RequiredPlayerLevel { get; init; }

    /// <summary>Gets the required trader reputation.</summary>
    public double RequiredReputation { get; init; }

    /// <summary>Gets the required commerce total.</summary>
    public int RequiredCommerce { get; init; }

    /// <summary>Gets the trader payout rate.</summary>
    public double PayRate { get; init; }

    /// <summary>Gets the insurance price multiplier.</summary>
    public double? InsuranceRate { get; init; }

    /// <summary>Gets the item repair cost multiplier.</summary>
    public double? RepairCostMultiplier { get; init; }
}
