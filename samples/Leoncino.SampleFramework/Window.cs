// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;
using SDL3;
using static SDL3.SDL3;
using static SDL3.SDL_EventType;
using System.Runtime.InteropServices;

namespace Leoncino.Samples;

[Flags]
public enum WindowFlags : uint
{
    None = 0,
    Fullscreen = 1 << 0,
    Borderless = 1 << 1,
    Resizable = 1 << 2,
    Minimized = 1 << 3,
    Maximized = 1 << 4,
}

public sealed unsafe partial class Window
{
    private readonly SDL_Window _window;

    public Window(GraphicsFactory factory, string title, int width, int height, WindowFlags flags = WindowFlags.None)
    {
        Title = title;

        SDL_WindowFlags sdl_flags = SDL_WindowFlags.Vulkan | SDL_WindowFlags.HighPixelDensity | SDL_WindowFlags.Hidden;
        if ((flags & WindowFlags.Fullscreen) != WindowFlags.None)
        {
            sdl_flags |= SDL_WindowFlags.Fullscreen;
        }
        else
        {
            if ((flags & WindowFlags.Borderless) != WindowFlags.None)
            {
                sdl_flags |= SDL_WindowFlags.Borderless;
            }

            if ((flags & WindowFlags.Resizable) != WindowFlags.None)
            {
                sdl_flags |= SDL_WindowFlags.Resizable;
            }

            if ((flags & WindowFlags.Minimized) != WindowFlags.None)
            {
                sdl_flags |= SDL_WindowFlags.Minimized;
            }

            if ((flags & WindowFlags.Maximized) != WindowFlags.None)
            {
                sdl_flags |= SDL_WindowFlags.Maximized;
            }
        }

        _window = SDL_CreateWindow(title, width, height, sdl_flags);
        if (_window.IsNull)
        {
            throw new Exception("SDL: failed to create window");
        }

        SDL_SetWindowPosition(_window, (int)SDL_WINDOWPOS_CENTERED, (int)SDL_WINDOWPOS_CENTERED);
        Id = SDL_GetWindowID(_window);
        Surface = CreateSurface(factory, _window);
    }

    public string Title { get; }

    public SDL_WindowID Id { get; }

    public GraphicsSurface Surface { get; }

    public Size ClientSize
    {
        get
        {
            _ = SDL_GetWindowSize(_window, out int width, out int height);
            return new(width, height);
        }
    }
    public Size DrawableSize
    {
        get
        {
            _ = SDL_GetWindowSizeInPixels(_window, out int width, out int height);
            return new(width, height);
        }
    }

    public void Show()
    {
        _ = SDL_ShowWindow(_window);
    }

    private static GraphicsSurface CreateSurface(GraphicsFactory factory, in SDL_Window window)
    {
        SurfaceSource? source = default;
        SDL_PropertiesID props = SDL_GetWindowProperties(window);
        if (OperatingSystem.IsWindows())
        {
            nint hwnd = SDL_GetPointerProperty(props, SDL_PROP_WINDOW_WIN32_HWND_POINTER, 0);
            source = SurfaceSource.CreateWin32(GetModuleHandleW(null), hwnd);
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
        {
            NSWindow ns_window = new(SDL_GetPointerProperty(props, SDL_PROP_WINDOW_COCOA_WINDOW_POINTER, 0));
            CAMetalLayer metal_layer = CAMetalLayer.New();
            ns_window.contentView.wantsLayer = true;
            ns_window.contentView.layer = metal_layer.Handle;

            source = SurfaceSource.CreateMetalLayer(metal_layer.Handle);
        }
        else if (OperatingSystem.IsLinux())
        {
            string? videoDriver = SDL_GetCurrentVideoDriver();
            if (videoDriver == "wayland")
            {
                nint display = SDL_GetPointerProperty(props, SDL_PROP_WINDOW_WAYLAND_DISPLAY_POINTER, 0);
                nint surface = SDL_GetPointerProperty(props, SDL_PROP_WINDOW_WAYLAND_SURFACE_POINTER, 0);
                source = SurfaceSource.CreateWaylandSurface(display, surface);
            }
            else if (videoDriver == "x11")
            {
                nint xlibDisplay = SDL_GetPointerProperty(props, SDL_PROP_WINDOW_X11_DISPLAY_POINTER, 0);
                ulong xlibWindow = (ulong)SDL_GetNumberProperty(props, SDL_PROP_WINDOW_X11_WINDOW_NUMBER, 0);
                source = SurfaceSource.CreateXlibWindow(xlibDisplay, xlibWindow);
            }
        }

        if (source is null)
        {
            throw new PlatformNotSupportedException();
        }

        SurfaceDescriptor descriptor = new()
        {
            Source = source!
        };

        return factory.CreateSurface(in descriptor);
    }

    [LibraryImport("kernel32")]
    private static partial nint GetModuleHandleW(ushort* lpModuleName);
}
