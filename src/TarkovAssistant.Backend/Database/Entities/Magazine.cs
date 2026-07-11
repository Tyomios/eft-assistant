namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Magazine : Entity
{
    internal Guid ItemId { get; set; }

    internal decimal? Ergonomics { get; set; }

    internal decimal? RecoilModifier { get; set; }

    internal int? Capacity { get; set; }

    internal decimal? LoadModifier { get; set; }

    internal decimal? AmmunitionCheckModifier { get; set; }

    internal decimal? MalfunctionChance { get; set; }
}
