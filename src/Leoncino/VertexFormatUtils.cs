// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Leoncino;

public enum VertexFormatBaseType
{
    /// <summary>
    /// Unsigned normalized formats.
    /// </summary>
    Unorm,
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

public readonly record struct VertexFormatInfo(
    VertexFormat Format,
    uint ByteSize,
    uint ComponentCount,
    uint ComponentByteSize,
    VertexFormatBaseType BaseType
);

public static class VertexFormatUtils
{
    private static readonly VertexFormatInfo[] s_vertexFormatInfos = new VertexFormatInfo[]
   {
        new(VertexFormat.Undefined, 0, 0, 0,    VertexFormatBaseType.Float),
        new(VertexFormat.UByte2,     2, 2, 1,  VertexFormatBaseType.Uint),
        new(VertexFormat.UByte4,     4, 4, 1,  VertexFormatBaseType.Uint),
        new(VertexFormat.Byte2,     2, 2, 1,  VertexFormatBaseType.Sint),
        new(VertexFormat.Byte4,     4, 4, 1,  VertexFormatBaseType.Sint),
        new(VertexFormat.UByte2Normalized,    2, 2, 1,  VertexFormatBaseType.Unorm),
        new(VertexFormat.UByte4Normalized,    4, 4, 1,  VertexFormatBaseType.Unorm),
        new(VertexFormat.Byte2Normalized,    2, 2, 1,  VertexFormatBaseType.Snorm),
        new(VertexFormat.Byte4Normalized,    4, 4, 1,  VertexFormatBaseType.Snorm),
        new(VertexFormat.UShort2,    4, 2, 2,  VertexFormatBaseType.Uint),
        new(VertexFormat.UShort4,    8, 4, 2,  VertexFormatBaseType.Uint),
        new(VertexFormat.Short2,    4, 2, 2,  VertexFormatBaseType.Sint),
        new(VertexFormat.Short4,    8, 4, 2,  VertexFormatBaseType.Sint),
        new(VertexFormat.UShort2Normalized,   4, 2, 2, VertexFormatBaseType.Unorm),
        new(VertexFormat.UShort4Normalized,   8, 4, 2,  VertexFormatBaseType.Unorm),
        new(VertexFormat.Short2Normalized,   4, 2, 2,  VertexFormatBaseType.Snorm),
        new(VertexFormat.Short4Normalized,   8, 4, 2,  VertexFormatBaseType.Snorm),
        new(VertexFormat.Half2,   4, 2, 2,  VertexFormatBaseType.Float),
        new(VertexFormat.Half4,   8, 4, 2,  VertexFormatBaseType.Float),
        new(VertexFormat.Float,     4, 1, 4,  VertexFormatBaseType.Float),
        new(VertexFormat.Float2,   8, 2, 4,  VertexFormatBaseType.Float),
        new(VertexFormat.Float3,   12, 3, 4, VertexFormatBaseType.Float),
        new(VertexFormat.Float4,   16, 4, 4, VertexFormatBaseType.Float),
        new(VertexFormat.UInt,      4, 1, 4, VertexFormatBaseType.Uint),
        new(VertexFormat.UInt2,    8, 2, 4, VertexFormatBaseType.Uint),
        new(VertexFormat.UInt3,    12, 3, 4, VertexFormatBaseType.Uint),
        new(VertexFormat.UInt4,    16, 4, 4, VertexFormatBaseType.Uint),
        new(VertexFormat.Int,      4, 1, 4, VertexFormatBaseType.Sint),
        new(VertexFormat.Int2,    8, 2, 4, VertexFormatBaseType.Sint),
        new(VertexFormat.Int3,    12, 3, 4, VertexFormatBaseType.Sint),
        new(VertexFormat.Int4,    16, 4, 4, VertexFormatBaseType.Sint),

        new (VertexFormat.Int1010102Normalized, 32, 4, 4, VertexFormatBaseType.Unorm),
        new (VertexFormat.UInt1010102Normalized, 32, 4, 4, VertexFormatBaseType.Uint),
   };

    public static ref readonly VertexFormatInfo GetFormatInfo(this VertexFormat format)
    {
        if (format >= VertexFormat.Count)
        {
            return ref s_vertexFormatInfos[0]; // Undefines
        }

        Guard.IsTrue(s_vertexFormatInfos[(uint)format].Format == format);
        return ref s_vertexFormatInfos[(uint)format];
    }

    public static uint GetSizeInBytes(this VertexFormat format) => GetFormatInfo(format).ByteSize;
}
