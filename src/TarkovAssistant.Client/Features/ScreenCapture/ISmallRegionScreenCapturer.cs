using TarkovAssistant.Client.Features.CursorTracking;
using TarkovAssistant.Client.Features.GameWindowDetection;

namespace TarkovAssistant.Client.Features.ScreenCapture;

/// <summary>
/// Captures only the smallest useful physical-pixel region around a cursor position.
/// </summary>
internal interface ISmallRegionScreenCapturer : IDisposable
{
    /// <summary>
    /// Captures a BGRA32 region from the given game window without capturing the desktop.
    /// </summary>
    /// <param name="gameWindow">The current foreground Tarkov window and client bounds.</param>
    /// <param name="cursorPosition">The physical virtual-screen position to center in the returned region.</param>
    /// <param name="cancellationToken">Cancels the pending frame wait.</param>
    /// <returns>A classified capture result.</returns>
    public ValueTask<ScreenCaptureResult> CaptureAsync(
        TarkovGameWindow gameWindow,
        ScreenPoint cursorPosition,
        CancellationToken cancellationToken);
}
