using TarkovAssistant.Client.Features.CursorTracking;
using TarkovAssistant.Client.Features.ScreenCapture;

namespace TarkovAssistant.Client.Features.InventoryGridDetection;

/// <summary>
/// Locates the non-empty stash cell beneath a cursor from locally captured BGRA32 pixels.
/// </summary>
internal interface IStashGridDetector
{
    /// <summary>
    /// Determines whether the cursor lies over a non-empty stash cell.
    /// </summary>
    /// <param name="region">The small BGRA32 region captured around the cursor.</param>
    /// <param name="cursorPosition">The cursor in matching physical virtual-screen pixels.</param>
    /// <returns>A conservative cell classification that excludes ambiguous UI areas.</returns>
    public StashGridDetectionResult Detect(CapturedInventoryRegion region, ScreenPoint cursorPosition);
}
