using System.Text.Json.Serialization;

namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

public sealed class TaskDto
{
    public string? Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
    public int Experience { get; init; }
    public int? MinPlayerLevel { get; init; }
    public string? WikiLink { get; init; }
    public string? TaskImageLink { get; init; }
    public string? FactionName { get; init; }
    public bool? KappaRequired { get; init; }
    public bool? LightkeeperRequired { get; init; }
    public TraderReferenceDto Trader { get; init; } = new();
    public MapDto? Map { get; init; }
    public IReadOnlyList<TaskObjectiveDto> Objectives { get; init; } = [];
}

public sealed class TaskObjectiveDto
{
    [JsonPropertyName("__typename")]
    public string TypeName { get; init; } = string.Empty;

    public string? Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool Optional { get; init; }
    public IReadOnlyList<MapDto> Maps { get; init; } = [];
    public IReadOnlyList<ItemReferenceDto>? Items { get; init; }
    public int? Count { get; init; }
    public bool? FoundInRaid { get; init; }
    public int? DogTagLevel { get; init; }
    public int? MinDurability { get; init; }
    public int? MaxDurability { get; init; }
}

public sealed class MapDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
}
