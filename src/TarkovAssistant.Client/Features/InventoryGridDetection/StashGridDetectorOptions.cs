namespace TarkovAssistant.Client.Features.InventoryGridDetection;

/// <summary>
/// Configures conservative local detection of repeated stash-cell geometry.
/// </summary>
internal sealed class StashGridDetectorOptions
{
    /// <summary>Gets the smallest supported physical-pixel cell pitch.</summary>
    public int MinimumCellSize { get; init; } = 24;

    /// <summary>Gets the largest supported physical-pixel cell pitch.</summary>
    public int MaximumCellSize { get; init; } = 160;

    /// <summary>Gets the required repeated grid-line count on each axis.</summary>
    public int MinimumGridLines { get; init; } = 4;

    /// <summary>Gets the line-to-background contrast ratio required to accept grid geometry.</summary>
    public double MinimumLineContrast { get; init; } = 1.65;

    /// <summary>Gets the minimum luminance variance that marks a cell as item-like rather than empty.</summary>
    public double MinimumItemLuminanceVariance { get; init; } = 260;
}
