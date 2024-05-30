// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal class VulkanAdapter : GPUAdapter
{
    private readonly VkPhysicalDeviceProperties _properties;

    public unsafe VulkanAdapter(VkPhysicalDevice handle)
    {
        Handle = handle;

        vkGetPhysicalDeviceProperties(handle, out _properties);
    }

    public VkPhysicalDevice Handle { get; }
    public ref readonly VkPhysicalDeviceProperties Properties => ref _properties;

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanAdapter" /> class.
    /// </summary>
    ~VulkanAdapter() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    /// <inheritdoc />
    public override PixelFormat GetSurfacePreferredFormat(GPUSurface surface)
    {
        VulkanSurface backendSurface = (VulkanSurface)surface;

        ReadOnlySpan<VkSurfaceFormatKHR> swapchainFormats = vkGetPhysicalDeviceSurfaceFormatsKHR(Handle, backendSurface.Handle);

        VkFormat wantedFormat = VkFormat.B8G8R8A8Srgb;

        bool valid = false;
        bool allowHDR = true;
        VkSurfaceFormatKHR foundFormat = default;
        foreach (VkSurfaceFormatKHR format in swapchainFormats)
        {
            if (!allowHDR && format.colorSpace != VkColorSpaceKHR.SrgbNonLinear)
                continue;

            if (format.format == wantedFormat)
            {
                foundFormat = format;
                valid = true;
                break;
            }
        }

        if (!valid)
        {
            foundFormat.format = VkFormat.B8G8R8A8Unorm;
            foundFormat.colorSpace = VkColorSpaceKHR.SrgbNonLinear;
        }

        // TODO:
        return PixelFormat.BGRA8UnormSrgb;
    }

    /// <inheritdoc />
    protected override ValueTask<GPUDevice> CreateDeviceAsyncCore(in DeviceDescriptor descriptor)
    {
        return ValueTask.FromResult<GPUDevice>(new VulkanDevice(this));
    }
}
