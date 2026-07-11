using System.Text.Json.Serialization;

namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents a task objective and its item-specific requirements when applicable.
/// </summary>
public sealed class TaskObjectiveDto
{
    /// <summary>Gets the concrete GraphQL objective type.</summary>
    [JsonPropertyName("__typename")]
    public string TypeName { get; init; } = string.Empty;

    /// <summary>Gets the objective identifier.</summary>
    public string? Id { get; init; }

    /// <summary>Gets the upstream objective type.</summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>Gets the localized objective description.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>Gets a value indicating whether the objective is optional.</summary>
    public bool Optional { get; init; }

    /// <summary>Gets the maps on which the objective can be completed.</summary>
    public IReadOnlyList<MapDto> Maps { get; init; } = [];

    /// <summary>Gets the accepted items for an item objective.</summary>
    public IReadOnlyList<ItemReferenceDto>? Items { get; init; }

    /// <summary>Gets the number of required items or actions.</summary>
    public int? Count { get; init; }

    /// <summary>Gets a value indicating whether required items must be found in raid.</summary>
    public bool? FoundInRaid { get; init; }

    /// <summary>Gets the minimum dog-tag level.</summary>
    public int? DogTagLevel { get; init; }

    /// <summary>Gets the minimum accepted item durability.</summary>
    public int? MinDurability { get; init; }

    /// <summary>Gets the maximum accepted item durability.</summary>
    public int? MaxDurability { get; init; }
}
