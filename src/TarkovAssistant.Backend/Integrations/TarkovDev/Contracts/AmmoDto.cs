namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

public sealed class AmmoDto
{
    public ItemReferenceDto Item { get; init; } = new();
    public double Weight { get; init; }
    public string? Caliber { get; init; }
    public int StackMaxSize { get; init; }
    public bool Tracer { get; init; }
    public string? TracerColor { get; init; }
    public string AmmoType { get; init; } = string.Empty;
    public int? ProjectileCount { get; init; }
    public int Damage { get; init; }
    public int ArmorDamage { get; init; }
    public double FragmentationChance { get; init; }
    public double RicochetChance { get; init; }
    public double PenetrationChance { get; init; }
    public int PenetrationPower { get; init; }
    public double? PenetrationPowerDeviation { get; init; }
    public double? AccuracyModifier { get; init; }
    public double? RecoilModifier { get; init; }
    public double? InitialSpeed { get; init; }
    public double LightBleedModifier { get; init; }
    public double HeavyBleedModifier { get; init; }
    public double? StaminaBurnPerDamage { get; init; }
}
