namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Contains user-safe text and optional locally loaded image bytes for the item overlay.
/// </summary>
/// <param name="Title">The recognized item name or concise state title.</param>
/// <param name="Summary">The primary recognition or recommendation summary.</param>
/// <param name="Detail">An optional explanation, uncertainty, or recovery hint.</param>
/// <param name="Confidence">The optional normalized recognition confidence from zero through one.</param>
/// <param name="Tone">The visual emphasis appropriate for the result.</param>
/// <param name="ImageContent">Optional encoded image bytes read from the local catalog cache.</param>
internal readonly record struct ItemOverlayContent(
    string Title,
    string Summary,
    string? Detail,
    double? Confidence,
    OverlayTone Tone,
    ReadOnlyMemory<byte> ImageContent);
