// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a graphics (physical device) adapter.
/// </summary>
public abstract class GraphicsAdapter : GraphicsObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicsAdapter" /> class.
    /// </summary>
    protected GraphicsAdapter()
    {
    }

    public abstract PixelFormat GetSurfacePreferredFormat(GraphicsSurface surface);

    public GraphicsDevice CreateDevice(in GraphicsDeviceDescription description)
    {
        return CreateDeviceCore(in description);
    }

    protected abstract GraphicsDevice CreateDeviceCore(in GraphicsDeviceDescription description);
}
