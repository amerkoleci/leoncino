// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="GPUTexture"/>.
/// </summary>
public readonly record struct TextureDescriptor
{
    [SetsRequiredMembers]
    public TextureDescriptor(
        TextureDimension dimension,
        PixelFormat format,
        int width,
        int height,
        int depthOrArrayLayers = 1,
        int mipLevelCount = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        uint sampleCount = 1u,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        Guard.IsTrue(format != PixelFormat.Undefined);
        Guard.IsGreaterThanOrEqualTo(width, 1);
        Guard.IsGreaterThanOrEqualTo(height, 1);
        Guard.IsGreaterThanOrEqualTo(depthOrArrayLayers, 1);
        Guard.IsGreaterThanOrEqualTo(mipLevelCount, 0);

        Dimension = dimension;
        Format = format;
        Width = width;
        Height = height;
        DepthOrArrayLayers = depthOrArrayLayers;
        MipLevelCount = mipLevelCount == 0 ? GPUUtilities.GetMipLevelCount((int)width, (int)height, dimension == TextureDimension.Texture3D ? (int)depthOrArrayLayers : 1) : mipLevelCount;
        SampleCount = sampleCount;
        Usage = usage;
        CpuAccess = access;
        Label = label;
    }

    public static TextureDescriptor Texture1D(
        PixelFormat format,
        int width,
        int mipLevelCount = 1,
        int arrayLayers = 1,
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
            1,
            access,
            label);
    }

    public static TextureDescriptor Texture2D(
        PixelFormat format,
        int width,
        int height,
        int mipLevelCount = 1,
        int arrayLayers = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        uint sampleCount = 1,
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
        int width,
        int height,
        int depth,
        int mipLevelCount = 1,
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
            1,
            access,
            label);
    }

    /// <summary>
    /// Gets the dimension of <see cref="GPUTexture"/>
    /// </summary>
    public required TextureDimension Dimension { get; init; }

    /// <summary>
    /// Gets the pixel format of <see cref="GPUTexture"/>
    /// </summary>
    public required PixelFormat Format { get; init; }

    /// <summary>
    /// Gets the width of <see cref="GPUTexture"/>
    /// </summary>
    public required int Width { get; init; } = 1;

    /// <summary>
    /// Gets the height of <see cref="GPUTexture"/>
    /// </summary>
    public required int Height { get; init; } = 1;

    /// <summary>
    /// Gets the depth of <see cref="GPUTexture"/>, if it is 3D, or the array layers if it is an array of 1D or 2D resources.
    /// </summary>
    public required int DepthOrArrayLayers { get; init; } = 1;

    /// <summary>
    /// The number of mipmap levels in the <see cref="GPUTexture"/>.
    /// </summary>
    public required int MipLevelCount { get; init; } = 1;

    /// <summary>
    /// Gets the <see cref="TextureUsage"/> of <see cref="GPUTexture"/>.
    /// </summary>
    public TextureUsage Usage { get; init; } = TextureUsage.ShaderRead;

    /// <summary>
    /// Gets the texture sample count.
    /// </summary>
    public uint SampleCount { get; init; } = 1;

    /// <summary>
    /// CPU access of the <see cref="GPUTexture"/>.
    /// </summary>
    public CpuAccessMode CpuAccess { get; init; } = CpuAccessMode.None;

    // <summary>
    /// Gets or sets the label of <see cref="GPUTexture"/>.
    /// </summary>
    public string? Label { get; init; }
}
