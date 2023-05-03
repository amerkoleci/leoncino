// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Leoncino;

/// <summary>
/// Define a <see cref="PixelFormat"/> kind.
/// </summary>
public enum PixelFormatKind
{
    /// <summary>
    /// Unsigned normalized formats.
    /// </summary>
    Unorm,
    /// <summary>
    /// Unsigned normalized SRGB formats.
    /// </summary>
    UnormSrgb,
    /// <summary>
    /// Signed normalized formats
    /// </summary>
    Snorm,
    /// <summary>
    /// Unsigned integer formats
    /// </summary>
    Uint,
    /// <summary>
    /// Signed integer formats
    /// </summary>
    Sint,
    /// <summary>
    /// Floating-point formats.
    /// </summary>
    Float,
}

public readonly record struct PixelFormatInfo(
    PixelFormat Format,
    int BytesPerBlock,
    int BlockWidth,
    int BlockHeight,
    PixelFormatKind Kind
    );

public static class PixelFormatUtils
{
    private static readonly PixelFormatInfo[] s_formatInfos = new PixelFormatInfo[]
    {
        new(PixelFormat.Undefined,              0, 0, 0, PixelFormatKind.Unorm),
        // 8-bit pixel formats
        new(PixelFormat.R8Unorm,                1, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.R8Snorm,                1, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.R8Uint,                 1, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.R8Sint,                 1, 1, 1, PixelFormatKind.Sint),
        // 16-bit pixel formats
        new(PixelFormat.R16Unorm,               2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.R16Snorm,               2, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.R16Uint,                2, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.R16Sint,                2, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.R16Float,               2, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.Rg8Unorm,               2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.Rg8Snorm,               2, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.Rg8Uint,                2, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Rg8Sint,                2, 1, 1, PixelFormatKind.Sint),
        // Packed 16-Bit Pixel Formats
        new(PixelFormat.Bgra4Unorm,             2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.B5G6R5Unorm,            2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.Bgr5A1Unorm,            2, 1, 1, PixelFormatKind.Unorm),
        // 32-bit pixel formats
        new(PixelFormat.R32Uint,                4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.R32Sint,                4, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.R32Float,               4, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.Rg16Unorm,              4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.Rg16Snorm,              4, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.Rg16Uint,               4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Rg16Sint,               4, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.Rg16Float,              4, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.Rgba8Unorm,             4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.Rgba8UnormSrgb,         4, 1, 1, PixelFormatKind.UnormSrgb),
        new(PixelFormat.Rgba8Snorm,             4, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.Rgba8Uint,              4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Rgba8Sint,              4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Bgra8Unorm,             4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.Bgra8UnormSrgb,         4, 1, 1, PixelFormatKind.UnormSrgb),
        // Packed 32-Bit Pixel formats
        new(PixelFormat.Rgb9e5Ufloat,           4, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.Rgb10a2Unorm,           4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.Rgb10a2Uint,            4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Rg11b10Float,           4, 1, 1, PixelFormatKind.Float),
        // 64-Bit Pixel Formats
        new(PixelFormat.Rg32Uint,               8, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Rg32Sint,               8, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.Rg32Float,              8, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.Rgba16Unorm,            8, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.Rgba16Snorm,            8, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.Rgba16Uint,             8, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Rgba16Sint,             8, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.Rgba16Float,            8, 1, 1, PixelFormatKind.Float),
        // 128-Bit Pixel Formats
        new(PixelFormat.Rgba32Uint,            16, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.Rgba32Sint,            16, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.Rgba32Float,           16, 1, 1, PixelFormatKind.Float),
        // Depth-stencil formats
        new (PixelFormat.Stencil8,              1, 1, 1, PixelFormatKind.Unorm),
        new (PixelFormat.Depth16Unorm,          2, 1, 1, PixelFormatKind.Unorm),
        new (PixelFormat.Depth24UnormStencil8,  4, 1, 1, PixelFormatKind.Unorm),
        new (PixelFormat.Depth32Float,          4, 1, 1, PixelFormatKind.Float),
        new (PixelFormat.Depth32FloatStencil8,  8, 1, 1, PixelFormatKind.Float),
        // BC compressed formats
        new (PixelFormat.Bc1RgbaUnorm,          8, 4, 4,  PixelFormatKind.Unorm),
        new (PixelFormat.Bc1RgbaUnormSrgb,      8, 4, 4,  PixelFormatKind.UnormSrgb),
        new (PixelFormat.Bc2RgbaUnorm,          16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Bc2RgbaUnormSrgb,      16, 4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Bc3RgbaUnorm,          16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Bc3RgbaUnormSrgb,      16, 4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Bc4RUnorm,             8,  4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Bc4RSnorm,             8,  4, 4, PixelFormatKind.Snorm),
        new (PixelFormat.Bc5RgUnorm,            16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Bc5RgSnorm,            16, 4, 4, PixelFormatKind.Snorm),
        new (PixelFormat.Bc6hRgbUfloat,         16, 4, 4, PixelFormatKind.Float),
        new (PixelFormat.Bc6hRgbSfloat,         16, 4, 4, PixelFormatKind.Float),
        new (PixelFormat.Bc7RgbaUnorm,          16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Bc7RgbaUnormSrgb,      16, 4, 4, PixelFormatKind.UnormSrgb),
        // ETC2/EAC compressed formats
        new (PixelFormat.Etc2Rgb8Unorm,        8,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Etc2Rgb8UnormSrgb,    8,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Etc2Rgb8A1Unorm,     16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Etc2Rgb8A1UnormSrgb, 16,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Etc2Rgba8Unorm,      16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Etc2Rgba8UnormSrgb,  16,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.EacR11Unorm,         8,    4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.EacR11Snorm,         8,    4, 4, PixelFormatKind.Snorm),
        new (PixelFormat.EacRg11Unorm,        16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.EacRg11Snorm,        16,   4, 4, PixelFormatKind.Snorm),

        // ASTC compressed formats
        new (PixelFormat.Astc4x4Unorm,        16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Astc4x4UnormSrgb,    16,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc5x4Unorm,        16,   5, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Astc5x4UnormSrgb,    16,   5, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc5x5Unorm,        16,   5, 5, PixelFormatKind.Unorm),
        new (PixelFormat.Astc5x5UnormSrgb,    16,   5, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc6x5Unorm,        16,   6, 5, PixelFormatKind.Unorm),
        new (PixelFormat.Astc6x5UnormSrgb,    16,   6, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc6x6Unorm,        16,   6, 6, PixelFormatKind.Unorm),
        new (PixelFormat.Astc6x6UnormSrgb,    16,   6, 6, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc8x5Unorm,        16,   8, 5, PixelFormatKind.Unorm),
        new (PixelFormat.Astc8x5UnormSrgb,    16,   8, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc8x6Unorm,        16,   8, 6, PixelFormatKind.Unorm),
        new (PixelFormat.Astc8x6UnormSrgb,    16,   8, 6, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc8x8Unorm,        16,   8, 8, PixelFormatKind.Unorm),
        new (PixelFormat.Astc8x8UnormSrgb,    16,   8, 8, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc10x5Unorm,       16,   10, 5, PixelFormatKind.Unorm),
        new (PixelFormat.Astc10x5UnormSrgb,   16,   10, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc10x6Unorm,       16,   10, 6, PixelFormatKind.Unorm),
        new (PixelFormat.Astc10x6UnormSrgb,   16,   10, 6, PixelFormatKind.UnormSrgb ),
        new (PixelFormat.Astc10x8Unorm,       16,   10, 8, PixelFormatKind.Unorm),
        new (PixelFormat.Astc10x8UnormSrgb,   16,   10, 8, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc10x10Unorm,      16,   10, 10, PixelFormatKind.Unorm ),
        new (PixelFormat.Astc10x10UnormSrgb,  16,   10, 10, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc12x10Unorm,      16,   12, 10, PixelFormatKind.Unorm),
        new (PixelFormat.Astc12x10UnormSrgb,  16,   12, 10, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Astc12x12Unorm,      16,   12, 12, PixelFormatKind.Unorm),
        new (PixelFormat.Astc12x12UnormSrgb,  16,   12, 12, PixelFormatKind.UnormSrgb),
    };

