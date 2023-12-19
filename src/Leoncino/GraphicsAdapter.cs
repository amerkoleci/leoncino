// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a graphics (physical device) adapter.
/// </summary>
public abstract class GraphicsAdapter : GraphicsObjectBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicsAdapter" /> class.
    /// </summary>
    protected GraphicsAdapter()
    {
    }

    public abstract PixelFormat GetPreferredFormat(Surface surface);

    // TODO: Async
    public GraphicsDevice CreateDevice(in DeviceDescriptor descriptor)
    {
        return CreateDeviceCore(in descriptor);
    }

    protected abstract GraphicsDevice CreateDeviceCore(in DeviceDescriptor descriptor);
}
