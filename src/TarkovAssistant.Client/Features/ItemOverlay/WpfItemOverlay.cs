using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
using TarkovAssistant.Client.Features.CursorTracking;

namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Marshals safe overlay operations to the owning WPF dispatcher.
/// </summary>
internal sealed class WpfItemOverlay : IItemOverlay
{
    private readonly Dispatcher _dispatcher;
    private ItemOverlayWindow? _window;

    /// <summary>
    /// Initializes an overlay bound to the application UI dispatcher.
    /// </summary>
    /// <param name="dispatcher">The dispatcher that owns all overlay windows.</param>
    public WpfItemOverlay(Dispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;
    }

    /// <inheritdoc />
    public async Task<OverlayOperationResult> ShowAsync(
        ItemOverlayContent content,
        ScreenPoint cursorPosition,
        CancellationToken cancellationToken)
    {
        try
        {
            await _dispatcher.InvokeAsync(
                () =>
                {
                    _window ??= new ItemOverlayWindow();
                    _window.ShowContent(content, cursorPosition);
                },
                DispatcherPriority.Normal,
                cancellationToken);

            return Completed();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (IsExpectedOverlayFailure(exception))
        {
            await CloseFailedWindowAsync().ConfigureAwait(false);
            return Failed("The item overlay could not be displayed safely.");
        }
    }

    /// <inheritdoc />
    public async Task<OverlayOperationResult> HideAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dispatcher.InvokeAsync(
                () => _window?.Hide(),
                DispatcherPriority.Normal,
                cancellationToken);
            return Completed();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            return Failed("The item overlay could not be hidden safely.");
        }
    }

    private static OverlayOperationResult Completed()
    {
        return new OverlayOperationResult(OverlayOperationState.Completed, null);
    }

    private static OverlayOperationResult Failed(string detail)
    {
        return new OverlayOperationResult(OverlayOperationState.Failed, detail);
    }

    private static bool IsExpectedOverlayFailure(Exception exception)
    {
        return exception is Win32Exception
            or InvalidOperationException
            or IOException
            or NotSupportedException
            or FormatException
            or ArgumentException;
    }

    private async Task CloseFailedWindowAsync()
    {
        try
        {
            await _dispatcher.InvokeAsync(
                CloseFailedWindowOnDispatcher,
                DispatcherPriority.Send);
        }
        catch (InvalidOperationException)
        {
            _window = null;
        }
    }

    private void CloseFailedWindowOnDispatcher()
    {
        if (_window is null)
        {
            return;
        }

        try
        {
            _window.Close();
        }
        catch (InvalidOperationException)
        {
        }
        finally
        {
            _window = null;
        }
    }
}
