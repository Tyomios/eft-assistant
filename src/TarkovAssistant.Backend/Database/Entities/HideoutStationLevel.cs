namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class HideoutStationLevel : SourceEntity
{
    internal Guid HideoutStationId { get; set; }

    internal int Level { get; set; }

    internal int ConstructionTimeSeconds { get; set; }

    internal string Description { get; set; } = string.Empty;

    internal int? TarkovDataId { get; set; }
}
