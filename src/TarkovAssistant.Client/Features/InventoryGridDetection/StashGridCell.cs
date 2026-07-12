using TarkovAssistant.Client.Features.GameWindowDetection;

namespace TarkovAssistant.Client.Features.InventoryGridDetection;

/// <summary>
/// Describes one physical-pixel stash cell determined from local grid geometry.
/// </summary>
/// <param name="Bounds">The cell bounds in physical virtual-screen pixels.</param>
/// <param name="CellWidth">The detected horizontal cell pitch in physical pixels.</param>
/// <param name="CellHeight">The detected vertical cell pitch in physical pixels.</param>
internal readonly record struct StashGridCell(
    PhysicalScreenRectangle Bounds,
    int CellWidth,
    int CellHeight);
