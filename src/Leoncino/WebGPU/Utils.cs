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
        switch (format)
        {
            // 8-bit formats
            case WGPUTextureFormat.R8Unorm: return PixelFormat.R8Unorm;
            case WGPUTextureFormat.R8Snorm: return PixelFormat.R8Snorm;
            case WGPUTextureFormat.R8Uint: return PixelFormat.R8Uint;
            case WGPUTextureFormat.R8Sint: return PixelFormat.R8Sint;
            // 16-bit formats
            //case PixelFormat.R16Unorm: return WGPUTextureFormat.R16Unorm;
            //case PixelFormat.R16Snorm: return WGPUTextureFormat.R16Snorm;
            case WGPUTextureFormat.R16Uint: return PixelFormat.R16Uint;
            case WGPUTextureFormat.R16Sint: return PixelFormat.R16Sint;
            case WGPUTextureFormat.R16Float: return PixelFormat.R16Float;
            case WGPUTextureFormat.RG8Unorm: return PixelFormat.RG8Unorm;
            case WGPUTextureFormat.RG8Snorm: return PixelFormat.RG8Snorm;
            case WGPUTextureFormat.RG8Uint: return PixelFormat.RG8Uint;
            case WGPUTextureFormat.RG8Sint: return PixelFormat.RG8Sint;
            // Packed 16-Bit Pixel Formats
            //case PixelFormat.Bgra4Unorm: return WGPUTextureFormat.Bgra4Unorm;
            //case PixelFormat.B5G6R5Unorm: return WGPUTextureFormat.B5G6R5UnormPack16;
            //case PixelFormat.Bgr5A1Unorm: return WGPUTextureFormat.B5G5R5A1UnormPack16;
            // 32-bit formats
            case WGPUTextureFormat.R32Uint: return PixelFormat.R32Uint;
            case WGPUTextureFormat.R32Sint: return PixelFormat.R32Sint;
            case WGPUTextureFormat.R32Float: return PixelFormat.R32Float;
            //case PixelFormat.Rg16Unorm: return WGPUTextureFormat.Rg16Unorm;
            //case PixelFormat.Rg16Snorm: return WGPUTextureFormat.Rg16Snorm;
            case WGPUTextureFormat.RG16Uint: return PixelFormat.RG16Uint;
            case WGPUTextureFormat.RG16Sint: return PixelFormat.RG16Sint;
            case WGPUTextureFormat.RG16Float: return PixelFormat.RG16Float;
            case WGPUTextureFormat.RGBA8Unorm: return PixelFormat.RGBA8Unorm;
            case WGPUTextureFormat.RGBA8UnormSrgb: return PixelFormat.RGBA8UnormSrgb;
            case WGPUTextureFormat.RGBA8Snorm: return PixelFormat.RGBA8Snorm;
            case WGPUTextureFormat.RGBA8Uint: return PixelFormat.RGBA8Uint;
            case WGPUTextureFormat.RGBA8Sint: return PixelFormat.RGBA8Sint;
            case WGPUTextureFormat.BGRA8Unorm: return PixelFormat.BGRA8Unorm;
            case WGPUTextureFormat.BGRA8UnormSrgb: return PixelFormat.BGRA8UnormSrgb;
            // Packed 32-Bit formats
            case WGPUTextureFormat.RGB10A2Unorm: return PixelFormat.RGB10A2Unorm;
            case WGPUTextureFormat.RGB10A2Uint: return PixelFormat.RGB10A2Uint;
            case WGPUTextureFormat.RG11B10Ufloat: return PixelFormat.RG11B10Ufloat;
            case WGPUTextureFormat.RGB9E5Ufloat: return PixelFormat.RGB9E5Ufloat;
            // 64-Bit formats
            case WGPUTextureFormat.RG32Uint: return PixelFormat.RG32Uint;
            case WGPUTextureFormat.RG32Sint: return PixelFormat.RG32Sint;
            case WGPUTextureFormat.RG32Float: return PixelFormat.RG32Float;
            //case PixelFormat.Rgba16Unorm: return WGPUTextureFormat.Rgba16Unorm;
            //case PixelFormat.Rgba16Snorm: return WGPUTextureFormat.Rgba16Snorm;
            case WGPUTextureFormat.RGBA16Uint: return PixelFormat.RGBA16Uint;
            case WGPUTextureFormat.RGBA16Sint: return PixelFormat.RGBA16Sint;
            case WGPUTextureFormat.RGBA16Float: return PixelFormat.RGBA16Float;
            // 128-Bit formats
            case WGPUTextureFormat.RGBA32Uint: return PixelFormat.RGBA32Uint;
            case WGPUTextureFormat.RGBA32Sint: return PixelFormat.RGBA32Sint;
            case WGPUTextureFormat.RGBA32Float: return PixelFormat.RGBA32Float;

            // Depth-stencil formats
            case WGPUTextureFormat.Stencil8:
                return PixelFormat.Undefined;

            case WGPUTextureFormat.Depth16Unorm:
                return PixelFormat.Depth16Unorm;

            case WGPUTextureFormat.Depth24Plus:
            case WGPUTextureFormat.Depth24PlusStencil8:
                return PixelFormat.Depth24UnormStencil8;

            case WGPUTextureFormat.Depth32Float:
                return PixelFormat.Depth32Float;

            case WGPUTextureFormat.Depth32FloatStencil8:
                return PixelFormat.Depth32FloatStencil8;

            // Compressed BC formats
            case WGPUTextureFormat.BC1RGBAUnorm:
                return PixelFormat.BC1RGBAUnorm;
            case WGPUTextureFormat.BC1RGBAUnormSrgb:
                return PixelFormat.BC1RGBAUnormSrgb;
            case WGPUTextureFormat.BC2RGBAUnorm:
                return PixelFormat.BC2RGBAUnorm;
            case WGPUTextureFormat.BC2RGBAUnormSrgb:
                return PixelFormat.BC2RGBAUnormSrgb;
            case WGPUTextureFormat.BC3RGBAUnorm:
                return PixelFormat.BC3RGBAUnorm;
            case WGPUTextureFormat.BC3RGBAUnormSrgb:
                return PixelFormat.BC3RGBAUnormSrgb;
            case WGPUTextureFormat.BC4RSnorm:
                return PixelFormat.BC4RSnorm;
            case WGPUTextureFormat.BC4RUnorm:
                return PixelFormat.BC4RUnorm;
            case WGPUTextureFormat.BC5RGUnorm:
                return PixelFormat.BC5RGUnorm;
            case WGPUTextureFormat.BC5RGSnorm:
                return PixelFormat.BC5RGSnorm;
            case WGPUTextureFormat.BC6HRGBUfloat:
                return PixelFormat.BC6HRGBUfloat;
            case WGPUTextureFormat.BC6HRGBFloat:
                return PixelFormat.BC6HRGBFloat;
            case WGPUTextureFormat.BC7RGBAUnorm:
                return PixelFormat.BC7RGBAUnorm;
            case WGPUTextureFormat.BC7RGBAUnormSrgb:
                return PixelFormat.BC7RGBAUnormSrgb;

            // Etc2/Eac compressed formats
            case WGPUTextureFormat.ETC2RGB8Unorm:
                return PixelFormat.ETC2RGB8Unorm;
            case WGPUTextureFormat.ETC2RGB8UnormSrgb:
                return PixelFormat.ETC2RGB8UnormSrgb;
            case WGPUTextureFormat.ETC2RGB8A1Unorm:
                return PixelFormat.ETC2RGB8A1Unorm;
            case WGPUTextureFormat.ETC2RGB8A1UnormSrgb:
                return PixelFormat.ETC2RGB8A1UnormSrgb;
            case WGPUTextureFormat.ETC2RGBA8Unorm:
                return PixelFormat.ETC2RGBA8Unorm;
            case WGPUTextureFormat.ETC2RGBA8UnormSrgb:
                return PixelFormat.ETC2RGBA8UnormSrgb;

            case WGPUTextureFormat.EACR11Unorm: return PixelFormat.EACR11Unorm;
            case WGPUTextureFormat.EACR11Snorm: return PixelFormat.EACR11Snorm;
            case WGPUTextureFormat.EACRG11Unorm: return PixelFormat.EACRG11Unorm;
            case WGPUTextureFormat.EACRG11Snorm: return PixelFormat.EACRG11Snorm;

            case WGPUTextureFormat.ASTC4x4Unorm: return PixelFormat.ASTC4x4Unorm;
            case WGPUTextureFormat.ASTC4x4UnormSrgb: return PixelFormat.ASTC4x4UnormSrgb;
            case WGPUTextureFormat.ASTC5x4Unorm: return PixelFormat.ASTC5x4Unorm;
            case WGPUTextureFormat.ASTC5x4UnormSrgb: return PixelFormat.ASTC5x4UnormSrgb;
            case WGPUTextureFormat.ASTC5x5Unorm: return PixelFormat.ASTC5x5Unorm;
            case WGPUTextureFormat.ASTC5x5UnormSrgb: return PixelFormat.ASTC5x5UnormSrgb;
            case WGPUTextureFormat.ASTC6x5Unorm: return PixelFormat.ASTC6x5Unorm;
            case WGPUTextureFormat.ASTC6x5UnormSrgb: return PixelFormat.ASTC6x5UnormSrgb;
            case WGPUTextureFormat.ASTC6x6Unorm: return PixelFormat.ASTC6x6Unorm;
            case WGPUTextureFormat.ASTC6x6UnormSrgb: return PixelFormat.ASTC6x6UnormSrgb;
            case WGPUTextureFormat.ASTC8x5Unorm: return PixelFormat.ASTC8x5Unorm;
            case WGPUTextureFormat.ASTC8x5UnormSrgb: return PixelFormat.ASTC8x5UnormSrgb;
            case WGPUTextureFormat.ASTC8x6Unorm: return PixelFormat.ASTC8x6Unorm;
            case WGPUTextureFormat.ASTC8x6UnormSrgb: return PixelFormat.ASTC8x6UnormSrgb;
            case WGPUTextureFormat.ASTC8x8Unorm: return PixelFormat.ASTC8x8Unorm;
            case WGPUTextureFormat.ASTC8x8UnormSrgb: return PixelFormat.ASTC8x8UnormSrgb;
            case WGPUTextureFormat.ASTC10x5Unorm: return PixelFormat.ASTC10x5Unorm;
            case WGPUTextureFormat.ASTC10x5UnormSrgb: return PixelFormat.ASTC10x5UnormSrgb;
            case WGPUTextureFormat.ASTC10x6Unorm: return PixelFormat.ASTC10x6Unorm;
            case WGPUTextureFormat.ASTC10x6UnormSrgb: return PixelFormat.ASTC10x6UnormSrgb;
            case WGPUTextureFormat.ASTC10x8Unorm: return PixelFormat.ASTC10x8Unorm;
            case WGPUTextureFormat.ASTC10x8UnormSrgb: return PixelFormat.ASTC10x8UnormSrgb;
            case WGPUTextureFormat.ASTC10x10Unorm: return PixelFormat.ASTC10x10Unorm;
            case WGPUTextureFormat.ASTC10x10UnormSrgb: return PixelFormat.ASTC10x10UnormSrgb;
            case WGPUTextureFormat.ASTC12x10Unorm: return PixelFormat.ASTC12x10Unorm;
            case WGPUTextureFormat.ASTC12x10UnormSrgb: return PixelFormat.ASTC12x10UnormSrgb;
            case WGPUTextureFormat.ASTC12x12Unorm: return PixelFormat.ASTC12x12Unorm;
            case WGPUTextureFormat.ASTC12x12UnormSrgb: return PixelFormat.ASTC12x12UnormSrgb;

            default:
                return PixelFormat.Undefined;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUVertexFormat ToWGPU(this VertexFormat format)
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WGPUCompareFunction ToWGPU(this CompareFunction value)
    {
        switch (value)
        {
            case CompareFunction.Never: return WGPUCompareFunction.Never;
            case CompareFunction.Less: return WGPUCompareFunction.Less;
            case CompareFunction.Equal: return WGPUCompareFunction.Equal;
            case CompareFunction.LessEqual: return WGPUCompareFunction.LessEqual;
            case CompareFunction.Greater: return WGPUCompareFunction.Greater;
            case CompareFunction.NotEqual: return WGPUCompareFunction.NotEqual;
            case CompareFunction.GreaterEqual: return WGPUCompareFunction.GreaterEqual;
            case CompareFunction.Always: return WGPUCompareFunction.Always;
            default:
                return WGPUCompareFunction.Never;
        }
    }
}
