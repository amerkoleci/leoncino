// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal partial class WebGPUGraphicsDevice : GraphicsDevice
{
    public WebGPUGraphicsDevice(WGPUDevice handle)
    {
        Handle = handle;

        // Get the queue associated with the device
        Queue = wgpuDeviceGetQueue(handle);
    }

    public WGPUDevice Handle { get; }
    public WGPUQueue Queue { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUGraphicsDevice" /> class.
    /// </summary>
    ~WebGPUGraphicsDevice() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            wgpuQueueRelease(Queue);
            wgpuDeviceRelease(Handle);
        }
    }
}
