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
        // TODO:
        return PixelFormat.BGRA8UnormSrgb;
    }

    /// <inheritdoc />
    protected override ValueTask<GPUDevice> CreateDeviceAsyncCore(in DeviceDescriptor descriptor)
    {
        return ValueTask.FromResult<GPUDevice>(new VulkanDevice(this));
    }
}
