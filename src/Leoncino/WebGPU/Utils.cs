// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using WebGPU;

namespace Leoncino.WebGPU;

internal static unsafe class Utils
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUInstanceFlags ToWGPU(this ValidationMode value)
    {
        switch (value)
        {
            case ValidationMode.Disabled: return WGPUInstanceFlags.None;
            default:
                return WGPUInstanceFlags.Validation;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUPowerPreference ToWGPU(this PowerPreference value)
    {
        switch (value)
        {
            case PowerPreference.LowPower: return WGPUPowerPreference.LowPower;
            case PowerPreference.HighPerformance: return WGPUPowerPreference.HighPerformance;
            default:
                return WGPUPowerPreference.Undefined;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUPresentMode ToWGPU(this PresentMode value)
    {
        switch (value)
        {
            case PresentMode.Fifo: return WGPUPresentMode.Fifo;
            case PresentMode.FifoRelaxed: return WGPUPresentMode.FifoRelaxed;
            case PresentMode.Immediate: return WGPUPresentMode.Immediate;
            case PresentMode.Mailbox: return WGPUPresentMode.Mailbox;

            default:
                return WGPUPresentMode.Fifo;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUTextureFormat ToWGPU(this PixelFormat format)
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
            case PixelFormat.RG11B10Ufloat: return WGPUTextureFormat.RG11B10Ufloat;
            case PixelFormat.RGB9E5Ufloat: return WGPUTextureFormat.RGB9E5Ufloat;
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


            case PixelFormat.ASTC4x4Unorm: return WGPUTextureFormat.ASTC4x4Unorm;
            case PixelFormat.ASTC4x4UnormSrgb: return WGPUTextureFormat.ASTC4x4UnormSrgb;
            case PixelFormat.ASTC5x4Unorm: return WGPUTextureFormat.ASTC5x4Unorm;
            case PixelFormat.ASTC5x4UnormSrgb: return WGPUTextureFormat.ASTC5x4UnormSrgb;
            case PixelFormat.ASTC5x5Unorm: return WGPUTextureFormat.ASTC5x5Unorm;
            case PixelFormat.ASTC5x5UnormSrgb: return WGPUTextureFormat.ASTC5x5UnormSrgb;
            case PixelFormat.ASTC6x5Unorm: return WGPUTextureFormat.ASTC6x5Unorm;
            case PixelFormat.ASTC6x5UnormSrgb: return WGPUTextureFormat.ASTC6x5UnormSrgb;
            case PixelFormat.ASTC6x6Unorm: return WGPUTextureFormat.ASTC6x6Unorm;
            case PixelFormat.ASTC6x6UnormSrgb: return WGPUTextureFormat.ASTC6x6UnormSrgb;
            case PixelFormat.ASTC8x5Unorm: return WGPUTextureFormat.ASTC8x5Unorm;
            case PixelFormat.ASTC8x5UnormSrgb: return WGPUTextureFormat.ASTC8x5UnormSrgb;
            case PixelFormat.ASTC8x6Unorm: return WGPUTextureFormat.ASTC8x6Unorm;
            case PixelFormat.ASTC8x6UnormSrgb: return WGPUTextureFormat.ASTC8x6UnormSrgb;
            case PixelFormat.ASTC8x8Unorm: return WGPUTextureFormat.ASTC8x8Unorm;
            case PixelFormat.ASTC8x8UnormSrgb: return WGPUTextureFormat.ASTC8x8UnormSrgb;
            case PixelFormat.ASTC10x5Unorm: return WGPUTextureFormat.ASTC10x5Unorm;
            case PixelFormat.ASTC10x5UnormSrgb: return WGPUTextureFormat.ASTC10x5UnormSrgb;
            case PixelFormat.ASTC10x6Unorm: return WGPUTextureFormat.ASTC10x6Unorm;
            case PixelFormat.ASTC10x6UnormSrgb: return WGPUTextureFormat.ASTC10x6UnormSrgb;
            case PixelFormat.ASTC10x8Unorm: return WGPUTextureFormat.ASTC10x8Unorm;
            case PixelFormat.ASTC10x8UnormSrgb: return WGPUTextureFormat.ASTC10x8UnormSrgb;
            case PixelFormat.ASTC10x10Unorm: return WGPUTextureFormat.ASTC10x10Unorm;
            case PixelFormat.ASTC10x10UnormSrgb: return WGPUTextureFormat.ASTC10x10UnormSrgb;
            case PixelFormat.ASTC12x10Unorm: return WGPUTextureFormat.ASTC12x10Unorm;
            case PixelFormat.ASTC12x10UnormSrgb: return WGPUTextureFormat.ASTC12x10UnormSrgb;
            case PixelFormat.ASTC12x12Unorm: return WGPUTextureFormat.ASTC12x12Unorm;
            case PixelFormat.ASTC12x12UnormSrgb: return WGPUTextureFormat.ASTC12x12UnormSrgb;

            default:
                return WGPUTextureFormat.Undefined;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PixelFormat ToLeoncino(this WGPUTextureFormat format)
    {
        return format switch
        {
            // 8-bit formats
            WGPUTextureFormat.R8Unorm => PixelFormat.R8Unorm,
            WGPUTextureFormat.R8Snorm => PixelFormat.R8Snorm,
            WGPUTextureFormat.R8Uint => PixelFormat.R8Uint,
            WGPUTextureFormat.R8Sint => PixelFormat.R8Sint,
            // 16-bit formats
            //case PixelFormat.R16Unorm: return WGPUTextureFormat.R16Unorm;
            //case PixelFormat.R16Snorm: return WGPUTextureFormat.R16Snorm;
            WGPUTextureFormat.R16Uint => PixelFormat.R16Uint,
            WGPUTextureFormat.R16Sint => PixelFormat.R16Sint,
            WGPUTextureFormat.R16Float => PixelFormat.R16Float,
            WGPUTextureFormat.RG8Unorm => PixelFormat.RG8Unorm,
            WGPUTextureFormat.RG8Snorm => PixelFormat.RG8Snorm,
            WGPUTextureFormat.RG8Uint => PixelFormat.RG8Uint,
            WGPUTextureFormat.RG8Sint => PixelFormat.RG8Sint,
            // Packed 16-Bit Pixel Formats
            //case PixelFormat.Bgra4Unorm: return WGPUTextureFormat.Bgra4Unorm;
            //case PixelFormat.B5G6R5Unorm: return WGPUTextureFormat.B5G6R5UnormPack16;
            //case PixelFormat.Bgr5A1Unorm: return WGPUTextureFormat.B5G5R5A1UnormPack16;
            // 32-bit formats
            WGPUTextureFormat.R32Uint => PixelFormat.R32Uint,
            WGPUTextureFormat.R32Sint => PixelFormat.R32Sint,
            WGPUTextureFormat.R32Float => PixelFormat.R32Float,
            //case PixelFormat.Rg16Unorm: return WGPUTextureFormat.Rg16Unorm;
            //case PixelFormat.Rg16Snorm: return WGPUTextureFormat.Rg16Snorm;
            WGPUTextureFormat.RG16Uint => PixelFormat.RG16Uint,
            WGPUTextureFormat.RG16Sint => PixelFormat.RG16Sint,
            WGPUTextureFormat.RG16Float => PixelFormat.RG16Float,
            WGPUTextureFormat.RGBA8Unorm => PixelFormat.RGBA8Unorm,
            WGPUTextureFormat.RGBA8UnormSrgb => PixelFormat.RGBA8UnormSrgb,
            WGPUTextureFormat.RGBA8Snorm => PixelFormat.RGBA8Snorm,
            WGPUTextureFormat.RGBA8Uint => PixelFormat.RGBA8Uint,
            WGPUTextureFormat.RGBA8Sint => PixelFormat.RGBA8Sint,
            WGPUTextureFormat.BGRA8Unorm => PixelFormat.BGRA8Unorm,
            WGPUTextureFormat.BGRA8UnormSrgb => PixelFormat.BGRA8UnormSrgb,
            // Packed 32-Bit formats
            WGPUTextureFormat.RGB10A2Unorm => PixelFormat.RGB10A2Unorm,
            WGPUTextureFormat.RGB10A2Uint => PixelFormat.RGB10A2Uint,
            WGPUTextureFormat.RG11B10Ufloat => PixelFormat.RG11B10Ufloat,
            WGPUTextureFormat.RGB9E5Ufloat => PixelFormat.RGB9E5Ufloat,
            // 64-Bit formats
            WGPUTextureFormat.RG32Uint => PixelFormat.RG32Uint,
            WGPUTextureFormat.RG32Sint => PixelFormat.RG32Sint,
            WGPUTextureFormat.RG32Float => PixelFormat.RG32Float,
            //case PixelFormat.Rgba16Unorm: return WGPUTextureFormat.Rgba16Unorm;
            //case PixelFormat.Rgba16Snorm: return WGPUTextureFormat.Rgba16Snorm;
            WGPUTextureFormat.RGBA16Uint => PixelFormat.RGBA16Uint,
            WGPUTextureFormat.RGBA16Sint => PixelFormat.RGBA16Sint,
            WGPUTextureFormat.RGBA16Float => PixelFormat.RGBA16Float,
            // 128-Bit formats
            WGPUTextureFormat.RGBA32Uint => PixelFormat.RGBA32Uint,
            WGPUTextureFormat.RGBA32Sint => PixelFormat.RGBA32Sint,
            WGPUTextureFormat.RGBA32Float => PixelFormat.RGBA32Float,
            // Depth-stencil formats
            WGPUTextureFormat.Stencil8 => PixelFormat.Undefined,
            WGPUTextureFormat.Depth16Unorm => PixelFormat.Depth16Unorm,
            WGPUTextureFormat.Depth24Plus or WGPUTextureFormat.Depth24PlusStencil8 => PixelFormat.Depth24UnormStencil8,
            WGPUTextureFormat.Depth32Float => PixelFormat.Depth32Float,
            WGPUTextureFormat.Depth32FloatStencil8 => PixelFormat.Depth32FloatStencil8,
            // Compressed BC formats
            WGPUTextureFormat.BC1RGBAUnorm => PixelFormat.BC1RGBAUnorm,
            WGPUTextureFormat.BC1RGBAUnormSrgb => PixelFormat.BC1RGBAUnormSrgb,
            WGPUTextureFormat.BC2RGBAUnorm => PixelFormat.BC2RGBAUnorm,
            WGPUTextureFormat.BC2RGBAUnormSrgb => PixelFormat.BC2RGBAUnormSrgb,
            WGPUTextureFormat.BC3RGBAUnorm => PixelFormat.BC3RGBAUnorm,
            WGPUTextureFormat.BC3RGBAUnormSrgb => PixelFormat.BC3RGBAUnormSrgb,
            WGPUTextureFormat.BC4RSnorm => PixelFormat.BC4RSnorm,
            WGPUTextureFormat.BC4RUnorm => PixelFormat.BC4RUnorm,
            WGPUTextureFormat.BC5RGUnorm => PixelFormat.BC5RGUnorm,
            WGPUTextureFormat.BC5RGSnorm => PixelFormat.BC5RGSnorm,
            WGPUTextureFormat.BC6HRGBUfloat => PixelFormat.BC6HRGBUfloat,
            WGPUTextureFormat.BC6HRGBFloat => PixelFormat.BC6HRGBFloat,
            WGPUTextureFormat.BC7RGBAUnorm => PixelFormat.BC7RGBAUnorm,
            WGPUTextureFormat.BC7RGBAUnormSrgb => PixelFormat.BC7RGBAUnormSrgb,
            // Etc2/Eac compressed formats
            WGPUTextureFormat.ETC2RGB8Unorm => PixelFormat.ETC2RGB8Unorm,
            WGPUTextureFormat.ETC2RGB8UnormSrgb => PixelFormat.ETC2RGB8UnormSrgb,
            WGPUTextureFormat.ETC2RGB8A1Unorm => PixelFormat.ETC2RGB8A1Unorm,
            WGPUTextureFormat.ETC2RGB8A1UnormSrgb => PixelFormat.ETC2RGB8A1UnormSrgb,
            WGPUTextureFormat.ETC2RGBA8Unorm => PixelFormat.ETC2RGBA8Unorm,
            WGPUTextureFormat.ETC2RGBA8UnormSrgb => PixelFormat.ETC2RGBA8UnormSrgb,
            WGPUTextureFormat.EACR11Unorm => PixelFormat.EACR11Unorm,
            WGPUTextureFormat.EACR11Snorm => PixelFormat.EACR11Snorm,
            WGPUTextureFormat.EACRG11Unorm => PixelFormat.EACRG11Unorm,
            WGPUTextureFormat.EACRG11Snorm => PixelFormat.EACRG11Snorm,
            WGPUTextureFormat.ASTC4x4Unorm => PixelFormat.ASTC4x4Unorm,
            WGPUTextureFormat.ASTC4x4UnormSrgb => PixelFormat.ASTC4x4UnormSrgb,
            WGPUTextureFormat.ASTC5x4Unorm => PixelFormat.ASTC5x4Unorm,
            WGPUTextureFormat.ASTC5x4UnormSrgb => PixelFormat.ASTC5x4UnormSrgb,
            WGPUTextureFormat.ASTC5x5Unorm => PixelFormat.ASTC5x5Unorm,
            WGPUTextureFormat.ASTC5x5UnormSrgb => PixelFormat.ASTC5x5UnormSrgb,
            WGPUTextureFormat.ASTC6x5Unorm => PixelFormat.ASTC6x5Unorm,
            WGPUTextureFormat.ASTC6x5UnormSrgb => PixelFormat.ASTC6x5UnormSrgb,
            WGPUTextureFormat.ASTC6x6Unorm => PixelFormat.ASTC6x6Unorm,
            WGPUTextureFormat.ASTC6x6UnormSrgb => PixelFormat.ASTC6x6UnormSrgb,
            WGPUTextureFormat.ASTC8x5Unorm => PixelFormat.ASTC8x5Unorm,
            WGPUTextureFormat.ASTC8x5UnormSrgb => PixelFormat.ASTC8x5UnormSrgb,
            WGPUTextureFormat.ASTC8x6Unorm => PixelFormat.ASTC8x6Unorm,
            WGPUTextureFormat.ASTC8x6UnormSrgb => PixelFormat.ASTC8x6UnormSrgb,
            WGPUTextureFormat.ASTC8x8Unorm => PixelFormat.ASTC8x8Unorm,
            WGPUTextureFormat.ASTC8x8UnormSrgb => PixelFormat.ASTC8x8UnormSrgb,
            WGPUTextureFormat.ASTC10x5Unorm => PixelFormat.ASTC10x5Unorm,
            WGPUTextureFormat.ASTC10x5UnormSrgb => PixelFormat.ASTC10x5UnormSrgb,
            WGPUTextureFormat.ASTC10x6Unorm => PixelFormat.ASTC10x6Unorm,
            WGPUTextureFormat.ASTC10x6UnormSrgb => PixelFormat.ASTC10x6UnormSrgb,
            WGPUTextureFormat.ASTC10x8Unorm => PixelFormat.ASTC10x8Unorm,
            WGPUTextureFormat.ASTC10x8UnormSrgb => PixelFormat.ASTC10x8UnormSrgb,
            WGPUTextureFormat.ASTC10x10Unorm => PixelFormat.ASTC10x10Unorm,
            WGPUTextureFormat.ASTC10x10UnormSrgb => PixelFormat.ASTC10x10UnormSrgb,
            WGPUTextureFormat.ASTC12x10Unorm => PixelFormat.ASTC12x10Unorm,
            WGPUTextureFormat.ASTC12x10UnormSrgb => PixelFormat.ASTC12x10UnormSrgb,
            WGPUTextureFormat.ASTC12x12Unorm => PixelFormat.ASTC12x12Unorm,
            WGPUTextureFormat.ASTC12x12UnormSrgb => PixelFormat.ASTC12x12UnormSrgb,
            _ => PixelFormat.Undefined,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUVertexFormat ToWGPU(this VertexFormat format)
    {
        return format switch
        {
            VertexFormat.UByte2 => WGPUVertexFormat.Uint8x2,
            VertexFormat.UByte4 => WGPUVertexFormat.Uint8x4,
            VertexFormat.Byte2 => WGPUVertexFormat.Sint8x2,
            VertexFormat.Byte4 => WGPUVertexFormat.Sint8x4,
            VertexFormat.UByte2Normalized => WGPUVertexFormat.Unorm8x2,
            VertexFormat.UByte4Normalized => WGPUVertexFormat.Unorm8x4,
            VertexFormat.Byte2Normalized => WGPUVertexFormat.Snorm8x2,
            VertexFormat.Byte4Normalized => WGPUVertexFormat.Snorm8x4,
            VertexFormat.UShort2 => WGPUVertexFormat.Uint16x2,
            VertexFormat.UShort4 => WGPUVertexFormat.Uint16x4,
            VertexFormat.Short2 => WGPUVertexFormat.Sint16x2,
            VertexFormat.Short4 => WGPUVertexFormat.Sint16x4,
            VertexFormat.UShort2Normalized => WGPUVertexFormat.Unorm16x2,
            VertexFormat.UShort4Normalized => WGPUVertexFormat.Unorm16x4,
            VertexFormat.Short2Normalized => WGPUVertexFormat.Snorm16x2,
            VertexFormat.Short4Normalized => WGPUVertexFormat.Snorm16x4,
            VertexFormat.Half2 => WGPUVertexFormat.Float16x2,
            VertexFormat.Half4 => WGPUVertexFormat.Float16x4,
            VertexFormat.Float => WGPUVertexFormat.Float32,
            VertexFormat.Float2 => WGPUVertexFormat.Float32x2,
            VertexFormat.Float3 => WGPUVertexFormat.Float32x3,
            VertexFormat.Float4 => WGPUVertexFormat.Float32x4,
            VertexFormat.UInt => WGPUVertexFormat.Uint32,
            VertexFormat.UInt2 => WGPUVertexFormat.Uint32x2,
            VertexFormat.UInt3 => WGPUVertexFormat.Uint32x3,
            VertexFormat.UInt4 => WGPUVertexFormat.Uint32x4,
            VertexFormat.Int => WGPUVertexFormat.Sint32,
            VertexFormat.Int2 => WGPUVertexFormat.Sint32x2,
            VertexFormat.Int3 => WGPUVertexFormat.Sint32x3,
            VertexFormat.Int4 => WGPUVertexFormat.Sint32x4,
            //case VertexFormat.Int1010102Normalized: return WGPUVertexFormat.A2B10G10R10SnormPack32;
            //case VertexFormat.UInt1010102Normalized: return WGPUVertexFormat.A2B10G10R10UnormPack32;
            _ => WGPUVertexFormat.Undefined,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUCompareFunction ToWGPU(this CompareFunction value)
    {
        return value switch
        {
            CompareFunction.Never => WGPUCompareFunction.Never,
            CompareFunction.Less => WGPUCompareFunction.Less,
            CompareFunction.Equal => WGPUCompareFunction.Equal,
            CompareFunction.LessEqual => WGPUCompareFunction.LessEqual,
            CompareFunction.Greater => WGPUCompareFunction.Greater,
            CompareFunction.NotEqual => WGPUCompareFunction.NotEqual,
            CompareFunction.GreaterEqual => WGPUCompareFunction.GreaterEqual,
            CompareFunction.Always => WGPUCompareFunction.Always,
            _ => WGPUCompareFunction.Never,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUShaderStage ToWGPU(this ShaderStages stage)
    {
        if (stage == ShaderStages.All)
            return WGPUShaderStage.Vertex | WGPUShaderStage.Fragment | WGPUShaderStage.Compute;

        WGPUShaderStage result = WGPUShaderStage.None;
        if ((stage & ShaderStages.Vertex) != 0)
            result |= WGPUShaderStage.Vertex;

        if ((stage & ShaderStages.Hull) != 0)
            throw new GraphicsException($"WGPU doesn't support {ShaderStages.Hull} shader stage");

        if ((stage & ShaderStages.Domain) != 0)
            throw new GraphicsException($"WGPU doesn't support {ShaderStages.Domain} shader stage");

        if ((stage & ShaderStages.Geometry) != 0)
            throw new GraphicsException($"WGPU doesn't support {ShaderStages.Geometry} shader stage");

        if ((stage & ShaderStages.Fragment) != 0)
            result |= WGPUShaderStage.Fragment;

        if ((stage & ShaderStages.Compute) != 0)
            result |= WGPUShaderStage.Compute;

        if ((stage & ShaderStages.Amplification) != 0)
            throw new GraphicsException($"WGPU doesn't support {ShaderStages.Amplification} shader stage");

        if ((stage & ShaderStages.Mesh) != 0)
            throw new GraphicsException($"WGPU doesn't support {ShaderStages.Mesh} shader stage");

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUBufferBindingType ToWGPU(this BufferBindingType value)
    {
        return value switch
        {
            BufferBindingType.Constant => WGPUBufferBindingType.Uniform,
            BufferBindingType.Storage => WGPUBufferBindingType.Storage,
            BufferBindingType.ReadOnlyStorage => WGPUBufferBindingType.ReadOnlyStorage,
            _ => WGPUBufferBindingType.Undefined,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUSamplerBindingType ToWGPU(this SamplerBindingType value)
    {
        return value switch
        {
            SamplerBindingType.Filtering => WGPUSamplerBindingType.Filtering,
            SamplerBindingType.NonFiltering => WGPUSamplerBindingType.NonFiltering,
            SamplerBindingType.Comparison => WGPUSamplerBindingType.Comparison,
            _ => WGPUSamplerBindingType.Undefined,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUTextureSampleType ToWGPU(this TextureSampleType value)
    {
        return value switch
        {
            TextureSampleType.Float => WGPUTextureSampleType.Float,
            TextureSampleType.UnfilterableFloat => WGPUTextureSampleType.UnfilterableFloat,
            TextureSampleType.Depth => WGPUTextureSampleType.Depth,
            TextureSampleType.Sint => WGPUTextureSampleType.Sint,
            TextureSampleType.Uint => WGPUTextureSampleType.Uint,
            _ => WGPUTextureSampleType.Undefined,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUStorageTextureAccess ToWGPU(this StorageTextureAccess value)
    {
        return value switch
        {
            StorageTextureAccess.WriteOnly => WGPUStorageTextureAccess.WriteOnly,
            StorageTextureAccess.ReadOnly => WGPUStorageTextureAccess.ReadOnly,
            StorageTextureAccess.ReadWrite => WGPUStorageTextureAccess.ReadWrite,
            _ => WGPUStorageTextureAccess.Undefined,
        };
    }
}
