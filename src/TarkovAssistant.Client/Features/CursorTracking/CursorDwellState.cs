namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Describes whether the cursor has remained within the configured positional tolerance long enough.
/// </summary>
internal enum CursorDwellState
{
    /// <summary>The cursor has not yet remained stationary for the configured dwell duration.</summary>
    Pending,

    /// <summary>The cursor has remained stationary for the configured dwell duration.</summary>
    Satisfied,
}
