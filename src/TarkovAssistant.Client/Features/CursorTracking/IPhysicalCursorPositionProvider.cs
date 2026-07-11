namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Reads the global cursor position without producing input or interacting with the game process.
/// </summary>
internal interface IPhysicalCursorPositionProvider
{
    /// <summary>
    /// Reads the current physical cursor position in virtual-screen coordinates.
    /// </summary>
    /// <returns>The current cursor position or a classified Windows API failure.</returns>
    public CursorPositionResult GetPosition();
}
