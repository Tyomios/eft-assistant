namespace TarkovAssistant.Backend.Integrations.TarkovDev;

/// <summary>
/// Defines localization, game mode, paging, and item filtering for a Tarkov.dev query.
/// </summary>
public sealed record TarkovDevQuery
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TarkovDevQuery"/> record.
    /// </summary>
    /// <param name="language">The Tarkov.dev language code.</param>
    /// <param name="gameMode">The Tarkov.dev game mode.</param>
    /// <param name="limit">The maximum number of records to return.</param>
    /// <param name="offset">The zero-based record offset.</param>
    /// <param name="itemType">The Tarkov.dev item type.</param>
    public TarkovDevQuery(
        string language = "en",
        string gameMode = "regular",
        int limit = 100,
        int offset = 0,
        string itemType = "any")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(language);
        ArgumentException.ThrowIfNullOrWhiteSpace(gameMode);
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 500);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentException.ThrowIfNullOrWhiteSpace(itemType);

        Language = language;
        GameMode = gameMode;
        Limit = limit;
        Offset = offset;
        ItemType = itemType;
    }

    /// <summary>Gets the Tarkov.dev language code.</summary>
    public string Language { get; init; }

    /// <summary>Gets the Tarkov.dev game mode.</summary>
    public string GameMode { get; init; }

    /// <summary>Gets the maximum number of records to return.</summary>
    public int Limit { get; init; }

    /// <summary>Gets the zero-based record offset.</summary>
    public int Offset { get; init; }

    /// <summary>Gets the Tarkov.dev item type.</summary>
    public string ItemType { get; init; }
}
