// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino.D3D12;

internal unsafe partial class D3D12GraphicsSurface : GraphicsSurface
{
    private readonly D3D12GraphicsFactory _factory;

    public D3D12GraphicsSurface(D3D12GraphicsFactory factory, in SurfaceDescriptor description)
        : base(description)
    {
        _factory = factory;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12GraphicsFactory" /> class.
    /// </summary>
    ~D3D12GraphicsSurface() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (IsConfigured)
            {
                // TODO: Destroy Swapchain
            }
        }
    }

    /// <inheritdoc />
    protected override void ConfigureCore(GraphicsDevice device, in SurfaceConfiguration configuration)
    {
        //D3D12GraphicsDevice backendDevice = (D3D12GraphicsDevice)device;
        //PixelFormat format = configuration.Format == PixelFormat.Undefined ? backendDevice.Adapter.GetSurfacePreferredFormat(this) : configuration.Format;
    }
}
