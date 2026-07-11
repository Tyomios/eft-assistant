using TarkovAssistant.Backend.Integrations.TarkovDev;

namespace TarkovAssistant.Backend.Features.TarkovData;

internal static class PageResponseFactory
{
    internal static PageResponse<T> Create<T>(TarkovDevQuery query, IReadOnlyList<T> items)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(items);

        return new PageResponse<T>(query.Offset, query.Limit, items.Count, items);
    }
}
