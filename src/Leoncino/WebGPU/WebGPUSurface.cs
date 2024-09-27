// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe partial class WebGPUSurface : GraphicsSurface
{
    public WebGPUSurface(WebGPUInstance instance, in SurfaceDescriptor descriptor)
        : base(descriptor)
    {
        WGPUChainedStruct* chainStruct = default;
        switch (descriptor.Source)
        {
            case AndroidWindowSurfaceSource androidWindowSurface:
                WGPUSurfaceDescriptorFromAndroidNativeWindow androidWindowChain = new()
                {
                    window = androidWindowSurface.Window.ToPointer(),
                    chain = new WGPUChainedStruct()
                    {
                        sType = WGPUSType.SurfaceDescriptorFromAndroidNativeWindow
                    }
                };

                chainStruct = (WGPUChainedStruct*)&androidWindowChain;
                break;


            case MetalLayerSurfaceSource metalLayerSurfaceSource:
                WGPUSurfaceDescriptorFromMetalLayer metalLayerChain = new()
                {
                    layer = metalLayerSurfaceSource.Layer.ToPointer(),
                    chain = new WGPUChainedStruct()
                    {
                        sType = WGPUSType.SurfaceDescriptorFromMetalLayer
                    }
                };

                chainStruct = (WGPUChainedStruct*)&metalLayerChain;
                break;

            case Win32SurfaceSource win32SurfaceSource:
                WGPUSurfaceDescriptorFromWindowsHWND win32Chain = new()
                {
                    hwnd = win32SurfaceSource.Hwnd.ToPointer(),
                    hinstance = GetModuleHandleW(null),
                    chain = new WGPUChainedStruct()
                    {
                        sType = WGPUSType.SurfaceDescriptorFromWindowsHWND
                    }
                };

                chainStruct = (WGPUChainedStruct*)&win32Chain;
                break;

            case WaylandSurfaceSource waylandSurfaceSource:
                WGPUSurfaceDescriptorFromWaylandSurface waylandChain = new()
                {
                    display = waylandSurfaceSource.Display.ToPointer(),
                    surface = waylandSurfaceSource.Surface.ToPointer(),
                    chain = new WGPUChainedStruct()
                    {
                        sType = WGPUSType.SurfaceDescriptorFromWaylandSurface
                    }
                };

                chainStruct = (WGPUChainedStruct*)&waylandChain;
                break;

            case XcbWindowSurfaceSource xcbWindowSurfaceSource:
                WGPUSurfaceDescriptorFromXcbWindow xcbChain = new()
                {
                    connection = xcbWindowSurfaceSource.Connection.ToPointer(),
                    window = xcbWindowSurfaceSource.Window,
                    chain = new WGPUChainedStruct()
                    {
                        sType = WGPUSType.SurfaceDescriptorFromXcbWindow
                    }
                };

                chainStruct = (WGPUChainedStruct*)&xcbChain;
                break;

            case XlibWindowSurfaceSource xlibWindowSurfaceSource:
                WGPUSurfaceDescriptorFromXlibWindow xlibChain = new()
                {
                    display = xlibWindowSurfaceSource.Display.ToPointer(),
                    window = xlibWindowSurfaceSource.Window,
                    chain = new WGPUChainedStruct()
                    {
                        sType = WGPUSType.SurfaceDescriptorFromXlibWindow
                    }
                };

                chainStruct = (WGPUChainedStruct*)&xlibChain;
                break;
        }

        WGPUSurfaceDescriptor surfaceDescriptor = new()
        {
            nextInChain = chainStruct
        };
        Handle = wgpuInstanceCreateSurface(instance.Handle, &surfaceDescriptor);
    }

    public WGPUSurface Handle { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUSurface" /> class.
    /// </summary>
    ~WebGPUSurface() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (IsConfigured)
                wgpuSurfaceUnconfigure(Handle);
            wgpuSurfaceRelease(Handle);
        }
    }

    /// <inheritdoc />
    protected override void ConfigureCore(GraphicsDevice device, in SurfaceConfiguration configuration)
    {
        WebGPUDevice backendDevice = (WebGPUDevice)device;

        PixelFormat format = configuration.Format == PixelFormat.Undefined ? backendDevice.Adapter.GetSurfacePreferredFormat(this) : configuration.Format;
        WGPUTextureFormat viewFormat = format.ToWGPU();

        WGPUSurfaceConfiguration surfaceConfiguration = new()
        {
            nextInChain = null,
            device = backendDevice.Handle,
            format = viewFormat,
            usage = WGPUTextureUsage.RenderAttachment,
            viewFormatCount = 1,
            viewFormats = &viewFormat,
            alphaMode = WGPUCompositeAlphaMode.Auto,
            width = (uint)configuration.Width,
            height = (uint)configuration.Height,
            presentMode = configuration.PresentMode.ToWGPU(),
        };
        wgpuSurfaceConfigure(Handle, &surfaceConfiguration);
    }

    [LibraryImport("kernel32")]
    private static partial void* GetModuleHandleW(ushort* lpModuleName);
}
