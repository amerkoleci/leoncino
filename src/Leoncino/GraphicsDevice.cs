// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Collections.Concurrent;

namespace Leoncino;

/// <summary>
/// Defines a graphics logical device.
/// </summary>
public abstract class GraphicsDevice : GraphicsObject
{
    protected uint _frameIndex = 0;
    protected ulong _frameCount = 0;
    protected readonly ConcurrentQueue<Tuple<GPUObject, ulong>> _deferredDestroyObjects = new();
    protected bool _shuttingDown;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicsDevice" /> class.
    /// </summary>
    /// <param name="label">The label of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    protected GraphicsDevice(string? label = default)
        : base(label)
    {
    }

    // <summary>
    /// Get the <see cref="GraphicsAdapter"/> object that created this object.
    /// </summary>
    public abstract GraphicsAdapter Adapter { get; }

    public unsafe GraphicsBuffer CreateBuffer(in BufferDescriptor descriptor, nint initialData = 0)
    {
#if VALIDATE_USAGE
        if (descriptor.Size < 4)
        {
            throw new GraphicsException("Buffer size must be greater or equal to 4");
        }
#endif

        return CreateBufferCore(descriptor, initialData.ToPointer());
    }

    public GraphicsBuffer CreateBuffer(ulong size,
        BufferUsage usage = BufferUsage.ShaderReadWrite,
        CpuAccessMode cpuAccess = CpuAccessMode.None,
        string? label = default)
    {
        return CreateBuffer(new BufferDescriptor(size, usage, cpuAccess, label), IntPtr.Zero);
    }

    public unsafe Texture CreateTexture(in TextureDescription description)
    {
#if VALIDATE_USAGE
        if (description.Format == PixelFormat.Undefined)
        {
            throw new GraphicsException($"Format must be different than {PixelFormat.Undefined}");
        }

        if (description.Width <= 0 || description.Height <= 0 || description.DepthOrArrayLayers <= 0)
        {
            throw new GraphicsException("Width, Height, and DepthOrArrayLayers must be non-zero.");
        }

        if (description.MipLevelCount < 0)
        {
            throw new GraphicsException("mipLevelCount must be greater or equal to zero.");
        }
#endif

        return CreateTextureCore(in description, default);
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

    protected abstract unsafe GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData);
    protected abstract unsafe Texture CreateTextureCore(in TextureDescription description, TextureData* initialData);
    protected abstract BindGroupLayout CreateBindGroupLayoutCore(in BindGroupLayoutDescriptor descriptor);
}
