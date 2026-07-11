namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Contains the classified result of locating the Tarkov game window.
/// </summary>
/// <param name="State">The capture-relevant detection state.</param>
/// <param name="Window">The matching game window when one was found.</param>
/// <param name="Detail">An optional user-safe explanation for a failed detection.</param>
internal readonly record struct GameWindowDetectionResult(
    GameWindowDetectionState State,
    TarkovGameWindow? Window,
    string? Detail);
