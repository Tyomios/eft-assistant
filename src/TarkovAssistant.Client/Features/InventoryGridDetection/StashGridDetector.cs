using TarkovAssistant.Client.Features.CursorTracking;
using TarkovAssistant.Client.Features.GameWindowDetection;
using TarkovAssistant.Client.Features.ScreenCapture;

namespace TarkovAssistant.Client.Features.InventoryGridDetection;

/// <summary>
/// Finds stash-cell geometry from repeated local BGRA32 edge lines without relying on a fixed resolution or UI scale.
/// </summary>
internal sealed class StashGridDetector : IStashGridDetector
{
    private readonly StashGridDetectorOptions _options;

    /// <summary>
    /// Initializes conservative stash-grid detection settings.
    /// </summary>
    /// <param name="options">The supported pitch, line repetition, and empty-cell thresholds.</param>
    /// <exception cref="ArgumentOutOfRangeException">A geometry or confidence threshold is invalid.</exception>
    public StashGridDetector(StashGridDetectorOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (options.MinimumCellSize < 2
            || options.MaximumCellSize < options.MinimumCellSize
            || options.MinimumGridLines < 3
            || options.MinimumLineContrast <= 1
            || options.MinimumItemLuminanceVariance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "Stash-grid detection settings are invalid.");
        }

        _options = options;
    }

    /// <inheritdoc />
    public StashGridDetectionResult Detect(CapturedInventoryRegion region, ScreenPoint cursorPosition)
    {
        ArgumentNullException.ThrowIfNull(region);
        if (!region.Bounds.Contains(cursorPosition))
        {
            return new StashGridDetectionResult(StashGridDetectionState.OutsideStash, null, 0, "The cursor is outside the captured region.");
        }

        if (region.Width < _options.MinimumCellSize * 2 || region.Height < _options.MinimumCellSize * 2)
        {
            return new StashGridDetectionResult(StashGridDetectionState.GridNotDetected, null, 0, "The capture is too small to validate stash geometry.");
        }

        var cursorX = cursorPosition.X - region.Bounds.Left;
        var cursorY = cursorPosition.Y - region.Bounds.Top;
        var (horizontalCellSize, horizontalLeftBoundary, horizontalContrast) = FindAxis(
            CreateHorizontalEdgeProfile(region),
            cursorX);
        var (verticalCellSize, verticalLeftBoundary, verticalContrast) = FindAxis(
            CreateVerticalEdgeProfile(region),
            cursorY);
        if (horizontalCellSize == 0 || verticalCellSize == 0)
        {
            return new StashGridDetectionResult(StashGridDetectionState.OutsideStash, null, 0, "The cursor region does not contain a repeated stash grid.");
        }

        var confidence = Math.Min(1, Math.Min(horizontalContrast, verticalContrast) / (_options.MinimumLineContrast * 1.5));
        var cellBounds = new PhysicalScreenRectangle(
            region.Bounds.Left + horizontalLeftBoundary + 1,
            region.Bounds.Top + verticalLeftBoundary + 1,
            horizontalCellSize - 1,
            verticalCellSize - 1);
        var cell = new StashGridCell(cellBounds, horizontalCellSize, verticalCellSize);
        return HasItemDetail(region, cellBounds)
            ? new StashGridDetectionResult(StashGridDetectionState.ItemCellDetected, cell, confidence, null)
            : new StashGridDetectionResult(StashGridDetectionState.EmptyCell, cell, confidence, "The stash cell has no item-like pixel detail.");
    }

    private (int CellSize, int LeftBoundary, double Contrast) FindAxis(double[] profile, int cursorCoordinate)
    {
        var baseline = profile.Average();
        if (baseline <= 0)
        {
            return default;
        }

        var maximumCellSize = Math.Min(_options.MaximumCellSize, (profile.Length - 1) / (_options.MinimumGridLines - 1));
        var bestCellSize = 0;
        var bestBoundary = 0;
        var bestContrast = 0d;
        for (var cellSize = _options.MinimumCellSize; cellSize <= maximumCellSize; cellSize++)
        {
            for (var phase = 0; phase < cellSize; phase++)
            {
                var sum = 0d;
                var count = 0;
                for (var position = phase; position < profile.Length; position += cellSize)
                {
                    sum += NeighborAverage(profile, position);
                    count++;
                }

                if (count < _options.MinimumGridLines)
                {
                    continue;
                }

                var contrast = (sum / count) / baseline;
                if (contrast > bestContrast)
                {
                    bestContrast = contrast;
                    bestCellSize = cellSize;
                    bestBoundary = phase + (int)Math.Floor((double)(cursorCoordinate - phase) / cellSize) * cellSize;
                }
            }
        }

        return bestContrast >= _options.MinimumLineContrast
            && bestBoundary >= 0
            && bestBoundary + bestCellSize < profile.Length
            ? (bestCellSize, bestBoundary, bestContrast)
            : default;
    }

    private static double[] CreateHorizontalEdgeProfile(CapturedInventoryRegion region)
    {
        var profile = new double[region.Width];
        var pixels = region.Pixels.Span;
        for (var x = 1; x < region.Width; x++)
        {
            var total = 0d;
            for (var y = 0; y < region.Height; y++)
            {
                var offset = (y * region.Stride) + (x * 4);
                total += Math.Abs(Luminance(pixels, offset) - Luminance(pixels, offset - 4));
            }

            profile[x] = total / region.Height;
        }

        return profile;
    }

    private static double[] CreateVerticalEdgeProfile(CapturedInventoryRegion region)
    {
        var profile = new double[region.Height];
        var pixels = region.Pixels.Span;
        for (var y = 1; y < region.Height; y++)
        {
            var total = 0d;
            var offset = y * region.Stride;
            for (var x = 0; x < region.Width; x++)
            {
                total += Math.Abs(Luminance(pixels, offset + (x * 4)) - Luminance(pixels, offset - region.Stride + (x * 4)));
            }

            profile[y] = total / region.Width;
        }

        return profile;
    }

    private bool HasItemDetail(CapturedInventoryRegion region, PhysicalScreenRectangle cellBounds)
    {
        var localLeft = cellBounds.Left - region.Bounds.Left;
        var localTop = cellBounds.Top - region.Bounds.Top;
        var insetX = Math.Max(2, cellBounds.Width / 8);
        var insetY = Math.Max(2, cellBounds.Height / 8);
        var left = localLeft + insetX;
        var top = localTop + insetY;
        var right = localLeft + cellBounds.Width - insetX;
        var bottom = localTop + cellBounds.Height - insetY;
        var pixels = region.Pixels.Span;
        var count = 0;
        var sum = 0d;
        var sumSquares = 0d;
        for (var y = top; y < bottom; y++)
        {
            for (var x = left; x < right; x++)
            {
                var luma = Luminance(pixels, (y * region.Stride) + (x * 4));
                sum += luma;
                sumSquares += luma * luma;
                count++;
            }
        }

        if (count == 0)
        {
            return false;
        }

        var mean = sum / count;
        return (sumSquares / count) - (mean * mean) >= _options.MinimumItemLuminanceVariance;
    }

    private static double NeighborAverage(double[] values, int index)
    {
        var start = Math.Max(0, index - 1);
        var end = Math.Min(values.Length - 1, index + 1);
        var sum = 0d;
        for (var position = start; position <= end; position++)
        {
            sum += values[position];
        }

        return sum / (end - start + 1);
    }

    private static double Luminance(ReadOnlySpan<byte> pixels, int offset)
    {
        return (pixels[offset] * 0.0722) + (pixels[offset + 1] * 0.7152) + (pixels[offset + 2] * 0.2126);
    }
}
