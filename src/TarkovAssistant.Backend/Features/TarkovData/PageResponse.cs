namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Wraps a page of API results with its paging metadata.
/// </summary>
/// <typeparam name="T">The result item type.</typeparam>
public sealed record PageResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageResponse{T}"/> record.
    /// </summary>
    /// <param name="offset">The zero-based record offset.</param>
    /// <param name="limit">The requested page size.</param>
    /// <param name="count">The number of records in the current page.</param>
    /// <param name="items">The records in the current page.</param>
    public PageResponse(int offset, int limit, int count, IReadOnlyList<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        Offset = offset;
        Limit = limit;
        Count = count;
        Items = items;
    }

    /// <summary>Gets the zero-based record offset.</summary>
    public int Offset { get; }

    /// <summary>Gets the requested page size.</summary>
    public int Limit { get; }

    /// <summary>Gets the number of records in the current page.</summary>
    public int Count { get; }

    /// <summary>Gets the records in the current page.</summary>
    public IReadOnlyList<T> Items { get; }

}
