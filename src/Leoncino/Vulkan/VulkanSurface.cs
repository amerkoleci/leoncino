// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal unsafe partial class VulkanSurface : GPUSurface
{
    private readonly VulkanGraphicsFactory _factory;

    public VulkanSurface(VulkanGraphicsFactory factory, in SurfaceDescriptor descriptor)
        : base(descriptor)
    {
        _factory = factory;

        VkResult result = VkResult.ErrorInitializationFailed;
        VkSurfaceKHR surface = VkSurfaceKHR.Null;

        switch (descriptor.Source)
        {
            case AndroidWindowSurfaceSource androidWindowSurface:
                {
                    VkAndroidSurfaceCreateInfoKHR surfaceCreateInfo = new()
                    {
                        window = androidWindowSurface.Window,
                    };
                    result = vkCreateAndroidSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &surface);
                    break;
                }

            case MetalLayerSurfaceSource metalLayerSurfaceSource:
                {
                    VkMetalSurfaceCreateInfoEXT surfaceCreateInfo = new()
                    {
                        pLayer = metalLayerSurfaceSource.Layer,
                    };
                    result = vkCreateMetalSurfaceEXT(_factory.Handle, &surfaceCreateInfo, null, &surface);
                    break;
                }

            case Win32SurfaceSource win32SurfaceSource:
                {
                    VkWin32SurfaceCreateInfoKHR surfaceCreateInfo = new()
                    {
                        hinstance = GetModuleHandleW(null),
                        hwnd = win32SurfaceSource.Hwnd
                    };

                    result = vkCreateWin32SurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &surface);
                    break;
                }

            case WaylandSurfaceSource waylandSurfaceSource:
                {
                    VkWaylandSurfaceCreateInfoKHR surfaceCreateInfo = new()
                    {
                        display = waylandSurfaceSource.Display,
                        surface = waylandSurfaceSource.Surface,
                    };
                    result = vkCreateWaylandSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &surface);
                    break;
                }

            case XcbWindowSurfaceSource xcbWindowSurfaceSource:
                {
                    VkXcbSurfaceCreateInfoKHR surfaceCreateInfo = new()
                    {
                        connection = xcbWindowSurfaceSource.Connection,
                        window = xcbWindowSurfaceSource.Window,
                    };
                    result = vkCreateXcbSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &surface);
                    break;
                }

            case XlibWindowSurfaceSource xlibWindowSurfaceSource:
                {
                    VkXlibSurfaceCreateInfoKHR surfaceCreateInfo = new()
                    {
                        display = xlibWindowSurfaceSource.Display,
                        window = (nuint)xlibWindowSurfaceSource.Window,
                    };
                    result = vkCreateXlibSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &surface);
                    break;
                }
        }

        result.CheckResult();
        Handle = surface;
    }

    public VkSurfaceKHR Handle { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanSurface" /> class.
    /// </summary>
    ~VulkanSurface() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (IsConfigured)
            {
                // TODO: Destroy Swapchain
            }

            vkDestroySurfaceKHR(_factory.Handle, Handle, null);
        }
    }

    /// <inheritdoc />
    protected override void ConfigureCore(GPUDevice device, in SurfaceConfiguration configuration)
    {
        VulkanDevice backendDevice = (VulkanDevice)device;

        PixelFormat format = configuration.Format == PixelFormat.Undefined ? backendDevice.Adapter.GetSurfacePreferredFormat(this) : configuration.Format;
    }

    [LibraryImport("kernel32")]
    private static partial nint GetModuleHandleW(ushort* lpModuleName);
}
