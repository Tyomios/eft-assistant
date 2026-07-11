namespace TarkovAssistant.Client.Features.ScreenCapture;

/// <summary>
/// Contains a BGRA32 screen region captured around the hovered inventory cell.
/// </summary>
internal sealed class CapturedInventoryRegion
{
    /// <summary>
    /// Initializes a captured inventory region.
    /// </summary>
    /// <param name="pixels">The top-down BGRA32 pixel buffer.</param>
    /// <param name="width">The region width in physical pixels.</param>
    /// <param name="height">The region height in physical pixels.</param>
    /// <param name="stride">The number of bytes between adjacent pixel rows.</param>
    /// <exception cref="ArgumentOutOfRangeException">A dimension or stride is invalid.</exception>
    /// <exception cref="ArgumentException">The pixel buffer is smaller than the declared region.</exception>
    public CapturedInventoryRegion(ReadOnlyMemory<byte> pixels, int width, int height, int stride)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "The captured width must be positive.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "The captured height must be positive.");
        }

        if (stride < (long)width * 4)
        {
            throw new ArgumentOutOfRangeException(nameof(stride), "The BGRA32 stride is smaller than one pixel row.");
        }

        if (pixels.Length < (long)stride * height)
        {
            throw new ArgumentException("The pixel buffer is smaller than the declared region.", nameof(pixels));
        }

        Pixels = pixels;
        Width = width;
        Height = height;
        Stride = stride;
    }

    /// <summary>Gets the top-down BGRA32 pixel buffer.</summary>
    public ReadOnlyMemory<byte> Pixels { get; }

    /// <summary>Gets the region width in physical pixels.</summary>
    public int Width { get; }

    /// <summary>Gets the region height in physical pixels.</summary>
    public int Height { get; }

    /// <summary>Gets the number of bytes between adjacent pixel rows.</summary>
    public int Stride { get; }
}
