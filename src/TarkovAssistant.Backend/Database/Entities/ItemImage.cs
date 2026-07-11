namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class ItemImage : Entity
{
    internal Guid ItemId { get; set; }

    internal ItemImageKind Kind { get; set; }

    internal string SourceUrl { get; set; } = string.Empty;

    internal string? ContentHash { get; set; }

    internal string? LocalPath { get; set; }

    internal DateTimeOffset? DownloadedAt { get; set; }
}
