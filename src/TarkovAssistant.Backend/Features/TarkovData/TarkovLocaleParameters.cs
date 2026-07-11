using TarkovAssistant.Backend.Integrations.TarkovDev;

namespace TarkovAssistant.Backend.Features.TarkovData;

internal sealed class TarkovLocaleParameters
{
    /// <summary>Gets the Tarkov.dev language code.</summary>
    public string? Language { get; init; }

    /// <summary>Gets the Tarkov.dev game mode.</summary>
    public string? GameMode { get; init; }

    internal TarkovDevQuery ToQuery()
    {
        var language = Language ?? "en";
        var gameMode = GameMode ?? "regular";
        TarkovQueryValidator.ValidateLocale(language, gameMode);
        return new TarkovDevQuery(language, gameMode);
    }
}
