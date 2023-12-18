// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;
using SDL;
using static SDL.SDL;

namespace Leoncino.Samples;

[Flags]
public enum WindowFlags
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
        if(_window.IsNull)
        {
            throw new Exception("SDL: failed to create window");
        }

        SDL_SetWindowPosition(_window, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
        Id = SDL_GetWindowID(_window);
    }

    public string Title { get; }

    public SDL_WindowID Id { get; }

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
        SDL_ShowWindow(_window);
    }
}
