// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;
using Alimer.Bindings.SDL;
using static Alimer.Bindings.SDL.SDL;
using static Alimer.Bindings.SDL.SDL_EventType;
using static Alimer.Bindings.SDL.SDL_WindowFlags;

namespace Leoncino.SampleFramework;

[Flags]
public enum WindowFlags
{
    None = 0,
    Fullscreen = 1 << 0,
    FullscreenDesktop = 1 << 1,
    Hidden = 1 << 2,
    Borderless = 1 << 3,
    Resizable = 1 << 4,
    Minimized = 1 << 5,
    Maximized = 1 << 6,
}

public sealed unsafe class Window
{
    private readonly SDL_Window _window;

    public unsafe Window(string title, int width, int height, WindowFlags flags = WindowFlags.None)
    {
        Title = title;

        bool fullscreen = false;
        SDL_WindowFlags windowFlags = SDL_WINDOW_HIDDEN;
        if ((flags & WindowFlags.Fullscreen) != WindowFlags.None)
        {
            windowFlags |= SDL_WINDOW_FULLSCREEN;
            fullscreen = true;
        }
        else if ((flags & WindowFlags.FullscreenDesktop) != WindowFlags.None)
        {
            windowFlags |= SDL_WINDOW_FULLSCREEN;
            fullscreen = true;
        }

        if (!fullscreen)
        {
            if ((flags & WindowFlags.Borderless) != WindowFlags.None)
            {
                windowFlags |= SDL_WINDOW_BORDERLESS;
            }

            if ((flags & WindowFlags.Resizable) != WindowFlags.None)
            {
                windowFlags |= SDL_WINDOW_RESIZABLE;
            }

            if ((flags & WindowFlags.Hidden) != WindowFlags.None)
            {
                windowFlags |= SDL_WINDOW_HIDDEN;
            }

            if ((flags & WindowFlags.Minimized) != WindowFlags.None)
            {
                windowFlags |= SDL_WINDOW_MINIMIZED;
            }

            if ((flags & WindowFlags.Maximized) != WindowFlags.None)
            {
                windowFlags |= SDL_WINDOW_MAXIMIZED;
            }
        }

        _window = SDL_CreateWindowWithPosition(title, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, width, height, windowFlags);
        //Handle = hwnd;

        SDL_GetWindowSizeInPixels(_window, out width, out height);
        ClientSize = new(width, height);
    }

    public string Title { get; }
    public Size ClientSize { get; }
    //public IntPtr Handle { get; }

    public void Show()
    {
        SDL_ShowWindow(_window);
    }
}
