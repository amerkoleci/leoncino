// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="Texture"/>.
/// </summary>
public record struct TextureDescriptor
{
    public const int NumCubeMapSlices = 6;

    /// <summary>
    /// Gets the dimension of <see cref="Texture"/>
    /// </summary>
    public required TextureDimension Dimension;

    /// <summary>
    /// Gets the pixel format of <see cref="Texture"/>
    /// </summary>
    public required PixelFormat Format;

    /// <summary>
    /// Gets the width of <see cref="Texture"/>
    /// </summary>
    public required uint Width = 1;

    /// <summary>
    /// Gets the height of <see cref="Texture"/>
    /// </summary>
    public required uint Height = 1;

    /// <summary>
    /// Gets the depth of <see cref="Texture"/>, if it is 3D, or the array layers if it is an array of 1D or 2D resources.
    /// </summary>
    public uint DepthOrArrayLayers = 1;

    /// <summary>
    /// The number of mipmap levels in the <see cref="Texture"/>.
    /// </summary>
    public uint MipLevelCount = 1;

    /// <summary>
    /// Gets the <see cref="TextureUsage"/> of <see cref="Texture"/>.
    /// </summary>
    public TextureUsage Usage = TextureUsage.ShaderRead;

    /// <summary>
    /// Gets the texture sample count.
    /// </summary>
    public TextureSampleCount SampleCount = TextureSampleCount.Count1;

    /// <summary>
    /// CPU access of the <see cref="Texture"/>.
    /// </summary>
    public CpuAccessMode CpuAccess { get; init; } = CpuAccessMode.None;

    // <summary>
    /// Gets or sets the label of <see cref="Texture"/>.
    /// </summary>
    public string? Label { get; init; }

    [SetsRequiredMembers]
    public TextureDescriptor(
        TextureDimension dimension,
        PixelFormat format,
        uint width,
        uint height,
        uint depthOrArrayLayers = 1,
        uint mipLevelCount = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        TextureSampleCount sampleCount = TextureSampleCount.Count1,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        Dimension = dimension;
        Format = format;
        Width = width;
        Height = height;
        DepthOrArrayLayers = depthOrArrayLayers;
        MipLevelCount = mipLevelCount == 0 ? GraphicsUtilities.GetMipLevelCount(width, height, dimension == TextureDimension.Texture3D ? depthOrArrayLayers : 1u) : mipLevelCount;
        SampleCount = sampleCount;
        Usage = usage;
        CpuAccess = access;
        Label = label;
    }

    public static TextureDescriptor Texture1D(
        PixelFormat format,
        uint width,
        uint mipLevelCount = 1,
        uint arrayLayers = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        return new TextureDescriptor(
            TextureDimension.Texture1D,
            format,
            width,
            1,
            arrayLayers,
            mipLevelCount,
            usage,
            TextureSampleCount.Count1,
            access,
            label);
    }

    public static TextureDescriptor Texture2D(
        PixelFormat format,
        uint width,
        uint height,
        uint mipLevelCount = 1,
        uint arrayLayers = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        TextureSampleCount sampleCount = TextureSampleCount.Count1,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        return new TextureDescriptor(
            TextureDimension.Texture2D,
            format,
            width,
            height,
            arrayLayers,
            mipLevelCount,
            usage,
            sampleCount,
            access,
            label);
    }

    public static TextureDescriptor Texture3D(
        PixelFormat format,
        uint width,
        uint height,
        uint depth,
        uint mipLevelCount = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        return new TextureDescriptor(
            TextureDimension.Texture3D,
            format,
            width,
            height,
            depth,
            mipLevelCount,
            usage,
            TextureSampleCount.Count1,
            access,
            label);
    }

    public static TextureDescriptor TextureCube(
        PixelFormat format,
        uint width,
        uint mipLevelCount = 1,
        uint arrayLayers = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        return new TextureDescriptor(
            TextureDimension.Texture2D,
            format,
            width,
            width,
            arrayLayers * NumCubeMapSlices,
            mipLevelCount,
            usage,
            TextureSampleCount.Count1,
            access,
            label);
    }
}
