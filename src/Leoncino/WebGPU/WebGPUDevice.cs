// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

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
        wgpuDeviceSetUncapturedErrorCallback(handle, HandleUncapturedErrorCallback);

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

    protected override GPUBuffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData)
    {
        return new WebGPUBuffer(this, in descriptor, initialData);
    }

    private static void HandleUncapturedErrorCallback(WGPUErrorType type, string message)
    {
#if DEBUG
        throw new GPUException($"Uncaptured device error: type: {type} ({message})");
#else
        //Log.Error($"Uncaptured device error: type: {type} ({message})");
#endif
    }
}
