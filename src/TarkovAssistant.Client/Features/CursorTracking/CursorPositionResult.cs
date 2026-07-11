namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Contains the current physical cursor position or a user-safe read failure.
/// </summary>
/// <param name="State">The position read outcome.</param>
/// <param name="Position">The current physical virtual-screen cursor position when available.</param>
/// <param name="Detail">An optional user-safe failure detail.</param>
internal readonly record struct CursorPositionResult(
    CursorPositionReadState State,
    ScreenPoint? Position,
    string? Detail);
