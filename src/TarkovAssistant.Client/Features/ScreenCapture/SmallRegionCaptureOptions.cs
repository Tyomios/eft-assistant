namespace TarkovAssistant.Client.Features.ScreenCapture;

/// <summary>
/// Configures the physical-pixel region copied from a Tarkov window capture frame.
/// </summary>
internal sealed class SmallRegionCaptureOptions
{
    /// <summary>Gets the requested output width in physical pixels.</summary>
    public int Width { get; init; } = 384;

    /// <summary>Gets the requested output height in physical pixels.</summary>
    public int Height { get; init; } = 384;
}
