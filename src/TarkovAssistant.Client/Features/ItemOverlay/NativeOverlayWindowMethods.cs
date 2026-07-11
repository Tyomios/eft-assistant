using System.ComponentModel;
using System.Runtime.InteropServices;
using TarkovAssistant.Client.Features.CursorTracking;

namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Applies the documented Win32 styles required by a non-activating click-through overlay.
/// </summary>
internal static partial class NativeOverlayWindowMethods
{
    private const int ExtendedStyleIndex = -20;
    private const long NoActivateStyle = 0x08000000L;
    private const long ToolWindowStyle = 0x00000080L;
    private const long TransparentStyle = 0x00000020L;
    private const int VirtualScreenLeftMetric = 76;
    private const int VirtualScreenTopMetric = 77;
    private const int VirtualScreenWidthMetric = 78;
    private const int VirtualScreenHeightMetric = 79;
    private const uint DoNotResizeFlag = 0x0001;
    private const uint DoNotActivateFlag = 0x0010;
    private const uint ShowWindowFlag = 0x0040;
    private const int CursorOffset = 18;
    private static readonly nint TopMostWindow = new(-1);

    internal static void ConfigureClickThrough(nint windowHandle)
    {
        var currentStyle = GetExtendedStyle(windowHandle);
        var requestedStyle = currentStyle | NoActivateStyle | ToolWindowStyle | TransparentStyle;
        SetExtendedStyle(windowHandle, requestedStyle);
    }

    internal static void PositionNearCursor(nint windowHandle, ScreenPoint cursorPosition)
    {
        if (!GetWindowRectangle(windowHandle, out var rectangle))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The overlay bounds could not be read.");
        }

        var width = rectangle.Right - rectangle.Left;
        var height = rectangle.Bottom - rectangle.Top;
        var virtualLeft = GetSystemMetrics(VirtualScreenLeftMetric);
        var virtualTop = GetSystemMetrics(VirtualScreenTopMetric);
        var virtualRight = virtualLeft + GetSystemMetrics(VirtualScreenWidthMetric);
        var virtualBottom = virtualTop + GetSystemMetrics(VirtualScreenHeightMetric);
        var left = Math.Clamp(cursorPosition.X + CursorOffset, virtualLeft, Math.Max(virtualLeft, virtualRight - width));
        var top = Math.Clamp(cursorPosition.Y + CursorOffset, virtualTop, Math.Max(virtualTop, virtualBottom - height));

        if (!SetWindowPosition(
                windowHandle,
                TopMostWindow,
                left,
                top,
                0,
                0,
                DoNotResizeFlag | DoNotActivateFlag | ShowWindowFlag))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The overlay could not be positioned safely.");
        }
    }

    private static long GetExtendedStyle(nint windowHandle)
    {
        Marshal.SetLastPInvokeError(0);
        var style = Environment.Is64BitProcess
            ? GetWindowLongPointer64(windowHandle, ExtendedStyleIndex)
            : GetWindowLong32(windowHandle, ExtendedStyleIndex);

        if (style == 0 && Marshal.GetLastPInvokeError() != 0)
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The overlay window style could not be read.");
        }

        return style;
    }

    private static void SetExtendedStyle(nint windowHandle, long style)
    {
        Marshal.SetLastPInvokeError(0);
        var previousStyle = Environment.Is64BitProcess
            ? SetWindowLongPointer64(windowHandle, ExtendedStyleIndex, style)
            : SetWindowLong32(windowHandle, ExtendedStyleIndex, checked((int)style));

        if (previousStyle == 0 && Marshal.GetLastPInvokeError() != 0)
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The click-through overlay style could not be applied.");
        }
    }

    [LibraryImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
    private static partial nint GetWindowLongPointer64(nint windowHandle, int index);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowLongW", SetLastError = true)]
    private static partial int GetWindowLong32(nint windowHandle, int index);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static partial nint SetWindowLongPointer64(nint windowHandle, int index, long newStyle);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true)]
    private static partial int SetWindowLong32(nint windowHandle, int index, int newStyle);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowRectangle(nint windowHandle, out NativeRectangle rectangle);

    [LibraryImport("user32.dll", EntryPoint = "GetSystemMetrics")]
    private static partial int GetSystemMetrics(int index);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetWindowPosition(
        nint windowHandle,
        nint insertAfter,
        int x,
        int y,
        int width,
        int height,
        uint flags);
}
