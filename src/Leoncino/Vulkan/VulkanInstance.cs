// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;
using static Leoncino.Vulkan.VulkanUtils;

namespace Leoncino.Vulkan;

internal unsafe class VulkanInstance : GPUInstance
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);
    private readonly VkInstance _instance;
    private readonly VkDebugUtilsMessengerEXT _debugMessenger = VkDebugUtilsMessengerEXT.Null;

    public static bool IsSupported() => s_isSupported.Value;

    public VulkanInstance(in InstanceDescriptor descriptor)
        : base(descriptor)
    {
        uint instanceLayerCount = 0;
        vkEnumerateInstanceLayerProperties(&instanceLayerCount, null).DebugCheckResult();
        VkLayerProperties* availableInstanceLayers = stackalloc VkLayerProperties[(int)instanceLayerCount];
        vkEnumerateInstanceLayerProperties(&instanceLayerCount, availableInstanceLayers).DebugCheckResult();

        uint extensionCount = 0;
        vkEnumerateInstanceExtensionProperties(null, &extensionCount, null).CheckResult();
        VkExtensionProperties* availableInstanceExtensions = stackalloc VkExtensionProperties[(int)extensionCount];
        vkEnumerateInstanceExtensionProperties(null, &extensionCount, availableInstanceExtensions).CheckResult();

        List<string> instanceExtensions = [];
        List<string> instanceLayers = [];
        bool validationFeatures = false;

        for (int i = 0; i < extensionCount; i++)
        {
            string extensionName = availableInstanceExtensions[i].GetExtensionName();
            if (extensionName == VK_EXT_DEBUG_UTILS_EXTENSION_NAME)
            {
                DebugUtils = true;
                instanceExtensions.Add(VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
            }
            else if (extensionName == VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME)
            {
                instanceExtensions.Add(VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME);
            }
            else if (extensionName == VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME)
            {
                HasPortability = true;
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
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
        {
            instanceExtensions.Add(VK_EXT_METAL_SURFACE_EXTENSION_NAME);
        }

        //if (HasHeadlessSurface)
        //{
        //    instanceExtensions.Add(VK_EXT_HEADLESS_SURFACE_EXTENSION_NAME);
        //}

        if (descriptor.ValidationMode != ValidationMode.Disabled)
        {
            // Determine the optimal validation layers to enable that are necessary for useful debugging
            GetOptimalValidationLayers(availableInstanceLayers, instanceLayerCount, instanceLayers);
        }

        if (descriptor.ValidationMode == ValidationMode.GPU)
        {
            ReadOnlySpan<VkExtensionProperties> availableLayerInstanceExtensions = vkEnumerateInstanceExtensionProperties("VK_LAYER_KHRONOS_validation");
            for (int i = 0; i < availableLayerInstanceExtensions.Length; i++)
            {
                string extensionName = availableLayerInstanceExtensions[i].GetExtensionName();
                if (extensionName == VK_EXT_VALIDATION_FEATURES_EXTENSION_NAME)
                {
                    validationFeatures = true;
                    instanceExtensions.Add(VK_EXT_VALIDATION_FEATURES_EXTENSION_NAME);
                }
            }
        }

        VkDebugUtilsMessengerCreateInfoEXT debugUtilsCreateInfo = new();

        using VkString pApplicationName = new(Label);
        using VkString pEngineName = new("Vortice");

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
            pApplicationInfo = &appInfo,
            enabledLayerCount = vkLayerNames.Length,
            ppEnabledLayerNames = vkLayerNames,
            enabledExtensionCount = vkInstanceExtensions.Length,
            ppEnabledExtensionNames = vkInstanceExtensions
        };

        if (descriptor.ValidationMode != ValidationMode.Disabled && DebugUtils)
        {
            debugUtilsCreateInfo.messageSeverity = VkDebugUtilsMessageSeverityFlagsEXT.Error | VkDebugUtilsMessageSeverityFlagsEXT.Warning;
            debugUtilsCreateInfo.messageType = VkDebugUtilsMessageTypeFlagsEXT.Validation | VkDebugUtilsMessageTypeFlagsEXT.Performance;

            if (descriptor.ValidationMode == ValidationMode.Verbose)
            {
                debugUtilsCreateInfo.messageSeverity |= VkDebugUtilsMessageSeverityFlagsEXT.Verbose;
                debugUtilsCreateInfo.messageSeverity |= VkDebugUtilsMessageSeverityFlagsEXT.Info;
            }

            debugUtilsCreateInfo.pfnUserCallback = &DebugMessengerCallback;
            createInfo.pNext = &debugUtilsCreateInfo;
        }

        VkValidationFeaturesEXT validationFeaturesInfo = new();

        if (descriptor.ValidationMode == ValidationMode.GPU && validationFeatures)
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

        if (HasPortability)
        {
            createInfo.flags |= VkInstanceCreateFlags.EnumeratePortabilityKHR;
        }

        VkResult result = vkCreateInstance(&createInfo, null, out _instance);
        if (result != VkResult.Success)
        {
            throw new InvalidOperationException($"Failed to create vulkan instance: {result}");
        }
        vkLoadInstanceOnly(_instance);

        if (descriptor.ValidationMode != ValidationMode.Disabled && DebugUtils)
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
                string layerName = Interop.GetString(Interop.GetUtf8Span(createInfo.ppEnabledLayerNames[i]))!;
                Debug.WriteLine($"\t{layerName}");
            }
        }

        Debug.WriteLine($"Enabled {createInfo.enabledExtensionCount} Instance Extensions:");
        for (uint i = 0; i < createInfo.enabledExtensionCount; ++i)
        {
            string extensionName = Interop.GetString(Interop.GetUtf8Span(createInfo.ppEnabledExtensionNames[i]))!;
            Debug.WriteLine($"\t{extensionName}");
        }
#endif
    }

    public bool DebugUtils { get; }
    public bool HasPortability { get; }
    public bool HasHeadlessSurface { get; }
    public bool HasXlibSurface { get; }
    public bool HasXcbSurface { get; }
    public bool HasWaylandSurface { get; }
    public VkInstance Handle => _instance;

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanInstance" /> class.
    /// </summary>
    ~VulkanInstance() => Dispose(disposing: false);

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
    protected override GPUSurface CreateSurfaceCore(in SurfaceDescriptor descriptor) => new VulkanSurface(this, in descriptor);

    protected override ValueTask<GPUAdapter> RequestAdapterAsyncCore(in RequestAdapterOptions options)
    {
        ReadOnlySpan<VkPhysicalDevice> physicalDevices = vkEnumeratePhysicalDevices(_instance);

        if (physicalDevices.Length == 0)
        {
            throw new LeoncinoException("Vulkan: Failed to find GPUs with Vulkan support");
        }

        VkPhysicalDevice foundPhysicalDevice = VkPhysicalDevice.Null;
        foreach (VkPhysicalDevice physicalDevice in physicalDevices)
        {
            // We require minimum 1.2
            vkGetPhysicalDeviceProperties(physicalDevice, out VkPhysicalDeviceProperties physicalDeviceProperties);
            if (physicalDeviceProperties.apiVersion < VkVersion.Version_1_2)
            {
                continue;
            }

            PhysicalDeviceExtensions physicalDeviceExtensions = physicalDevice.QueryExtensions();
            if (!Headless && !physicalDeviceExtensions.Swapchain)
            {
                continue;
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
            throw new LeoncinoException("Vulkan: Failed to find a suitable GPU");
        }

        return ValueTask.FromResult<GPUAdapter>(new VulkanAdapter(foundPhysicalDevice));
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
        string message = new(pCallbackData->pMessage);
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
