using TarkovAssistant.Client.Features.CursorTracking;
using TarkovAssistant.Client.Features.ScreenCapture;

namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Contains the cursor position and smallest useful captured region for one recognition attempt.
/// </summary>
/// <param name="CursorPosition">The cursor position in physical virtual-screen pixels.</param>
/// <param name="CapturedRegion">The captured BGRA32 pixels around the hovered inventory cell.</param>
internal readonly record struct HoverRecognitionRequest(
    ScreenPoint CursorPosition,
    CapturedInventoryRegion CapturedRegion);
