// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leoncino;

public readonly record struct PixelFormatInfo(
    PixelFormat Format,
    uint BytesPerBlock,
    uint BlockWidth,
    uint BlockHeight,
    FormatKind Kind
    );

public static class PixelFormatUtils
{
    private static readonly PixelFormatInfo[] s_formatInfos =
    [
        new(PixelFormat.Undefined,              0, 0, 0, FormatKind.Unorm),
        // 8-bit pixel formats
        new(PixelFormat.R8Unorm,                1, 1, 1, FormatKind.Unorm),
        new(PixelFormat.R8Snorm,                1, 1, 1, FormatKind.Snorm),
        new(PixelFormat.R8Uint,                 1, 1, 1, FormatKind.Uint),
        new(PixelFormat.R8Sint,                 1, 1, 1, FormatKind.Sint),
        // 16-bit pixel formats
        new(PixelFormat.R16Unorm,               2, 1, 1, FormatKind.Unorm),
        new(PixelFormat.R16Snorm,               2, 1, 1, FormatKind.Snorm),
        new(PixelFormat.R16Uint,                2, 1, 1, FormatKind.Uint),
        new(PixelFormat.R16Sint,                2, 1, 1, FormatKind.Sint),
        new(PixelFormat.R16Float,               2, 1, 1, FormatKind.Float),
        new(PixelFormat.RG8Unorm,               2, 1, 1, FormatKind.Unorm),
        new(PixelFormat.RG8Snorm,               2, 1, 1, FormatKind.Snorm),
        new(PixelFormat.RG8Uint,                2, 1, 1, FormatKind.Uint),
        new(PixelFormat.RG8Sint,                2, 1, 1, FormatKind.Sint),
        // Packed 16-Bit Pixel Formats
        new(PixelFormat.BGRA4Unorm,             2, 1, 1, FormatKind.Unorm),
        new(PixelFormat.B5G6R5Unorm,            2, 1, 1, FormatKind.Unorm),
        new(PixelFormat.BGR5A1Unorm,            2, 1, 1, FormatKind.Unorm),
        // 32-bit pixel formats
        new(PixelFormat.R32Uint,                4, 1, 1, FormatKind.Uint),
        new(PixelFormat.R32Sint,                4, 1, 1, FormatKind.Sint),
        new(PixelFormat.R32Float,               4, 1, 1, FormatKind.Float),
        new(PixelFormat.RG16Unorm,              4, 1, 1, FormatKind.Unorm),
        new(PixelFormat.RG16Snorm,              4, 1, 1, FormatKind.Snorm),
        new(PixelFormat.RG16Uint,               4, 1, 1, FormatKind.Uint),
        new(PixelFormat.RG16Sint,               4, 1, 1, FormatKind.Sint),
        new(PixelFormat.RG16Float,              4, 1, 1, FormatKind.Float),
        new(PixelFormat.RGBA8Unorm,             4, 1, 1, FormatKind.Unorm),
        new(PixelFormat.RGBA8UnormSrgb,         4, 1, 1, FormatKind.UnormSrgb),
        new(PixelFormat.RGBA8Snorm,             4, 1, 1, FormatKind.Snorm),
        new(PixelFormat.RGBA8Uint,              4, 1, 1, FormatKind.Uint),
        new(PixelFormat.RGBA8Sint,              4, 1, 1, FormatKind.Uint),
        new(PixelFormat.BGRA8Unorm,             4, 1, 1, FormatKind.Unorm),
        new(PixelFormat.BGRA8UnormSrgb,         4, 1, 1, FormatKind.UnormSrgb),
        // Packed 32-Bit Pixel formats
        new(PixelFormat.RGB10A2Unorm,           4, 1, 1, FormatKind.Unorm),
        new(PixelFormat.RGB10A2Uint,            4, 1, 1, FormatKind.Uint),
        new(PixelFormat.RG11B10Ufloat,          4, 1, 1, FormatKind.Float),
        new(PixelFormat.RGB9E5Ufloat,           4, 1, 1, FormatKind.Float),
        // 64-Bit Pixel Formats
        new(PixelFormat.RG32Uint,               8, 1, 1, FormatKind.Uint),
        new(PixelFormat.RG32Sint,               8, 1, 1, FormatKind.Sint),
        new(PixelFormat.RG32Float,              8, 1, 1, FormatKind.Float),
        new(PixelFormat.RGBA16Unorm,            8, 1, 1, FormatKind.Unorm),
        new(PixelFormat.RGBA16Snorm,            8, 1, 1, FormatKind.Snorm),
        new(PixelFormat.RGBA16Uint,             8, 1, 1, FormatKind.Uint),
        new(PixelFormat.RGBA16Sint,             8, 1, 1, FormatKind.Sint),
        new(PixelFormat.RGBA16Float,            8, 1, 1, FormatKind.Float),
        // 128-Bit Pixel Formats
        new(PixelFormat.RGBA32Uint,            16, 1, 1, FormatKind.Uint),
        new(PixelFormat.RGBA32Sint,            16, 1, 1, FormatKind.Sint),
        new(PixelFormat.RGBA32Float,           16, 1, 1, FormatKind.Float),
        // Depth-stencil formats
        new(PixelFormat.Depth16Unorm,          2, 1, 1, FormatKind.Unorm),
        new(PixelFormat.Depth24UnormStencil8,  4, 1, 1, FormatKind.Unorm),
        new(PixelFormat.Depth32Float,          4, 1, 1, FormatKind.Float),
        new(PixelFormat.Depth32FloatStencil8,  8, 1, 1, FormatKind.Float),
        // BC compressed formats
        new(PixelFormat.BC1RGBAUnorm,          8, 4, 4,  FormatKind.Unorm),
        new(PixelFormat.BC1RGBAUnormSrgb,      8, 4, 4,  FormatKind.UnormSrgb),
        new(PixelFormat.BC2RGBAUnorm,          16, 4, 4, FormatKind.Unorm),
        new(PixelFormat.BC2RGBAUnormSrgb,      16, 4, 4, FormatKind.UnormSrgb),
        new(PixelFormat.BC3RGBAUnorm,          16, 4, 4, FormatKind.Unorm),
        new(PixelFormat.BC3RGBAUnormSrgb,      16, 4, 4, FormatKind.UnormSrgb),
        new(PixelFormat.BC4RUnorm,             8,  4, 4, FormatKind.Unorm),
        new(PixelFormat.BC4RSnorm,             8,  4, 4, FormatKind.Snorm),
        new(PixelFormat.BC5RGUnorm,            16, 4, 4, FormatKind.Unorm),
        new(PixelFormat.BC5RGSnorm,            16, 4, 4, FormatKind.Snorm),
        new(PixelFormat.BC6HRGBUfloat,         16, 4, 4, FormatKind.Float),
        new(PixelFormat.BC6HRGBFloat,          16, 4, 4, FormatKind.Float),
        new(PixelFormat.BC7RGBAUnorm,          16, 4, 4, FormatKind.Unorm),
        new(PixelFormat.BC7RGBAUnormSrgb,      16, 4, 4, FormatKind.UnormSrgb),
        // ETC2/EAC compressed formats
        new(PixelFormat.ETC2RGB8Unorm,        8,   4, 4, FormatKind.Unorm),
        new(PixelFormat.ETC2RGB8UnormSrgb,    8,   4, 4, FormatKind.UnormSrgb),
        new(PixelFormat.ETC2RGB8A1Unorm,     16,   4, 4, FormatKind.Unorm),
        new(PixelFormat.ETC2RGB8A1UnormSrgb, 16,   4, 4, FormatKind.UnormSrgb),
        new(PixelFormat.ETC2RGBA8Unorm,      16,   4, 4, FormatKind.Unorm),
        new(PixelFormat.ETC2RGBA8UnormSrgb,  16,   4, 4, FormatKind.UnormSrgb),
        new(PixelFormat.EACR11Unorm,         8,    4, 4, FormatKind.Unorm),
        new(PixelFormat.EACR11Snorm,         8,    4, 4, FormatKind.Snorm),
        new(PixelFormat.EACRG11Unorm,        16,   4, 4, FormatKind.Unorm),
        new(PixelFormat.EACRG11Snorm,        16,   4, 4, FormatKind.Snorm),

        // ASTC compressed formats
        new(PixelFormat.ASTC4x4Unorm,        16,   4, 4, FormatKind.Unorm),
        new(PixelFormat.ASTC4x4UnormSrgb,    16,   4, 4, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC5x4Unorm,        16,   5, 4, FormatKind.Unorm),
        new(PixelFormat.ASTC5x4UnormSrgb,    16,   5, 4, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC5x5Unorm,        16,   5, 5, FormatKind.Unorm),
        new(PixelFormat.ASTC5x5UnormSrgb,    16,   5, 5, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC6x5Unorm,        16,   6, 5, FormatKind.Unorm),
        new(PixelFormat.ASTC6x5UnormSrgb,    16,   6, 5, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC6x6Unorm,        16,   6, 6, FormatKind.Unorm),
        new(PixelFormat.ASTC6x6UnormSrgb,    16,   6, 6, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC8x5Unorm,        16,   8, 5, FormatKind.Unorm),
        new(PixelFormat.ASTC8x5UnormSrgb,    16,   8, 5, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC8x6Unorm,        16,   8, 6, FormatKind.Unorm),
        new(PixelFormat.ASTC8x6UnormSrgb,    16,   8, 6, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC8x8Unorm,        16,   8, 8, FormatKind.Unorm),
        new(PixelFormat.ASTC8x8UnormSrgb,    16,   8, 8, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC10x5Unorm,       16,   10, 5, FormatKind.Unorm),
        new(PixelFormat.ASTC10x5UnormSrgb,   16,   10, 5, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC10x6Unorm,       16,   10, 6, FormatKind.Unorm),
        new(PixelFormat.ASTC10x6UnormSrgb,   16,   10, 6, FormatKind.UnormSrgb ),
        new(PixelFormat.ASTC10x8Unorm,       16,   10, 8, FormatKind.Unorm),
        new(PixelFormat.ASTC10x8UnormSrgb,   16,   10, 8, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC10x10Unorm,      16,   10, 10, FormatKind.Unorm ),
        new(PixelFormat.ASTC10x10UnormSrgb,  16,   10, 10, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC12x10Unorm,      16,   12, 10, FormatKind.Unorm),
        new(PixelFormat.ASTC12x10UnormSrgb,  16,   12, 10, FormatKind.UnormSrgb),
        new(PixelFormat.ASTC12x12Unorm,      16,   12, 12, FormatKind.Unorm),
        new(PixelFormat.ASTC12x12UnormSrgb,  16,   12, 12, FormatKind.UnormSrgb),
         // ASTC HDR compressed formats
        new(PixelFormat.ASTC4x4Hdr,             16,   4, 4, FormatKind.Hdr),
        new(PixelFormat.ASTC5x4Hdr,             16,   5, 4, FormatKind.Hdr),
        new(PixelFormat.ASTC5x5Hdr,             16,   5, 5, FormatKind.Hdr),
        new(PixelFormat.ASTC6x5Hdr,             16,   6, 5, FormatKind.Hdr),
        new(PixelFormat.ASTC6x6Hdr,             16,   6, 6, FormatKind.Hdr),
        new(PixelFormat.ASTC8x5Hdr,             16,   8, 5, FormatKind.Hdr),
        new(PixelFormat.ASTC8x6Hdr,             16,   8, 6, FormatKind.Hdr),
        new(PixelFormat.ASTC8x8Hdr,             16,   8, 6, FormatKind.Hdr),
        new(PixelFormat.ASTC10x5Hdr,            16,   10, 5, FormatKind.Hdr),
        new(PixelFormat.ASTC10x6Hdr,            16,   10, 6, FormatKind.Hdr),
        new(PixelFormat.ASTC10x8Hdr,            16,   10, 8, FormatKind.Hdr),
        new(PixelFormat.ASTC10x10Hdr,           16,   10, 10, FormatKind.Hdr),
        new(PixelFormat.ASTC12x10Hdr,           16,   12, 10, FormatKind.Hdr),
        new(PixelFormat.ASTC12x12Hdr,           16,   12, 12, FormatKind.Hdr),
    ];

