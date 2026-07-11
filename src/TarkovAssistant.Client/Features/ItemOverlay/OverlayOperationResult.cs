namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Contains the outcome of an overlay operation and an optional user-safe failure detail.
/// </summary>
/// <param name="State">The operation outcome.</param>
/// <param name="Detail">An optional user-safe failure detail.</param>
internal readonly record struct OverlayOperationResult(OverlayOperationState State, string? Detail);
