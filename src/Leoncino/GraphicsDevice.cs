// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Leoncino;

public abstract class GraphicsDevice : DisposableObject
{
    public abstract GraphicsAdapter Adapter { get; }

    /// <summary>
    /// Wait for device to finish pending GPU operations.
    /// </summary>
    public abstract void WaitIdle();

    public unsafe Texture CreateTexture(in TextureDescriptor descriptor)
    {
        Guard.IsGreaterThanOrEqualTo(descriptor.Width, 1, nameof(TextureDescriptor.Width));
        Guard.IsGreaterThanOrEqualTo(descriptor.Height, 1, nameof(TextureDescriptor.Height));
        Guard.IsGreaterThanOrEqualTo(descriptor.DepthOrArrayLayers, 1, nameof(TextureDescriptor.DepthOrArrayLayers));

        return CreateTextureCore(descriptor, default);
    }

    public QueryHeap CreateQueryHeap(in QueryHeapDescription description)
    {
        return CreateQueryHeapCore(description);
    }

    public SwapChain CreateSwapChain(Surface surface, in SwapChainDescriptor descriptor)
    {
        Guard.IsNotNull(surface, nameof(surface));

        return CreateSwapChainCore(surface, descriptor);
    }

    protected abstract unsafe Buffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData);
    protected abstract unsafe Texture CreateTextureCore(in TextureDescriptor descriptor, void* initialData);
    protected abstract QueryHeap CreateQueryHeapCore(in QueryHeapDescription description);
    protected abstract SwapChain CreateSwapChainCore(Surface surface, in SwapChainDescriptor descriptor);
}
