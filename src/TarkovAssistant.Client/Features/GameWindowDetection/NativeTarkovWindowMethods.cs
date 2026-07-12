using System.ComponentModel;
using System.Runtime.InteropServices;
using TarkovAssistant.Client.Features.CursorTracking;

namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Provides the documented Win32 windowing calls required to locate a separate game process window.
/// </summary>
internal static partial class NativeTarkovWindowMethods
{
    internal static IReadOnlyList<nint> EnumerateTopLevelWindows()
    {
        var windows = new List<nint>();
        var callback = new EnumWindowsCallback(
            (windowHandle, _) =>
            {
                windows.Add(windowHandle);
                return true;
            });

        if (!EnumWindows(callback, nint.Zero))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "Top-level windows could not be enumerated.");
        }

        return windows;
    }

    internal static int GetProcessId(nint windowHandle)
    {
        GetWindowThreadProcessId(windowHandle, out var processId);
        return checked((int)processId);
    }

    internal static bool IsVisible(nint windowHandle)
    {
        return IsWindowVisible(windowHandle);
    }

    internal static bool IsMinimized(nint windowHandle)
    {
        return IsIconic(windowHandle);
    }

    internal static unsafe string GetTitle(nint windowHandle)
    {
        var length = GetWindowTextLength(windowHandle);
        if (length == 0)
        {
            return string.Empty;
        }

        var maximumCount = checked(length + 1);
        var text = stackalloc char[maximumCount];
        var copiedCharacters = GetWindowText(windowHandle, text, maximumCount);
        return copiedCharacters > 0 ? new string(text, 0, copiedCharacters) : string.Empty;
    }

    internal static PhysicalScreenRectangle GetBounds(nint windowHandle)
    {
        if (!GetWindowRectangle(windowHandle, out var rectangle))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The game window bounds could not be read.");
        }

        return new PhysicalScreenRectangle(
            rectangle.Left,
            rectangle.Top,
            rectangle.Right - rectangle.Left,
            rectangle.Bottom - rectangle.Top);
    }

    internal static PhysicalScreenRectangle GetClientBounds(nint windowHandle)
    {
        if (!GetClientRect(windowHandle, out var rectangle))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The game client bounds could not be read.");
        }

        var origin = new NativeCursorPoint { X = rectangle.Left, Y = rectangle.Top };
        if (!ClientToScreen(windowHandle, ref origin))
        {
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "The game client origin could not be converted to screen coordinates.");
        }

        return new PhysicalScreenRectangle(
            origin.X,
            origin.Y,
            rectangle.Right - rectangle.Left,
            rectangle.Bottom - rectangle.Top);
    }

    internal static bool IsWindow(nint windowHandle)
    {
        return IsWindowNative(windowHandle);
    }

    internal static nint GetForegroundWindow()
    {
        return GetForegroundWindowNative();
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate bool EnumWindowsCallback(nint windowHandle, nint lParam);

    [LibraryImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EnumWindows(EnumWindowsCallback callback, nint lParam);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
    private static partial uint GetWindowThreadProcessId(nint windowHandle, out uint processId);

    [LibraryImport("user32.dll", EntryPoint = "IsWindowVisible")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindowVisible(nint windowHandle);

    [LibraryImport("user32.dll", EntryPoint = "IsIconic")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsIconic(nint windowHandle);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowTextLengthW")]
    private static partial int GetWindowTextLength(nint windowHandle);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowTextW")]
    private static unsafe partial int GetWindowText(nint windowHandle, char* text, int maximumCount);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowRectangle(nint windowHandle, out NativeWindowRectangle rectangle);

    [LibraryImport("user32.dll", EntryPoint = "GetClientRect", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetClientRect(nint windowHandle, out NativeWindowRectangle rectangle);

    [LibraryImport("user32.dll", EntryPoint = "ClientToScreen", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ClientToScreen(nint windowHandle, ref NativeCursorPoint point);

    [LibraryImport("user32.dll", EntryPoint = "IsWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindowNative(nint windowHandle);

    [LibraryImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    private static partial nint GetForegroundWindowNative();
}
