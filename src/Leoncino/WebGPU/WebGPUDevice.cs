// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe class WebGPUDevice : GraphicsDevice
{
    private WebGPUAdapter _adapter;

    public WebGPUDevice(WebGPUAdapter adapter, in GraphicsDeviceDescriptor descriptor)
    {
        _adapter = adapter;

        fixed (byte* pDeviceName = descriptor.Label.GetUtf8Span())
        fixed (WGPUFeatureName* requiredFeatures = adapter.Features)
        {
            WGPURequiredLimits requiredLimits;
            requiredLimits.nextInChain = null;
            requiredLimits.limits = adapter.AdapterLimits.limits;

            WGPUDeviceDescriptor deviceDesc = new()
            {
                nextInChain = null,
                label = pDeviceName,
                requiredFeatureCount = (nuint)adapter.Features.Length,
                requiredFeatures = requiredFeatures,
                requiredLimits = &requiredLimits,
                deviceLostCallback = &HandleDeviceLostCallback
            };
            deviceDesc.defaultQueue.nextInChain = null;
            deviceDesc.uncapturedErrorCallbackInfo.callback = &HandleUncapturedErrorCallback;
            //deviceDesc.defaultQueue.label = "The default queue";

            WGPUDevice handle = WGPUDevice.Null;
            wgpuAdapterRequestDevice(
                adapter.Handle,
                &deviceDesc,
                &OnDeviceRequestEnded,
                &handle
            );

            Handle = handle;
        }

        // Get the queue associated with the device
        Queue = wgpuDeviceGetQueue(Handle);
    }

    /// <inheritdoc />
    public override GraphicsAdapter Adapter => _adapter;

    public WGPUDevice Handle { get; }
    public WGPUQueue Queue { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUDevice" /> class.
    /// </summary>
    ~WebGPUDevice() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            wgpuQueueRelease(Queue);
            wgpuDeviceRelease(Handle);
        }
    }

    /// <inheritdoc />
    protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData)
    {
        return new WebGPUBuffer(this, in descriptor, initialData);
    }

    /// <inheritdoc />
    protected override unsafe Texture CreateTextureCore(in TextureDescriptor descriptor, TextureData* initialData)
    {
        return new WebGPUTexture(this, in descriptor, initialData);
    }

    /// <inheritdoc />
    protected override BindGroupLayout CreateBindGroupLayoutCore(in BindGroupLayoutDescriptor descriptor)
    {
        return new WebGPUBindGroupLayout(this, in descriptor);
    }

    [UnmanagedCallersOnly]
    private static unsafe void OnDeviceRequestEnded(WGPURequestDeviceStatus status, WGPUDevice device, byte* message, void* pUserData)
    {
        if (status == WGPURequestDeviceStatus.Success)
        {
            *(WGPUDevice*)pUserData = device;
        }
        else
        {
            throw new GraphicsException("Could not get WGPU adapter: " + Interop.GetString(message));
        }
    }

    [UnmanagedCallersOnly]
    private static void HandleDeviceLostCallback(WGPUDeviceLostReason type, byte* pMessage, void* pUserData)
    {
        string message = Interop.GetString(pMessage)!;
        throw new GraphicsException($"WGPU Device lost: reason {type} ({message})");
    }

    [UnmanagedCallersOnly]
    private static void HandleUncapturedErrorCallback(WGPUErrorType type, byte* pMessage, void* pUserData)
    {
        string message = Interop.GetString(pMessage)!;
        throw new GraphicsException($"Uncaptured device error: type: {type} ({message})");
    }
}
