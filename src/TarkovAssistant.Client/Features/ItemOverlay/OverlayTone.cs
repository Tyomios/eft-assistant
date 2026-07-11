namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Selects the restrained visual emphasis used for overlay content.
/// </summary>
internal enum OverlayTone
{
    /// <summary>Neutral informational content.</summary>
    Informational,

    /// <summary>A confident successful recognition result.</summary>
    Positive,

    /// <summary>An uncertain result that should not present a strong recommendation.</summary>
    Warning,

    /// <summary>A user-safe local processing failure.</summary>
    Error,
}
