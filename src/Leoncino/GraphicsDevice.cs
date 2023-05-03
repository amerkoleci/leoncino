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

    public SwapChain CreateSwapChain(Surface surface, in SwapChainDescriptor descriptor)
    {
        Guard.IsNotNull(surface, nameof(surface));

        return CreateSwapChainCore(surface, descriptor);
    }

    protected abstract SwapChain CreateSwapChainCore(Surface surface, in SwapChainDescriptor descriptor);
}
