// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe class WebGPUBuffer : GPUBuffer
{
    private readonly WebGPUDevice _device;
    public readonly void* pMappedData = default;

    public WebGPUBuffer(WebGPUDevice device, in BufferDescriptor descriptor, void* initialData)
        : base(descriptor)
    {
        _device = device;

        fixed (sbyte* pLabel = descriptor.Label.GetUtf8Span())
        {
            WGPUBufferDescriptor wgpuDescriptor = new()
            {
                nextInChain = null,
                label = pLabel,
                size = descriptor.Size,
                usage = WGPUBufferUsage.Vertex,
                mappedAtCreation = false
            };

            if (initialData != null)
            {
                wgpuDescriptor.usage |= WGPUBufferUsage.CopyDst;
            }

            Handle = wgpuDeviceCreateBuffer(device.Handle, &wgpuDescriptor);
            if (initialData != null)
            {
                wgpuQueueWriteBuffer(device.Queue, Handle, 0u, initialData, (nuint)descriptor.Size);
            }
        }
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUBuffer" /> class.
    /// </summary>
    ~WebGPUBuffer() => Dispose(disposing: false);

    public WGPUBuffer Handle { get; }

    /// <inheritdoc />
    public override GPUDevice Device => _device;

    /// <inheritdoc />
    protected internal override void Destroy()
    {
        wgpuBufferRelease(Handle);
    }

    /// <inheritdoc />
    protected override void OnLabelChanged(string newLabel)
    {
        wgpuBufferSetLabel(Handle, newLabel);
    }

    /// <inheitdoc />
    protected override void SetDataUnsafe(void* dataPtr, int offsetInBytes)
    {
        Unsafe.CopyBlockUnaligned((byte*)pMappedData + offsetInBytes, dataPtr, (uint)Size);
    }

    /// <inheitdoc />
    protected override void GetDataUnsafe(void* destPtr, int offsetInBytes)
    {
        Unsafe.CopyBlockUnaligned(destPtr, (byte*)pMappedData + offsetInBytes, (uint)Size);
    }
}
