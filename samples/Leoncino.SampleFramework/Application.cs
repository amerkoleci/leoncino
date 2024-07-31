// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using System.Drawing;
using SDL;
using static SDL.SDL3;
using static SDL.SDL_LogPriority;
using static SDL.SDL_EventType;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Leoncino.Samples;

public abstract class Application : IDisposable
{
    private volatile uint _isDisposed = 0;
    private bool _closeRequested = false;

    protected unsafe Application()
    {
        if (SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO) != 0)
        {
            var error = SDL_GetError();
            throw new Exception($"Failed to start SDL2: {error}");
        }

        SDL_SetLogOutputFunction(&Log_SDL, IntPtr.Zero);
        GraphicsFactoryDescription factoryDescription = new()
        {
            PreferredBackend = GraphicsBackend.Vulkan
        };
        Factory = GraphicsFactory.Create(in factoryDescription);

        // Create main window.
        MainWindow = new Window(Factory, Name, 1280, 720);

        RequestAdapterOptions requestAdapterOptions = new()
        {
            CompatibleSurface = MainWindow.Surface,
            PowerPreference = PowerPreference.HighPerformance
        };

        Adapter = Factory.RequestAdapter(in requestAdapterOptions);
        SurfaceFormat = Adapter.GetSurfacePreferredFormat(MainWindow.Surface);
        Debug.Assert(SurfaceFormat != PixelFormat.Undefined);

        GraphicsDeviceDescription deviceDescription = new();
        Device = Adapter.CreateDevice(in deviceDescription);

        VSync = true;
        Resize(MainWindow.ClientSize);
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Application" /> class.
    /// </summary>
    ~Application() => Dispose(disposing: false);

    /// <summary>
    /// Gets <c>true</c> if the application has been disposed; otherwise, <c>false</c>.
    /// </summary>
    public bool IsDisposed => _isDisposed != 0;

    public abstract string Name { get; }

    public Window MainWindow { get; }

    public GraphicsFactory Factory { get; }
    public GraphicsAdapter Adapter { get; }
    public GraphicsDevice Device { get; }
    public PixelFormat SurfaceFormat { get; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public bool VSync { get; set; }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="disposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            MainWindow.Surface?.Dispose();
            Device.Dispose();
            Adapter.Dispose();
            Factory.Dispose();
        }
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void OnTick()
    {
    }

    public void Resize(Size size)
    {
        Width = size.Width;
        Height = size.Height;

        SurfaceConfiguration surfaceConfiguration = new()
        {
            Format = SurfaceFormat,
            Usage = TextureUsage.RenderTarget,
            //alphaMode = WGPUCompositeAlphaMode.Auto,
            Width = Width,
            Height = Height,
            PresentMode = VSync ? PresentMode.Fifo : PresentMode.Immediate,
        };

        MainWindow.Surface.Configure(Device, in surfaceConfiguration);
    }

    public unsafe void Run()
    {
        Initialize();
        MainWindow.Show();

        bool running = true;

        while (running && !_closeRequested)
        {
            SDL_Event evt;
            while (SDL_PollEvent(&evt) == SDL_TRUE)
            {
                if (evt.type == (uint)SDL_EVENT_QUIT)
                {
                    running = false;
                    break;
                }

                if (evt.type == (uint)SDL_EVENT_WINDOW_CLOSE_REQUESTED
                    && evt.window.windowID == MainWindow.Id)
                {
                    running = false;
                    break;
                }
            }

            if (!running)
                break;

            OnTick();
        }
    }

    protected virtual void OnDraw(int width, int height)
    {

    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void Log_SDL(nint _, SDL_LogCategory category, SDL_LogPriority priority, byte* messagePtr)
    {
        string? message = PtrToStringUTF8(messagePtr);
        if (priority >= SDL_LOG_PRIORITY_ERROR)
        {
            Log.Error($"[{priority}] SDL: {message}");
        }
        else
        {
            Log.Info($"[{priority}] SDL: {message}");
        }
    }
}
