namespace TarkovAssistant.Client.Features.ScreenCapture;

/// <summary>
/// Contains a classified small-region screen capture result.
/// </summary>
/// <param name="State">The capture outcome.</param>
/// <param name="Region">The BGRA32 region when <paramref name="State"/> is <see cref="ScreenCaptureState.Captured"/>.</param>
/// <param name="Detail">An optional user-safe explanation when no region was captured.</param>
internal readonly record struct ScreenCaptureResult(
    ScreenCaptureState State,
    CapturedInventoryRegion? Region,
    string? Detail);
