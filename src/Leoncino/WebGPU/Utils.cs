// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using WebGPU;

namespace Leoncino.WebGPU;

internal static unsafe class Utils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUTextureFormat ToWebGPU(this PixelFormat format)
    {
        switch (format)
        {
            // 8-bit formats
            case PixelFormat.R8Unorm: return WGPUTextureFormat.R8Unorm;
            case PixelFormat.R8Snorm: return WGPUTextureFormat.R8Snorm;
            case PixelFormat.R8Uint: return WGPUTextureFormat.R8Uint;
            case PixelFormat.R8Sint: return WGPUTextureFormat.R8Sint;
            // 16-bit formats
            //case PixelFormat.R16Unorm: return WGPUTextureFormat.R16Unorm;
            //case PixelFormat.R16Snorm: return WGPUTextureFormat.R16Snorm;
            case PixelFormat.R16Uint: return WGPUTextureFormat.R16Uint;
            case PixelFormat.R16Sint: return WGPUTextureFormat.R16Sint;
            case PixelFormat.R16Float: return WGPUTextureFormat.R16Float;
            case PixelFormat.RG8Unorm: return WGPUTextureFormat.RG8Unorm;
            case PixelFormat.RG8Snorm: return WGPUTextureFormat.RG8Snorm;
            case PixelFormat.RG8Uint: return WGPUTextureFormat.RG8Uint;
            case PixelFormat.RG8Sint: return WGPUTextureFormat.RG8Sint;
            // Packed 16-Bit Pixel Formats
            //case PixelFormat.Bgra4Unorm: return WGPUTextureFormat.Bgra4Unorm;
            //case PixelFormat.B5G6R5Unorm: return WGPUTextureFormat.B5G6R5UnormPack16;
            //case PixelFormat.Bgr5A1Unorm: return WGPUTextureFormat.B5G5R5A1UnormPack16;
            // 32-bit formats
            case PixelFormat.R32Uint: return WGPUTextureFormat.R32Uint;
            case PixelFormat.R32Sint: return WGPUTextureFormat.R32Sint;
            case PixelFormat.R32Float: return WGPUTextureFormat.R32Float;
            //case PixelFormat.Rg16Unorm: return WGPUTextureFormat.Rg16Unorm;
            //case PixelFormat.Rg16Snorm: return WGPUTextureFormat.Rg16Snorm;
            case PixelFormat.RG16Uint: return WGPUTextureFormat.RG16Uint;
            case PixelFormat.RG16Sint: return WGPUTextureFormat.RG16Sint;
            case PixelFormat.RG16Float: return WGPUTextureFormat.RG16Float;
            case PixelFormat.RGBA8Unorm: return WGPUTextureFormat.RGBA8Unorm;
            case PixelFormat.RGBA8UnormSrgb: return WGPUTextureFormat.RGBA8UnormSrgb;
            case PixelFormat.RGBA8Snorm: return WGPUTextureFormat.RGBA8Snorm;
            case PixelFormat.RGBA8Uint: return WGPUTextureFormat.RGBA8Uint;
            case PixelFormat.RGBA8Sint: return WGPUTextureFormat.RGBA8Sint;
            case PixelFormat.BGRA8Unorm: return WGPUTextureFormat.BGRA8Unorm;
            case PixelFormat.BGRA8UnormSrgb: return WGPUTextureFormat.BGRA8UnormSrgb;
            // Packed 32-Bit formats
            case PixelFormat.RGB10A2Unorm: return WGPUTextureFormat.RGB10A2Unorm;
            case PixelFormat.RGB10A2Uint: return WGPUTextureFormat.RGB10A2Uint;
            case PixelFormat.RG11B10Float: return WGPUTextureFormat.RG11B10Ufloat;
            case PixelFormat.RGB9E5Float: return WGPUTextureFormat.RGB9E5Ufloat;
            // 64-Bit formats
            case PixelFormat.RG32Uint: return WGPUTextureFormat.RG32Uint;
            case PixelFormat.RG32Sint: return WGPUTextureFormat.RG32Sint;
            case PixelFormat.RG32Float: return WGPUTextureFormat.RG32Float;
            //case PixelFormat.Rgba16Unorm: return WGPUTextureFormat.Rgba16Unorm;
            //case PixelFormat.Rgba16Snorm: return WGPUTextureFormat.Rgba16Snorm;
            case PixelFormat.RGBA16Uint: return WGPUTextureFormat.RGBA16Uint;
            case PixelFormat.RGBA16Sint: return WGPUTextureFormat.RGBA16Sint;
            case PixelFormat.RGBA16Float: return WGPUTextureFormat.RGBA16Float;
            // 128-Bit formats
            case PixelFormat.RGBA32Uint:
                return WGPUTextureFormat.RGBA32Uint;
            case PixelFormat.RGBA32Sint:
                return WGPUTextureFormat.RGBA32Sint;
            case PixelFormat.RGBA32Float:
                return WGPUTextureFormat.RGBA32Float;

            // Depth-stencil formats
            //case PixelFormat.Stencil8:
            //    return VkFormat.S8Uint;

            case PixelFormat.Depth16Unorm:
                return WGPUTextureFormat.Depth16Unorm;

            case PixelFormat.Depth24UnormStencil8:
                return WGPUTextureFormat.Depth24PlusStencil8;

            case PixelFormat.Depth32Float:
                return WGPUTextureFormat.Depth32Float;

            case PixelFormat.Depth32FloatStencil8:
                return WGPUTextureFormat.Depth32FloatStencil8;

            // Compressed BC formats
            case PixelFormat.BC1RGBAUnorm:
                return WGPUTextureFormat.BC1RGBAUnorm;
            case PixelFormat.BC1RGBAUnormSrgb:
                return WGPUTextureFormat.BC1RGBAUnormSrgb;
            case PixelFormat.BC2RGBAUnorm:
                return WGPUTextureFormat.BC2RGBAUnorm;
            case PixelFormat.BC2RGBAUnormSrgb:
                return WGPUTextureFormat.BC2RGBAUnormSrgb;
            case PixelFormat.BC3RGBAUnorm:
                return WGPUTextureFormat.BC3RGBAUnorm;
            case PixelFormat.BC3RGBAUnormSrgb:
                return WGPUTextureFormat.BC3RGBAUnormSrgb;
            case PixelFormat.BC4RSnorm:
                return WGPUTextureFormat.BC4RSnorm;
            case PixelFormat.BC4RUnorm:
                return WGPUTextureFormat.BC4RUnorm;
            case PixelFormat.BC5RGUnorm:
                return WGPUTextureFormat.BC5RGUnorm;
            case PixelFormat.BC5RGSnorm:
                return WGPUTextureFormat.BC5RGSnorm;
            case PixelFormat.BC6HRGBUfloat:
                return WGPUTextureFormat.BC6HRGBUfloat;
            case PixelFormat.BC6HRGBFloat:
                return WGPUTextureFormat.BC6HRGBFloat;
            case PixelFormat.BC7RGBAUnorm:
                return WGPUTextureFormat.BC7RGBAUnorm;
            case PixelFormat.BC7RGBAUnormSrgb:
                return WGPUTextureFormat.BC7RGBAUnormSrgb;

            // Etc2/Eac compressed formats
            case PixelFormat.ETC2RGB8Unorm:
                return WGPUTextureFormat.ETC2RGB8Unorm;
            case PixelFormat.ETC2RGB8UnormSrgb:
                return WGPUTextureFormat.ETC2RGB8UnormSrgb;
            case PixelFormat.ETC2RGB8A1Unorm:
                return WGPUTextureFormat.ETC2RGB8A1Unorm;
            case PixelFormat.ETC2RGB8A1UnormSrgb:
                return WGPUTextureFormat.ETC2RGB8A1UnormSrgb;
            case PixelFormat.ETC2RGBA8Unorm:
                return WGPUTextureFormat.ETC2RGBA8Unorm;
            case PixelFormat.ETC2RGBA8UnormSrgb:
                return WGPUTextureFormat.ETC2RGBA8UnormSrgb;
            case PixelFormat.EACR11Unorm:
                return WGPUTextureFormat.EACR11Unorm;
            case PixelFormat.EACR11Snorm:
                return WGPUTextureFormat.EACR11Snorm;
            case PixelFormat.EACRG11Unorm:
                return WGPUTextureFormat.EACRG11Unorm;
            case PixelFormat.EACRG11Snorm:
                return WGPUTextureFormat.EACRG11Snorm;

            default:
                return WGPUTextureFormat.Undefined;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUVertexFormat ToWebGPU(this VertexFormat format)
    {
        switch (format)
        {
            case VertexFormat.UByte2: return WGPUVertexFormat.Uint8x2;
            case VertexFormat.UByte4: return WGPUVertexFormat.Uint8x4;
            case VertexFormat.Byte2: return WGPUVertexFormat.Sint8x2;
            case VertexFormat.Byte4: return WGPUVertexFormat.Sint8x4;
            case VertexFormat.UByte2Normalized: return WGPUVertexFormat.Unorm8x2;
            case VertexFormat.UByte4Normalized: return WGPUVertexFormat.Unorm8x4;
            case VertexFormat.Byte2Normalized: return WGPUVertexFormat.Snorm8x2;
            case VertexFormat.Byte4Normalized: return WGPUVertexFormat.Snorm8x4;

            case VertexFormat.UShort2: return WGPUVertexFormat.Uint16x2;
            case VertexFormat.UShort4: return WGPUVertexFormat.Uint16x4;
            case VertexFormat.Short2: return WGPUVertexFormat.Sint16x2;
            case VertexFormat.Short4: return WGPUVertexFormat.Sint16x4;
            case VertexFormat.UShort2Normalized: return WGPUVertexFormat.Unorm16x2;
            case VertexFormat.UShort4Normalized: return WGPUVertexFormat.Unorm16x4;
            case VertexFormat.Short2Normalized: return WGPUVertexFormat.Snorm16x2;
            case VertexFormat.Short4Normalized: return WGPUVertexFormat.Snorm16x4;
            case VertexFormat.Half2: return WGPUVertexFormat.Float16x2;
            case VertexFormat.Half4: return WGPUVertexFormat.Float16x4;

            case VertexFormat.Float: return WGPUVertexFormat.Float32;
            case VertexFormat.Float2: return WGPUVertexFormat.Float32x2;
            case VertexFormat.Float3: return WGPUVertexFormat.Float32x3;
            case VertexFormat.Float4: return WGPUVertexFormat.Float32x4;

            case VertexFormat.UInt: return WGPUVertexFormat.Uint32;
            case VertexFormat.UInt2: return WGPUVertexFormat.Uint32x2;
            case VertexFormat.UInt3: return WGPUVertexFormat.Uint32x3;
            case VertexFormat.UInt4: return WGPUVertexFormat.Uint32x4;

            case VertexFormat.Int: return WGPUVertexFormat.Sint32;
            case VertexFormat.Int2: return WGPUVertexFormat.Sint32x2;
            case VertexFormat.Int3: return WGPUVertexFormat.Sint32x3;
            case VertexFormat.Int4: return WGPUVertexFormat.Sint32x4;

            //case VertexFormat.Int1010102Normalized: return WGPUVertexFormat.A2B10G10R10SnormPack32;
            //case VertexFormat.UInt1010102Normalized: return WGPUVertexFormat.A2B10G10R10UnormPack32;

            default:
                return WGPUVertexFormat.Undefined;
        }
    }
}
