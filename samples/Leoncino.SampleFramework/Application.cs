// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using SDL;
using static SDL.SDL;

namespace Leoncino.Samples;

public abstract class Application : IDisposable
{
    private volatile uint _isDisposed = 0;
    private bool _closeRequested = false;
    private readonly Instance _instance;
    private readonly GraphicsAdapter _adapter;

    protected Application()
    {
        if (SDL_Init(SDL_InitFlags.Video) != 0)
        {
            var error = SDL_GetErrorString();
            throw new Exception($"Failed to start SDL2: {error}");
        }

        SDL_LogSetOutputFunction(Log_SDL);
        InstanceDescriptor instanceDescriptor = new();
        _instance = Instance.Create(in instanceDescriptor);

        // Create main window.
        MainWindow = new Window(Name, 1280, 720);
        MainWindow.CreateSurface(_instance);

        RequestAdapterOptions requestAdapterOptions = new()
        {
            CompatibleSurface = MainWindow.Surface,
            PowerPreference = PowerPreference.HighPerformance
        };

        _adapter = _instance.RequestAdapter(in requestAdapterOptions);

        DeviceDescriptor deviceDescriptor = new();
        Device = _adapter.CreateDevice(in deviceDescriptor);
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
    public GraphicsDevice Device { get; }

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
            _adapter.Dispose();
            _instance.Dispose();
        }
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void OnTick()
    {
    }

    public unsafe void Run()
    {
        Initialize();
        MainWindow.Show();

        bool running = true;

        while (running && !_closeRequested)
        {
            SDL_Event evt;
            while (SDL_PollEvent(&evt))
            {
                if (evt.type == SDL_EventType.Quit)
                {
                    running = false;
                    break;
                }

                if (evt.type == SDL_EventType.WindowCloseRequested && evt.window.windowID == MainWindow.Id)
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

    //[UnmanagedCallersOnly]
    private static void Log_SDL(SDL_LogCategory category, SDL_LogPriority priority, string description)
    {
        if (priority >= SDL_LogPriority.Error)
        {
            Log.Error($"[{priority}] SDL: {description}");
        }
        else
        {
            Log.Info($"[{priority}] SDL: {description}");
        }
    }
}
