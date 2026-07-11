namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Describes the current availability of the Tarkov game window for screen capture.
/// </summary>
internal enum GameWindowDetectionState
{
    /// <summary>No visible window owned by the Tarkov game process was found.</summary>
    NotFound,

    /// <summary>A visible, non-minimized Tarkov game window was found.</summary>
    Found,

    /// <summary>A Tarkov game window was found but is minimized and cannot be captured.</summary>
    Minimized,

    /// <summary>The Windows windowing APIs could not complete the detection.</summary>
    Failed,
}
