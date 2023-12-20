// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Leoncino;

public static class GPUUtilities
{
    /// <summary>Rounds a given address up to the nearest alignment.</summary>
    /// <param name="address">The address to be aligned.</param>
    /// <param name="alignment">The target alignment, which should be a power of two.</param>
    /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
    /// <remarks>This method does not account for an <paramref name="alignment" /> which is not a <c>power of two</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint AlignUp(uint address, uint alignment)
    {
        Debug.Assert(BitOperations.IsPow2(alignment));

        return (address + (alignment - 1)) & ~(alignment - 1);
    }

    /// <summary>Rounds a given address up to the nearest alignment.</summary>
    /// <param name="address">The address to be aligned.</param>
    /// <param name="alignment">The target alignment, which should be a power of two.</param>
    /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
    /// <remarks>This method does not account for an <paramref name="alignment" /> which is not a <c>power of two</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong AlignUp(ulong address, ulong alignment)
    {
        Debug.Assert(BitOperations.IsPow2(alignment));

        return (address + (alignment - 1)) & ~(alignment - 1);
    }

    /// <summary>Rounds a given address up to the nearest alignment.</summary>
    /// <param name="address">The address to be aligned.</param>
    /// <param name="alignment">The target alignment, which should be a power of two.</param>
    /// <returns><paramref name="address" /> rounded up to the specified <paramref name="alignment" />.</returns>
    /// <remarks>This method does not account for an <paramref name="alignment" /> which is not a <c>power of two</c>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint AlignUp(nuint address, nuint alignment)
    {
        Debug.Assert(BitOperations.IsPow2(alignment));

        return (address + (alignment - 1)) & ~(alignment - 1);
    }

    public static int GetMipLevelCount(int width, int height, int depth = 1, int minDimension = 1, uint requiredAlignment = 1u)
    {
        int mipLevelCount = 1;
        while (width > minDimension || height > minDimension || depth > minDimension)
        {
            width = Math.Max(minDimension, width >> 1);
            height = Math.Max(minDimension, height >> 1);
            depth = Math.Max(minDimension, depth >> 1);
            if (AlignUp((uint)width, requiredAlignment) != width ||
                AlignUp((uint)height, requiredAlignment) != height ||
                AlignUp((uint)depth, requiredAlignment) != depth)
            {
                break;
            }

            mipLevelCount++;
        }

        return mipLevelCount;
    }

    public static ulong ComputeTextureMemorySizeInBytes(in TextureDescriptor descriptor)
    {
        ulong size = 0;
        uint bytesPerBlock = descriptor.Format.GetFormatBytesPerBlock();
        uint pixelsPerBlock = descriptor.Format.GetFormatHeightCompressionRatio();
        uint numBlocksX = (uint)descriptor.Width / pixelsPerBlock;
        uint numBlocksY = (uint)descriptor.Height / pixelsPerBlock;
        int mipLevelCount = descriptor.MipLevelCount == 0 ? GetMipLevelCount(descriptor.Width, descriptor.Height, descriptor.DepthOrArrayLayers) : descriptor.MipLevelCount;
        for (uint arrayLayer = 0; arrayLayer < descriptor.DepthOrArrayLayers; ++arrayLayer)
        {
            for (int mipLevel = 0; mipLevel < mipLevelCount; ++mipLevel)
            {
                uint width = Math.Max(1u, numBlocksX >> mipLevel);
                uint height = Math.Max(1u, numBlocksY >> mipLevel);
                uint depth = (uint)Math.Max(1u, descriptor.DepthOrArrayLayers >> mipLevel);
                size += width * height * depth * bytesPerBlock;
            }
        }

        size *= (uint)descriptor.SampleCount;
        return size;
    }

    public static bool StencilTestEnabled(in DepthStencilState state)
    {
        return
            state.StencilBack.CompareFunction != CompareFunction.Always ||
            state.StencilBack.FailOperation != StencilOperation.Keep ||
            state.StencilBack.DepthFailOperation != StencilOperation.Keep ||
            state.StencilBack.PassOperation != StencilOperation.Keep ||
            state.StencilFront.CompareFunction != CompareFunction.Always ||
            state.StencilFront.FailOperation != StencilOperation.Keep ||
            state.StencilFront.DepthFailOperation != StencilOperation.Keep ||
            state.StencilFront.PassOperation != StencilOperation.Keep;
    }

}
