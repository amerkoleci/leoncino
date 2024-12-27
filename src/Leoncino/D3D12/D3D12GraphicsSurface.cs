// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino.D3D12;

internal unsafe partial class D3D12GraphicsSurface : GraphicsSurface
{
    private readonly D3D12GraphicsFactory _factory;

    public D3D12GraphicsSurface(D3D12GraphicsFactory factory, in SurfaceDescription description)
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
    public override bool GetCapabilites(GraphicsAdapter adapter, out SurfaceCapabilities capabilities)
    {
        capabilities = new SurfaceCapabilities
        {
            PreferredFormat = PixelFormat.BGRA8UnormSrgb,
            SupportedUsage = TextureUsage.ShaderRead | TextureUsage.RenderTarget,
            Formats = [
                PixelFormat.BGRA8Unorm,
                PixelFormat.BGRA8UnormSrgb,
                PixelFormat.RGBA8Unorm,
                PixelFormat.RGBA8UnormSrgb,
                PixelFormat.RGBA16Float,
                PixelFormat.RGB10A2Unorm,
            ],
            PresentModes = [
                PresentMode.Fifo,
                PresentMode.Mailbox,
                PresentMode.Immediate,
            ],
        };
        return true;
    }

    /// <inheritdoc />
    protected override void ConfigureCore(GraphicsDevice device, in SurfaceConfiguration configuration)
    {
        //D3D12GraphicsDevice backendDevice = (D3D12GraphicsDevice)device;
        //PixelFormat format = configuration.Format == PixelFormat.Undefined ? backendDevice.Adapter.GetSurfacePreferredFormat(this) : configuration.Format;
    }
}
