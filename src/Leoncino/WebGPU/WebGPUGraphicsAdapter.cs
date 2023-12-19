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

        ReadOnlySpan<WGPUFeatureName> features = wgpuAdapterEnumerateFeatures(handle);

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

    /// <inheritdoc />
    public override PixelFormat GetPreferredFormat(Surface surface)
    {
        WebGPUSurface backendSurface = (WebGPUSurface) surface;
        WGPUTextureFormat format = wgpuSurfaceGetPreferredFormat(backendSurface.Handle, Handle);
        return format.ToLeoncino();
    }

    /// <inheritdoc />
    protected unsafe override GraphicsDevice CreateDeviceCore(in DeviceDescriptor descriptor)
    {
        fixed (sbyte* pDeviceName = descriptor.Label.GetUtf8Span())
        {
            WGPUDeviceDescriptor deviceDesc = new()
            {
                nextInChain = null,
                label = pDeviceName,
                requiredFeatureCount = 0,
                requiredLimits = null
            };
            deviceDesc.defaultQueue.nextInChain = null;
            //deviceDesc.defaultQueue.label = "The default queue";

            WGPUDevice result = WGPUDevice.Null;
            wgpuAdapterRequestDevice(
                Handle,
                &deviceDesc,
                OnDeviceRequestEnded,
                IntPtr.Zero
            );
            return new WebGPUGraphicsDevice(result);

            void OnDeviceRequestEnded(WGPURequestDeviceStatus status, WGPUDevice device, sbyte* message, nint pUserData)
            {
                if (status == WGPURequestDeviceStatus.Success)
                {
                    result = device;
                }
                else
                {
                    //Log.Error("Could not get WebGPU adapter: " + Interop.GetString(message));
                }
            }
        }
    }
}
