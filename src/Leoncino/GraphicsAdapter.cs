// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public abstract class GraphicsAdapter
{
    public abstract uint VendorId { get; }
    public abstract uint DeviceId { get; }
    public abstract string Name { get; }
    public abstract string DriverDescription { get; }
    public abstract GraphicsAdapterType AdapterType { get; }
    public abstract BackendType Backend { get; }

    public abstract PixelFormat GetPreferredFormat(Surface surface);

    public abstract GraphicsDevice CreateDevice(in GraphicsDeviceDescriptor descriptor);
    public GraphicsDevice CreateDevice() => CreateDevice(new GraphicsDeviceDescriptor());
}
