using System.Runtime.InteropServices;

namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Carries a native physical cursor point across the Win32 boundary.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct NativeCursorPoint
{
    internal int X;
    internal int Y;
}
