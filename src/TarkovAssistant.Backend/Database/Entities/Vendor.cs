namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Vendor : SourceEntity
{
    internal string Name { get; set; } = string.Empty;

    internal string NormalizedName { get; set; } = string.Empty;
}
