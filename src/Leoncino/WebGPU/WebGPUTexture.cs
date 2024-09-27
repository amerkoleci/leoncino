// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe class WebGPUTexture : Texture
{
    private readonly WebGPUDevice _device;

    public WebGPUTexture(WebGPUDevice device, in TextureDescriptor descriptor, TextureData* initialData)
        : base(descriptor)
    {
        _device = device;
        WGPUTextureFormat format = descriptor.Format.ToWGPU();

        fixed (byte* pLabel = descriptor.Label.GetUtf8Span())
        {
            WGPUTextureDescriptor wgpuDescriptor = new()
            {
                nextInChain = null,
                label = pLabel,
                usage = WGPUTextureUsage.TextureBinding,
                dimension = WGPUTextureDimension._2D,
                size = new WGPUExtent3D(descriptor.Width, descriptor.Height, descriptor.DepthOrArrayLayers),
                format = format,
                mipLevelCount = descriptor.MipLevelCount,
                sampleCount = (uint)descriptor.SampleCount,
                viewFormatCount = 0,
                viewFormats = null,
            };

            Handle = wgpuDeviceCreateTexture(device.Handle, &wgpuDescriptor);
        }
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUTexture" /> class.
    /// </summary>
    ~WebGPUTexture() => Dispose(disposing: false);

    public WGPUTexture Handle { get; }

    /// <inheritdoc />
    public override GraphicsDevice Device => _device;

    /// <inheritdoc />
    protected internal override void Destroy()
    {
        wgpuTextureRelease(Handle);
    }

    /// <inheritdoc />
    protected override void OnLabelChanged(string newLabel)
    {
        wgpuTextureSetLabel(Handle, newLabel);
    }
}
