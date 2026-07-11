namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Describes the ballistic and inventory characteristics of an ammunition item.
/// </summary>
public sealed class AmmoDto
{
    /// <summary>Gets the catalog item associated with the ammunition.</summary>
    public ItemReferenceDto Item { get; init; } = new();

    /// <summary>Gets the weight of one round in kilograms.</summary>
    public double Weight { get; init; }

    /// <summary>Gets the ammunition caliber.</summary>
    public string? Caliber { get; init; }

    /// <summary>Gets the maximum inventory stack size.</summary>
    public int StackMaxSize { get; init; }

    /// <summary>Gets a value indicating whether the round is a tracer.</summary>
    public bool Tracer { get; init; }

    /// <summary>Gets the tracer color.</summary>
    public string? TracerColor { get; init; }

    /// <summary>Gets the upstream ammunition type.</summary>
    public string AmmoType { get; init; } = string.Empty;

    /// <summary>Gets the number of projectiles produced by one round.</summary>
    public int? ProjectileCount { get; init; }

    /// <summary>Gets the flesh damage per projectile.</summary>
    public int Damage { get; init; }

    /// <summary>Gets the armor damage value.</summary>
    public int ArmorDamage { get; init; }

    /// <summary>Gets the fragmentation probability.</summary>
    public double FragmentationChance { get; init; }

    /// <summary>Gets the ricochet probability.</summary>
    public double RicochetChance { get; init; }

    /// <summary>Gets the penetration probability.</summary>
    public double PenetrationChance { get; init; }

    /// <summary>Gets the penetration power.</summary>
    public int PenetrationPower { get; init; }

    /// <summary>Gets the penetration power deviation.</summary>
    public double? PenetrationPowerDeviation { get; init; }

    /// <summary>Gets the weapon accuracy modifier.</summary>
    public double? AccuracyModifier { get; init; }

    /// <summary>Gets the weapon recoil modifier.</summary>
    public double? RecoilModifier { get; init; }

    /// <summary>Gets the projectile's initial speed in meters per second.</summary>
    public double? InitialSpeed { get; init; }

    /// <summary>Gets the light-bleeding probability modifier.</summary>
    public double LightBleedModifier { get; init; }

    /// <summary>Gets the heavy-bleeding probability modifier.</summary>
    public double HeavyBleedModifier { get; init; }

    /// <summary>Gets the stamina burn multiplier applied per point of damage.</summary>
    public double? StaminaBurnPerDamage { get; init; }
}
