// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal class WebGPUAdapter : GraphicsAdapter
{
    public unsafe WebGPUAdapter(WGPUAdapter handle)
    {
        Handle = handle;

        ReadOnlySpan<WGPUFeatureName> features = wgpuAdapterEnumerateFeatures(handle);

        wgpuAdapterGetInfo(handle, out WGPUAdapterInfo adapterInfo);
        wgpuAdapterInfoFreeMembers(adapterInfo);

        WGPUSupportedLimits supportedLimits;
        wgpuAdapterGetLimits(handle, &supportedLimits);

        AdapterLimits = supportedLimits;
        Features = features.ToArray();
    }

    public WGPUAdapter Handle { get; }
    public WGPUSupportedLimits AdapterLimits { get; }
    public WGPUFeatureName[] Features { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUAdapter" /> class.
    /// </summary>
    ~WebGPUAdapter() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            wgpuAdapterRelease(Handle);
        }
    }

    /// <inheritdoc />
    public override PixelFormat GetSurfacePreferredFormat(GraphicsSurface surface)
    {
        WebGPUSurface backendSurface = (WebGPUSurface) surface;
        WGPUTextureFormat format = WGPUTextureFormat.BGRA8Unorm; // wgpuSurfaceGetPreferredFormat(backendSurface.Handle, Handle);
        return format.ToLeoncino();
    }

    /// <inheritdoc />
    protected unsafe override GraphicsDevice CreateDeviceCore(in GraphicsDeviceDescriptor descriptor)
    {
        return new WebGPUDevice(this, in descriptor);
    }
}
