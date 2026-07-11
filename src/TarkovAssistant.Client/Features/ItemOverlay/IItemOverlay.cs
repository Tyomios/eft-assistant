using TarkovAssistant.Client.Features.CursorTracking;

namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Displays recognition information without activation or pointer input interception.
/// </summary>
internal interface IItemOverlay
{
    /// <summary>
    /// Shows or updates the overlay near the physical cursor position.
    /// </summary>
    /// <param name="content">The user-safe overlay content and optional locally cached image.</param>
    /// <param name="cursorPosition">The cursor position in physical virtual-screen pixels.</param>
    /// <param name="cancellationToken">Stops the operation before it reaches the UI thread.</param>
    /// <returns>The safe display outcome.</returns>
    public Task<OverlayOperationResult> ShowAsync(
        ItemOverlayContent content,
        ScreenPoint cursorPosition,
        CancellationToken cancellationToken);

    /// <summary>
    /// Hides the overlay when the cursor leaves the recognized item.
    /// </summary>
    /// <param name="cancellationToken">Stops the operation before it reaches the UI thread.</param>
    /// <returns>The safe hide outcome.</returns>
    public Task<OverlayOperationResult> HideAsync(CancellationToken cancellationToken);
}
