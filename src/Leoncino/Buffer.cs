// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a Graphics buffer.
/// </summary>
public abstract class Buffer : GraphicsResource
{
    protected Buffer(GraphicsDevice device, in BufferDescriptor descriptor)
        : base(device, descriptor.Label)
    {
        Size = descriptor.Size;
        Usage = descriptor.Usage;
    }

    public ulong Size { get; }
    public BufferUsage Usage { get; }
}
