using TarkovAssistant.Backend.Integrations.TarkovDev;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Defines optional localization, paging, and item filtering parameters accepted by Tarkov API controllers.
/// </summary>
public sealed class TarkovQueryParameters
{
    /// <summary>Gets the Tarkov.dev language code.</summary>
    public string? Language { get; init; }

    /// <summary>Gets the Tarkov.dev game mode.</summary>
    public string? GameMode { get; init; }

    /// <summary>Gets the maximum number of records to return.</summary>
    public int? Limit { get; init; }

    /// <summary>Gets the zero-based record offset.</summary>
    public int? Offset { get; init; }

    /// <summary>Gets the optional Tarkov.dev item type.</summary>
    public string? ItemType { get; init; }

    internal TarkovDevQuery ToQuery(bool allowItemType)
    {
        var language = Language ?? "en";
        var gameMode = GameMode ?? "regular";
        var limit = Limit ?? 100;
        var offset = Offset ?? 0;
        TarkovQueryValidator.Validate(language, gameMode, limit, offset, ItemType, allowItemType);
        return new TarkovDevQuery(language, gameMode, limit, offset, ItemType ?? "any");
    }
}
