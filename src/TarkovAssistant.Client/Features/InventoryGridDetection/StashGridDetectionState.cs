namespace TarkovAssistant.Client.Features.InventoryGridDetection;

/// <summary>
/// Classifies whether the cursor is over a supported non-empty stash cell.
/// </summary>
internal enum StashGridDetectionState
{
    /// <summary>A non-empty stash cell was found beneath the cursor.</summary>
    ItemCellDetected,

    /// <summary>A stash grid cell was found but it contains no item-like pixel detail.</summary>
    EmptyCell,

    /// <summary>The cursor region does not exhibit the repeated square geometry of the stash.</summary>
    OutsideStash,

    /// <summary>The region was too small or ambiguous to safely identify a stash grid.</summary>
    GridNotDetected,
}
