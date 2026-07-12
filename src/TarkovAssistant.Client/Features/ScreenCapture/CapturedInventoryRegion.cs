namespace TarkovAssistant.Client.Features.ScreenCapture;

using TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Contains a BGRA32 screen region captured around the hovered inventory cell.
/// </summary>
internal sealed class CapturedInventoryRegion
{
    /// <summary>
    /// Initializes a captured inventory region.
    /// </summary>
    /// <param name="pixels">The top-down BGRA32 pixel buffer.</param>
    /// <param name="bounds">The physical virtual-screen bounds of the captured region.</param>
    /// <param name="stride">The number of bytes between adjacent pixel rows.</param>
    /// <exception cref="ArgumentOutOfRangeException">A dimension or stride is invalid.</exception>
    /// <exception cref="ArgumentException">The pixel buffer is smaller than the declared region.</exception>
    public CapturedInventoryRegion(
        ReadOnlyMemory<byte> pixels,
        PhysicalScreenRectangle bounds,
        int stride)
    {
        if (bounds.Width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bounds), "The captured width must be positive.");
        }

        if (bounds.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bounds), "The captured height must be positive.");
        }

        if (stride < (long)bounds.Width * 4)
        {
            throw new ArgumentOutOfRangeException(nameof(stride), "The BGRA32 stride is smaller than one pixel row.");
        }

        if (pixels.Length < (long)stride * bounds.Height)
        {
            throw new ArgumentException("The pixel buffer is smaller than the declared region.", nameof(pixels));
        }

        Pixels = pixels;
        Bounds = bounds;
        Stride = stride;
    }

    /// <summary>Gets the top-down BGRA32 pixel buffer.</summary>
    public ReadOnlyMemory<byte> Pixels { get; }

    /// <summary>Gets the region bounds in physical virtual-screen pixels.</summary>
    public PhysicalScreenRectangle Bounds { get; }

    /// <summary>Gets the region width in physical pixels.</summary>
    public int Width => Bounds.Width;

    /// <summary>Gets the region height in physical pixels.</summary>
    public int Height => Bounds.Height;

    /// <summary>Gets the number of bytes between adjacent pixel rows.</summary>
    public int Stride { get; }
}
