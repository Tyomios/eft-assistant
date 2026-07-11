namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Describes the classified outcome of a hover recognition attempt.
/// </summary>
internal enum HoverRecognitionState
{
    /// <summary>One catalog item was resolved above the configured confidence threshold.</summary>
    Recognized,

    /// <summary>Multiple candidates remain plausible and no strong recommendation should be shown.</summary>
    Ambiguous,

    /// <summary>No locally cached catalog item matched the captured region.</summary>
    NotRecognized,

    /// <summary>Required local catalog data or images are unavailable.</summary>
    CacheUnavailable,

    /// <summary>The recognition implementation could not complete the attempt.</summary>
    Failed,
}
