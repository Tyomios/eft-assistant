using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class AmmoData
{
    /// <summary>Gets the ammunition records returned by the query.</summary>
    public IReadOnlyList<AmmoDto> Ammo { get; init; } = [];
}
