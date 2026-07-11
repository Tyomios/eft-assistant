namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Describes the outcome of obtaining the current physical cursor position from Windows.
/// </summary>
internal enum CursorPositionReadState
{
    /// <summary>The physical cursor position was read successfully.</summary>
    Available,

    /// <summary>Windows could not provide the cursor position.</summary>
    Failed,
}
