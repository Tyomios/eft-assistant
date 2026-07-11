namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Contains a recognition decision, confidence, alternatives, and an optional user-safe failure detail.
/// </summary>
/// <param name="State">The classified recognition outcome.</param>
/// <param name="ItemId">The resolved stable item identifier when recognition succeeded.</param>
/// <param name="Confidence">The normalized confidence from zero through one.</param>
/// <param name="Alternatives">Alternative candidates ordered from most to least likely.</param>
/// <param name="Detail">An optional user-safe explanation for a non-success state.</param>
internal readonly record struct HoverRecognitionResult(
    HoverRecognitionState State,
    Guid? ItemId,
    double Confidence,
    IReadOnlyList<RecognitionCandidate> Alternatives,
    string? Detail);
