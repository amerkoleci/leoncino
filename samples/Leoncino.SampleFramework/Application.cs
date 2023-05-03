// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using Alimer.Bindings.SDL;
using static Alimer.Bindings.SDL.SDL;
using static Alimer.Bindings.SDL.SDL_InitFlags;
using static Alimer.Bindings.SDL.SDL_EventType;
using static Alimer.Bindings.SDL.SDL_LogPriority;

namespace Leoncino.SampleFramework;

public abstract class Application : DisposableObject
{
    private const int _eventsPerPeep = 64;
    private unsafe readonly SDL_Event* _events = (SDL_Event*)NativeMemory.Alloc(_eventsPerPeep, (nuint)sizeof(SDL_Event));
    private bool _exitRequested = false;

    protected Application(ValidationMode validationMode = ValidationMode.Disabled)
    {
        SDL_LogSetAllPriority(SDL_LOG_PRIORITY_VERBOSE);
        SDL_LogSetOutputFunction(OnLog);
        SDL_GetVersion(out SDL_version version);

        // Init SDL2
        if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER | SDL_INIT_GAMEPAD) != 0)
        {
            Log.Error($"Unable to initialize SDL: {SDL_GetError()}");
            throw new PlatformNotSupportedException("GLFW is not supported");
        }

        // Create graphics instance first
        GraphicsInstanceDescriptor descriptor = new()
        {
            ValidationMode = validationMode,
            Label = Name
        };
        Instance = GraphicsInstance.CreateDefault(descriptor);

        // Create main window.
        MainWindow = new Window(Name, 1280, 720);

        // Request GraphicsAdapter for window surface and create device.
        Surface surface = Instance.CreateSurface(MainWindow.SurfaceSource!);
        GraphicsAdapter adapter = Instance.RequestAdapter(surface);
        Device = adapter.CreateDevice();

        // Create SwapChain for main window
        SwapChainDescriptor swapChainDescriptor = new()
        {
            Format = adapter.GetPreferredFormat(surface)
        };
        MainWindowSwapChain = Device.CreateSwapChain(surface, swapChainDescriptor);
    }

    public abstract string Name { get; }

    public GraphicsInstance Instance { get; }
    public GraphicsDevice Device { get; }   
    public Window MainWindow { get; }
    public SwapChain MainWindowSwapChain { get; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Device.WaitIdle();
            MainWindowSwapChain.Dispose();
            Device.Dispose();

            Instance.Dispose();
        }
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void OnTick()
    {
    }

    public void Run()
    {
        Initialize();
        MainWindow.Show();

        while (!_exitRequested)
        {
            PollEvents();
            OnTick();
        }

        SDL_Quit();
    }

    protected virtual void OnDraw(int width, int height)
    {

    }

    private unsafe void PollEvents()
    {
        SDL_PumpEvents();
        int eventsRead;

        do
        {
            eventsRead = SDL_PeepEvents(_events, _eventsPerPeep, SDL_eventaction.SDL_GETEVENT, SDL_FIRSTEVENT, SDL_EVENT_LAST);
            for (int i = 0; i < eventsRead; i++)
            {
                HandleSDLEvent(_events[i]);
            }
        } while (eventsRead == _eventsPerPeep);
    }

    private void HandleSDLEvent(SDL_Event evt)
    {
        switch (evt.type)
        {
            case SDL_QUIT:
            case SDL_EVENT_TERMINATING:
                _exitRequested = true;
                break;

            default:
                if (evt.type >= SDL_EVENT_WINDOW_FIRST && evt.type <= SDL_EVENT_WINDOW_LAST)
                {
                    HandleWindowEvent(evt);
                }
                break;
        }
    }

    private void HandleWindowEvent(in SDL_Event evt)
    {
    }

    private static void OnLog(SDL_LogCategory category, SDL_LogPriority priority, string message)
    {
        Log.Info($"SDL: {message}");
    }
}
