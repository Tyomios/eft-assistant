using System.Text.Json.Serialization;

namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents weapon- or magazine-specific item properties.
/// </summary>
public sealed class ItemPropertiesDto
{
    /// <summary>Gets the concrete GraphQL property type.</summary>
    [JsonPropertyName("__typename")]
    public string TypeName { get; init; } = string.Empty;

    /// <summary>Gets the weapon caliber.</summary>
    public string? Caliber { get; init; }

    /// <summary>Gets the default ammunition item.</summary>
    public ItemReferenceDto? DefaultAmmo { get; init; }

    /// <summary>Gets the effective firing distance in meters.</summary>
    public int? EffectiveDistance { get; init; }

    /// <summary>Gets the ergonomics value.</summary>
    public double? Ergonomics { get; init; }

    /// <summary>Gets the supported fire modes.</summary>
    public IReadOnlyList<string>? FireModes { get; init; }

    /// <summary>Gets the cyclic fire rate.</summary>
    public int? FireRate { get; init; }

    /// <summary>Gets the maximum durability.</summary>
    public int? MaxDurability { get; init; }

    /// <summary>Gets the vertical recoil value.</summary>
    public int? RecoilVertical { get; init; }

    /// <summary>Gets the horizontal recoil value.</summary>
    public int? RecoilHorizontal { get; init; }

    /// <summary>Gets the sighting range in meters.</summary>
    public int? SightingRange { get; init; }

    /// <summary>Gets the default inventory width in cells.</summary>
    public int? DefaultWidth { get; init; }

    /// <summary>Gets the default inventory height in cells.</summary>
    public int? DefaultHeight { get; init; }

    /// <summary>Gets the default preset ergonomics.</summary>
    public double? DefaultErgonomics { get; init; }

    /// <summary>Gets the default preset vertical recoil.</summary>
    public int? DefaultRecoilVertical { get; init; }

    /// <summary>Gets the default preset horizontal recoil.</summary>
    public int? DefaultRecoilHorizontal { get; init; }

    /// <summary>Gets the default preset weight in kilograms.</summary>
    public double? DefaultWeight { get; init; }

    /// <summary>Gets the magazine recoil modifier.</summary>
    public double? RecoilModifier { get; init; }

    /// <summary>Gets the magazine capacity.</summary>
    public int? Capacity { get; init; }

    /// <summary>Gets the magazine loading-speed modifier.</summary>
    public double? LoadModifier { get; init; }

    /// <summary>Gets the ammunition-check speed modifier.</summary>
    public double? AmmoCheckModifier { get; init; }

    /// <summary>Gets the magazine malfunction probability modifier.</summary>
    public double? MalfunctionChance { get; init; }

    /// <summary>Gets ammunition compatible with the weapon or magazine.</summary>
    public IReadOnlyList<ItemReferenceDto>? AllowedAmmo { get; init; }
}
