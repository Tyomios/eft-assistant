namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class WeaponAmmunitionCompatibility : Entity
{
    internal Guid WeaponId { get; set; }

    internal Guid AmmunitionId { get; set; }
}
