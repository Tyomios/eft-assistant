namespace TarkovAssistant.Client.Features.InventoryGridDetection;

/// <summary>
/// Contains the local stash-grid classification for a captured cursor region.
/// </summary>
/// <param name="State">The classified grid state.</param>
/// <param name="Cell">The resolved cell when grid geometry was found.</param>
/// <param name="Confidence">The normalized repeated-line confidence from zero through one.</param>
/// <param name="Detail">An optional safe explanation for an inconclusive result.</param>
internal readonly record struct StashGridDetectionResult(
    StashGridDetectionState State,
    StashGridCell? Cell,
    double Confidence,
    string? Detail);
