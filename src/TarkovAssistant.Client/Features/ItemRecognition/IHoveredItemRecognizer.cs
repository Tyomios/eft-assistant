namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Resolves a hovered stash item from captured screen pixels and locally cached catalog data.
/// </summary>
internal interface IHoveredItemRecognizer
{
    /// <summary>
    /// Attempts to identify the item visible beneath the stationary cursor.
    /// </summary>
    /// <param name="request">The cursor location and captured inventory pixels.</param>
    /// <param name="cancellationToken">Stops recognition when the cursor moves or application shutdown is requested.</param>
    /// <returns>The resolved item, confidence, alternatives, or a classified failure.</returns>
    public ValueTask<HoverRecognitionResult> RecognizeAsync(
        HoverRecognitionRequest request,
        CancellationToken cancellationToken);
}
