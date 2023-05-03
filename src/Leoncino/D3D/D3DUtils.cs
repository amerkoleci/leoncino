// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.Diagnostics;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using Win32.Graphics.Dxgi.Common;
using D3DPrimitiveTopology = Win32.Graphics.Direct3D.PrimitiveTopology;

namespace Leoncino;

internal static unsafe class D3DUtils
{
    private static readonly D3DPrimitiveTopology[] s_d3dPrimitiveTopologyMap = new D3DPrimitiveTopology[(int)PrimitiveTopology.Count] {
        D3DPrimitiveTopology.PointList,
        D3DPrimitiveTopology.LineList,
        D3DPrimitiveTopology.LineStrip,
        D3DPrimitiveTopology.TriangleList,
        D3DPrimitiveTopology.TriangleStrip,
        D3DPrimitiveTopology.P1ControlPointPatchList
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Format ToDxgiSwapChainFormat(this PixelFormat format)
    {
        // FLIP_DISCARD and FLIP_SEQEUNTIAL swapchain buffers only support these formats
        switch (format)
        {
            case PixelFormat.Rgba16Float:
                return Format.R16G16B16A16Float;

            case PixelFormat.Bgra8Unorm:
            case PixelFormat.Bgra8UnormSrgb:
                return Format.B8G8R8A8Unorm;

            case PixelFormat.Rgba8Unorm:
            case PixelFormat.Rgba8UnormSrgb:
                return Format.R8G8B8A8Unorm;

            case PixelFormat.Rgb10a2Unorm:
                return Format.R10G10B10A2Unorm;

            default:
                return Format.B8G8R8A8Unorm;
        }
    }

    public static Format ToDxgiFormat(this PixelFormat format)
    {
        switch (format)
        {
            // 8-bit formats
            case PixelFormat.R8Unorm: return Format.R8Unorm;
            case PixelFormat.R8Snorm: return Format.R8Snorm;
            case PixelFormat.R8Uint: return Format.R8Uint;
            case PixelFormat.R8Sint: return Format.R8Sint;
            // 16-bit formats
            case PixelFormat.R16Unorm: return Format.R16Unorm;
            case PixelFormat.R16Snorm: return Format.R16Snorm;
            case PixelFormat.R16Uint: return Format.R16Uint;
            case PixelFormat.R16Sint: return Format.R16Sint;
            case PixelFormat.R16Float: return Format.R16Float;
            case PixelFormat.Rg8Unorm: return Format.R8G8Unorm;
            case PixelFormat.Rg8Snorm: return Format.R8G8Snorm;
            case PixelFormat.Rg8Uint: return Format.R8G8Uint;
            case PixelFormat.Rg8Sint: return Format.R8G8Sint;
            // Packed 16-Bit Pixel Formats
            case PixelFormat.Bgra4Unorm: return Format.B4G4R4A4Unorm;
            case PixelFormat.B5G6R5Unorm: return Format.B5G6R5Unorm;
            case PixelFormat.Bgr5A1Unorm: return Format.B5G5R5A1Unorm;
            // 32-bit formats
            case PixelFormat.R32Uint: return Format.R32Uint;
            case PixelFormat.R32Sint: return Format.R32Sint;
            case PixelFormat.R32Float: return Format.R32Float;
            case PixelFormat.Rg16Unorm: return Format.R16G16Unorm;
            case PixelFormat.Rg16Snorm: return Format.R16G16Snorm;
            case PixelFormat.Rg16Uint: return Format.R16G16Uint;
            case PixelFormat.Rg16Sint: return Format.R16G16Sint;
            case PixelFormat.Rg16Float: return Format.R16G16Float;
            case PixelFormat.Rgba8Unorm: return Format.R8G8B8A8Unorm;
            case PixelFormat.Rgba8UnormSrgb: return Format.R8G8B8A8UnormSrgb;
            case PixelFormat.Rgba8Snorm: return Format.R8G8B8A8Snorm;
            case PixelFormat.Rgba8Uint: return Format.R8G8B8A8Uint;
            case PixelFormat.Rgba8Sint: return Format.R8G8B8A8Sint;
            case PixelFormat.Bgra8Unorm: return Format.B8G8R8A8Unorm;
            case PixelFormat.Bgra8UnormSrgb: return Format.B8G8R8A8UnormSrgb;
            // Packed 32-Bit formats
            case PixelFormat.Rgb9e5Ufloat: return Format.R9G9B9E5SharedExp;
            case PixelFormat.Rgb10a2Unorm: return Format.R10G10B10A2Unorm;
            case PixelFormat.Rgb10a2Uint: return Format.R10G10B10A2Uint;
            case PixelFormat.Rg11b10Float: return Format.R11G11B10Float;
            // 64-Bit formats
            case PixelFormat.Rg32Uint: return Format.R32G32Uint;
            case PixelFormat.Rg32Sint: return Format.R32G32Sint;
            case PixelFormat.Rg32Float: return Format.R32G32Float;
            case PixelFormat.Rgba16Unorm: return Format.R16G16B16A16Unorm;
            case PixelFormat.Rgba16Snorm: return Format.R16G16B16A16Snorm;
            case PixelFormat.Rgba16Uint: return Format.R16G16B16A16Uint;
            case PixelFormat.Rgba16Sint: return Format.R16G16B16A16Sint;
            case PixelFormat.Rgba16Float: return Format.R16G16B16A16Float;
            // 128-Bit formats
            case PixelFormat.Rgba32Uint: return Format.R32G32B32A32Uint;
            case PixelFormat.Rgba32Sint: return Format.R32G32B32A32Sint;
            case PixelFormat.Rgba32Float: return Format.R32G32B32A32Float;
            // Depth-stencil formats
            case PixelFormat.Stencil8: return Format.D24UnormS8Uint;
            case PixelFormat.Depth16Unorm: return Format.D16Unorm;
            case PixelFormat.Depth32Float: return Format.D32Float;
            case PixelFormat.Depth24UnormStencil8: return Format.D24UnormS8Uint;
            case PixelFormat.Depth32FloatStencil8: return Format.D32FloatS8X24Uint;
            // Compressed BC formats
            case PixelFormat.Bc1RgbaUnorm: return Format.BC1Unorm;
            case PixelFormat.Bc1RgbaUnormSrgb: return Format.BC1UnormSrgb;
            case PixelFormat.Bc2RgbaUnorm: return Format.BC2Unorm;
            case PixelFormat.Bc2RgbaUnormSrgb: return Format.BC2UnormSrgb;
            case PixelFormat.Bc3RgbaUnorm: return Format.BC3Unorm;
            case PixelFormat.Bc3RgbaUnormSrgb: return Format.BC3UnormSrgb;
            case PixelFormat.Bc4RSnorm: return Format.BC4Unorm;
            case PixelFormat.Bc4RUnorm: return Format.BC4Snorm;
            case PixelFormat.Bc5RgUnorm: return Format.BC5Unorm;
            case PixelFormat.Bc5RgSnorm: return Format.BC5Snorm;
            case PixelFormat.Bc6hRgbSfloat: return Format.BC6HSF16;
            case PixelFormat.Bc6hRgbUfloat: return Format.BC6HUF16;
            case PixelFormat.Bc7RgbaUnorm: return Format.BC7Unorm;
            case PixelFormat.Bc7RgbaUnormSrgb: return Format.BC7UnormSrgb;

            default:
                return Format.Unknown;
        }
    }

    public static PixelFormat FromDxgiFormat(this Format format)
    {
        switch (format)
        {
            // 8-bit formats
            case Format.R8Unorm: return PixelFormat.R8Unorm;
            case Format.R8Snorm: return PixelFormat.R8Snorm;
            case Format.R8Uint: return PixelFormat.R8Uint;
            case Format.R8Sint: return PixelFormat.R8Sint;
            // 16-bit formats
            case Format.R16Unorm: return PixelFormat.R16Unorm;
            case Format.R16Snorm: return PixelFormat.R16Snorm;
            case Format.R16Uint: return PixelFormat.R16Uint;
            case Format.R16Sint: return PixelFormat.R16Sint;
            case Format.R16Float: return PixelFormat.R16Float;
            case Format.R8G8Unorm: return PixelFormat.Rg8Unorm;
            case Format.R8G8Snorm: return PixelFormat.Rg8Snorm;
            case Format.R8G8Uint: return PixelFormat.Rg8Uint;
            case Format.R8G8Sint: return PixelFormat.Rg8Sint;
            // Packed 16-Bit Pixel Formats
            case Format.B4G4R4A4Unorm: return PixelFormat.Bgra4Unorm;
            case Format.B5G6R5Unorm: return PixelFormat.B5G6R5Unorm;
            case Format.B5G5R5A1Unorm: return PixelFormat.Bgr5A1Unorm;
            // 32-bit formats
            case Format.R32Uint: return PixelFormat.R32Uint;
            case Format.R32Sint: return PixelFormat.R32Sint;
            case Format.R32Float: return PixelFormat.R32Float;
            case Format.R16G16Unorm: return PixelFormat.Rg16Unorm;
            case Format.R16G16Snorm: return PixelFormat.Rg16Snorm;
            case Format.R16G16Uint: return PixelFormat.Rg16Uint;
            case Format.R16G16Sint: return PixelFormat.Rg16Sint;
            case Format.R16G16Float: return PixelFormat.Rg16Float;
            case Format.R8G8B8A8Unorm: return PixelFormat.Rgba8Unorm;
            case Format.R8G8B8A8UnormSrgb: return PixelFormat.Rgba8UnormSrgb;
            case Format.R8G8B8A8Snorm: return PixelFormat.Rgba8Snorm;
            case Format.R8G8B8A8Uint: return PixelFormat.Rgba8Uint;
            case Format.R8G8B8A8Sint: return PixelFormat.Rgba8Sint;
            case Format.B8G8R8A8Unorm: return PixelFormat.Bgra8Unorm;
            case Format.B8G8R8A8UnormSrgb: return PixelFormat.Bgra8UnormSrgb;
            // Packed 32-Bit formats
            case Format.R9G9B9E5SharedExp: return PixelFormat.Rgb9e5Ufloat;
            case Format.R10G10B10A2Unorm: return PixelFormat.Rgb10a2Unorm;
            case Format.R10G10B10A2Uint: return PixelFormat.Rgb10a2Uint;
            case Format.R11G11B10Float: return PixelFormat.Rg11b10Float;
            // 64-Bit formats
            case Format.R32G32Uint: return PixelFormat.Rg32Uint;
            case Format.R32G32Sint: return PixelFormat.Rg32Sint;
            case Format.R32G32Float: return PixelFormat.Rg32Float;
            case Format.R16G16B16A16Unorm: return PixelFormat.Rgba16Unorm;
            case Format.R16G16B16A16Snorm: return PixelFormat.Rgba16Snorm;
            case Format.R16G16B16A16Uint: return PixelFormat.Rgba16Uint;
            case Format.R16G16B16A16Sint: return PixelFormat.Rgba16Sint;
            case Format.R16G16B16A16Float: return PixelFormat.Rgba16Float;
            // 128-Bit formats
            case Format.R32G32B32A32Uint: return PixelFormat.Rgba32Uint;
            case Format.R32G32B32A32Sint: return PixelFormat.Rgba32Sint;
            case Format.R32G32B32A32Float: return PixelFormat.Rgba32Float;
            // Depth-stencil formats
            case Format.D16Unorm: return PixelFormat.Depth16Unorm;
            case Format.D32Float: return PixelFormat.Depth32Float;
            //case Format.D24UnormS8Uint: return PixelFormat.Stencil8;
            case Format.D24UnormS8Uint: return PixelFormat.Depth24UnormStencil8;
            case Format.D32FloatS8X24Uint: return PixelFormat.Depth32FloatStencil8;
            // Compressed BC formats
            case Format.BC1Unorm: return PixelFormat.Bc1RgbaUnorm;
            case Format.BC1UnormSrgb: return PixelFormat.Bc1RgbaUnormSrgb;
            case Format.BC2Unorm: return PixelFormat.Bc2RgbaUnorm;
            case Format.BC2UnormSrgb: return PixelFormat.Bc2RgbaUnormSrgb;
            case Format.BC3Unorm: return PixelFormat.Bc3RgbaUnorm;
            case Format.BC3UnormSrgb: return PixelFormat.Bc3RgbaUnormSrgb;
            case Format.BC4Unorm: return PixelFormat.Bc4RSnorm;
            case Format.BC4Snorm: return PixelFormat.Bc4RUnorm;
            case Format.BC5Unorm: return PixelFormat.Bc5RgUnorm;
            case Format.BC5Snorm: return PixelFormat.Bc5RgSnorm;
            case Format.BC6HSF16: return PixelFormat.Bc6hRgbSfloat;
            case Format.BC6HUF16: return PixelFormat.Bc6hRgbUfloat;
            case Format.BC7Unorm: return PixelFormat.Bc7RgbaUnorm;
            case Format.BC7UnormSrgb: return PixelFormat.Bc7RgbaUnormSrgb;

            default:
                return PixelFormat.Undefined;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Format GetTypelessFormatFromDepthFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.Stencil8:
                return Format.R24G8Typeless;
            case PixelFormat.Depth16Unorm:
                return Format.R16Typeless;

            case PixelFormat.Depth32Float:
                return Format.R32Typeless;

            case PixelFormat.Depth24UnormStencil8:
                return Format.R24G8Typeless;
            case PixelFormat.Depth32FloatStencil8:
                return Format.R32G8X24Typeless;

            default:
                Guard.IsFalse(format.IsDepthFormat(), nameof(format));
                return ToDxgiFormat(format);
        }
    }

    public static Format ToDxgiFormat(this VertexFormat format)
    {
        switch (format)
        {
            case VertexFormat.UByte2: return Format.R8G8Uint;
            case VertexFormat.UByte4: return Format.R8G8B8A8Uint;
            case VertexFormat.Byte2: return Format.R8G8Sint;
            case VertexFormat.Byte4: return Format.R8G8B8A8Sint;
            case VertexFormat.UByte2Normalized: return Format.R8G8Unorm;
            case VertexFormat.UByte4Normalized: return Format.R8G8B8A8Unorm;
            case VertexFormat.Byte2Normalized: return Format.R8G8Snorm;
            case VertexFormat.Byte4Normalized: return Format.R8G8B8A8Snorm;

            case VertexFormat.UShort2: return Format.R16G16Uint;
            case VertexFormat.UShort4: return Format.R16G16B16A16Uint;
            case VertexFormat.Short2: return Format.R16G16Sint;
            case VertexFormat.Short4: return Format.R16G16B16A16Sint;
            case VertexFormat.UShort2Normalized: return Format.R16G16Unorm;
            case VertexFormat.UShort4Normalized: return Format.R16G16B16A16Unorm;
            case VertexFormat.Short2Normalized: return Format.R16G16Snorm;
            case VertexFormat.Short4Normalized: return Format.R16G16B16A16Snorm;
            case VertexFormat.Half2: return Format.R16G16Float;
            case VertexFormat.Half4: return Format.R16G16B16A16Float;

            case VertexFormat.Float: return Format.R32Float;
            case VertexFormat.Float2: return Format.R32G32Float;
            case VertexFormat.Float3: return Format.R32G32B32Float;
            case VertexFormat.Float4: return Format.R32G32B32A32Float;

            case VertexFormat.UInt: return Format.R32Uint;
            case VertexFormat.UInt2: return Format.R32G32Uint;
            case VertexFormat.UInt3: return Format.R32G32B32Uint;
            case VertexFormat.UInt4: return Format.R32G32B32A32Uint;

            case VertexFormat.Int: return Format.R32Sint;
            case VertexFormat.Int2: return Format.R32G32Sint;
            case VertexFormat.Int3: return Format.R32G32B32Sint;
            case VertexFormat.Int4: return Format.R32G32B32A32Sint;

            case VertexFormat.Int1010102Normalized: return Format.R10G10B10A2Unorm;
            case VertexFormat.UInt1010102Normalized: return Format.R10G10B10A2Uint;

            default:
                return Format.Unknown;
        }
    }

    public static Format ToDxgiFormat(this IndexType indexType)
    {
        switch (indexType)
        {
            case IndexType.UInt16: return Format.R16Uint;
            case IndexType.UInt32: return Format.R32Uint;

            default:
                return Format.R16Uint;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ToSampleCount(TextureSampleCount sampleCount)
    {
        return sampleCount switch
        {
            TextureSampleCount.Count1 => 1,
            TextureSampleCount.Count2 => 2,
            TextureSampleCount.Count4 => 4,
            TextureSampleCount.Count8 => 8,
            TextureSampleCount.Count16 => 16,
            TextureSampleCount.Count32 => 32,
            TextureSampleCount.Count64 => 64,
            _ => 1,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TextureSampleCount FromSampleCount(uint sampleCount)
    {
        return sampleCount switch
        {
            1 => TextureSampleCount.Count1,
            2 => TextureSampleCount.Count2,
            4 => TextureSampleCount.Count4,
            8 => TextureSampleCount.Count8,
            16 => TextureSampleCount.Count16,
            32 => TextureSampleCount.Count32,
            64 => TextureSampleCount.Count64,
            _ => TextureSampleCount.Count1,
        };
    }

    public static GpuPreference ToDxgi(this PowerPreference preference)
    {
        switch (preference)
        {
            case PowerPreference.LowPower:
                return GpuPreference.MinimumPower;

            default:
            case PowerPreference.HighPerformance:
                return GpuPreference.HighPerformance;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static D3DPrimitiveTopology ToD3DPrimitiveTopology(this PrimitiveTopology value) => s_d3dPrimitiveTopologyMap[(uint)value];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint PresentModeToBufferCount(PresentMode mode)
    {
        return mode switch
        {
            PresentMode.Immediate or PresentMode.Fifo => 2,
            PresentMode.Mailbox => 3,
            _ => ThrowHelper.ThrowArgumentException<uint>("Invalid present mode"),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint PresentModeToSwapInterval(PresentMode mode)
    {
        switch (mode)
        {
            case PresentMode.Immediate:
            case PresentMode.Mailbox:
                return 0;
            case PresentMode.Fifo:
                return 1;
            default:
                return ThrowHelper.ThrowArgumentException<uint>("Invalid present mode");
        }
    }
}
