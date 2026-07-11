using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Provides the documented Win32 call used to observe the global cursor position.
/// </summary>
internal static partial class NativeCursorMethods
{
    internal static ScreenPoint GetPosition()
    {
        if (!GetCursorPosition(out var point))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The cursor position could not be read.");
        }

        return new ScreenPoint(point.X, point.Y);
    }

    [LibraryImport("user32.dll", EntryPoint = "GetCursorPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetCursorPosition(out NativeCursorPoint point);
}
