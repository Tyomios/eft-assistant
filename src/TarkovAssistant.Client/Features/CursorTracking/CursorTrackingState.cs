namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Describes the current ability to evaluate a hover inside the Tarkov game window.
/// </summary>
internal enum CursorTrackingState
{
    /// <summary>The Tarkov window is absent, minimized, or could not be inspected.</summary>
    GameUnavailable,

    /// <summary>The cursor is outside the Tarkov window bounds.</summary>
    OutsideGameWindow,

    /// <summary>The cursor is inside the visible Tarkov window and waiting for dwell.</summary>
    WaitingForDwell,

    /// <summary>The cursor has satisfied dwell and a recognition attempt may begin.</summary>
    DwellSatisfied,

    /// <summary>The global cursor position could not be read from Windows.</summary>
    CursorUnavailable,
}
