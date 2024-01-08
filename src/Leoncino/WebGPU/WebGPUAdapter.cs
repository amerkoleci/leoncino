// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal class WebGPUAdapter : GPUAdapter
{
    public unsafe WebGPUAdapter(WGPUAdapter handle)
    {
        Handle = handle;

        ReadOnlySpan<WGPUFeatureName> features = wgpuAdapterEnumerateFeatures(handle);

        WGPUAdapterProperties properties;
        WGPUSupportedLimits supportedLimits;

        wgpuAdapterGetProperties(handle, &properties);
        wgpuAdapterGetLimits(handle, &supportedLimits);

        AdapterProperties = properties;
        AdapterLimits = supportedLimits;
        Features = features.ToArray();
    }

    public WGPUAdapter Handle { get; }
    public WGPUAdapterProperties AdapterProperties { get; }
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
    public override PixelFormat GetSurfacePreferredFormat(GPUSurface surface)
    {
        WebGPUSurface backendSurface = (WebGPUSurface) surface;
        WGPUTextureFormat format = wgpuSurfaceGetPreferredFormat(backendSurface.Handle, Handle);
        return format.ToLeoncino();
    }

    /// <inheritdoc />
    protected unsafe override ValueTask<GPUDevice> CreateDeviceAsyncCore(in DeviceDescriptor descriptor)
    {
        fixed (sbyte* pDeviceName = descriptor.Label.GetUtf8Span())
        {
            WGPUFeatureName* requiredFeatures = stackalloc WGPUFeatureName[2]
            {
                WGPUFeatureName.Depth32FloatStencil8,
                WGPUFeatureName.Float32Filterable
            };

            WGPURequiredLimits requiredLimits;
            requiredLimits.nextInChain = null;
            requiredLimits.limits = AdapterLimits.limits;

            WGPUDeviceDescriptor deviceDesc = new()
            {
                nextInChain = null,
                label = pDeviceName,
                requiredFeatureCount = 1,
                requiredFeatures = requiredFeatures,
                requiredLimits = &requiredLimits
            };
            deviceDesc.defaultQueue.nextInChain = null;
            //deviceDesc.defaultQueue.label = "The default queue";

            WGPUDevice result = WGPUDevice.Null;
            wgpuAdapterRequestDevice(
                Handle,
                &deviceDesc,
                &OnDeviceRequestEnded,
                new nint(&result)
            );
            return ValueTask.FromResult<GPUDevice>(new WebGPUDevice(this, result));
        }
    }

    [UnmanagedCallersOnly]
    private static unsafe void OnDeviceRequestEnded(WGPURequestDeviceStatus status, WGPUDevice device, sbyte* message, nint pUserData)
    {
        if (status == WGPURequestDeviceStatus.Success)
        {
            *(WGPUDevice*)pUserData = device;
        }
        else
        {
            //Log.Error("Could not get WebGPU adapter: " + Interop.GetString(message));
        }
    }
}
