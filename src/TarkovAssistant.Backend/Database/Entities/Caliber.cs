namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Caliber : SourceEntity
{
    internal string Name { get; set; } = string.Empty;

    internal string NormalizedName { get; set; } = string.Empty;
}
