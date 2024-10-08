// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// A graphics resource that holds image data in a specific format.
/// </summary>
public abstract class Texture : GPUObject
{
    protected Texture(in TextureDescriptor descriptor)
        : base(descriptor.Label)
    {
        Dimension = descriptor.Dimension;
        Format = descriptor.Format;
        Width = descriptor.Width;
        Height = descriptor.Height;
        Depth = (descriptor.Dimension == TextureDimension.Texture3D) ? descriptor.DepthOrArrayLayers : 1;
        ArrayLayers = (descriptor.Dimension != TextureDimension.Texture3D) ? descriptor.DepthOrArrayLayers : 1;
        MipLevelCount = descriptor.MipLevelCount;
        SampleCount = descriptor.SampleCount;
        Usage = descriptor.Usage;
        CpuAccess = descriptor.CpuAccess;
    }

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
    public uint Width { get; }

    /// <summary>
    /// Gets the texture height, in texels.
    /// </summary>
    public uint Height { get; }

    /// <summary>
    /// Gets the texture depth, in texels.
    /// </summary>
    public uint Depth { get; }

    /// <summary>
    /// Gets the texture total number of array layers.
    /// </summary>
    public uint ArrayLayers { get; }

    /// <summary>
    /// Gets the total number of mipmap levels in this texture.
    /// </summary>
    public uint MipLevelCount { get; }

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

    /// <summary>
    /// Get a mip-level width.
    /// </summary>
    /// <param name="mipLevel"></param>
    /// <returns></returns>
    public uint GetWidth(uint mipLevel = 0)
    {
        return (mipLevel == 0) || (mipLevel < MipLevelCount) ? Math.Max(1, Width >> (int)mipLevel) : 0;
    }

    // <summary>
    /// Get a mip-level height.
    /// </summary>
    /// <param name="mipLevel"></param>
    /// <returns></returns>
    public uint GetHeight(uint mipLevel = 0)
    {
        return (mipLevel == 0) || (mipLevel < MipLevelCount) ? Math.Max(1, Height >> (int)mipLevel) : 0;
    }

    // <summary>
    /// Get a mip-level depth.
    /// </summary>
    /// <param name="mipLevel"></param>
    /// <returns></returns>
    public uint GetDepth(uint mipLevel = 0)
    {
        return (mipLevel == 0) || (mipLevel < MipLevelCount) ? Math.Max(1, Depth >> (int)mipLevel) : 0;
    }

    public uint CalculateSubresource(uint mipLevel, uint arrayLayer, uint planeSlice = 0)
    {
        return mipLevel + arrayLayer * MipLevelCount + planeSlice * MipLevelCount * ArrayLayers;
    }
}
