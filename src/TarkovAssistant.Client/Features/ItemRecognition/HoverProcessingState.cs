namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Describes the UI-visible outcome of processing one stationary-cursor capture.
/// </summary>
internal enum HoverProcessingState
{
    /// <summary>The classified recognition result was displayed.</summary>
    Displayed,

    /// <summary>The recognizer failed before returning a classified result.</summary>
    RecognitionFailed,

    /// <summary>The overlay could not be updated safely.</summary>
    OverlayFailed,
}
