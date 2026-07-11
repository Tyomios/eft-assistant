using System.Runtime.InteropServices;

namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Carries a native physical-pixel window rectangle across the Win32 boundary.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct NativeRectangle
{
    internal int Left;
    internal int Top;
    internal int Right;
    internal int Bottom;
}
