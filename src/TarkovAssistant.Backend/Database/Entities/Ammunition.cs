namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class Ammunition : Entity
{
    internal Guid ItemId { get; set; }

    internal Guid CaliberId { get; set; }

    internal decimal WeightKilograms { get; set; }

    internal int StackMaximumSize { get; set; }

    internal bool IsTracer { get; set; }

    internal string? TracerColor { get; set; }

    internal string AmmunitionType { get; set; } = string.Empty;

    internal int? ProjectileCount { get; set; }

    internal int Damage { get; set; }

    internal int ArmorDamage { get; set; }

    internal decimal FragmentationChance { get; set; }

    internal decimal RicochetChance { get; set; }

    internal decimal PenetrationChance { get; set; }

    internal int PenetrationPower { get; set; }

    internal decimal? PenetrationPowerDeviation { get; set; }

    internal decimal? AccuracyModifier { get; set; }

    internal decimal? RecoilModifier { get; set; }

    internal decimal? InitialSpeedMetersPerSecond { get; set; }

    internal decimal LightBleedModifier { get; set; }

    internal decimal HeavyBleedModifier { get; set; }

    internal decimal? StaminaBurnPerDamage { get; set; }
}