    public static ref readonly PixelFormatInfo GetFormatInfo(this PixelFormat format)
    {
        if (format >= PixelFormat.Count)
        {
            return ref s_formatInfos[0]; // UNKNOWN
        }

        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return ref s_formatInfos[(int)format];
    }

    /// <summary>
    /// Get the number of bytes per format
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int GetFormatBytesPerBlock(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(uint)format].BytesPerBlock;
    }

    public static int GetFormatPixelsPerBlock(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(uint)format].BlockWidth * s_formatInfos[(uint)format].BlockHeight;
    }

    public static PixelFormatKind GetKind(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(uint)format].Kind;
    }

    public static bool IsInteger(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(int)format].Kind == PixelFormatKind.Uint || s_formatInfos[(int)format].Kind == PixelFormatKind.Sint;
    }

    public static bool IsSigned(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].Kind == PixelFormatKind.Sint;
    }

    public static bool IsSrgb(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].Kind == PixelFormatKind.UnormSrgb;
    }

    /// <summary>
    /// Check if the format is a compressed format.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static bool IsCompressedFormat(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].BlockWidth > 1;
    }

    /// <summary>
    /// Get the format compression ration along the x-axis.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int GetFormatWidthCompressionRatio(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].BlockWidth;
    }

    /// <summary>
    /// Get the format compression ration along the y-axis.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int GetFormatHeightCompressionRatio(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);

        return s_formatInfos[(int)format].BlockHeight;
    }

    /// <summary>
    /// Check if the format has a depth component.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has depth component, false otherwise.</returns>
    public static bool IsDepthFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.Depth16Unorm:
            case PixelFormat.Depth24UnormStencil8:
            case PixelFormat.Depth32Float:
            case PixelFormat.Depth32FloatStencil8:
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Check if the format has a stencil component.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has stencil component, false otherwise.</returns>
    public static bool IsStencilFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.Stencil8:
            case PixelFormat.Depth24UnormStencil8:
            case PixelFormat.Depth32FloatStencil8:
                return true;

            default:
                return false;
        }
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
    /// Get the number of bytes per row. If format is compressed, width should be evenly divisible by the compression ratio.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static int GetFormatRowPitch(this PixelFormat format, int width)
    {
        Guard.IsTrue(width % GetFormatWidthCompressionRatio(format) == 0);
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
            case PixelFormat.Rgba8UnormSrgb:
                return PixelFormat.Rgba8Unorm;
            case PixelFormat.Bgra8UnormSrgb:
                return PixelFormat.Bgra8Unorm;
            // Bc compressed formats
            case PixelFormat.Bc1RgbaUnormSrgb:
                return PixelFormat.Bc1RgbaUnorm;
            case PixelFormat.Bc2RgbaUnormSrgb:
                return PixelFormat.Bc2RgbaUnorm;
            case PixelFormat.Bc3RgbaUnormSrgb:
                return PixelFormat.Bc3RgbaUnorm;
            case PixelFormat.Bc7RgbaUnormSrgb:
                return PixelFormat.Bc7RgbaUnorm;

            // Etc2/Eac compressed formats
            case PixelFormat.Etc2Rgb8UnormSrgb:
                return PixelFormat.Etc2Rgb8Unorm;
            case PixelFormat.Etc2Rgb8A1UnormSrgb:
                return PixelFormat.Etc2Rgb8A1Unorm;
            case PixelFormat.Etc2Rgba8UnormSrgb:
                return PixelFormat.Etc2Rgba8Unorm;

            // Astc compressed formats
            case PixelFormat.Astc4x4UnormSrgb:
                return PixelFormat.Astc4x4Unorm;
            case PixelFormat.Astc5x4UnormSrgb:
                return PixelFormat.Astc5x4Unorm;
            case PixelFormat.Astc5x5UnormSrgb:
                return PixelFormat.Astc5x5Unorm;
            case PixelFormat.Astc6x5UnormSrgb:
                return PixelFormat.Astc6x5Unorm;
            case PixelFormat.Astc6x6UnormSrgb:
                return PixelFormat.Astc6x6Unorm;
            case PixelFormat.Astc8x5UnormSrgb:
                return PixelFormat.Astc8x5Unorm;
            case PixelFormat.Astc8x6UnormSrgb:
                return PixelFormat.Astc8x6Unorm;
            case PixelFormat.Astc8x8UnormSrgb:
                return PixelFormat.Astc8x8Unorm;
            case PixelFormat.Astc10x5UnormSrgb:
                return PixelFormat.Astc10x5Unorm;
            case PixelFormat.Astc10x6UnormSrgb:
                return PixelFormat.Astc10x6Unorm;
            case PixelFormat.Astc10x8UnormSrgb:
                return PixelFormat.Astc10x8Unorm;
            case PixelFormat.Astc10x10UnormSrgb:
                return PixelFormat.Astc10x10Unorm;
            case PixelFormat.Astc12x10UnormSrgb:
                return PixelFormat.Astc12x10Unorm;
            case PixelFormat.Astc12x12UnormSrgb:
                return PixelFormat.Astc12x12Unorm;

            default:
                Guard.IsFalse(IsSrgb(format));
                return format;
        }
    }

    /// <summary>
    /// Convert an linear format to sRGB. If the format doesn't have a matching sRGB format, will return the original
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static PixelFormat LinearToSrgbFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.Rgba8Unorm:
                return PixelFormat.Rgba8UnormSrgb;
            case PixelFormat.Bgra8Unorm:
                return PixelFormat.Bgra8UnormSrgb;

            // Bc compressed formats
            case PixelFormat.Bc1RgbaUnorm:
                return PixelFormat.Bc1RgbaUnormSrgb;
            case PixelFormat.Bc2RgbaUnorm:
                return PixelFormat.Bc2RgbaUnormSrgb;
            case PixelFormat.Bc3RgbaUnorm:
                return PixelFormat.Bc3RgbaUnormSrgb;
            case PixelFormat.Bc7RgbaUnorm:
                return PixelFormat.Bc7RgbaUnormSrgb;

            // Etc2/Eac compressed formats
            case PixelFormat.Etc2Rgb8Unorm:
                return PixelFormat.Etc2Rgb8UnormSrgb;
            case PixelFormat.Etc2Rgb8A1Unorm:
                return PixelFormat.Etc2Rgb8A1UnormSrgb;
            case PixelFormat.Etc2Rgba8Unorm:
                return PixelFormat.Etc2Rgba8UnormSrgb;

            // Astc compressed formats
            case PixelFormat.Astc4x4Unorm:
                return PixelFormat.Astc4x4UnormSrgb;
            case PixelFormat.Astc5x4Unorm:
                return PixelFormat.Astc5x4UnormSrgb;
            case PixelFormat.Astc5x5Unorm:
                return PixelFormat.Astc5x5UnormSrgb;
            case PixelFormat.Astc6x5Unorm:
                return PixelFormat.Astc6x5UnormSrgb;
            case PixelFormat.Astc6x6Unorm:
                return PixelFormat.Astc6x6UnormSrgb;
            case PixelFormat.Astc8x5Unorm:
                return PixelFormat.Astc8x5UnormSrgb;
            case PixelFormat.Astc8x6Unorm:
                return PixelFormat.Astc8x6UnormSrgb;
            case PixelFormat.Astc8x8Unorm:
                return PixelFormat.Astc8x8UnormSrgb;
            case PixelFormat.Astc10x5Unorm:
                return PixelFormat.Astc10x5UnormSrgb;
            case PixelFormat.Astc10x6Unorm:
                return PixelFormat.Astc10x6UnormSrgb;
            case PixelFormat.Astc10x8Unorm:
                return PixelFormat.Astc10x8UnormSrgb;
            case PixelFormat.Astc10x10Unorm:
                return PixelFormat.Astc10x10UnormSrgb;
            case PixelFormat.Astc12x10Unorm:
                return PixelFormat.Astc12x10UnormSrgb;
            case PixelFormat.Astc12x12Unorm:
                return PixelFormat.Astc12x12UnormSrgb;

            default:
                return format;
        }
    }
}
