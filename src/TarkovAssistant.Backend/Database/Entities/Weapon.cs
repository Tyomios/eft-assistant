namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Weapon : Entity
{
    internal Guid ItemId { get; set; }

    internal Guid CaliberId { get; set; }

    internal Guid? DefaultAmmunitionId { get; set; }

    internal int? EffectiveDistanceMeters { get; set; }

    internal decimal? Ergonomics { get; set; }

    internal int? FireRateRoundsPerMinute { get; set; }

    internal int? MaximumDurability { get; set; }

    internal int? VerticalRecoil { get; set; }

    internal int? HorizontalRecoil { get; set; }

    internal int? SightingRangeMeters { get; set; }

    internal int? DefaultWidth { get; set; }

    internal int? DefaultHeight { get; set; }

    internal decimal? DefaultErgonomics { get; set; }

    internal int? DefaultVerticalRecoil { get; set; }

    internal int? DefaultHorizontalRecoil { get; set; }

    internal decimal? DefaultWeightKilograms { get; set; }
}
