namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Contains the outcome of one hover processing operation and an optional user-safe failure detail.
/// </summary>
/// <param name="State">The processing outcome.</param>
/// <param name="Detail">An optional user-safe recovery hint.</param>
internal readonly record struct HoverProcessingResult(HoverProcessingState State, string? Detail);