    public static ref readonly PixelFormatInfo GetFormatInfo(this PixelFormat format)
    {
        if (format >= PixelFormat.Count)
        {
            return ref s_formatInfos[0]; // UNKNOWN
        }

        Debug.Assert(s_formatInfos[(int)format].Format == format);
        return ref s_formatInfos[(int)format];
    }

    /// <summary>
    /// Get the number of bytes per format.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static uint GetFormatBytesPerBlock(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(uint)format].BytesPerBlock;
    }

    public static uint GetFormatPixelsPerBlock(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(uint)format].BlockWidth * s_formatInfos[(uint)format].BlockHeight;
    }

    public static FormatKind GetKind(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(uint)format].Kind;
    }

    public static bool IsInteger(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].Kind == FormatKind.Uint || s_formatInfos[(int)format].Kind == FormatKind.Sint;
    }

    public static bool IsSigned(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].Kind == FormatKind.Sint;
    }

    public static bool IsSrgb(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].Kind == FormatKind.UnormSrgb;
    }

    /// <summary>
    /// Check if the format is a compressed format.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static bool IsCompressedFormat(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].BlockWidth > 1;
    }

    /// <summary>
    /// Check if the format is a BC compressed format.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static bool IsCompressedBCFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.BC1RGBAUnorm:
            case PixelFormat.BC1RGBAUnormSrgb:
            case PixelFormat.BC2RGBAUnorm:
            case PixelFormat.BC2RGBAUnormSrgb:
            case PixelFormat.BC3RGBAUnorm:
            case PixelFormat.BC3RGBAUnormSrgb:
            case PixelFormat.BC4RUnorm:
            case PixelFormat.BC4RSnorm:
            case PixelFormat.BC5RGUnorm:
            case PixelFormat.BC5RGSnorm:
            case PixelFormat.BC6HRGBUfloat:
            case PixelFormat.BC6HRGBFloat:
            case PixelFormat.BC7RGBAUnorm:
            case PixelFormat.BC7RGBAUnormSrgb:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Check if the format is a ASTC compressed format.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static bool IsCompressedASTCFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.ASTC4x4Unorm:
            case PixelFormat.ASTC4x4UnormSrgb:
            case PixelFormat.ASTC5x4Unorm:
            case PixelFormat.ASTC5x4UnormSrgb:
            case PixelFormat.ASTC5x5Unorm:
            case PixelFormat.ASTC5x5UnormSrgb:
            case PixelFormat.ASTC6x5Unorm:
            case PixelFormat.ASTC6x5UnormSrgb:
            case PixelFormat.ASTC6x6Unorm:
            case PixelFormat.ASTC6x6UnormSrgb:
            case PixelFormat.ASTC8x5Unorm:
            case PixelFormat.ASTC8x5UnormSrgb:
            case PixelFormat.ASTC8x6Unorm:
            case PixelFormat.ASTC8x6UnormSrgb:
            case PixelFormat.ASTC8x8Unorm:
            case PixelFormat.ASTC8x8UnormSrgb:
            case PixelFormat.ASTC10x5Unorm:
            case PixelFormat.ASTC10x5UnormSrgb:
            case PixelFormat.ASTC10x6Unorm:
            case PixelFormat.ASTC10x6UnormSrgb:
            case PixelFormat.ASTC10x8Unorm:
            case PixelFormat.ASTC10x8UnormSrgb:
            case PixelFormat.ASTC10x10Unorm:
            case PixelFormat.ASTC10x10UnormSrgb:
            case PixelFormat.ASTC12x10Unorm:
            case PixelFormat.ASTC12x10UnormSrgb:
            case PixelFormat.ASTC12x12Unorm:
                return true;

            case PixelFormat.ASTC4x4Hdr:
            case PixelFormat.ASTC5x4Hdr:
            case PixelFormat.ASTC5x5Hdr:
            case PixelFormat.ASTC6x5Hdr:
            case PixelFormat.ASTC6x6Hdr:
            case PixelFormat.ASTC8x5Hdr:
            case PixelFormat.ASTC8x6Hdr:
            case PixelFormat.ASTC8x8Hdr:
            case PixelFormat.ASTC10x5Hdr:
            case PixelFormat.ASTC10x6Hdr:
            case PixelFormat.ASTC10x8Hdr:
            case PixelFormat.ASTC10x10Hdr:
            case PixelFormat.ASTC12x10Hdr:
            case PixelFormat.ASTC12x12Hdr:
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Get the format compression ration along the x-axis.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static uint GetFormatWidthCompressionRatio(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].BlockWidth;
    }

    /// <summary>
    /// Get the format compression ration along the y-axis.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static uint GetFormatHeightCompressionRatio(this PixelFormat format)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].BlockHeight;
    }

    /// <summary>
    /// Check if the format has a depth component.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has depth component, false otherwise.</returns>
    public static bool IsDepthFormat(this PixelFormat format)
    {
        return format switch
        {
            PixelFormat.Depth16Unorm or PixelFormat.Depth24UnormStencil8 or PixelFormat.Depth32Float or PixelFormat.Depth32FloatStencil8 => true,
            _ => false,
        };
    }

    /// <summary>
    /// Check if the format has a stencil component.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has stencil component, false otherwise.</returns>
    public static bool IsStencilFormat(this PixelFormat format)
    {
        return format switch
        {
            PixelFormat.Depth24UnormStencil8 or PixelFormat.Depth32FloatStencil8 => true,
            _ => false,
        };
    }

    /// <summary>
    /// Check if the format has depth or stencil components.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has depth or stencil component, false otherwise.</returns>
    public static bool IsDepthStencilFormat(this PixelFormat format)
    {
        return IsDepthFormat(format) || IsStencilFormat(format);
    }

    /// <summary>
    /// Check if the format has depth only components.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has depth or stencil component, false otherwise.</returns>
    public static bool IsDepthOnlyFormat(this PixelFormat format)
    {
        return format switch
        {
            PixelFormat.Depth16Unorm or PixelFormat.Depth32Float => true,
            _ => false,
        };
    }

    /// <summary>
    /// Get the number of bytes per row. If format is compressed, width should be evenly divisible by the compression ratio.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static uint GetFormatRowPitch(this PixelFormat format, uint width)
    {
        Debug.Assert(width % GetFormatWidthCompressionRatio(format) == 0);

        return (width / GetFormatWidthCompressionRatio(format)) * GetFormatBytesPerBlock(format);
    }

    /// <summary>
    /// Convert an SRGB format to linear. If the format is already linear, will return it
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static PixelFormat SrgbToLinearFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.RGBA8UnormSrgb:
                return PixelFormat.RGBA8Unorm;
            case PixelFormat.BGRA8UnormSrgb:
                return PixelFormat.BGRA8Unorm;
            // Bc compressed formats
            case PixelFormat.BC1RGBAUnormSrgb:
                return PixelFormat.BC1RGBAUnorm;
            case PixelFormat.BC2RGBAUnormSrgb:
                return PixelFormat.BC2RGBAUnorm;
            case PixelFormat.BC3RGBAUnormSrgb:
                return PixelFormat.BC3RGBAUnorm;
            case PixelFormat.BC7RGBAUnormSrgb:
                return PixelFormat.BC7RGBAUnorm;

            // Etc2/Eac compressed formats
            case PixelFormat.ETC2RGB8UnormSrgb:
                return PixelFormat.ETC2RGB8Unorm;
            case PixelFormat.ETC2RGB8A1UnormSrgb:
                return PixelFormat.ETC2RGB8A1Unorm;
            case PixelFormat.ETC2RGBA8UnormSrgb:
                return PixelFormat.ETC2RGBA8Unorm;

            // Astc compressed formats
            case PixelFormat.ASTC4x4UnormSrgb:
                return PixelFormat.ASTC4x4Unorm;
            case PixelFormat.ASTC5x4UnormSrgb:
                return PixelFormat.ASTC5x4Unorm;
            case PixelFormat.ASTC5x5UnormSrgb:
                return PixelFormat.ASTC5x5Unorm;
            case PixelFormat.ASTC6x5UnormSrgb:
                return PixelFormat.ASTC6x5Unorm;
            case PixelFormat.ASTC6x6UnormSrgb:
                return PixelFormat.ASTC6x6Unorm;
            case PixelFormat.ASTC8x5UnormSrgb:
                return PixelFormat.ASTC8x5Unorm;
            case PixelFormat.ASTC8x6UnormSrgb:
                return PixelFormat.ASTC8x6Unorm;
            case PixelFormat.ASTC8x8UnormSrgb:
                return PixelFormat.ASTC8x8Unorm;
            case PixelFormat.ASTC10x5UnormSrgb:
                return PixelFormat.ASTC10x5Unorm;
            case PixelFormat.ASTC10x6UnormSrgb:
                return PixelFormat.ASTC10x6Unorm;
            case PixelFormat.ASTC10x8UnormSrgb:
                return PixelFormat.ASTC10x8Unorm;
            case PixelFormat.ASTC10x10UnormSrgb:
                return PixelFormat.ASTC10x10Unorm;
            case PixelFormat.ASTC12x10UnormSrgb:
                return PixelFormat.ASTC12x10Unorm;
            case PixelFormat.ASTC12x12UnormSrgb:
                return PixelFormat.ASTC12x12Unorm;

            default:
                Debug.Assert(!IsSrgb(format));
                return format;
        }
    }

    /// <summary>
    /// Convert an linear format to sRGB. If the format doesn't have a matching sRGB format, will return the original
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PixelFormat LinearToSrgbFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.RGBA8Unorm:
                return PixelFormat.RGBA8UnormSrgb;
            case PixelFormat.BGRA8Unorm:
                return PixelFormat.BGRA8UnormSrgb;

            // Bc compressed formats
            case PixelFormat.BC1RGBAUnorm:
                return PixelFormat.BC1RGBAUnormSrgb;
            case PixelFormat.BC2RGBAUnorm:
                return PixelFormat.BC2RGBAUnormSrgb;
            case PixelFormat.BC3RGBAUnorm:
                return PixelFormat.BC3RGBAUnormSrgb;
            case PixelFormat.BC7RGBAUnorm:
                return PixelFormat.BC7RGBAUnormSrgb;

            // Etc2/Eac compressed formats
            case PixelFormat.ETC2RGB8Unorm:
                return PixelFormat.ETC2RGB8UnormSrgb;
            case PixelFormat.ETC2RGB8A1Unorm:
                return PixelFormat.ETC2RGB8A1UnormSrgb;
            case PixelFormat.ETC2RGBA8Unorm:
                return PixelFormat.ETC2RGBA8UnormSrgb;

            // Astc compressed formats
            case PixelFormat.ASTC4x4Unorm:
                return PixelFormat.ASTC4x4UnormSrgb;
            case PixelFormat.ASTC5x4Unorm:
                return PixelFormat.ASTC5x4UnormSrgb;
            case PixelFormat.ASTC5x5Unorm:
                return PixelFormat.ASTC5x5UnormSrgb;
            case PixelFormat.ASTC6x5Unorm:
                return PixelFormat.ASTC6x5UnormSrgb;
            case PixelFormat.ASTC6x6Unorm:
                return PixelFormat.ASTC6x6UnormSrgb;
            case PixelFormat.ASTC8x5Unorm:
                return PixelFormat.ASTC8x5UnormSrgb;
            case PixelFormat.ASTC8x6Unorm:
                return PixelFormat.ASTC8x6UnormSrgb;
            case PixelFormat.ASTC8x8Unorm:
                return PixelFormat.ASTC8x8UnormSrgb;
            case PixelFormat.ASTC10x5Unorm:
                return PixelFormat.ASTC10x5UnormSrgb;
            case PixelFormat.ASTC10x6Unorm:
                return PixelFormat.ASTC10x6UnormSrgb;
            case PixelFormat.ASTC10x8Unorm:
                return PixelFormat.ASTC10x8UnormSrgb;
            case PixelFormat.ASTC10x10Unorm:
                return PixelFormat.ASTC10x10UnormSrgb;
            case PixelFormat.ASTC12x10Unorm:
                return PixelFormat.ASTC12x10UnormSrgb;
            case PixelFormat.ASTC12x12Unorm:
                return PixelFormat.ASTC12x12UnormSrgb;

            default:
                return format;
        }
    }

    /// <summary>
    /// Return the BPP for a given <see cref="PixelFormat"/>.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/>.</param>
    /// <returns>Bits per pixel of the format</returns>
    public static uint BitsPerPixel(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.RGBA32Uint:
            case PixelFormat.RGBA32Sint:
            case PixelFormat.RGBA32Float:
                return 128;

            //case PixelFormat.Rgb32Uint:
            //case PixelFormat.Rgb32Sint:
            //case PixelFormat.Rgb32Float:
            //    return 96;

            case PixelFormat.RGBA16Unorm:
            case PixelFormat.RGBA16Snorm:
            case PixelFormat.RGBA16Uint:
            case PixelFormat.RGBA16Sint:
            case PixelFormat.RGBA16Float:
            case PixelFormat.RG32Uint:
            case PixelFormat.RG32Sint:
            case PixelFormat.RG32Float:
            case PixelFormat.Depth32FloatStencil8:
                return 64;

            case PixelFormat.RGB10A2Unorm:
            case PixelFormat.RGB10A2Uint:
            case PixelFormat.RG11B10Ufloat:
            case PixelFormat.RGB9E5Ufloat:
            case PixelFormat.RGBA8Unorm:
            case PixelFormat.RGBA8UnormSrgb:
            case PixelFormat.RGBA8Snorm:
            case PixelFormat.RGBA8Uint:
            case PixelFormat.RGBA8Sint:
            case PixelFormat.RG16Unorm:
            case PixelFormat.RG16Snorm:
            case PixelFormat.RG16Uint:
            case PixelFormat.RG16Sint:
            case PixelFormat.RG16Float:
            case PixelFormat.Depth32Float:
            case PixelFormat.R32Uint:
            case PixelFormat.R32Sint:
            case PixelFormat.R32Float:
            case PixelFormat.Depth24UnormStencil8:
            case PixelFormat.BGRA8Unorm:
            case PixelFormat.BGRA8UnormSrgb:
                return 32;

            case PixelFormat.RG8Unorm:
            case PixelFormat.RG8Snorm:
            case PixelFormat.RG8Uint:
            case PixelFormat.RG8Sint:
            case PixelFormat.R16Unorm:
            case PixelFormat.R16Snorm:
            case PixelFormat.R16Uint:
            case PixelFormat.R16Sint:
            case PixelFormat.R16Float:
            case PixelFormat.Depth16Unorm:
            case PixelFormat.B5G6R5Unorm:
            case PixelFormat.BGR5A1Unorm:
            case PixelFormat.BGRA4Unorm:
                return 16;

            case PixelFormat.R8Unorm:
            case PixelFormat.R8Snorm:
            case PixelFormat.R8Uint:
            case PixelFormat.R8Sint:
            case PixelFormat.BC2RGBAUnorm:
            case PixelFormat.BC2RGBAUnormSrgb:
            case PixelFormat.BC3RGBAUnorm:
            case PixelFormat.BC3RGBAUnormSrgb:
            case PixelFormat.BC5RGUnorm:
            case PixelFormat.BC5RGSnorm:
            case PixelFormat.BC6HRGBUfloat:
            case PixelFormat.BC6HRGBFloat:
            case PixelFormat.BC7RGBAUnorm:
            case PixelFormat.BC7RGBAUnormSrgb:
            case PixelFormat.ETC2RGB8A1Unorm:
            case PixelFormat.ETC2RGB8A1UnormSrgb:
            case PixelFormat.ETC2RGBA8Unorm:
            case PixelFormat.ETC2RGBA8UnormSrgb:
            case PixelFormat.EACRG11Unorm:
            case PixelFormat.EACRG11Snorm:
                return 8;

            case PixelFormat.BC1RGBAUnorm:
            case PixelFormat.BC1RGBAUnormSrgb:
            case PixelFormat.BC4RUnorm:
            case PixelFormat.BC4RSnorm:
            case PixelFormat.ETC2RGB8Unorm:
            case PixelFormat.ETC2RGB8UnormSrgb:
            case PixelFormat.EACR11Unorm:
            case PixelFormat.EACR11Snorm:
                return 4;

            default:
                return 0;
        }
    }

    public static void GetSurfaceInfo(in PixelFormat format, in uint width, in uint height,
        out uint rowPitch, out uint slicePitch, out uint widthCount, out uint heightCount)
    {
        Debug.Assert(s_formatInfos[(int)format].Format == format);
        ref PixelFormatInfo formatInfo = ref s_formatInfos[(int)format];

        widthCount = width;
        heightCount = height;

        switch (format)
        {
            case PixelFormat.BC1RGBAUnorm:
            case PixelFormat.BC1RGBAUnormSrgb:
            case PixelFormat.BC2RGBAUnorm:
            case PixelFormat.BC2RGBAUnormSrgb:
            case PixelFormat.BC3RGBAUnorm:
            case PixelFormat.BC3RGBAUnormSrgb:
            case PixelFormat.BC4RUnorm:
            case PixelFormat.BC4RSnorm:
            case PixelFormat.BC5RGUnorm:
            case PixelFormat.BC5RGSnorm:
            case PixelFormat.BC6HRGBUfloat:
            case PixelFormat.BC6HRGBFloat:
            case PixelFormat.BC7RGBAUnorm:
            case PixelFormat.BC7RGBAUnormSrgb:
                widthCount = Math.Max(1, (width + 3) / 4);
                heightCount = Math.Max(1, (height + 3) / 4);
                rowPitch = widthCount * formatInfo.BytesPerBlock; // BytesPerBlock is 8 or 16
                slicePitch = rowPitch * heightCount;
                break;

            // ETC2/EAC compressed formats
            case PixelFormat.ETC2RGB8Unorm:
            case PixelFormat.ETC2RGB8UnormSrgb:
            case PixelFormat.ETC2RGB8A1Unorm:
            case PixelFormat.ETC2RGB8A1UnormSrgb:
            case PixelFormat.ETC2RGBA8Unorm:
            case PixelFormat.ETC2RGBA8UnormSrgb:
            case PixelFormat.EACR11Unorm:
            case PixelFormat.EACR11Snorm:
            case PixelFormat.EACRG11Unorm:
            case PixelFormat.EACRG11Snorm:
                widthCount = Math.Max(1, (width + formatInfo.BlockWidth - 1) / formatInfo.BlockWidth);
                heightCount = Math.Max(1, (height + formatInfo.BlockHeight - 1) / formatInfo.BlockHeight);
                rowPitch = widthCount * formatInfo.BytesPerBlock; // BytesPerBlock is 8 or 16
                slicePitch = rowPitch * heightCount;
                break;

            // ASTC compressed formats
            case PixelFormat.ASTC4x4Unorm:
            case PixelFormat.ASTC4x4UnormSrgb:
            case PixelFormat.ASTC5x4Unorm:
            case PixelFormat.ASTC5x4UnormSrgb:
            case PixelFormat.ASTC5x5Unorm:
            case PixelFormat.ASTC5x5UnormSrgb:
            case PixelFormat.ASTC6x5Unorm:
            case PixelFormat.ASTC6x5UnormSrgb:
            case PixelFormat.ASTC6x6Unorm:
            case PixelFormat.ASTC6x6UnormSrgb:
            case PixelFormat.ASTC8x5Unorm:
            case PixelFormat.ASTC8x5UnormSrgb:
            case PixelFormat.ASTC8x6Unorm:
            case PixelFormat.ASTC8x6UnormSrgb:
            case PixelFormat.ASTC8x8Unorm:
            case PixelFormat.ASTC8x8UnormSrgb:
            case PixelFormat.ASTC10x5Unorm:
            case PixelFormat.ASTC10x5UnormSrgb:
            case PixelFormat.ASTC10x6Unorm:
            case PixelFormat.ASTC10x6UnormSrgb:
            case PixelFormat.ASTC10x8Unorm:
            case PixelFormat.ASTC10x8UnormSrgb:
            case PixelFormat.ASTC10x10Unorm:
            case PixelFormat.ASTC10x10UnormSrgb:
            case PixelFormat.ASTC12x10Unorm:
            case PixelFormat.ASTC12x10UnormSrgb:
            case PixelFormat.ASTC12x12Unorm:
                widthCount = Math.Max(1, (width + formatInfo.BlockWidth - 1) / formatInfo.BlockWidth);
                heightCount = Math.Max(1, (height + formatInfo.BlockHeight - 1) / formatInfo.BlockHeight);
                rowPitch = widthCount * formatInfo.BytesPerBlock;  // BytesPerBlock is always 16
                slicePitch = rowPitch * heightCount;
                break;

            // ASTC HDR compressed formats
            case PixelFormat.ASTC4x4Hdr:
            case PixelFormat.ASTC5x4Hdr:
            case PixelFormat.ASTC5x5Hdr:
            case PixelFormat.ASTC6x5Hdr:
            case PixelFormat.ASTC6x6Hdr:
            case PixelFormat.ASTC8x5Hdr:
            case PixelFormat.ASTC8x6Hdr:
            case PixelFormat.ASTC8x8Hdr:
            case PixelFormat.ASTC10x5Hdr:
            case PixelFormat.ASTC10x6Hdr:
            case PixelFormat.ASTC10x8Hdr:
            case PixelFormat.ASTC10x10Hdr:
            case PixelFormat.ASTC12x10Hdr:
            case PixelFormat.ASTC12x12Hdr:
                throw new NotImplementedException();
            //widthCount = Math.Max(1, (width + formatInfo.BlockWidth - 1) / formatInfo.BlockWidth);
            //heightCount = Math.Max(1, (height + formatInfo.BlockHeight - 1) / formatInfo.BlockHeight);
            //rowPitch = widthCount * formatInfo.BytesPerBlock;  // BytesPerBlock is always 16
            //slicePitch = rowPitch * heightCount;
            //break;

            //case Format.R8G8_B8G8_UNorm:
            //case Format.G8R8_G8B8_UNorm:
            //case Format.YUY2:
            //    packed = true;
            //    bpe = 4;
            //    break;
            //
            //case Format.Y210:
            //case Format.Y216:
            //    packed = true;
            //    bpe = 8;
            //    break;
            //
            //case Format.NV12:
            //case Format.Opaque420:
            //case Format.P208:
            //    planar = true;
            //    bpe = 2;
            //    break;
            //
            //case Format.P010:
            //case Format.P016:
            //    planar = true;
            //    bpe = 4;
            //    break;

            default:
                uint bpp = BitsPerPixel(format);
                rowPitch = (width * bpp + 7) / 8; // round up to nearest byte
                slicePitch = rowPitch * height;
                return;
        }
    }

    public static void GetSurfaceInfo(in PixelFormat format, in uint width, in uint height, out uint rowPitch, out uint slicePitch)
    {
        GetSurfaceInfo(format, width, height, out rowPitch, out slicePitch, out _, out _);
    }
}
