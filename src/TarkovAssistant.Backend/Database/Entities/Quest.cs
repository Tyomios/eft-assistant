namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Quest : SourceEntity
{
    internal Guid TraderId { get; set; }

    internal Guid? MapId { get; set; }

    internal string Name { get; set; } = string.Empty;

    internal string NormalizedName { get; set; } = string.Empty;

    internal int Experience { get; set; }

    internal int? MinimumPlayerLevel { get; set; }

    internal string? WikiUrl { get; set; }

    internal string? ImageUrl { get; set; }

    internal string? FactionName { get; set; }

    internal bool? IsKappaRequired { get; set; }

    internal bool? IsLightkeeperRequired { get; set; }
}
