using System.Runtime.InteropServices;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;

namespace TarkovAssistant.Client.Features.ScreenCapture;

/// <summary>
/// Bridges the documented Win32 and Windows Runtime capture interop used by the desktop client.
/// </summary>
internal static partial class WindowsGraphicsCaptureInterop
{
    private const uint D3D11CreateDeviceBgraSupport = 0x20;
    private const uint D3DDriverTypeHardware = 1;
    private const uint D3D11SdkVersion = 7;
    private static readonly Guid GraphicsCaptureItemRuntimeClassId = typeof(GraphicsCaptureItem).GUID;
    private static readonly Guid DxgiDeviceInterfaceId = new("54EC77FA-1377-44E6-8C32-88FD5F44C84C");

    /// <summary>
    /// Creates a capture item for one HWND rather than the full desktop.
    /// </summary>
    /// <param name="windowHandle">The top-level capture target HWND.</param>
    /// <returns>The Windows Runtime item for the specified window.</returns>
    public static GraphicsCaptureItem CreateItemForWindow(nint windowHandle)
    {
        var activationFactory = nint.Zero;
        var itemPointer = nint.Zero;
        var classNameHandle = nint.Zero;

        try
        {
            Marshal.ThrowExceptionForHR(
                WindowsCreateString(
                    "Windows.Graphics.Capture.GraphicsCaptureItem",
                    checked((uint)"Windows.Graphics.Capture.GraphicsCaptureItem".Length),
                    out classNameHandle));
            Marshal.ThrowExceptionForHR(
                RoGetActivationFactory(classNameHandle, GraphicsCaptureItemInteropInterfaceId, out activationFactory));
            var factory = (IGraphicsCaptureItemInterop)Marshal.GetObjectForIUnknown(activationFactory);
            Marshal.ThrowExceptionForHR(factory.CreateForWindow(windowHandle, GraphicsCaptureItemRuntimeClassId, out itemPointer));
            return (GraphicsCaptureItem)Marshal.GetObjectForIUnknown(itemPointer);
        }
        finally
        {
            if (itemPointer != nint.Zero)
            {
                Marshal.Release(itemPointer);
            }

            if (activationFactory != nint.Zero)
            {
                Marshal.Release(activationFactory);
            }

            if (classNameHandle != nint.Zero)
            {
                Marshal.ThrowExceptionForHR(WindowsDeleteString(classNameHandle));
            }
        }
    }

    /// <summary>
    /// Creates the Direct3D device required by <see cref="Direct3D11CaptureFramePool"/>.
    /// </summary>
    /// <returns>A Windows Runtime Direct3D device backed by a hardware D3D11 device.</returns>
    public static IDirect3DDevice CreateDirect3DDevice()
    {
        Marshal.ThrowExceptionForHR(
            D3D11CreateDevice(
                nint.Zero,
                D3DDriverTypeHardware,
                nint.Zero,
                D3D11CreateDeviceBgraSupport,
                nint.Zero,
                0,
                D3D11SdkVersion,
                out var d3dDevice,
                out _,
                out var immediateContext));

        var dxgiDevice = nint.Zero;
        var direct3DDevice = nint.Zero;
        try
        {
            var dxgiDeviceInterfaceId = new Guid("54EC77FA-1377-44E6-8C32-88FD5F44C84C");
            Marshal.ThrowExceptionForHR(Marshal.QueryInterface(d3dDevice, in dxgiDeviceInterfaceId, out dxgiDevice));
            Marshal.ThrowExceptionForHR(CreateDirect3D11DeviceFromDXGIDevice(dxgiDevice, out direct3DDevice));
            return (IDirect3DDevice)Marshal.GetObjectForIUnknown(direct3DDevice);
        }
        finally
        {
            if (direct3DDevice != nint.Zero)
            {
                Marshal.Release(direct3DDevice);
            }

            if (dxgiDevice != nint.Zero)
            {
                Marshal.Release(dxgiDevice);
            }

            if (immediateContext != nint.Zero)
            {
                Marshal.Release(immediateContext);
            }

            if (d3dDevice != nint.Zero)
            {
                Marshal.Release(d3dDevice);
            }
        }
    }

    private static Guid GraphicsCaptureItemInteropInterfaceId { get; } = new("3628E81B-3CAC-4C60-B7F4-23CE0E0C3356");

    [LibraryImport("combase.dll", EntryPoint = "WindowsCreateString", StringMarshalling = StringMarshalling.Utf16)]
    private static partial int WindowsCreateString(string sourceString, uint length, out nint stringHandle);

    [LibraryImport("combase.dll", EntryPoint = "WindowsDeleteString")]
    private static partial int WindowsDeleteString(nint stringHandle);

    [LibraryImport("combase.dll", EntryPoint = "RoGetActivationFactory")]
    private static partial int RoGetActivationFactory(nint activatableClassId, in Guid interfaceId, out nint factory);

    [LibraryImport("d3d11.dll", EntryPoint = "D3D11CreateDevice")]
    private static partial int D3D11CreateDevice(
        nint adapter,
        uint driverType,
        nint software,
        uint flags,
        nint featureLevels,
        uint featureLevelsCount,
        uint sdkVersion,
        out nint device,
        out uint featureLevel,
        out nint immediateContext);

    [LibraryImport("d3d11.dll", EntryPoint = "CreateDirect3D11DeviceFromDXGIDevice")]
    private static partial int CreateDirect3D11DeviceFromDXGIDevice(nint dxgiDevice, out nint graphicsDevice);

    // Windows Runtime returns an IInspectable RCW; source-generated COM cannot cast that runtime type.
#pragma warning disable SYSLIB1096
    [ComImport]
    [Guid("3628E81B-3CAC-4C60-B7F4-23CE0E0C3356")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IGraphicsCaptureItemInterop
    {
        public int CreateForWindow(nint windowHandle, in Guid interfaceId, out nint result);
    }
#pragma warning restore SYSLIB1096
}
