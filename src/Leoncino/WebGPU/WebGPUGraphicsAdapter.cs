// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal partial class WebGPUGraphicsAdapter : GraphicsAdapter
{
    public unsafe WebGPUGraphicsAdapter(WGPUAdapter handle)
    {
        Handle = handle;

        nuint featureCount = wgpuAdapterEnumerateFeatures(handle, null);
        ReadOnlySpan<WGPUFeatureName> features = stackalloc WGPUFeatureName[(int)featureCount];
        fixed (WGPUFeatureName* pFeatures = features)
            wgpuAdapterEnumerateFeatures(handle, pFeatures);

        WGPUAdapterProperties properties;
        wgpuAdapterGetProperties(handle, &properties);

        WGPUSupportedLimits limits;
        wgpuAdapterGetLimits(handle, &limits);

        AdapterProperties = properties;
        AdapterLimits = limits;
    }

    public WGPUAdapter Handle { get; }
    public WGPUAdapterProperties AdapterProperties { get; }
    public WGPUSupportedLimits AdapterLimits { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUGraphicsAdapter" /> class.
    /// </summary>
    ~WebGPUGraphicsAdapter() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            wgpuAdapterRelease(Handle);
        }
    }
}
