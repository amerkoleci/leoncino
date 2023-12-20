// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal partial class WebGPUDevice : GPUDevice
{
    public WebGPUDevice(WGPUDevice handle)
    {
        Handle = handle;
        wgpuDeviceSetUncapturedErrorCallback(handle, HandleUncapturedErrorCallback);

        // Get the queue associated with the device
        Queue = wgpuDeviceGetQueue(handle);
    }

    public WGPUDevice Handle { get; }
    public WGPUQueue Queue { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUGraphicsDevice" /> class.
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

    private static void HandleUncapturedErrorCallback(WGPUErrorType type, string message)
    {
#if DEBUG
        throw new GPUException($"Uncaptured device error: type: {type} ({message})");
#else
        //Log.Error($"Uncaptured device error: type: {type} ({message})");
#endif
    }
}
