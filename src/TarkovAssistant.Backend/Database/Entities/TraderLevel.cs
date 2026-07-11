namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class TraderLevel : SourceEntity
{
    internal Guid TraderId { get; set; }

    internal int Level { get; set; }

    internal int RequiredPlayerLevel { get; set; }

    internal decimal RequiredReputation { get; set; }

    internal int RequiredCommerce { get; set; }

    internal decimal PayRate { get; set; }

    internal decimal? InsuranceRate { get; set; }

    internal decimal? RepairCostMultiplier { get; set; }
}
