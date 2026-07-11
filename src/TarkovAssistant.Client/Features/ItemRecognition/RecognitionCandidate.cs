namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Identifies one locally matched catalog item and its normalized confidence.
/// </summary>
/// <param name="ItemId">The stable internal catalog item identifier.</param>
/// <param name="Confidence">The normalized confidence from zero through one.</param>
internal readonly record struct RecognitionCandidate(Guid ItemId, double Confidence);
