namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Contains the stability state for the latest cursor sample.
/// </summary>
/// <param name="State">Whether the configured dwell duration has elapsed.</param>
/// <param name="StableFor">The current duration within the configured positional tolerance.</param>
/// <param name="ShouldTriggerRecognition">Whether this sample newly satisfied dwell and should trigger one recognition attempt.</param>
internal readonly record struct CursorDwellEvaluation(
    CursorDwellState State,
    TimeSpan StableFor,
    bool ShouldTriggerRecognition);
