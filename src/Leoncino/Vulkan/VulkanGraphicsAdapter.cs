// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal class VulkanGraphicsAdapter : GraphicsAdapter
{
    private readonly VkPhysicalDeviceProperties _properties;
    private readonly VulkanPhysicalDeviceExtensions _extensions;
    public unsafe VulkanGraphicsAdapter(VkPhysicalDevice handle)
    {
        Handle = handle;

        vkGetPhysicalDeviceProperties(handle, out _properties);
        _extensions = handle.QueryExtensions();
    }

    public VkPhysicalDevice Handle { get; }
    public ref readonly VkPhysicalDeviceProperties Properties => ref _properties;
    public ref readonly VulkanPhysicalDeviceExtensions Extensions => ref _extensions;

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanGraphicsAdapter" /> class.
    /// </summary>
    ~VulkanGraphicsAdapter() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    /// <inheritdoc />
    public override PixelFormat GetSurfacePreferredFormat(GraphicsSurface surface)
    {
        VulkanGraphicsSurface backendSurface = (VulkanGraphicsSurface)surface;

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
    protected override GraphicsDevice CreateDeviceCore(in GraphicsDeviceDescriptor description)
    {
        return new VulkanGraphicsDevice(this, in description);
    }
}
