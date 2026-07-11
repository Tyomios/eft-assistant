namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class MagazineAmmunitionCompatibility : Entity
{
    internal Guid MagazineId { get; set; }

    internal Guid AmmunitionId { get; set; }
}
