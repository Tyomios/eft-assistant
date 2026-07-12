using System.Runtime.InteropServices;
using TarkovAssistant.Client.Features.CursorTracking;
using TarkovAssistant.Client.Features.GameWindowDetection;
using Windows.Foundation;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;

namespace TarkovAssistant.Client.Features.ScreenCapture;

/// <summary>
/// Captures a small BGRA32 region from a Tarkov HWND by using Windows.Graphics.Capture and D3D11 interop.
/// </summary>
internal sealed partial class WindowsGraphicsCaptureRegionCapturer : ISmallRegionScreenCapturer
{
    private IDirect3DDevice? _device;
    private readonly int _height;
    private readonly int _width;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a physical-pixel capturer for the specified output size.
    /// </summary>
    /// <param name="options">The bounded output dimensions copied from each capture frame.</param>
    /// <exception cref="ArgumentOutOfRangeException">A requested dimension is not positive.</exception>
    public WindowsGraphicsCaptureRegionCapturer(SmallRegionCaptureOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.Width <= 0 || options.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "Capture dimensions must be positive.");
        }

        _width = options.Width;
        _height = options.Height;
    }

    /// <inheritdoc />
    public async ValueTask<ScreenCaptureResult> CaptureAsync(
        TarkovGameWindow gameWindow,
        ScreenPoint cursorPosition,
        CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (!GraphicsCaptureSession.IsSupported())
        {
            return new ScreenCaptureResult(ScreenCaptureState.Unsupported, null, "Windows.Graphics.Capture is not supported on this system.");
        }

        if (!IsCaptureEligible(gameWindow, cursorPosition))
        {
            return new ScreenCaptureResult(ScreenCaptureState.WindowUnavailable, null, "The Tarkov client area is no longer available for capture.");
        }

        try
        {
            var device = _device ??= WindowsGraphicsCaptureInterop.CreateDirect3DDevice();
            var captureItem = WindowsGraphicsCaptureInterop.CreateItemForWindow(gameWindow.Handle);
            using var framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(
                device,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                1,
                captureItem.Size);
            using var session = framePool.CreateCaptureSession(captureItem);
            using var frame = await WaitForFrameAsync(framePool, session, cancellationToken).ConfigureAwait(false);

            if (frame is null)
            {
                return new ScreenCaptureResult(ScreenCaptureState.FrameUnavailable, null, "The Tarkov window did not provide a capture frame.");
            }

            if (!IsCaptureEligible(gameWindow, cursorPosition))
            {
                return new ScreenCaptureResult(ScreenCaptureState.WindowUnavailable, null, "The Tarkov client area changed before the frame could be used.");
            }

            if (frame.ContentSize.Width != gameWindow.ClientBounds.Width
                || frame.ContentSize.Height != gameWindow.ClientBounds.Height)
            {
                return new ScreenCaptureResult(ScreenCaptureState.ResolutionChanged, null, "The Tarkov client size changed; a new capture will be requested.");
            }

            return new ScreenCaptureResult(
                ScreenCaptureState.Captured,
                await CopyCursorRegionAsync(frame, gameWindow.ClientBounds, cursorPosition, cancellationToken).ConfigureAwait(false),
                null);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (COMException)
        {
            return new ScreenCaptureResult(ScreenCaptureState.Failed, null, "Windows rejected the capture request for the current game window.");
        }
        catch (InvalidCastException)
        {
            return new ScreenCaptureResult(ScreenCaptureState.Failed, null, "Windows could not initialize capture for the current game window.");
        }
        catch (UnauthorizedAccessException)
        {
            return new ScreenCaptureResult(ScreenCaptureState.Failed, null, "Windows denied access to the game capture surface.");
        }
        catch (InvalidOperationException)
        {
            return new ScreenCaptureResult(ScreenCaptureState.Failed, null, "The game capture surface became unavailable.");
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _isDisposed = true;
    }

    private async ValueTask<CapturedInventoryRegion> CopyCursorRegionAsync(
        Direct3D11CaptureFrame frame,
        PhysicalScreenRectangle clientBounds,
        ScreenPoint cursorPosition,
        CancellationToken cancellationToken)
    {
        using var source = await SoftwareBitmap.CreateCopyFromSurfaceAsync(frame.Surface).AsTask(cancellationToken).ConfigureAwait(false);
        if (source.BitmapPixelFormat == BitmapPixelFormat.Bgra8)
        {
            return CopyBitmapRegion(source, clientBounds, cursorPosition, cancellationToken);
        }

        using var converted = SoftwareBitmap.Convert(source, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        return CopyBitmapRegion(converted, clientBounds, cursorPosition, cancellationToken);
    }

    private CapturedInventoryRegion CopyBitmapRegion(
        SoftwareBitmap bitmap,
        PhysicalScreenRectangle clientBounds,
        ScreenPoint cursorPosition,
        CancellationToken cancellationToken)
    {
        using var buffer = bitmap.LockBuffer(BitmapBufferAccessMode.Read);
        using var reference = buffer.CreateReference();
        var byteAccess = (IMemoryBufferByteAccess)reference;
        var plane = buffer.GetPlaneDescription(0);
        unsafe
        {
            byteAccess.GetBuffer(out var sourceBytes, out var sourceCapacity);
            var bitmapWidth = bitmap.PixelWidth;
            var bitmapHeight = bitmap.PixelHeight;
            var sourceStride = plane.Stride;
            var copyWidth = Math.Min(_width, bitmapWidth);
            var copyHeight = Math.Min(_height, bitmapHeight);
            var cursorX = cursorPosition.X - clientBounds.Left;
            var cursorY = cursorPosition.Y - clientBounds.Top;
            var sourceLeft = Clamp(cursorX - (copyWidth / 2), 0, bitmapWidth - copyWidth);
            var sourceTop = Clamp(cursorY - (copyHeight / 2), 0, bitmapHeight - copyHeight);
            var destinationStride = checked(copyWidth * 4);
            var pixels = new byte[checked(destinationStride * copyHeight)];

            for (var row = 0; row < copyHeight; row++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var sourceOffset = checked(
                    plane.StartIndex
                    + ((sourceTop + row) * sourceStride)
                    + (sourceLeft * 4));
                if ((long)sourceOffset + destinationStride > sourceCapacity)
                {
                    throw new InvalidOperationException("The capture surface buffer is smaller than its declared plane.");
                }

                Marshal.Copy(
                    (nint)(sourceBytes + sourceOffset),
                    pixels,
                    row * destinationStride,
                    destinationStride);
            }

            return new CapturedInventoryRegion(
                pixels,
                new PhysicalScreenRectangle(clientBounds.Left + sourceLeft, clientBounds.Top + sourceTop, copyWidth, copyHeight),
                destinationStride);
        }
    }

    private static bool IsCaptureEligible(TarkovGameWindow gameWindow, ScreenPoint cursorPosition)
    {
        return gameWindow.Handle != nint.Zero
            && !gameWindow.IsMinimized
            && NativeTarkovWindowMethods.IsWindow(gameWindow.Handle)
            && gameWindow.ClientBounds.Contains(cursorPosition);
    }

    private static async ValueTask<Direct3D11CaptureFrame?> WaitForFrameAsync(
        Direct3D11CaptureFramePool framePool,
        GraphicsCaptureSession session,
        CancellationToken cancellationToken)
    {
        var completion = new TaskCompletionSource<Direct3D11CaptureFrame?>(TaskCreationOptions.RunContinuationsAsynchronously);
        framePool.FrameArrived += FrameArrived;

        try
        {
            session.StartCapture();
            return await completion.Task.WaitAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            framePool.FrameArrived -= FrameArrived;
        }

        void FrameArrived(Direct3D11CaptureFramePool sender, object _)
        {
            completion.TrySetResult(sender.TryGetNextFrame());
        }
    }

    private static int Clamp(int value, int minimum, int maximum)
    {
        return Math.Min(Math.Max(value, minimum), maximum);
    }

    // BitmapBuffer.CreateReference returns a Windows Runtime RCW that requires the classic COM view.
#pragma warning disable SYSLIB1096
    [ComImport]
    [Guid("5B0D3235-4DBA-4D44-865E-8F1D0E4FD04D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private unsafe interface IMemoryBufferByteAccess
    {
        public void GetBuffer(out byte* value, out uint capacity);
    }
#pragma warning restore SYSLIB1096
}
