// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="Texture"/>.
/// </summary>
public record struct TextureDescriptor
{
    [SetsRequiredMembers]
    public TextureDescriptor(
        TextureDimension dimension,
        PixelFormat format,
        int width,
        int height,
        int depthOrArrayLayers,
        int mipLevels = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        TextureSampleCount sampleCount = TextureSampleCount.Count1,
        string? label = default)
    {
        Guard.IsTrue(format != PixelFormat.Undefined);
        Guard.IsGreaterThanOrEqualTo(width, 1);
        Guard.IsGreaterThanOrEqualTo(height, 1);
        Guard.IsGreaterThanOrEqualTo(depthOrArrayLayers, 1);

        Dimension = dimension;
        Format = format;
        Width = width;
        Height = height;
        DepthOrArrayLayers = depthOrArrayLayers;
        MipLevels = mipLevels == 0 ? CountMipLevels(width, height, dimension == TextureDimension.Texture3D ? depthOrArrayLayers : 1) : mipLevels;
        SampleCount = sampleCount;
        Usage = usage;
        Label = label;
    }

    public static TextureDescriptor Texture1D(
        PixelFormat format,
        int width,
        int mipLevels = 1,
        int arrayLayers = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        string? label = default)
    {
        return new(
            TextureDimension.Texture1D,
            format,
            width,
            1,
            arrayLayers,
            mipLevels,
            usage,
            TextureSampleCount.Count1,
            label);
    }

    public static TextureDescriptor Texture2D(
        PixelFormat format,
        int width,
        int height,
        int mipLevels = 1,
        int arrayLayers = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        TextureSampleCount sampleCount = TextureSampleCount.Count1,
        string? label = default)
    {
        return new(
            TextureDimension.Texture2D,
            format,
            width,
            height,
            arrayLayers,
            mipLevels,
            usage,
            sampleCount,
            label);
    }

    public static TextureDescriptor Texture3D(
        PixelFormat format,
        int width,
        int height,
        int depth = 1,
        int mipLevels = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        string? label = default)
    {
        return new(
            TextureDimension.Texture3D,
            format,
            width,
            height,
            depth,
            mipLevels,
            usage,
            TextureSampleCount.Count1,
            label);
    }

    /// <summary>
    /// Gets the dimension of <see cref="Texture"/>
    /// </summary>
    public required TextureDimension Dimension { get; init; }

    /// <summary>
    /// Gets the pixel format of <see cref="Texture"/>
    /// </summary>
    public required PixelFormat Format { get; init; }

    /// <summary>
    /// Gets the width of <see cref="Texture"/>
    /// </summary>
    public required int Width { get; init; }

    /// <summary>
    /// Gets the height of <see cref="Texture"/>
    /// </summary>
    public required int Height { get; init; }

    /// <summary>
    /// Gets the depth of <see cref="Texture"/>, if it is 3D, or the array layers if it is an array of 1D or 2D resources.
    /// </summary>
    public required int DepthOrArrayLayers { get; init; }

    /// <summary>
    /// Gets the number of MIP levels in the <see cref="Texture"/>
    /// </summary>
    public required int MipLevels { get; init; }

    /// <summary>
    /// Gets the <see cref="TextureUsage"/> of <see cref="Texture"/>.
    /// </summary>
    public TextureUsage Usage { get; init; } = TextureUsage.ShaderRead;

    /// <summary>
    /// Gets the texture sample count.
    /// </summary>
    public TextureSampleCount SampleCount { get; init; } = TextureSampleCount.Count1;

    /// <summary>
    /// CPU access of the <see cref="Texture"/>.
    /// </summary>
    public CpuAccessMode CpuAccess { get; init; } = CpuAccessMode.None;

    // <summary>
    /// Gets or sets the label of <see cref="Texture"/>.
    /// </summary>
    public string? Label { get; init; }

    /// <summary>
    /// Returns the number of mip levels given a texture size
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private static int CountMipLevels(int width, int height, int depth = 1)
    {
        int numMips = 0;
        int size = Math.Max(Math.Max(width, height), depth);
        while (1u << numMips <= size)
        {
            ++numMips;
        }

        if (1 << numMips < size)
            ++numMips;

        return numMips;
    }
}
