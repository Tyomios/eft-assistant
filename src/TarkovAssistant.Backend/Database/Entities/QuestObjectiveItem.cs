namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class QuestObjectiveItem : Entity
{
    internal Guid QuestObjectiveId { get; set; }

    internal Guid ItemId { get; set; }
}
