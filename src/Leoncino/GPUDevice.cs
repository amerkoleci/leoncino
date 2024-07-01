// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Collections.Concurrent;

namespace Leoncino;

/// <summary>
/// Defines a GPU logical device
/// </summary>
public abstract class GPUDevice : GraphicsObject
{
    protected uint _frameIndex = 0;
    protected ulong _frameCount = 0;
    protected readonly ConcurrentQueue<Tuple<GPUObject, ulong>> _deferredDestroyObjects = new();
    protected bool _shuttingDown;

    /// <summary>
    /// Initializes a new instance of the <see cref="GPUDevice" /> class.
    /// </summary>
    /// <param name="label">The label of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    protected GPUDevice(string? label = default)
        : base(label)
    {
    }

    // <summary>
    /// Get the <see cref="GPUAdapter"/> object that created this object.
    /// </summary>
    public abstract GPUAdapter Adapter { get; }

    public unsafe GPUBuffer CreateBuffer(in BufferDescriptor descriptor, nint initialData = 0)
    {
#if VALIDATE_USAGE
        if (descriptor.Size < 4)
        {
            throw new GraphicsException("Buffer size must be greater or equal to 4");
        }
#endif

        return CreateBufferCore(descriptor, initialData.ToPointer());
    }

    public GPUBuffer CreateBuffer(ulong size,
        BufferUsage usage = BufferUsage.ShaderReadWrite,
        CpuAccessMode cpuAccess = CpuAccessMode.None,
        string? label = default)
    {
        return CreateBuffer(new BufferDescriptor(size, usage, cpuAccess, label), IntPtr.Zero);
    }

    public unsafe GPUTexture CreateTexture(in TextureDescriptor descriptor)
    {
#if VALIDATE_USAGE
        if (descriptor.Format == PixelFormat.Undefined)
        {
            throw new GraphicsException($"Format must be different than {PixelFormat.Undefined}");
        }

        if (descriptor.Width == 0 || descriptor.Height == 0 || descriptor.DepthOrArrayLayers == 0)
        {
            throw new GraphicsException("Width, Height, and DepthOrArrayLayers must be non-zero.");
        }
#endif

        return CreateTextureCore(in descriptor, default);
    }

    public BindGroupLayout CreateBindGroupLayout(in BindGroupLayoutDescriptor descriptor)
    {
        return CreateBindGroupLayoutCore(in descriptor);
    }

    public BindGroupLayout CreateBindGroupLayout(params BindGroupLayoutEntry[] entries)
    {
        return CreateBindGroupLayoutCore(new BindGroupLayoutDescriptor(entries));
    }

    internal void QueueDestroy(GPUObject @object)
    {
        if (_shuttingDown)
        {
            @object.Destroy();
            return;
        }

        _deferredDestroyObjects.Enqueue(Tuple.Create(@object, _frameCount));
    }

    protected void ProcessDeletionQueue()
    {
        while (!_deferredDestroyObjects.IsEmpty)
        {
            if (_deferredDestroyObjects.TryPeek(out Tuple<GPUObject, ulong>? item) &&
                item.Item2 + Constants.MaxFramesInFlight < _frameCount)
            {
                if (_deferredDestroyObjects.TryDequeue(out item))
                {
                    item.Item1.Destroy();
                }
            }
            else
            {
                break;
            }
        }
    }

    protected abstract unsafe GPUBuffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData);
    protected abstract unsafe GPUTexture CreateTextureCore(in TextureDescriptor descriptor, TextureData* initialData);
    protected abstract BindGroupLayout CreateBindGroupLayoutCore(in BindGroupLayoutDescriptor descriptor);
}
