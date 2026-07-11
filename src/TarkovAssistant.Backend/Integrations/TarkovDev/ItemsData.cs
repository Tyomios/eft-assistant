using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class ItemsData
{
    /// <summary>Gets the items returned by the query.</summary>
    public IReadOnlyList<ItemDto> Items { get; init; } = [];
}
