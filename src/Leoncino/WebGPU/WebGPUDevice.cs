// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe class WebGPUDevice : GPUDevice
{
    private WebGPUAdapter _adapter;

    public WebGPUDevice(WebGPUAdapter adapter, WGPUDevice handle)
    {
        _adapter = adapter;
        Handle = handle;
        wgpuDeviceSetUncapturedErrorCallback(handle, &HandleUncapturedErrorCallback, 0);

        // Get the queue associated with the device
        Queue = wgpuDeviceGetQueue(handle);
    }

    /// <inheritdoc />
    public override GPUAdapter Adapter => _adapter;

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
    protected override GPUBuffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData)
    {
        return new WebGPUBuffer(this, in descriptor, initialData);
    }

    /// <inheritdoc />
    protected override unsafe GPUTexture CreateTextureCore(in TextureDescriptor descriptor, TextureData* initialData)
    {
        return new WebGPUTexture(this, in descriptor, initialData);
    }

    /// <inheritdoc />
    protected override BindGroupLayout CreateBindGroupLayoutCore(in BindGroupLayoutDescriptor descriptor)
    {
        return new WebGPUBindGroupLayout(this, in descriptor);
    }

    [UnmanagedCallersOnly]
    private static void HandleUncapturedErrorCallback(WGPUErrorType type, sbyte* pMessage, nint pUserData)
    {
        string message = Interop.GetString(pMessage)!;
        throw new LeoncinoException($"Uncaptured device error: type: {type} ({message})");
    }
}
