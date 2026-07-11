namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Locates the supported Tarkov game window through documented Windows windowing APIs.
/// </summary>
internal interface ITarkovGameWindowDetector
{
    /// <summary>
    /// Locates the largest eligible top-level window owned by the Tarkov game process.
    /// </summary>
    /// <returns>The current matching window, a non-capturable minimized state, or a classified failure.</returns>
    public GameWindowDetectionResult Detect();
}
