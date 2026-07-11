namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Defines the polling cadence and cursor stability threshold for hover recognition.
/// </summary>
internal sealed class CursorTrackingOptions
{
    /// <summary>
    /// Gets the interval between global cursor samples.
    /// </summary>
    public TimeSpan PollInterval { get; init; } = TimeSpan.FromMilliseconds(50);

    /// <summary>
    /// Gets the required stationary duration before one recognition attempt may start.
    /// </summary>
    public TimeSpan DwellDuration { get; init; } = TimeSpan.FromMilliseconds(250);

    /// <summary>
    /// Gets the maximum cursor drift in physical pixels allowed during dwell.
    /// </summary>
    public int PositionTolerancePixels { get; init; } = 3;
}
