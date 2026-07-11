using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class TradersData
{
    /// <summary>Gets the traders returned by the query.</summary>
    public IReadOnlyList<TraderDto> Traders { get; init; } = [];
}
