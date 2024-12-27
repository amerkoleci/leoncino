// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal unsafe partial class VulkanGraphicsSurface : GraphicsSurface
{
    private readonly VulkanGraphicsFactory _factory;

    public VulkanGraphicsSurface(VulkanGraphicsFactory factory, in SurfaceDescription description)
        : base(description)
    {
        _factory = factory;

        Handle = CreateVkSurface(description.Source);
        
        VkSurfaceKHR CreateVkSurface(SurfaceSource source)
        {
            VkSurfaceKHR vkSurface = VkSurfaceKHR.Null;

            switch (source)
            {
                case AndroidWindowSurfaceSource androidWindowSurface:
                    {
                        VkAndroidSurfaceCreateInfoKHR surfaceCreateInfo = new()
                        {
                            window = androidWindowSurface.Window,
                        };
                        vkCreateAndroidSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &vkSurface).CheckResult();
                        break;
                    }

                case MetalLayerSurfaceSource metalLayerSurfaceSource:
                    {
                        VkMetalSurfaceCreateInfoEXT surfaceCreateInfo = new()
                        {
                            pLayer = metalLayerSurfaceSource.Layer,
                        };
                        vkCreateMetalSurfaceEXT(_factory.Handle, &surfaceCreateInfo, null, &vkSurface).CheckResult();
                        break;
                    }

                case Win32SurfaceSource win32SurfaceSource:
                    {
                        VkWin32SurfaceCreateInfoKHR surfaceCreateInfo = new()
                        {
                            hinstance = win32SurfaceSource.HInstance,
                            hwnd = win32SurfaceSource.Hwnd
                        };

                        vkCreateWin32SurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &vkSurface).CheckResult();
                        break;
                    }

                case WaylandSurfaceSource waylandSurfaceSource:
                    {
                        VkWaylandSurfaceCreateInfoKHR surfaceCreateInfo = new()
                        {
                            display = waylandSurfaceSource.Display,
                            surface = waylandSurfaceSource.Surface,
                        };
                        vkCreateWaylandSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &vkSurface).CheckResult();
                        break;
                    }

                case XcbWindowSurfaceSource xcbWindowSurfaceSource:
                    {
                        VkXcbSurfaceCreateInfoKHR surfaceCreateInfo = new()
                        {
                            connection = xcbWindowSurfaceSource.Connection,
                            window = xcbWindowSurfaceSource.Window,
                        };
                        vkCreateXcbSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &vkSurface).CheckResult();
                        break;
                    }

                case XlibWindowSurfaceSource xlibWindowSurfaceSource:
                    {
                        VkXlibSurfaceCreateInfoKHR surfaceCreateInfo = new()
                        {
                            dpy = xlibWindowSurfaceSource.Display,
                            window = (nuint)xlibWindowSurfaceSource.Window,
                        };
                        vkCreateXlibSurfaceKHR(_factory.Handle, &surfaceCreateInfo, null, &vkSurface).CheckResult();
                        break;
                    }
            }

            return vkSurface;
        }
    }

    public VkSurfaceKHR Handle { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanGraphicsSurface" /> class.
    /// </summary>
    ~VulkanGraphicsSurface() => Dispose(disposing: false);

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
    public override bool GetCapabilites(GraphicsAdapter adapter, out SurfaceCapabilities capabilities)
    {
        VulkanGraphicsAdapter backendAdapter = (VulkanGraphicsAdapter)adapter;

        VkSurfaceCapabilitiesKHR surfaceCaps;
        VkResult result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(backendAdapter.Handle, Handle, &surfaceCaps);
        if (result != VK_SUCCESS)
        {
            capabilities = default;
            return false;
        }

        result = vkGetPhysicalDeviceSurfaceFormatsKHR(backendAdapter.Handle, Handle, out uint formatCount);
        if (result != VK_SUCCESS)
        {
            capabilities = default;
            return false;
        }

        Span<VkSurfaceFormatKHR> vkFormats = stackalloc VkSurfaceFormatKHR[(int)formatCount];
        result = vkGetPhysicalDeviceSurfaceFormatsKHR(backendAdapter.Handle, Handle, vkFormats);
        if (result != VK_SUCCESS)
        {
            capabilities = default;
            return false;
        }

        capabilities = new SurfaceCapabilities
        {
            PreferredFormat = PixelFormat.BGRA8UnormSrgb,
            SupportedUsage = TextureUsage.ShaderRead | TextureUsage.RenderTarget
        };
        return true;
    }

    /// <inheritdoc />
    protected override void ConfigureCore(GraphicsDevice device, in SurfaceConfiguration configuration)
    {
        VulkanGraphicsDevice backendDevice = (VulkanGraphicsDevice)device;

        PixelFormat format = configuration.Format == PixelFormat.Undefined ? backendDevice.Adapter.GetSurfacePreferredFormat(this) : configuration.Format;
    }
}
