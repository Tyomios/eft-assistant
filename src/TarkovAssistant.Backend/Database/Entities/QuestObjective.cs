namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class QuestObjective : SourceEntity
{
    internal Guid QuestId { get; set; }

    internal string ObjectiveType { get; set; } = string.Empty;

    internal string Description { get; set; } = string.Empty;

    internal bool IsOptional { get; set; }

    internal int? RequiredCount { get; set; }

    internal bool? MustBeFoundInRaid { get; set; }

    internal int? MinimumDogTagLevel { get; set; }

    internal int? MinimumDurability { get; set; }

    internal int? MaximumDurability { get; set; }
}
