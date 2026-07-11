namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Describes whether an overlay operation completed safely.
/// </summary>
internal enum OverlayOperationState
{
    /// <summary>The requested overlay operation completed.</summary>
    Completed,

    /// <summary>The overlay could not be shown or hidden safely.</summary>
    Failed,
}
