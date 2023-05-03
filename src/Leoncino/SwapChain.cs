// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;

namespace Leoncino;

public abstract class SwapChain : GraphicsObject
{
    public SwapChain(GraphicsDevice device, Surface surface, in SwapChainDescriptor descriptor)
        : base(device, descriptor.Label)
    {
        Surface = surface;
        ColorFormat = descriptor.Format;
        PresentMode = descriptor.PresentMode;
        Size = new(descriptor.Width, descriptor.Height);
    }

    public Surface Surface { get; }

    public PixelFormat ColorFormat { get; protected set; }
    public PresentMode PresentMode { get; }

    public Size Size { get; protected set; }
}
