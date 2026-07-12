using System.Windows;
using System.Windows.Input;
using TarkovAssistant.Client.Features.CatalogSync;
using TarkovAssistant.Client.Features.CursorTracking;
using TarkovAssistant.Client.Features.GameWindowDetection;
using TarkovAssistant.Client.Features.InventoryGridDetection;
using TarkovAssistant.Client.Features.ItemOverlay;
using TarkovAssistant.Client.Features.ScreenCapture;

namespace TarkovAssistant.Client;

/// <summary>
/// Hosts the initial desktop client shell.
/// </summary>
internal sealed partial class MainWindow : Window, IDisposable
{
    private readonly CancellationTokenSource _shutdown = new();
    private readonly CatalogSynchronizer _catalogSynchronizer;
    private readonly TarkovCursorHoverTracker _cursorHoverTracker;
    private readonly TarkovGameWindowDetector _gameWindowDetector;
    private readonly WpfItemOverlay _itemOverlay;
    private readonly StashGridDetector _stashGridDetector;
    private readonly WindowsGraphicsCaptureRegionCapturer _screenCapturer;
    private CancellationTokenSource? _cursorTrackingCancellation;
    private CancellationTokenSource? _catalogSyncCancellation;
    private bool _isDisposed;
    private CancellationTokenSource? _previewCancellation;

