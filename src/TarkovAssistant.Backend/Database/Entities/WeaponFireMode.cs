namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class WeaponFireMode : Entity
{
    internal Guid WeaponId { get; set; }

    internal string Mode { get; set; } = string.Empty;
}
