namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Defines the confidence threshold above which the client may present a strong recognition result.
/// </summary>
internal sealed class HoverRecognitionOptions
{
    /// <summary>
    /// Gets the minimum normalized confidence required for a strong result.
    /// </summary>
    public double MinimumStrongConfidence { get; init; } = 0.80;
}