    /// <summary>
    /// Initializes the client shell.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        _catalogSynchronizer = new CatalogSynchronizer(new CatalogSyncOptions());
        _gameWindowDetector = new TarkovGameWindowDetector(new TarkovGameWindowDetectionOptions());
        _cursorHoverTracker = new TarkovCursorHoverTracker(
            _gameWindowDetector,
            new WindowsCursorPositionProvider(),
            new CursorTrackingOptions());
        _itemOverlay = new WpfItemOverlay(Dispatcher);
        _screenCapturer = new WindowsGraphicsCaptureRegionCapturer(new SmallRegionCaptureOptions());
        _stashGridDetector = new StashGridDetector(new StashGridDetectorOptions());
    }

    /// <summary>
    /// Cancels pending overlay previews and releases their cancellation resources.
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _shutdown.Cancel();
        StopCursorTracking();
        _catalogSyncCancellation?.Cancel();
        _catalogSyncCancellation?.Dispose();
        _catalogSyncCancellation = null;
        _catalogSynchronizer.Dispose();
        _screenCapturer.Dispose();
        _previewCancellation?.Cancel();
        _previewCancellation?.Dispose();
        _previewCancellation = null;
        _shutdown.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnClosed(EventArgs eventArgs)
    {
        Dispose();
        base.OnClosed(eventArgs);
    }

    private async void PreviewOverlayButton_Click(object sender, RoutedEventArgs eventArgs)
    {
        _previewCancellation?.Cancel();
        _previewCancellation?.Dispose();
        _previewCancellation = CancellationTokenSource.CreateLinkedTokenSource(_shutdown.Token);
        var cancellationToken = _previewCancellation.Token;
        var cursor = PointToScreen(Mouse.GetPosition(this));
        var content = new ItemOverlayContent(
            "Recognition uncertain",
            "Example item candidate",
            "A strong recommendation is hidden below the configured confidence threshold.",
            0.72,
            OverlayTone.Warning,
            ReadOnlyMemory<byte>.Empty);

        try
        {
            var result = await _itemOverlay.ShowAsync(
                content,
                new ScreenPoint(checked((int)Math.Round(cursor.X)), checked((int)Math.Round(cursor.Y))),
                cancellationToken);

            OverlayPreviewStatusText.Text = result.State == OverlayOperationState.Completed
                ? "Click-through overlay displayed safely."
                : result.Detail ?? "Overlay preview failed.";

            if (result.State == OverlayOperationState.Completed)
            {
                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
                await _itemOverlay.HideAsync(CancellationToken.None);
                OverlayPreviewStatusText.Text = "Preview completed.";
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            OverlayPreviewStatusText.Text = "Preview cancelled.";
        }
        catch (OverflowException)
        {
            OverlayPreviewStatusText.Text = "The cursor position is outside the supported coordinate range.";
        }
    }

    private void DetectTarkovWindowButton_Click(object sender, RoutedEventArgs eventArgs)
    {
        var result = _gameWindowDetector.Detect();
        TarkovWindowStatusText.Text = DescribeWindowDetection(result);
    }

    private async void SynchronizeCatalogButton_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (_catalogSyncCancellation is not null)
        {
            return;
        }

        var cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(_shutdown.Token);
        _catalogSyncCancellation = cancellationSource;
        SynchronizeCatalogButton.IsEnabled = false;
        CatalogSyncStatusText.Text = "Synchronizing catalog and images…";

        try
        {
            var result = await _catalogSynchronizer.SynchronizeAsync(cancellationSource.Token);
            CatalogSyncStatusText.Text = DescribeCatalogSynchronization(result);
        }
        catch (OperationCanceledException) when (cancellationSource.IsCancellationRequested)
        {
            CatalogSyncStatusText.Text = "Catalog synchronization cancelled.";
        }
        finally
        {
            if (ReferenceEquals(_catalogSyncCancellation, cancellationSource))
            {
                _catalogSyncCancellation = null;
                cancellationSource.Dispose();
                SynchronizeCatalogButton.IsEnabled = true;
            }
        }
    }

    private async void CursorTrackingButton_Click(object sender, RoutedEventArgs eventArgs)
    {
        if (_cursorTrackingCancellation is not null)
        {
            StopCursorTracking();
            CursorTrackingStatusText.Text = "Cursor tracking stopped.";
            return;
        }

        var cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(_shutdown.Token);
        _cursorTrackingCancellation = cancellationSource;
        CursorTrackingButton.Content = "Stop cursor tracking";
        CursorTrackingStatusText.Text = "Waiting for an active Tarkov window.";

        try
        {
            await foreach (var update in _cursorHoverTracker.TrackAsync(cancellationSource.Token))
            {
                CursorTrackingStatusText.Text = DescribeCursorTracking(update);
                if (update.ShouldTriggerRecognition
                    && update.GameWindow is { } gameWindow
                    && update.Position is { } cursorPosition)
                {
                    var capture = await _screenCapturer.CaptureAsync(gameWindow, cursorPosition, cancellationSource.Token);
                    CursorTrackingStatusText.Text = DescribeCaptureAndGrid(capture, cursorPosition);
                }
            }
        }
        catch (OperationCanceledException) when (cancellationSource.IsCancellationRequested)
        {
        }
        finally
        {
            if (ReferenceEquals(_cursorTrackingCancellation, cancellationSource))
            {
                _cursorTrackingCancellation = null;
                cancellationSource.Dispose();
                CursorTrackingButton.Content = "Start cursor tracking";
            }
        }
    }

    private static string DescribeCursorTracking(CursorTrackingUpdate update)
    {
        return update.State switch
        {
            CursorTrackingState.GameUnavailable => update.Detail ?? "The Tarkov game window is unavailable.",
            CursorTrackingState.GameInactive => update.Detail ?? "The Tarkov window is not active.",
            CursorTrackingState.CursorUnavailable => update.Detail ?? "The cursor position is unavailable.",
            CursorTrackingState.OutsideGameWindow => "The cursor is outside the Tarkov window.",
            CursorTrackingState.WaitingForDwell => $"Cursor dwell: {update.StableFor.TotalMilliseconds:F0} ms.",
            CursorTrackingState.DwellSatisfied when update.ShouldTriggerRecognition =>
                "Cursor dwell satisfied. A future capture and recognition attempt may begin.",
            CursorTrackingState.DwellSatisfied => "Cursor remains stable; recognition was already triggered for this position.",
            _ => "Cursor tracking returned an unknown state.",
        };
    }

    private void StopCursorTracking()
    {
        _cursorTrackingCancellation?.Cancel();
    }

    private static string DescribeCatalogSynchronization(CatalogSyncResult result)
    {
        return result.State switch
        {
            CatalogSyncState.Updated => $"Catalog version {result.Version} is ready for offline use.",
            CatalogSyncState.UpToDate => $"Catalog version {result.Version} is already current.",
            CatalogSyncState.CatalogUnavailable => result.Detail ?? "The backend has not published a catalog yet.",
            CatalogSyncState.Unavailable => result.Detail ?? "The local backend is unavailable.",
            _ => result.Detail ?? "Catalog synchronization failed.",
        };
    }

    private string DescribeCaptureAndGrid(ScreenCaptureResult capture, ScreenPoint cursorPosition)
    {
        if (capture.State != ScreenCaptureState.Captured || capture.Region is null)
        {
            return capture.Detail ?? "The cursor region could not be captured.";
        }

        var grid = _stashGridDetector.Detect(capture.Region, cursorPosition);
        return grid.State switch
        {
            StashGridDetectionState.ItemCellDetected when grid.Cell is { } cell =>
                $"Stash item cell detected: {cell.CellWidth}×{cell.CellHeight} physical pixels; confidence {grid.Confidence:P0}.",
            StashGridDetectionState.EmptyCell => "Stash grid detected, but the hovered cell is empty.",
            StashGridDetectionState.OutsideStash => "Cursor is not over a supported stash grid area.",
            _ => grid.Detail ?? "Stash grid geometry is inconclusive.",
        };
    }

    private static string DescribeWindowDetection(GameWindowDetectionResult result)
    {
        if (result.State == GameWindowDetectionState.NotFound)
        {
            return "Tarkov game window was not found.";
        }

        if (result.State == GameWindowDetectionState.Failed)
        {
            return result.Detail ?? "The Tarkov game window could not be inspected.";
        }

        if (result.Window is not { } window)
        {
            return "The Tarkov game window returned incomplete state.";
        }

        var title = string.IsNullOrWhiteSpace(window.Title) ? "untitled" : window.Title;
        var state = result.State == GameWindowDetectionState.Minimized ? "minimized" : "ready for capture";
        var foreground = window.IsForeground ? "foreground" : "background";
        return $"{state}: {title}; PID {window.ProcessId}; {foreground}; "
            + $"{window.Bounds.Width}×{window.Bounds.Height} at {window.Bounds.Left},{window.Bounds.Top} physical pixels.";
    }
}
