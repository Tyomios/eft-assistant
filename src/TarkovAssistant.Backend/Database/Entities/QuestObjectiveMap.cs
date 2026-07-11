namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class QuestObjectiveMap : Entity
{
    internal Guid QuestObjectiveId { get; set; }

    internal Guid MapId { get; set; }
}
