using System.ComponentModel;

namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Reads the Windows global cursor position through the documented GetCursorPos API.
/// </summary>
internal sealed class WindowsCursorPositionProvider : IPhysicalCursorPositionProvider
{
    /// <inheritdoc />
    public CursorPositionResult GetPosition()
    {
        try
        {
            return new CursorPositionResult(
                CursorPositionReadState.Available,
                NativeCursorMethods.GetPosition(),
                null);
        }
        catch (Win32Exception)
        {
            return new CursorPositionResult(
                CursorPositionReadState.Failed,
                null,
                "The Windows cursor position is unavailable.");
        }
    }
}
