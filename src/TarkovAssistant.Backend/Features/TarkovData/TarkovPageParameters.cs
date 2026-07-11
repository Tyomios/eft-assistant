using TarkovAssistant.Backend.Integrations.TarkovDev;

namespace TarkovAssistant.Backend.Features.TarkovData;

/// <summary>
/// Defines optional localization and paging parameters accepted by Tarkov API controllers.
/// </summary>
public sealed class TarkovPageParameters
{
    /// <summary>Gets the Tarkov.dev language code.</summary>
    public string? Language { get; init; }

    /// <summary>Gets the Tarkov.dev game mode.</summary>
    public string? GameMode { get; init; }

    /// <summary>Gets the maximum number of records to return.</summary>
    public int? Limit { get; init; }

    /// <summary>Gets the zero-based record offset.</summary>
    public int? Offset { get; init; }

    internal TarkovDevQuery ToQuery()
    {
        var language = Language ?? "en";
        var gameMode = GameMode ?? "regular";
        var limit = Limit ?? 100;
        var offset = Offset ?? 0;
        TarkovQueryValidator.ValidatePage(language, gameMode, limit, offset);
        return new TarkovDevQuery(language, gameMode, limit, offset);
    }
}
