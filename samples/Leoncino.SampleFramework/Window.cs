// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;
using Alimer.Bindings.SDL;
using static Alimer.Bindings.SDL.SDL;
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

    public Window(string title, int width, int height, WindowFlags flags = WindowFlags.None)
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

        // Native handle
        SDL_SysWMinfo wmInfo = default;
        SDL_GetWindowWMInfo(_window, &wmInfo);

        // Window handle is selected per subsystem as defined at:
        // https://wiki.libsdl.org/SDL_SysWMinfo
        switch (wmInfo.subsystem)
        {
            case SDL_SYSWM_TYPE.SDL_SYSWM_WINDOWS:
                SurfaceSource = SurfaceSource.CreateWin32(wmInfo.info.win.hinstance, wmInfo.info.win.window);
                break;

            case SDL_SYSWM_TYPE.SDL_SYSWM_WINRT:
                //Surface = SwapChainSurface.CreateCoreWindow(wmInfo.info.winrt.window);
                break;

            case SDL_SYSWM_TYPE.SDL_SYSWM_X11:
                //return wmInfo.info.x11.window;
                break;

            case SDL_SYSWM_TYPE.SDL_SYSWM_COCOA:
                //return wmInfo.info.cocoa.window;
                break;

            case SDL_SYSWM_TYPE.SDL_SYSWM_UIKIT:
                //return wmInfo.info.uikit.window;
                break;

            case SDL_SYSWM_TYPE.SDL_SYSWM_WAYLAND:
                //return wmInfo.info.wl.shell_surface;
                break;

            case SDL_SYSWM_TYPE.SDL_SYSWM_ANDROID:
                //return wmInfo.info.android.window;
                break;

            default:
                break;
        }
    }

    public string Title { get; }
    public Size ClientSize { get; }
    public SurfaceSource? SurfaceSource { get; }

    public void Show()
    {
        SDL_ShowWindow(_window);
    }
}
