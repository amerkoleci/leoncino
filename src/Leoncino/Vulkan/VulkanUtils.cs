// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

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
}
