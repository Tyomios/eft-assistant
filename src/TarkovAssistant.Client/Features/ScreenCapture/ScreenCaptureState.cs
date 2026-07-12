namespace TarkovAssistant.Client.Features.ScreenCapture;

/// <summary>
/// Classifies the outcome of a small Windows.Graphics.Capture read.
/// </summary>
internal enum ScreenCaptureState
{
    /// <summary>A BGRA32 region was captured around the cursor.</summary>
    Captured,

    /// <summary>Windows.Graphics.Capture is unavailable on this Windows installation.</summary>
    Unsupported,

    /// <summary>The requested game window is no longer a valid capture target.</summary>
    WindowUnavailable,

    /// <summary>The game client changed size while the capture session was starting.</summary>
    ResolutionChanged,

    /// <summary>No frame was delivered before cancellation or source closure.</summary>
    FrameUnavailable,

    /// <summary>The capture API returned an expected operational failure.</summary>
    Failed,
}
