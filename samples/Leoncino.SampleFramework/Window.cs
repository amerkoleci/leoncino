// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;
using SDL;
using static SDL.SDL;

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

public sealed unsafe class Window
{
    private readonly SDL_Window _window;

    public unsafe Window(string title, int width, int height, WindowFlags flags = WindowFlags.None)
    {
        Title = title;

        SDL_WindowFlags sdl_flags = SDL_WindowFlags.HighPixelDensity | SDL_WindowFlags.Vulkan | SDL_WindowFlags.Hidden;
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

        SDL_SetWindowPosition(_window, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
        Id = SDL_GetWindowID(_window);
    }

    public string Title { get; }

    public SDL_WindowID Id { get; }

    public Surface? Surface { get; private set; }

    public Size ClientSize
    {
        get
        {
            SDL_GetWindowSize(_window, out int width, out int height);
            return new(width, height);
        }
    }
    public Size DrawableSize
    {
        get
        {
            SDL_GetWindowSizeInPixels(_window, out int width, out int height);
            return new(width, height);
        }
    }

    public void Show()
    {
        _ = SDL_ShowWindow(_window);
    }

    public void CreateSurface(Instance instance, bool useWayland = false)
    {
        SurfaceSource? source = default;
        if (OperatingSystem.IsWindows())
        {
            nint hwnd = SDL_GetProperty(SDL_GetWindowProperties(_window), "SDL.window.win32.hwnd");
            source = SurfaceSource.CreateWin32(hwnd);
        }
        else if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
        {
            NSWindow ns_window = new(SDL_GetProperty(SDL_GetWindowProperties(_window), "SDL.window.cocoa.window"));
            CAMetalLayer metal_layer = CAMetalLayer.New();
            ns_window.contentView.wantsLayer = true;
            ns_window.contentView.layer = metal_layer.Handle;

            source = SurfaceSource.CreateMetalLayer(metal_layer.Handle);
        }
        else if (OperatingSystem.IsLinux())
        {
            if (useWayland)
            {
                nint display = SDL_GetProperty(SDL_GetWindowProperties(_window), "SDL.window.wayland.display");
                nint surface = SDL_GetProperty(SDL_GetWindowProperties(_window), "SDL.window.wayland.surface");
                source = SurfaceSource.CreateWaylandSurface(display, surface);
            }
            else
            {
                nint display = SDL_GetProperty(SDL_GetWindowProperties(_window), "SDL.window.x11.display");
                ulong window = (ulong)SDL_GetProperty(SDL_GetWindowProperties(_window), "SDL.window.x11.window");
                source = SurfaceSource.CreateXlibWindow(display, window);
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

        Surface = instance.CreateSurface(in descriptor);
    }
}
