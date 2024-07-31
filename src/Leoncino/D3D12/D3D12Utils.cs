// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32.Graphics.Dxgi.Common;
using static Win32.Graphics.Dxgi.Common.Apis;

namespace Leoncino.D3D12;

internal static unsafe partial class D3D12Utils
{
    public static Format ToDxgiFormat(this VertexFormat format)
    {
        switch (format)
        {
            case VertexFormat.UByte2: return DXGI_FORMAT_R8G8_UINT;
            case VertexFormat.UByte4: return DXGI_FORMAT_R8G8B8A8_UINT;
            case VertexFormat.Byte2: return DXGI_FORMAT_R8G8_SINT;
            case VertexFormat.Byte4: return DXGI_FORMAT_R8G8B8A8_SINT;
            case VertexFormat.UByte2Normalized: return DXGI_FORMAT_R8G8_UNORM;
            case VertexFormat.UByte4Normalized: return DXGI_FORMAT_R8G8B8A8_UNORM;
            case VertexFormat.Byte2Normalized: return DXGI_FORMAT_R8G8_SNORM;
            case VertexFormat.Byte4Normalized: return DXGI_FORMAT_R8G8B8A8_SNORM;

            case VertexFormat.UShort2: return DXGI_FORMAT_R16G16_UINT;
            case VertexFormat.UShort4: return DXGI_FORMAT_R16G16B16A16_UINT;
            case VertexFormat.Short2: return DXGI_FORMAT_R16G16_SINT;
            case VertexFormat.Short4: return DXGI_FORMAT_R16G16B16A16_SINT;
            case VertexFormat.UShort2Normalized: return DXGI_FORMAT_R16G16_UNORM;
            case VertexFormat.UShort4Normalized: return DXGI_FORMAT_R16G16B16A16_UNORM;
            case VertexFormat.Short2Normalized: return DXGI_FORMAT_R16G16_SNORM;
            case VertexFormat.Short4Normalized: return DXGI_FORMAT_R16G16B16A16_SNORM;
            case VertexFormat.Half2: return DXGI_FORMAT_R16G16_FLOAT;
            case VertexFormat.Half4: return DXGI_FORMAT_R16G16B16A16_FLOAT;

            case VertexFormat.Float: return DXGI_FORMAT_R32_FLOAT;
            case VertexFormat.Float2: return DXGI_FORMAT_R32G32_FLOAT;
            case VertexFormat.Float3: return DXGI_FORMAT_R32G32B32_FLOAT;
            case VertexFormat.Float4: return DXGI_FORMAT_R32G32B32A32_FLOAT;

            case VertexFormat.UInt: return DXGI_FORMAT_R32_UINT;
            case VertexFormat.UInt2: return DXGI_FORMAT_R32G32_UINT;
            case VertexFormat.UInt3: return DXGI_FORMAT_R32G32B32_UINT;
            case VertexFormat.UInt4: return DXGI_FORMAT_R32G32B32A32_UINT;

            case VertexFormat.Int: return DXGI_FORMAT_R32_SINT;
            case VertexFormat.Int2: return DXGI_FORMAT_R32G32_SINT;
            case VertexFormat.Int3: return DXGI_FORMAT_R32G32B32_SINT;
            case VertexFormat.Int4: return DXGI_FORMAT_R32G32B32A32_SINT;

            case VertexFormat.UInt1010102Normalized: return DXGI_FORMAT_R10G10B10A2_UNORM;
            case VertexFormat.RG11B10Float: return DXGI_FORMAT_R11G11B10_FLOAT;
            case VertexFormat.RGB9E5Float: return DXGI_FORMAT_R9G9B9E5_SHAREDEXP;

            default:
                return DXGI_FORMAT_UNKNOWN;
        }
    }

}
