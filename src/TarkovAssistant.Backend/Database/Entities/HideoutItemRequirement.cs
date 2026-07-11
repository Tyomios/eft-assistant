namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class HideoutItemRequirement : SourceEntity
{
    internal Guid HideoutStationLevelId { get; set; }

    internal Guid ItemId { get; set; }

    internal int Count { get; set; }

    internal int Quantity { get; set; }
}
