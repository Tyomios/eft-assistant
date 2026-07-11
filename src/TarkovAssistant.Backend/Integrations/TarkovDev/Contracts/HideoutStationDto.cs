namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

public sealed class HideoutStationDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
    public string? ImageLink { get; init; }
    public int? TarkovDataId { get; init; }
    public IReadOnlyList<HideoutStationLevelDto> Levels { get; init; } = [];
}

public sealed class HideoutStationLevelDto
{
    public string Id { get; init; } = string.Empty;
    public int Level { get; init; }
    public int ConstructionTime { get; init; }
    public string Description { get; init; } = string.Empty;
    public int? TarkovDataId { get; init; }
    public IReadOnlyList<ItemRequirementDto> ItemRequirements { get; init; } = [];
}

public sealed class ItemRequirementDto
{
    public string? Id { get; init; }
    public int Count { get; init; }
    public int Quantity { get; init; }
    public ItemReferenceDto Item { get; init; } = new();
}
