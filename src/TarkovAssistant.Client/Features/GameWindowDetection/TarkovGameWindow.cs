namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Identifies a top-level Tarkov game window and its current capture-relevant state.
/// </summary>
/// <param name="Handle">The native top-level window handle.</param>
/// <param name="ProcessId">The owning game process identifier.</param>
/// <param name="Title">The current native window title.</param>
/// <param name="Bounds">The physical-pixel outer window bounds in virtual desktop coordinates.</param>
/// <param name="IsForeground">Whether the window owns the foreground input focus.</param>
/// <param name="IsMinimized">Whether Windows reports the window as minimized.</param>
internal readonly record struct TarkovGameWindow(
    nint Handle,
    int ProcessId,
    string Title,
    PhysicalScreenRectangle Bounds,
    bool IsForeground,
    bool IsMinimized);
