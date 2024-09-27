// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe class WebGPUInstance : GraphicsFactory
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);

    public static bool IsSupported() => s_isSupported.Value;

    public WebGPUInstance(in GraphicsFactoryDescriptor descriptor)
        : base(descriptor)
    {
        wgpuSetLogCallback(LogCallback);

        WGPUInstanceExtras extras = new()
        {
            flags = descriptor.ValidationMode.ToWGPU()
        };
#if DEBUG
        extras.flags |= WGPUInstanceFlags.Debug;
#endif

        WGPUInstanceDescriptor instanceDescriptor = new()
        {
            nextInChain = (WGPUChainedStruct*)&extras
        };
        Handle = wgpuCreateInstance(&instanceDescriptor);
    }

    /// <inheritdoc />
    public override GraphicsBackend BackendType => GraphicsBackend.WGPU;

    public WGPUInstance Handle { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUInstance" /> class.
    /// </summary>
    ~WebGPUInstance() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            wgpuInstanceRelease(Handle);
        }
    }

    /// <inheritdoc />
    protected override GraphicsSurface CreateSurfaceCore(in SurfaceDescriptor descriptor) => new WebGPUSurface(this, in descriptor);

    protected override GraphicsAdapter RequestAdapterCore(in RequestAdapterOptions options)
    {
        WGPURequestAdapterOptions requestAdapterOptions = new()
        {
            nextInChain = null,
            compatibleSurface = options.CompatibleSurface != null ? ((WebGPUSurface)options.CompatibleSurface).Handle : WGPUSurface.Null,
            powerPreference = options.PowerPreference.ToWGPU(),
            backendType = WGPUBackendType.Undefined,
            forceFallbackAdapter = false
        };

        WGPUAdapter result = WGPUAdapter.Null;
        wgpuInstanceRequestAdapter(Handle, &requestAdapterOptions, &OnAdapterRequestEnded, &result);
        return new WebGPUAdapter(result);
    }

    private static bool CheckIsSupported()
    {
        return true;
    }

    private static void LogCallback(WGPULogLevel level, string message, nint userdata = 0)
    {
        switch (level)
        {
            case WGPULogLevel.Error:
                throw new GraphicsException(message);
            case WGPULogLevel.Warn:
                //Log.Warn(message);
                break;
            case WGPULogLevel.Info:
            case WGPULogLevel.Debug:
            case WGPULogLevel.Trace:
                //Log.Info(message);
                break;
        }
    }

    [UnmanagedCallersOnly]
    private static void OnAdapterRequestEnded(WGPURequestAdapterStatus status, WGPUAdapter candidateAdapter, byte* message, void* pUserData)
    {
        if (status == WGPURequestAdapterStatus.Success)
        {
            *(WGPUAdapter*)pUserData = candidateAdapter;
        }
        else
        {
            throw new GraphicsException("Could not get WGPU adapter: " + Interop.GetString(message));
        }
    }
}
