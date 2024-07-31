// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;
using static Leoncino.Vulkan.VulkanUtils;
using Win32;
using XenoAtom.Collections;

namespace Leoncino.Vulkan;

internal unsafe class VulkanGraphicsFactory : GraphicsFactory
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);
    private readonly VkInstance _instance;
    private readonly VkDebugUtilsMessengerEXT _debugMessenger = VkDebugUtilsMessengerEXT.Null;

    public static bool IsSupported() => s_isSupported.Value;

    public VulkanGraphicsFactory(in GraphicsFactoryDescription description)
        : base(description)
    {
        vkEnumerateInstanceLayerProperties(out uint availableInstanceLayerCount).CheckResult();
        Span<VkLayerProperties> availableInstanceLayers = stackalloc VkLayerProperties[(int)availableInstanceLayerCount];
        vkEnumerateInstanceLayerProperties(availableInstanceLayers).CheckResult();

        vkEnumerateInstanceExtensionProperties(out uint extensionCount).CheckResult();
        Span<VkExtensionProperties> availableInstanceExtensions = stackalloc VkExtensionProperties[(int)extensionCount];
        vkEnumerateInstanceExtensionProperties(availableInstanceExtensions).CheckResult();

        UnsafeList<VkUtf8String> instanceExtensions = [];
        UnsafeList<VkUtf8String> instanceLayers = [];
        bool validationFeatures = false;
        bool hasPortability = false;

        for (int i = 0; i < extensionCount; i++)
        {
            fixed (byte* pExtensionName = availableInstanceExtensions[i].extensionName)
            {
                VkUtf8String extensionName = new(pExtensionName);
                if (extensionName == VK_EXT_DEBUG_UTILS_EXTENSION_NAME)
                {
                    DebugUtils = true;
                    instanceExtensions.Add(VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
                }
                else if (extensionName == VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME)
                {
                    // Core 1.1
                    instanceExtensions.Add(VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME);
                }
                else if (extensionName == VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME)
                {
                    hasPortability = true;
                    instanceExtensions.Add(VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME);
                }
                else if (extensionName == VK_EXT_SWAPCHAIN_COLOR_SPACE_EXTENSION_NAME)
                {
                    instanceExtensions.Add(VK_EXT_SWAPCHAIN_COLOR_SPACE_EXTENSION_NAME);
                }
                else if (extensionName == VK_KHR_XLIB_SURFACE_EXTENSION_NAME)
                {
                    HasXlibSurface = true;
                }
                else if (extensionName == VK_KHR_XCB_SURFACE_EXTENSION_NAME)
                {
                    HasXcbSurface = true;
                }
                else if (extensionName == VK_KHR_WAYLAND_SURFACE_EXTENSION_NAME)
                {
                    HasWaylandSurface = true;
                }
                else if (extensionName == VK_EXT_HEADLESS_SURFACE_EXTENSION_NAME)
                {
                    HasHeadlessSurface = true;
                }
            }
        }
        instanceExtensions.Add(VK_KHR_SURFACE_EXTENSION_NAME);

        // Enable surface extensions depending on os
        if (OperatingSystem.IsAndroid())
        {
            instanceExtensions.Add(VK_KHR_ANDROID_SURFACE_EXTENSION_NAME);
        }
        else if (OperatingSystem.IsWindows())
        {
            instanceExtensions.Add(VK_KHR_WIN32_SURFACE_EXTENSION_NAME);
        }
        else if (OperatingSystem.IsLinux())
        {
            if (HasXlibSurface)
            {
                instanceExtensions.Add(VK_KHR_XLIB_SURFACE_EXTENSION_NAME);
            }
            else if (HasXcbSurface)
            {
                instanceExtensions.Add(VK_KHR_XCB_SURFACE_EXTENSION_NAME);
            }

            if (HasWaylandSurface)
            {
                instanceExtensions.Add(VK_KHR_WAYLAND_SURFACE_EXTENSION_NAME);
            }
        }
        else if (OperatingSystem.IsMacOS()
            || OperatingSystem.IsMacCatalyst()
            || OperatingSystem.IsIOS()
            || OperatingSystem.IsTvOS())
        {
            if (!hasPortability)
            {
                throw new GraphicsException($"Required {new VkUtf8ReadOnlyString(VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME).ToString()} is missing");
            }

            instanceExtensions.Add(VK_EXT_METAL_SURFACE_EXTENSION_NAME);
        }

        //if (HasHeadlessSurface)
        //{
        //    instanceExtensions.Add(VK_EXT_HEADLESS_SURFACE_EXTENSION_NAME);
        //}

        if (description.ValidationMode != ValidationMode.Disabled)
        {
            // Determine the optimal validation layers to enable that are necessary for useful debugging
            GetOptimalValidationLayers(ref instanceLayers, availableInstanceLayers);
        }

        if (description.ValidationMode == ValidationMode.GPU)
        {
            vkEnumerateInstanceExtensionProperties(VK_LAYER_KHRONOS_VALIDATION_EXTENSION_NAME, out uint propertyCount).CheckResult();
            Span<VkExtensionProperties> availableLayerInstanceExtensions = stackalloc VkExtensionProperties[(int)propertyCount];
            vkEnumerateInstanceExtensionProperties(VK_LAYER_KHRONOS_VALIDATION_EXTENSION_NAME, availableLayerInstanceExtensions).CheckResult();

            for (int i = 0; i < availableLayerInstanceExtensions.Length; i++)
            {
                fixed (byte* pExtensionName = availableLayerInstanceExtensions[i].extensionName)
                {
                    VkUtf8String extensionName = new(pExtensionName);

                    if (extensionName == VK_EXT_VALIDATION_FEATURES_EXTENSION_NAME)
                    {
                        validationFeatures = true;
                        instanceExtensions.Add(VK_EXT_VALIDATION_FEATURES_EXTENSION_NAME);
                    }
                }
            }
        }

        VkDebugUtilsMessengerCreateInfoEXT debugUtilsCreateInfo = new();

        VkUtf8ReadOnlyString pApplicationName = Label.GetUtf8Span();
        VkUtf8ReadOnlyString pEngineName = "Leoncino"u8;

        VkApplicationInfo appInfo = new()
        {
            pApplicationName = pApplicationName,
            applicationVersion = new VkVersion(1, 0, 0),
            pEngineName = pEngineName,
            engineVersion = new VkVersion(1, 0, 0),
            apiVersion = VkVersion.Version_1_3
        };

        using VkStringArray vkLayerNames = new(instanceLayers);
        using VkStringArray vkInstanceExtensions = new(instanceExtensions);

        VkInstanceCreateInfo createInfo = new()
        {
            flags = hasPortability ? VK_INSTANCE_CREATE_ENUMERATE_PORTABILITY_BIT_KHR : 0,
            pApplicationInfo = &appInfo,
            enabledLayerCount = vkLayerNames.Length,
            ppEnabledLayerNames = vkLayerNames,
            enabledExtensionCount = vkInstanceExtensions.Length,
            ppEnabledExtensionNames = vkInstanceExtensions
        };

        if (description.ValidationMode != ValidationMode.Disabled && DebugUtils)
        {
            debugUtilsCreateInfo.messageSeverity = VkDebugUtilsMessageSeverityFlagsEXT.Error | VkDebugUtilsMessageSeverityFlagsEXT.Warning;
            debugUtilsCreateInfo.messageType = VkDebugUtilsMessageTypeFlagsEXT.Validation | VkDebugUtilsMessageTypeFlagsEXT.Performance;

            if (description.ValidationMode == ValidationMode.Verbose)
            {
                debugUtilsCreateInfo.messageSeverity |= VkDebugUtilsMessageSeverityFlagsEXT.Verbose;
                debugUtilsCreateInfo.messageSeverity |= VkDebugUtilsMessageSeverityFlagsEXT.Info;
            }

            debugUtilsCreateInfo.pfnUserCallback = &DebugMessengerCallback;
            createInfo.pNext = &debugUtilsCreateInfo;
        }

        VkValidationFeaturesEXT validationFeaturesInfo = new();

        if (description.ValidationMode == ValidationMode.GPU && validationFeatures)
        {
            VkValidationFeatureEnableEXT* enabledValidationFeatures = stackalloc VkValidationFeatureEnableEXT[2]
            {
                VkValidationFeatureEnableEXT.GpuAssistedReserveBindingSlot,
                VkValidationFeatureEnableEXT.GpuAssisted
            };

            validationFeaturesInfo.enabledValidationFeatureCount = 2;
            validationFeaturesInfo.pEnabledValidationFeatures = enabledValidationFeatures;
            validationFeaturesInfo.pNext = createInfo.pNext;

            createInfo.pNext = &validationFeaturesInfo;
        }

        VkResult result = vkCreateInstance(&createInfo, null, out _instance);
        if (result != VkResult.Success)
        {
            throw new InvalidOperationException($"Failed to create vulkan instance: {result}");
        }
        vkLoadInstanceOnly(_instance);

        if (description.ValidationMode != ValidationMode.Disabled && DebugUtils)
        {
            vkCreateDebugUtilsMessengerEXT(_instance, &debugUtilsCreateInfo, null, out _debugMessenger).CheckResult();
        }

#if DEBUG
        Debug.WriteLine($"Created VkInstance with version: {appInfo.apiVersion.Major}.{appInfo.apiVersion.Minor}.{appInfo.apiVersion.Patch}");

        if (createInfo.enabledLayerCount > 0)
        {
            Debug.WriteLine($"Enabled {createInfo.enabledLayerCount} Validation Layers:");

            for (uint i = 0; i < createInfo.enabledLayerCount; ++i)
            {
                string layerName = VkStringInterop.ConvertToManaged(createInfo.ppEnabledLayerNames[i])!;
                Debug.WriteLine($"\t{layerName}");
            }
        }

        Debug.WriteLine($"Enabled {createInfo.enabledExtensionCount} Instance Extensions:");
        for (uint i = 0; i < createInfo.enabledExtensionCount; ++i)
        {
            string extensionName = VkStringInterop.ConvertToManaged(createInfo.ppEnabledExtensionNames[i])!;
            Debug.WriteLine($"\t{extensionName}");
        }
#endif
    }

    public bool DebugUtils { get; }
    public bool HasHeadlessSurface { get; }
    public bool HasXlibSurface { get; }
    public bool HasXcbSurface { get; }
    public bool HasWaylandSurface { get; }
    public VkInstance Handle => _instance;

    /// <inheritdoc />
    public override GraphicsBackend BackendType => GraphicsBackend.Vulkan;

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanGraphicsFactory" /> class.
    /// </summary>
    ~VulkanGraphicsFactory() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_debugMessenger.IsNotNull)
            {
                vkDestroyDebugUtilsMessengerEXT(_instance, _debugMessenger);
            }

            vkDestroyInstance(_instance);
        }
    }

    /// <inheritdoc />
    protected override GraphicsSurface CreateSurfaceCore(in SurfaceDescription description) => new VulkanGraphicsSurface(this, in description);

    protected override GraphicsAdapter RequestAdapterCore(in RequestAdapterOptions options)
    {
        VkResult result = vkEnumeratePhysicalDevices(_instance, out uint physicalDeviceCount);
        if (result != VK_SUCCESS || physicalDeviceCount == 0)
        {
            throw new GraphicsException("Vulkan: Failed to find GraphicsAdapter with Vulkan support");
        }

        Span<VkPhysicalDevice> physicalDevices = stackalloc VkPhysicalDevice[(int)physicalDeviceCount];
        vkEnumeratePhysicalDevices(_instance, physicalDevices);

        bool headless = false;
        VkPhysicalDevice foundPhysicalDevice = VkPhysicalDevice.Null;
        foreach (VkPhysicalDevice physicalDevice in physicalDevices)
        {
            // We require minimum 1.2
            vkGetPhysicalDeviceProperties(physicalDevice, out VkPhysicalDeviceProperties physicalDeviceProperties);
            if (physicalDeviceProperties.apiVersion < VkVersion.Version_1_2)
            {
                continue;
            }

            VulkanPhysicalDeviceExtensions physicalDeviceExtensions = physicalDevice.QueryExtensions();
            if (!headless && !physicalDeviceExtensions.Swapchain)
            {
                continue;
            }

            if (options.CompatibleSurface != null)
            {
                VulkanGraphicsSurface vulkanSurface = (VulkanGraphicsSurface)options.CompatibleSurface;
                VkSurfaceCapabilitiesKHR surfaceCapabilities;
                result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, vulkanSurface.Handle, &surfaceCapabilities);
                if (result != VK_SUCCESS)
                {
                    continue;
                }

                vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, out uint queueFamilyCount);
                Span<VkQueueFamilyProperties> queueFamilies = new VkQueueFamilyProperties[(int)queueFamilyCount];
                vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, queueFamilies);

                for (uint i = 0; i < queueFamilyCount; i++)
                {
                    // A graphics queue candidate must support present for us to select it.
                    if ((queueFamilies[(int)i].queueFlags & VK_QUEUE_GRAPHICS_BIT) != 0)
                    {
                        VkBool32 supported = false;
                        result = vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, i, vulkanSurface.Handle, &supported);
                        if (result != VK_SUCCESS && !supported)
                            continue;

                        // Device supports presenting to our surface, stop searching.
                        break;
                    }
                }
            }

            bool priority = physicalDeviceProperties.deviceType == VkPhysicalDeviceType.DiscreteGpu;
            if (options.PowerPreference == PowerPreference.LowPower)
            {
                priority = physicalDeviceProperties.deviceType == VkPhysicalDeviceType.IntegratedGpu;
            }

            if (priority || foundPhysicalDevice.IsNull)
            {
                foundPhysicalDevice = physicalDevice;
                if (priority)
                {
                    // If this is prioritized adapter, look no further.
                    break;
                }
            }
        }

        if (foundPhysicalDevice.IsNull)
        {
            throw new GraphicsException("Vulkan: Failed to find a suitable GPU");
        }

        return new VulkanGraphicsAdapter(foundPhysicalDevice);
    }

    private static bool CheckIsSupported()
    {
        try
        {
            VkResult result = vkInitialize();
            if (result != VkResult.Success)
            {
                return false;
            }

            VkVersion apiVersion = vkEnumerateInstanceVersion();
            if (apiVersion < VkVersion.Version_1_2)
            {
                //Log.Warn("Vulkan 1.2 is required!");
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }


    [UnmanagedCallersOnly]
    private static uint DebugMessengerCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity,
        VkDebugUtilsMessageTypeFlagsEXT messageTypes,
        VkDebugUtilsMessengerCallbackDataEXT* pCallbackData,
        void* userData)
    {
        string message = VkStringInterop.ConvertToManaged(pCallbackData->pMessage)!;
        if (messageTypes == VkDebugUtilsMessageTypeFlagsEXT.Validation)
        {
            if (messageSeverity == VkDebugUtilsMessageSeverityFlagsEXT.Error)
            {
                //Log.Error($"[Vulkan]: Validation: {messageSeverity} - {message}");
            }
            else if (messageSeverity == VkDebugUtilsMessageSeverityFlagsEXT.Warning)
            {
                //Log.Warn($"[Vulkan]: Validation: {messageSeverity} - {message}");
            }

            Debug.WriteLine($"[Vulkan]: Validation: {messageSeverity} - {message}");
        }
        else
        {
            if (messageSeverity == VkDebugUtilsMessageSeverityFlagsEXT.Error)
            {
                //Log.Error($"[Vulkan]: {messageSeverity} - {message}");
            }
            else if (messageSeverity == VkDebugUtilsMessageSeverityFlagsEXT.Warning)
            {
                //Log.Warn($"[Vulkan]: {messageSeverity} - {message}");
            }

            Debug.WriteLine($"[Vulkan]: {messageSeverity} - {message}");
        }

        return VK_FALSE;
    }
}
