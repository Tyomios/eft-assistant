namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Defines a physical-pixel rectangle in the virtual desktop coordinate system.
/// </summary>
/// <param name="Left">The horizontal left edge in physical pixels.</param>
/// <param name="Top">The vertical top edge in physical pixels.</param>
/// <param name="Width">The rectangle width in physical pixels.</param>
/// <param name="Height">The rectangle height in physical pixels.</param>
internal readonly record struct PhysicalScreenRectangle(int Left, int Top, int Width, int Height)
{
    /// <summary>
    /// Determines whether a physical-pixel point is inside the rectangle.
    /// </summary>
    /// <param name="point">The virtual-screen point to evaluate.</param>
    /// <returns><see langword="true"/> when the point is within the rectangle's left/top-inclusive bounds.</returns>
    public bool Contains(CursorTracking.ScreenPoint point)
    {
        return point.X >= Left
            && point.X < (long)Left + Width
            && point.Y >= Top
            && point.Y < (long)Top + Height;
    }
}
