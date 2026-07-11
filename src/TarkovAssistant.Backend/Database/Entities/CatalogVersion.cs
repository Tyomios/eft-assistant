namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class CatalogVersion : Entity
{
    internal long Version { get; set; }

    internal DateTimeOffset PublishedAt { get; set; }

    internal string ContentHash { get; set; } = string.Empty;
}
