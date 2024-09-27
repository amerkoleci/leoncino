// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal static unsafe partial class VulkanUtils
{
    private static readonly VkImageType[] s_vkImageTypeMap = [
        VK_IMAGE_TYPE_1D,
        VK_IMAGE_TYPE_2D,
        VK_IMAGE_TYPE_3D,
        VK_IMAGE_TYPE_2D,
    ];

    #region Layers Methods
    public static bool ValidateLayers(List<VkUtf8String> required, Span<VkLayerProperties> availableLayers)
    {
        foreach (VkUtf8String layer in required)
        {
            bool found = false;
            for (int i = 0; i < availableLayers.Length; i++)
            {
                fixed (byte* pLayerName = availableLayers[i].layerName)
                {
                    VkUtf8String availableLayer = new(pLayerName);

                    if (availableLayer == layer)
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                //Log.Warn("Validation Layer '{}' not found", layer);
                return false;
            }
        }

        return true;
    }

    public static void GetOptimalValidationLayers(ref List<VkUtf8String> instanceLayers, Span<VkLayerProperties> availableLayers)
    {
        // The preferred validation layer is "VK_LAYER_KHRONOS_validation"
        List<VkUtf8String> validationLayers =
        [
            VK_LAYER_KHRONOS_VALIDATION_EXTENSION_NAME
        ];
        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.Add(VK_LAYER_KHRONOS_VALIDATION_EXTENSION_NAME);
            return;
        }

        // Otherwise we fallback to using the LunarG meta layer
        validationLayers =
        [
            "VK_LAYER_LUNARG_standard_validation"u8
        ];
        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.Add("VK_LAYER_LUNARG_standard_validation"u8);
            return;
        }

        // Otherwise we attempt to enable the individual layers that compose the LunarG meta layer since it doesn't exist
        validationLayers =
        [
            "VK_LAYER_GOOGLE_threading"u8,
            "VK_LAYER_LUNARG_parameter_validation"u8,
            "VK_LAYER_LUNARG_object_tracker"u8,
            "VK_LAYER_LUNARG_core_validation"u8,
            "VK_LAYER_GOOGLE_unique_objects"u8,
        ];

        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.Add("VK_LAYER_GOOGLE_threading"u8);
            instanceLayers.Add("VK_LAYER_LUNARG_parameter_validation"u8);
            instanceLayers.Add("VK_LAYER_LUNARG_object_tracker"u8);
            instanceLayers.Add("VK_LAYER_LUNARG_core_validation"u8);
            instanceLayers.Add("VK_LAYER_GOOGLE_unique_objects"u8);
            return;
        }

        // Otherwise as a last resort we fallback to attempting to enable the LunarG core layer
        validationLayers =
        [
            "VK_LAYER_LUNARG_core_validation"u8
        ];

        if (ValidateLayers(validationLayers, availableLayers))
        {
            instanceLayers.Add("VK_LAYER_LUNARG_core_validation"u8);
            return;
        }
    }
    #endregion

    public static VulkanPhysicalDeviceExtensions QueryExtensions(this VkPhysicalDevice physicalDevice)
    {
        uint count = 0;
        VkResult result = vkEnumerateDeviceExtensionProperties(physicalDevice, null, &count, null);
        if (result != VkResult.Success)
            return default;

        VkExtensionProperties* vk_extensions = stackalloc VkExtensionProperties[(int)count];
        vkEnumerateDeviceExtensionProperties(physicalDevice, null, &count, vk_extensions);

        VulkanPhysicalDeviceExtensions extensions = new();

        for (int i = 0; i < count; ++i)
        {
            VkUtf8String extensionName = new(vk_extensions[i].extensionName);

            if (extensionName == VK_KHR_MAINTENANCE_4_EXTENSION_NAME)
            {
                extensions.Maintenance4 = true;
            }
            else if (extensionName == VK_KHR_DYNAMIC_RENDERING_EXTENSION_NAME)
            {
                extensions.DynamicRendering = true;
            }
            else if (extensionName == VK_KHR_SYNCHRONIZATION_2_EXTENSION_NAME)
            {
                extensions.Synchronization2 = true;
            }
            else if (extensionName == VK_EXT_EXTENDED_DYNAMIC_STATE_EXTENSION_NAME)
            {
                extensions.ExtendedDynamicState = true;
            }
            else if (extensionName == VK_EXT_EXTENDED_DYNAMIC_STATE_2_EXTENSION_NAME)
            {
                extensions.ExtendedDynamicState2 = true;
            }
            else if (extensionName == VK_EXT_PIPELINE_CREATION_CACHE_CONTROL_EXTENSION_NAME)
            {
                extensions.PipelineCreationCacheControl = true;
            }
            else if (extensionName == VK_KHR_FORMAT_FEATURE_FLAGS_2_EXTENSION_NAME)
            {
                extensions.FormatFeatureFlags2 = true;
            }
            else if (extensionName == VK_KHR_SWAPCHAIN_EXTENSION_NAME)
            {
                extensions.Swapchain = true;
            }
            else if (extensionName == VK_EXT_DEPTH_CLIP_ENABLE_EXTENSION_NAME)
            {
                extensions.DepthClipEnable = true;
            }
            else if (extensionName == VK_EXT_MEMORY_BUDGET_EXTENSION_NAME)
            {
                extensions.MemoryBudget = true;
            }
            else if (extensionName == VK_AMD_DEVICE_COHERENT_MEMORY_EXTENSION_NAME)
            {
                extensions.AMD_DeviceCoherentMemory = true;
            }
            else if (extensionName == VK_EXT_MEMORY_PRIORITY_EXTENSION_NAME)
            {
                extensions.MemoryPriority = true;
            }
            else if (extensionName == VK_KHR_PERFORMANCE_QUERY_EXTENSION_NAME)
            {
                extensions.PerformanceQuery = true;
            }
            else if (extensionName == VK_EXT_HOST_QUERY_RESET_EXTENSION_NAME)
            {
                extensions.HostQueryReset = true;
            }
            else if (extensionName == VK_KHR_DEFERRED_HOST_OPERATIONS_EXTENSION_NAME)
            {
                extensions.DeferredHostOperations = true;
            }
            else if (extensionName == VK_KHR_PORTABILITY_SUBSET_EXTENSION_NAME)
            {
                extensions.PortabilitySubset = true;
            }
            else if (extensionName == VK_KHR_ACCELERATION_STRUCTURE_EXTENSION_NAME)
            {
                extensions.AccelerationStructure = true;
            }
            else if (extensionName == VK_KHR_RAY_TRACING_PIPELINE_EXTENSION_NAME)
            {
                extensions.RaytracingPipeline = true;
            }
            else if (extensionName == VK_KHR_RAY_QUERY_EXTENSION_NAME)
            {
                extensions.RayQuery = true;
            }
            else if (extensionName == VK_KHR_FRAGMENT_SHADING_RATE_EXTENSION_NAME)
            {
                extensions.FragmentShadingRate = true;
            }
            else if (extensionName == VK_EXT_MESH_SHADER_EXTENSION_NAME)
            {
                extensions.MeshShader = true;
            }
            else if (extensionName == VK_EXT_CONDITIONAL_RENDERING_EXTENSION_NAME)
            {
                extensions.ConditionalRendering = true;
            }
            else if (extensionName == VK_KHR_VIDEO_QUEUE_EXTENSION_NAME)
            {
                extensions.Video.Queue = true;
            }
            else if (extensionName == VK_KHR_VIDEO_DECODE_QUEUE_EXTENSION_NAME)
            {
                extensions.Video.DecodeQueue = true;
            }
            else if (extensionName == VK_KHR_VIDEO_DECODE_H264_EXTENSION_NAME)
            {
                extensions.Video.DecodeH264 = true;
            }
            else if (extensionName == VK_KHR_VIDEO_DECODE_H265_EXTENSION_NAME)
            {
                extensions.Video.DecodeH265 = true;
            }
            else if (extensionName == VK_KHR_VIDEO_ENCODE_QUEUE_EXTENSION_NAME)
            {
                extensions.Video.EncodeQueue = true;
            }
            else if (extensionName == VK_KHR_VIDEO_ENCODE_H264_EXTENSION_NAME)
            {
                extensions.Video.EncodeH264 = true;
            }
            else if (extensionName == VK_KHR_VIDEO_ENCODE_H265_EXTENSION_NAME)
            {
                extensions.Video.EncodeH265 = true;
            }

            if (OperatingSystem.IsWindows())
            {
                if (extensionName == VK_EXT_FULL_SCREEN_EXCLUSIVE_EXTENSION_NAME)
                {
                    extensions.win32_full_screen_exclusive = true;
                }
                else if (extensionName == VK_KHR_EXTERNAL_MEMORY_WIN32_EXTENSION_NAME)
                {
                    extensions.ExternalMemory = true;
                }
                else if (extensionName == VK_KHR_EXTERNAL_SEMAPHORE_WIN32_EXTENSION_NAME)
                {
                    extensions.ExternalSemaphore = true;
                }
                else if (extensionName == VK_KHR_EXTERNAL_FENCE_WIN32_EXTENSION_NAME)
                {
                    extensions.ExternalFence = true;
                }
            }
            else
            {
                if (extensionName == VK_KHR_EXTERNAL_MEMORY_FD_EXTENSION_NAME)
                {
                    extensions.ExternalMemory = true;
                }
                else if (extensionName == VK_KHR_EXTERNAL_SEMAPHORE_FD_EXTENSION_NAME)
                {
                    extensions.ExternalSemaphore = true;
                }
                else if (extensionName == VK_KHR_EXTERNAL_FENCE_FD_EXTENSION_NAME)
                {
                    extensions.ExternalFence = true;
                }
            }
        }

        VkPhysicalDeviceProperties gpuProps;
        vkGetPhysicalDeviceProperties(physicalDevice, &gpuProps);

        // Core 1.3
        if (gpuProps.apiVersion >= VkVersion.Version_1_3)
        {
            extensions.Maintenance4 = true;
            extensions.DynamicRendering = true;
            extensions.Synchronization2 = true;
            extensions.ExtendedDynamicState = true;
            extensions.ExtendedDynamicState2 = true;
            extensions.PipelineCreationCacheControl = true;
            extensions.FormatFeatureFlags2 = true;
        }

        return extensions;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VkFormat ToVkFormat(this PixelFormat format)
    {
        switch (format)
        {
            // 8-bit formats
            case PixelFormat.R8Unorm: return VK_FORMAT_R8_UNORM;
            case PixelFormat.R8Snorm: return VK_FORMAT_R8_SNORM;
            case PixelFormat.R8Uint: return VK_FORMAT_R8_UINT;
            case PixelFormat.R8Sint: return VK_FORMAT_R8_SINT;

            // 16-bit formats
            case PixelFormat.R16Uint: return VK_FORMAT_R16_UINT;
            case PixelFormat.R16Sint: return VK_FORMAT_R16_SINT;
            case PixelFormat.R16Unorm: return VK_FORMAT_R16_UNORM;
            case PixelFormat.R16Snorm: return VK_FORMAT_R16_SNORM;
            case PixelFormat.R16Float: return VK_FORMAT_R16_SFLOAT;
            case PixelFormat.RG8Unorm: return VK_FORMAT_R8G8_UNORM;
            case PixelFormat.RG8Snorm: return VK_FORMAT_R8G8_SNORM;
            case PixelFormat.RG8Uint: return VK_FORMAT_R8G8_UINT;
            case PixelFormat.RG8Sint: return VK_FORMAT_R8G8_SINT;
            // Packed 16-Bit Pixel Formats
            case PixelFormat.BGRA4Unorm: return VK_FORMAT_B4G4R4A4_UNORM_PACK16;
            case PixelFormat.B5G6R5Unorm: return VK_FORMAT_B5G6R5_UNORM_PACK16;
            case PixelFormat.BGR5A1Unorm: return VK_FORMAT_B5G5R5A1_UNORM_PACK16;
            // 32-bit formats
            case PixelFormat.R32Uint: return VK_FORMAT_R32_UINT;
            case PixelFormat.R32Sint: return VK_FORMAT_R32_SINT;
            case PixelFormat.R32Float: return VK_FORMAT_R32_SFLOAT;
            case PixelFormat.RG16Uint: return VK_FORMAT_R16G16_UINT;
            case PixelFormat.RG16Sint: return VK_FORMAT_R16G16_SINT;
            case PixelFormat.RG16Unorm: return VK_FORMAT_R16G16_UNORM;
            case PixelFormat.RG16Snorm: return VK_FORMAT_R16G16_SNORM;
            case PixelFormat.RG16Float: return VK_FORMAT_R16G16_SFLOAT;
            case PixelFormat.RGBA8Unorm: return VK_FORMAT_R8G8B8A8_UNORM;
            case PixelFormat.RGBA8UnormSrgb: return VK_FORMAT_R8G8B8A8_SRGB;
            case PixelFormat.RGBA8Snorm: return VK_FORMAT_R8G8B8A8_SNORM;
            case PixelFormat.RGBA8Uint: return VK_FORMAT_R8G8B8A8_UINT;
            case PixelFormat.RGBA8Sint: return VK_FORMAT_R8G8B8A8_SINT;
            case PixelFormat.BGRA8Unorm: return VK_FORMAT_B8G8R8A8_UNORM;
            case PixelFormat.BGRA8UnormSrgb: return VK_FORMAT_B8G8R8A8_SRGB;
            // Packed 32-Bit formats
            case PixelFormat.RGB10A2Unorm: return VK_FORMAT_A2B10G10R10_UNORM_PACK32;
            case PixelFormat.RGB10A2Uint: return VK_FORMAT_A2R10G10B10_UINT_PACK32;
            case PixelFormat.RG11B10Ufloat: return VK_FORMAT_B10G11R11_UFLOAT_PACK32;
            case PixelFormat.RGB9E5Ufloat: return VK_FORMAT_E5B9G9R9_UFLOAT_PACK32;
            // 64-Bit formats
            case PixelFormat.RG32Uint: return VK_FORMAT_R32G32_UINT;
            case PixelFormat.RG32Sint: return VK_FORMAT_R32G32_SINT;
            case PixelFormat.RG32Float: return VK_FORMAT_R32G32_SFLOAT;
            case PixelFormat.RGBA16Uint: return VK_FORMAT_R16G16B16A16_UINT;
            case PixelFormat.RGBA16Sint: return VK_FORMAT_R16G16B16A16_SINT;
            case PixelFormat.RGBA16Unorm: return VK_FORMAT_R16G16B16A16_UNORM;
            case PixelFormat.RGBA16Snorm: return VK_FORMAT_R16G16B16A16_SNORM;
            case PixelFormat.RGBA16Float: return VK_FORMAT_R16G16B16A16_SFLOAT;
            // 128-Bit formats
            case PixelFormat.RGBA32Uint: return VK_FORMAT_R32G32B32A32_UINT;
            case PixelFormat.RGBA32Sint: return VK_FORMAT_R32G32B32A32_SINT;
            case PixelFormat.RGBA32Float: return VK_FORMAT_R32G32B32A32_SFLOAT;

            // Depth-stencil formats
            //case PixelFormat::Stencil8:
            //    return supportsS8 ? VK_FORMAT_S8_UINT : VK_FORMAT_D24_UNORM_S8_UINT;
            case PixelFormat.Depth16Unorm: return VK_FORMAT_D16_UNORM;
            case PixelFormat.Depth24UnormStencil8: return VK_FORMAT_D24_UNORM_S8_UINT;
            case PixelFormat.Depth32Float: return VK_FORMAT_D32_SFLOAT;
            case PixelFormat.Depth32FloatStencil8: return VK_FORMAT_D32_SFLOAT_S8_UINT;

            // Compressed BC formats
            case PixelFormat.BC1RGBAUnorm: return VK_FORMAT_BC1_RGBA_UNORM_BLOCK;
            case PixelFormat.BC1RGBAUnormSrgb: return VK_FORMAT_BC1_RGBA_SRGB_BLOCK;
            case PixelFormat.BC2RGBAUnorm: return VK_FORMAT_BC2_UNORM_BLOCK;
            case PixelFormat.BC2RGBAUnormSrgb: return VK_FORMAT_BC2_SRGB_BLOCK;
            case PixelFormat.BC3RGBAUnorm: return VK_FORMAT_BC3_UNORM_BLOCK;
            case PixelFormat.BC3RGBAUnormSrgb: return VK_FORMAT_BC3_SRGB_BLOCK;
            case PixelFormat.BC4RUnorm: return VK_FORMAT_BC4_UNORM_BLOCK;
            case PixelFormat.BC4RSnorm: return VK_FORMAT_BC4_SNORM_BLOCK;
            case PixelFormat.BC5RGUnorm: return VK_FORMAT_BC5_UNORM_BLOCK;
            case PixelFormat.BC5RGSnorm: return VK_FORMAT_BC5_SNORM_BLOCK;
            case PixelFormat.BC6HRGBUfloat: return VK_FORMAT_BC6H_UFLOAT_BLOCK;
            case PixelFormat.BC6HRGBFloat: return VK_FORMAT_BC6H_SFLOAT_BLOCK;
            case PixelFormat.BC7RGBAUnorm: return VK_FORMAT_BC7_UNORM_BLOCK;
            case PixelFormat.BC7RGBAUnormSrgb: return VK_FORMAT_BC7_SRGB_BLOCK;
            // EAC/ETC compressed formats
            case PixelFormat.ETC2RGB8Unorm: return VK_FORMAT_ETC2_R8G8B8_UNORM_BLOCK;
            case PixelFormat.ETC2RGB8UnormSrgb: return VK_FORMAT_ETC2_R8G8B8_SRGB_BLOCK;
            case PixelFormat.ETC2RGB8A1Unorm: return VK_FORMAT_ETC2_R8G8B8A1_UNORM_BLOCK;
            case PixelFormat.ETC2RGB8A1UnormSrgb: return VK_FORMAT_ETC2_R8G8B8A1_SRGB_BLOCK;
            case PixelFormat.ETC2RGBA8Unorm: return VK_FORMAT_ETC2_R8G8B8A8_UNORM_BLOCK;
            case PixelFormat.ETC2RGBA8UnormSrgb: return VK_FORMAT_ETC2_R8G8B8A8_SRGB_BLOCK;
            case PixelFormat.EACR11Unorm: return VK_FORMAT_EAC_R11_UNORM_BLOCK;
            case PixelFormat.EACR11Snorm: return VK_FORMAT_EAC_R11_SNORM_BLOCK;
            case PixelFormat.EACRG11Unorm: return VK_FORMAT_EAC_R11G11_UNORM_BLOCK;
            case PixelFormat.EACRG11Snorm: return VK_FORMAT_EAC_R11G11_SNORM_BLOCK;
            // ASTC compressed formats
            case PixelFormat.ASTC4x4Unorm: return VK_FORMAT_ASTC_4x4_UNORM_BLOCK;
            case PixelFormat.ASTC4x4UnormSrgb: return VK_FORMAT_ASTC_4x4_SRGB_BLOCK;
            case PixelFormat.ASTC5x4Unorm: return VK_FORMAT_ASTC_5x4_UNORM_BLOCK;
            case PixelFormat.ASTC5x4UnormSrgb: return VK_FORMAT_ASTC_5x4_SRGB_BLOCK;
            case PixelFormat.ASTC5x5Unorm: return VK_FORMAT_ASTC_5x5_UNORM_BLOCK;
            case PixelFormat.ASTC5x5UnormSrgb: return VK_FORMAT_ASTC_5x5_SRGB_BLOCK;
            case PixelFormat.ASTC6x5Unorm: return VK_FORMAT_ASTC_6x5_UNORM_BLOCK;
            case PixelFormat.ASTC6x5UnormSrgb: return VK_FORMAT_ASTC_6x5_SRGB_BLOCK;
            case PixelFormat.ASTC6x6Unorm: return VK_FORMAT_ASTC_6x6_UNORM_BLOCK;
            case PixelFormat.ASTC6x6UnormSrgb: return VK_FORMAT_ASTC_6x6_SRGB_BLOCK;
            case PixelFormat.ASTC8x5Unorm: return VK_FORMAT_ASTC_8x5_UNORM_BLOCK;
            case PixelFormat.ASTC8x5UnormSrgb: return VK_FORMAT_ASTC_8x5_SRGB_BLOCK;
            case PixelFormat.ASTC8x6Unorm: return VK_FORMAT_ASTC_8x6_UNORM_BLOCK;
            case PixelFormat.ASTC8x6UnormSrgb: return VK_FORMAT_ASTC_8x6_SRGB_BLOCK;
            case PixelFormat.ASTC8x8Unorm: return VK_FORMAT_ASTC_8x8_UNORM_BLOCK;
            case PixelFormat.ASTC8x8UnormSrgb: return VK_FORMAT_ASTC_8x8_SRGB_BLOCK;
            case PixelFormat.ASTC10x5Unorm: return VK_FORMAT_ASTC_10x5_UNORM_BLOCK;
            case PixelFormat.ASTC10x5UnormSrgb: return VK_FORMAT_ASTC_10x5_SRGB_BLOCK;
            case PixelFormat.ASTC10x6Unorm: return VK_FORMAT_ASTC_10x6_UNORM_BLOCK;
            case PixelFormat.ASTC10x6UnormSrgb: return VK_FORMAT_ASTC_10x6_SRGB_BLOCK;
            case PixelFormat.ASTC10x8Unorm: return VK_FORMAT_ASTC_10x8_UNORM_BLOCK;
            case PixelFormat.ASTC10x8UnormSrgb: return VK_FORMAT_ASTC_10x8_SRGB_BLOCK;
            case PixelFormat.ASTC10x10Unorm: return VK_FORMAT_ASTC_10x10_UNORM_BLOCK;
            case PixelFormat.ASTC10x10UnormSrgb: return VK_FORMAT_ASTC_10x10_SRGB_BLOCK;
            case PixelFormat.ASTC12x10Unorm: return VK_FORMAT_ASTC_12x10_UNORM_BLOCK;
            case PixelFormat.ASTC12x10UnormSrgb: return VK_FORMAT_ASTC_12x10_SRGB_BLOCK;
            case PixelFormat.ASTC12x12Unorm: return VK_FORMAT_ASTC_12x12_UNORM_BLOCK;
            case PixelFormat.ASTC12x12UnormSrgb: return VK_FORMAT_ASTC_12x12_SRGB_BLOCK;
            // ASTC HDR compressed formats
            case PixelFormat.ASTC4x4Hdr: return VK_FORMAT_ASTC_4x4_SFLOAT_BLOCK;
            case PixelFormat.ASTC5x4Hdr: return VK_FORMAT_ASTC_5x4_SFLOAT_BLOCK;
            case PixelFormat.ASTC5x5Hdr: return VK_FORMAT_ASTC_5x5_SFLOAT_BLOCK;
            case PixelFormat.ASTC6x5Hdr: return VK_FORMAT_ASTC_6x5_SFLOAT_BLOCK;
            case PixelFormat.ASTC6x6Hdr: return VK_FORMAT_ASTC_6x6_SFLOAT_BLOCK;
            case PixelFormat.ASTC8x5Hdr: return VK_FORMAT_ASTC_8x5_SFLOAT_BLOCK;
            case PixelFormat.ASTC8x6Hdr: return VK_FORMAT_ASTC_8x6_SFLOAT_BLOCK;
            case PixelFormat.ASTC8x8Hdr: return VK_FORMAT_ASTC_8x8_SFLOAT_BLOCK;
            case PixelFormat.ASTC10x5Hdr: return VK_FORMAT_ASTC_10x5_SFLOAT_BLOCK;
            case PixelFormat.ASTC10x6Hdr: return VK_FORMAT_ASTC_10x6_SFLOAT_BLOCK;
            case PixelFormat.ASTC10x8Hdr: return VK_FORMAT_ASTC_10x8_SFLOAT_BLOCK;
            case PixelFormat.ASTC10x10Hdr: return VK_FORMAT_ASTC_10x10_SFLOAT_BLOCK;
            case PixelFormat.ASTC12x10Hdr: return VK_FORMAT_ASTC_12x10_SFLOAT_BLOCK;
            case PixelFormat.ASTC12x12Hdr: return VK_FORMAT_ASTC_12x12_SFLOAT_BLOCK;
            //case PixelFormat.R8BG8Biplanar420Unorm: return VK_FORMAT_G8_B8R8_2PLANE_420_UNORM;
            //case PixelFormat.R10X6BG10X6Biplanar420Unorm: return VK_FORMAT_G10X6_B10X6R10X6_2PLANE_420_UNORM_3PACK16;

            default:
                return VkFormat.Undefined;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VkFormat ToVkFormat(this VertexFormat format)
    {
        switch (format)
        {
            case VertexFormat.UByte2: return VkFormat.R8G8Uint;
            case VertexFormat.UByte4: return VkFormat.R8G8B8A8Uint;
            case VertexFormat.Byte2: return VkFormat.R8G8Sint;
            case VertexFormat.Byte4: return VkFormat.R8G8B8A8Sint;
            case VertexFormat.UByte2Normalized: return VkFormat.R8G8Unorm;
            case VertexFormat.UByte4Normalized: return VkFormat.R8G8B8A8Unorm;
            case VertexFormat.Byte2Normalized: return VkFormat.R8G8Snorm;
            case VertexFormat.Byte4Normalized: return VkFormat.R8G8B8A8Snorm;

            case VertexFormat.UShort2: return VkFormat.R16G16Uint;
            case VertexFormat.UShort4: return VkFormat.R16G16B16A16Uint;
            case VertexFormat.Short2: return VkFormat.R16G16Sint;
            case VertexFormat.Short4: return VkFormat.R16G16B16A16Sint;
            case VertexFormat.UShort2Normalized: return VkFormat.R16G16Unorm;
            case VertexFormat.UShort4Normalized: return VkFormat.R16G16B16A16Unorm;
            case VertexFormat.Short2Normalized: return VkFormat.R16G16Snorm;
            case VertexFormat.Short4Normalized: return VkFormat.R16G16B16A16Snorm;
            case VertexFormat.Half2: return VkFormat.R16G16Sfloat;
            case VertexFormat.Half4: return VkFormat.R16G16B16A16Sfloat;

            case VertexFormat.Float: return VkFormat.R32Sfloat;
            case VertexFormat.Float2: return VkFormat.R32G32Sfloat;
            case VertexFormat.Float3: return VkFormat.R32G32B32Sfloat;
            case VertexFormat.Float4: return VkFormat.R32G32B32A32Sfloat;

            case VertexFormat.UInt: return VkFormat.R32Uint;
            case VertexFormat.UInt2: return VkFormat.R32G32Uint;
            case VertexFormat.UInt3: return VkFormat.R32G32B32Uint;
            case VertexFormat.UInt4: return VkFormat.R32G32B32A32Uint;

            case VertexFormat.Int: return VkFormat.R32Sint;
            case VertexFormat.Int2: return VkFormat.R32G32Sint;
            case VertexFormat.Int3: return VkFormat.R32G32B32Sint;
            case VertexFormat.Int4: return VkFormat.R32G32B32A32Sint;

            //case VertexFormat.Int1010102Normalized: return VkFormat.A2B10G10R10SnormPack32;
            case VertexFormat.UInt1010102Normalized: return VK_FORMAT_A2B10G10R10_UNORM_PACK32;
            case VertexFormat.RG11B10Float: return VK_FORMAT_B10G11R11_UFLOAT_PACK32;
            case VertexFormat.RGB9E5Float: return VK_FORMAT_E5B9G9R9_UFLOAT_PACK32;

            default:
                return VkFormat.Undefined;
        }
    }
}
