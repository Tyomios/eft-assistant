namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

public sealed class TraderDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? ResetTime { get; init; }
    public double Discount { get; init; }
    public string? ImageLink { get; init; }
    public string? Image4xLink { get; init; }
    public int? TarkovDataId { get; init; }
    public ItemReferenceDto Currency { get; init; } = new();
    public IReadOnlyList<TraderLevelDto> Levels { get; init; } = [];
}

public sealed class TraderReferenceDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
}

public sealed class TraderLevelDto
{
    public string Id { get; init; } = string.Empty;
    public int Level { get; init; }
    public int RequiredPlayerLevel { get; init; }
    public double RequiredReputation { get; init; }
    public int RequiredCommerce { get; init; }
    public double PayRate { get; init; }
    public double? InsuranceRate { get; init; }
    public double? RepairCostMultiplier { get; init; }
}
