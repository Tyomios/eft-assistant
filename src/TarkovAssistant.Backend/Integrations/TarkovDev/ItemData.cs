using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class ItemData
{
    /// <summary>Gets the item returned by the query.</summary>
    public ItemDto? Item { get; init; }
}
