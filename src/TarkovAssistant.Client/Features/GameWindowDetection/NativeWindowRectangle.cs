using System.Runtime.InteropServices;

namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Carries a native physical-pixel window rectangle across the Win32 boundary.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct NativeWindowRectangle
{
    internal int Left;
    internal int Top;
    internal int Right;
    internal int Bottom;
}
