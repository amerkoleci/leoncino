// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public abstract class Texture : GraphicsResource
{
    //protected readonly Dictionary<TextureViewDescription, TextureView> _views = new();

    /// <summary>
    /// Gets the texture dimension.
    /// </summary>
    public TextureDimension Dimension { get; }

    /// <summary>
    /// Gets the texture format.
    /// </summary>
    public PixelFormat Format { get; }

    /// <summary>
    /// Gets the texture width, in texels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the texture height, in texels.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the texture depth, in texels for 3D texture or the total number of array layers.
    /// </summary>
    public int DepthOrArrayLayers { get; }

    /// <summary>
    /// Gets the texture total number of mipmap levels.
    /// </summary>
    public int MipLevels { get; }

    /// <summary>
    /// Gets the texture <see cref="TextureUsage"/>.
    /// </summary>
    public TextureUsage Usage { get; }

    /// <summary>
    /// Gets the texture sample count.
    /// </summary>
    public TextureSampleCount SampleCount { get; }

    /// <summary>
    /// Gets the CPU access of the texure.
    /// </summary>
    public CpuAccessMode CpuAccess { get; }

    protected Texture(GraphicsDevice device, in TextureDescriptor descriptor)
        : base(device, descriptor.Label)
    {
        Dimension = descriptor.Dimension;
        Format = descriptor.Format;
        Width = descriptor.Width;
        Height = descriptor.Height;
        DepthOrArrayLayers = descriptor.DepthOrArrayLayers;
        MipLevels = descriptor.MipLevels;
        SampleCount = descriptor.SampleCount;
        Usage = descriptor.Usage;
        CpuAccess = descriptor.CpuAccess;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            //foreach (KeyValuePair<TextureViewDescription, TextureView> kvp in _views)
            //{
            //    kvp.Value.Dispose();
            //}
        }
    }

    //public TextureView GetView() => GetView(new TextureViewDescription());
    //
    //public TextureView GetView(in TextureViewDescription description)
    //{
    //    if(!_views.TryGetValue(description, out TextureView? view))
    //    {
    //        view = CreateView(description);
    //        _views.Add(description, view);
    //    }
    //
    //    return view!;
    //}

    /// <summary>
    /// Get a mip-level width.
    /// </summary>
    /// <param name="mipLevel"></param>
    /// <returns></returns>
    public int GetWidth(int mipLevel = 0)
    {
        return (mipLevel == 0) || (mipLevel < MipLevels) ? Math.Max(1, Width >> mipLevel) : 0;
    }

    // <summary>
    /// Get a mip-level height.
    /// </summary>
    /// <param name="mipLevel"></param>
    /// <returns></returns>
    public int GetHeight(int mipLevel = 0)
    {
        return (mipLevel == 0) || (mipLevel < MipLevels) ? Math.Max(1, Height >> mipLevel) : 0;
    }

    // <summary>
    /// Get a mip-level depth.
    /// </summary>
    /// <param name="mipLevel"></param>
    /// <returns></returns>
    public int GetDepth(int mipLevel = 0)
    {
        if (Dimension != TextureDimension.Texture3D)
        {
            return 1;
        }

        return (mipLevel == 0) || (mipLevel < MipLevels) ? Math.Max(1, DepthOrArrayLayers >> mipLevel) : 0;
    }

    public int CalculateSubresource(int mipSlice, int arraySlice, int planeSlice = 0)
    {
        return mipSlice + arraySlice * MipLevels + planeSlice * MipLevels * DepthOrArrayLayers;
    }

    //protected abstract TextureView CreateView(in TextureViewDescription description);
}
