namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents an Escape from Tarkov task.
/// </summary>
public sealed class TaskDto
{
    /// <summary>Gets the Tarkov.dev task identifier.</summary>
    public string? Id { get; init; }

    /// <summary>Gets the localized task name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the normalized task name.</summary>
    public string NormalizedName { get; init; } = string.Empty;

    /// <summary>Gets the experience awarded by the task.</summary>
    public int Experience { get; init; }

    /// <summary>Gets the minimum player level required to start the task.</summary>
    public int? MinPlayerLevel { get; init; }

    /// <summary>Gets the task's wiki URL.</summary>
    public string? WikiLink { get; init; }

    /// <summary>Gets the task image URL.</summary>
    public string? TaskImageLink { get; init; }

    /// <summary>Gets the faction for which the task is available.</summary>
    public string? FactionName { get; init; }

    /// <summary>Gets a value indicating whether the task is required for the Kappa container.</summary>
    public bool? KappaRequired { get; init; }

    /// <summary>Gets a value indicating whether the task is required for Lightkeeper progression.</summary>
    public bool? LightkeeperRequired { get; init; }

    /// <summary>Gets the trader who offers the task.</summary>
    public TraderReferenceDto Trader { get; init; } = new();

    /// <summary>Gets the primary map associated with the task.</summary>
    public MapDto? Map { get; init; }

    /// <summary>Gets the task objectives.</summary>
    public IReadOnlyList<TaskObjectiveDto> Objectives { get; init; } = [];
}
