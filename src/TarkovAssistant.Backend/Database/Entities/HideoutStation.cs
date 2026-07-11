namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class HideoutStation : SourceEntity
{
    internal string Name { get; set; } = string.Empty;

    internal string NormalizedName { get; set; } = string.Empty;

    internal string? ImageUrl { get; set; }

    internal int? TarkovDataId { get; set; }
}
