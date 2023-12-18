// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// A platform-specific object representing a renderable surface.
/// A SwapchainSource can be created with one of several static factory methods.
/// </summary>
public abstract class SurfaceSource
{
    protected SurfaceSource()
    {
    }

    /// <summary>
    /// Creates a new SurfaceSource for a MetalLayer.
    /// </summary>
    /// <param name="window">The ANativeWindow window.</param>
    /// <returns>A new SurfaceSource which can be used to create a <see cref="Surface"/> for the given android window.
    /// </returns>
    public static SurfaceSource CreateAndroidWindow(nint window) => new AndroidWindowSurfaceSource(window);

    /// <summary>
    /// Creates a new SurfaceSource for a MetalLayer.
    /// </summary>
    /// <param name="hwnd">The metal layer handle.</param>
    /// <returns>A new SurfaceSource which can be used to create a <see cref="Surface"/> for the given metal layer.
    /// </returns>
    public static SurfaceSource CreateMetalLayer(nint layer) => new Win32SurfaceSource(layer);

    /// <summary>
    /// Creates a new SurfaceSource for a Win32 window.
    /// </summary>
    /// <param name="hwnd">The Win32 window handle.</param>
    /// <returns>A new SurfaceSource which can be used to create a <see cref="Surface"/> for the given Win32 window.
    /// </returns>
    public static SurfaceSource CreateWin32(nint hwnd) => new Win32SurfaceSource(hwnd);

    /// <summary>
    /// Creates a new SurfaceSource for a SwapChain panel.
    /// </summary>
    /// <param name="swapChainPanel"></param>
    /// <param name="logicalDpi"></param>
    /// <returns>A new SurfaceSource which can be used to create a <see cref="Surface"/> for the given WinUI SwapChainPanel.
    public static SurfaceSource CreateSwapChainPanel(object swapChainPanel, float logicalDpi) => new SwapChainPanelSurfaceSource(swapChainPanel, logicalDpi);

    /// <summary>
    /// Creates a new SurfaceSource from the given Wayland information.
    /// </summary>
    /// <param name="display">The Wayland display proxy.</param>
    /// <param name="surface">The Wayland surface proxy to map.</param>
    /// <returns>A new SurfaceSource which can be used to create a <see cref="Surface"/> for the given Wayland surface.
    /// </returns>
    public static SurfaceSource CreateWaylandSurface(nint display, nint surface) => new WaylandSurfaceSource(display, surface);

    /// <summary>
    /// Creates a new SurfaceSource from the given xcb window.
    /// </summary>
    /// <param name="connection">The xcb connection.</param>
    /// <param name="window">The xcb window.</param>
    /// <returns>A new SurfaceSource which can be used to create a <see cref="Surface"/> for the given xcb window.
    /// </returns>
    public static SurfaceSource CreateXcbWindow(nint connection, uint window) => new XcbWindowSurfaceSource(connection, window);

    /// <summary>
    /// Creates a new SurfaceSource from the given xlib window.
    /// </summary>
    /// <param name="display">The xlib display.</param>
    /// <param name="window">The xlib window.</param>
    /// <returns>A new SurfaceSource which can be used to create a <see cref="Surface"/> for the given xlib window.
    /// </returns>
    public static SurfaceSource CreateXlibWindow(nint display, ulong window) => new XlibWindowSurfaceSource(display, window);
}

internal class AndroidWindowSurfaceSource(nint window) : SurfaceSource
{
    public nint Window { get; } = window;
}

internal class MetalLayerSurfaceSource(nint layer) : SurfaceSource
{
    public nint Layer { get; } = layer;
}

internal class Win32SurfaceSource(nint hwnd) : SurfaceSource
{
    public nint Hwnd { get; } = hwnd;
}

internal class SwapChainPanelSurfaceSource(object swapChainPanelNative, float logicalDpi) : SurfaceSource
{
    public object SwapChainPanelNative { get; } = swapChainPanelNative;
    public float LogicalDpi { get; } = logicalDpi;
}

internal class WaylandSurfaceSource(nint display, nint surface) : SurfaceSource
{
    public nint Display { get; } = display;
    public nint Surface { get; } = surface;
}

internal class XcbWindowSurfaceSource(nint connection, uint window) : SurfaceSource
{
    public nint Connection { get; } = connection;
    public uint Window { get; } = window;
}

internal class XlibWindowSurfaceSource(nint display, ulong window) : SurfaceSource
{
    public nint Display { get; } = display;
    public ulong Window { get; } = window;
}
