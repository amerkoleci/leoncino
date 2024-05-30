// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal static unsafe class VulkanUtils
{
    private static readonly VkImageType[] s_vkImageTypeMap = [
        VkImageType.Image1D,
        VkImageType.Image2D,
        VkImageType.Image3D,
        VkImageType.Image2D,
    ];

    #region Layers Methods
    public static bool ValidateLayers(List<string> required, VkLayerProperties* availableLayers, uint availableLayersCount)
    {
        foreach (string layer in required)
        {
            bool found = false;
            for (uint i = 0; i < availableLayersCount; i++)
            {
                string availableLayer = availableLayers[i].GetLayerName();

                if (availableLayer == layer)
                {
                    found = true;
                    break;
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

    public static void GetOptimalValidationLayers(VkLayerProperties* availableLayers, uint availableLayersCount, List<string> instanceLayers)
    {
        // The preferred validation layer is "VK_LAYER_KHRONOS_validation"
        List<string> validationLayers = new()
        {
            "VK_LAYER_KHRONOS_validation"
        };
        if (ValidateLayers(validationLayers, availableLayers, availableLayersCount))
        {
            instanceLayers.AddRange(validationLayers);
            return;
        }

        // Otherwise we fallback to using the LunarG meta layer
        validationLayers = new()
        {
            "VK_LAYER_LUNARG_standard_validation"
        };
        if (ValidateLayers(validationLayers, availableLayers, availableLayersCount))
        {
            instanceLayers.AddRange(validationLayers);
            return;
        }

        // Otherwise we attempt to enable the individual layers that compose the LunarG meta layer since it doesn't exist
        validationLayers = new()
        {
            "VK_LAYER_GOOGLE_threading",
            "VK_LAYER_LUNARG_parameter_validation",
            "VK_LAYER_LUNARG_object_tracker",
            "VK_LAYER_LUNARG_core_validation",
            "VK_LAYER_GOOGLE_unique_objects",
        };

        if (ValidateLayers(validationLayers, availableLayers, availableLayersCount))
        {
            instanceLayers.AddRange(validationLayers);
            return;
        }

        // Otherwise as a last resort we fallback to attempting to enable the LunarG core layer
        validationLayers = new()
        {
            "VK_LAYER_LUNARG_core_validation"
        };

        if (ValidateLayers(validationLayers, availableLayers, availableLayersCount))
        {
            instanceLayers.AddRange(validationLayers);
            return;
        }
    }
    #endregion

    public static PhysicalDeviceExtensions QueryExtensions(this VkPhysicalDevice physicalDevice)
    {
        uint count = 0;
        VkResult result = vkEnumerateDeviceExtensionProperties(physicalDevice, null, &count, null);
        if (result != VkResult.Success)
            return default;

        VkExtensionProperties* vk_extensions = stackalloc VkExtensionProperties[(int)count];
        vkEnumerateDeviceExtensionProperties(physicalDevice, null, &count, vk_extensions);

        PhysicalDeviceExtensions extensions = new();

        for (int i = 0; i < count; ++i)
        {
            string extensionName = vk_extensions[i].GetExtensionName();

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
                extensions.performance_query = true;
            }
            else if (extensionName == VK_EXT_HOST_QUERY_RESET_EXTENSION_NAME)
            {
                extensions.host_query_reset = true;
            }
            else if (extensionName == VK_KHR_DEFERRED_HOST_OPERATIONS_EXTENSION_NAME)
            {
                extensions.deferred_host_operations = true;
            }
            else if (extensionName == VK_KHR_PORTABILITY_SUBSET_EXTENSION_NAME)
            {
                extensions.PortabilitySubset = true;
            }
            else if (extensionName == VK_KHR_ACCELERATION_STRUCTURE_EXTENSION_NAME)
            {
                extensions.accelerationStructure = true;
            }
            else if (extensionName == VK_KHR_RAY_TRACING_PIPELINE_EXTENSION_NAME)
            {
                extensions.raytracingPipeline = true;
            }
            else if (extensionName == VK_KHR_RAY_QUERY_EXTENSION_NAME)
            {
                extensions.rayQuery = true;
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
                else if (extensionName == VK_KHR_EXTERNAL_SEMAPHORE_WIN32_EXTENSION_NAME)
                {
                    extensions.SupportsExternalSemaphore = true;
                }
                else if (extensionName == VK_KHR_EXTERNAL_MEMORY_WIN32_EXTENSION_NAME)
                {
                    extensions.SupportsExternalMemory = true;
                }
            }
            else
            {
                if (extensionName == VK_KHR_EXTERNAL_SEMAPHORE_FD_EXTENSION_NAME)
                {
                    extensions.SupportsExternalSemaphore = true;
                }
                else if (extensionName == VK_KHR_EXTERNAL_MEMORY_FD_EXTENSION_NAME)
                {
                    extensions.SupportsExternalMemory = true;
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

            //case PixelFormat.R8BG8Biplanar420Unorm: return VK_FORMAT_G8_B8R8_2PLANE_420_UNORM;
            //case PixelFormat.R10X6BG10X6Biplanar420Unorm: return VK_FORMAT_G10X6_B10X6R10X6_2PLANE_420_UNORM_3PACK16;

            default:
                return VkFormat.Undefined;
        }
    }

    internal struct PhysicalDeviceVideoExtensions
    {
        public bool Queue;
        public bool DecodeQueue;
        public bool DecodeH264;
        public bool DecodeH265;
        public bool EncodeQueue;
        public bool EncodeH264;
        public bool EncodeH265;
    }

    internal struct PhysicalDeviceExtensions
    {
        // Core in 1.3
        public bool Maintenance4;
        public bool DynamicRendering;
        public bool Synchronization2;
        public bool ExtendedDynamicState;
        public bool ExtendedDynamicState2;
        public bool PipelineCreationCacheControl;
        public bool FormatFeatureFlags2;

        // Extensions
        public bool Swapchain;
        public bool DepthClipEnable;
        public bool MemoryBudget;
        public bool AMD_DeviceCoherentMemory;
        public bool MemoryPriority;

        public bool SupportsExternalSemaphore;
        public bool SupportsExternalMemory;

        public bool performance_query;
        public bool host_query_reset;
        public bool deferred_host_operations;
        public bool PortabilitySubset;
        public bool accelerationStructure;
        public bool raytracingPipeline;
        public bool rayQuery;
        public bool FragmentShadingRate;
        public bool MeshShader;
        public bool ConditionalRendering;
        public bool win32_full_screen_exclusive;

        public readonly bool SupportsExternal => SupportsExternalSemaphore && SupportsExternalMemory;

        public PhysicalDeviceVideoExtensions Video;
    }
}
