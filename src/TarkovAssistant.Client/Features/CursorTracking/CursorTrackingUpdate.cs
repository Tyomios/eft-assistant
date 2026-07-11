using TarkovAssistant.Client.Features.GameWindowDetection;

namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Contains one classified cursor-tracking observation suitable for driving screen capture.
/// </summary>
/// <param name="State">The current hover readiness state.</param>
/// <param name="Position">The current physical cursor position when it was available.</param>
/// <param name="GameWindow">The currently detected Tarkov window when available.</param>
/// <param name="StableFor">The current time inside the positional dwell tolerance.</param>
/// <param name="ShouldTriggerRecognition">Whether this update newly satisfied dwell and should start one recognition attempt.</param>
/// <param name="Detail">An optional user-safe explanation for an unavailable state.</param>
internal readonly record struct CursorTrackingUpdate(
    CursorTrackingState State,
    ScreenPoint? Position,
    TarkovGameWindow? GameWindow,
    TimeSpan StableFor,
    bool ShouldTriggerRecognition,
    string? Detail);
