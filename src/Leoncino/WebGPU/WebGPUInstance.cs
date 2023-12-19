// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe partial class WebGPUInstance : Instance
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);

    public static bool IsSupported() => s_isSupported.Value;

    public WebGPUInstance(in InstanceDescriptor descriptor)
        : base(descriptor)
    {
        WGPUInstanceExtras extras = new();
        extras.flags = descriptor.ValidationMode.ToWGPU();
#if DEBUG
        extras.flags |= WGPUInstanceFlags.Debug;
#endif

        WGPUInstanceDescriptor instanceDescriptor = new()
        {
            nextInChain = (WGPUChainedStruct*)&extras
        };
        Handle = wgpuCreateInstance(&instanceDescriptor);
    }

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
    protected override Surface CreateSurfaceCore(in SurfaceDescriptor descriptor) => new WebGPUSurface(this, in descriptor);

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
        wgpuInstanceRequestAdapter(Handle, &requestAdapterOptions, requestAdapterCallback, 0);
        return new WebGPUGraphicsAdapter(result);

        void requestAdapterCallback(WGPURequestAdapterStatus status, WGPUAdapter candidateAdapter, sbyte* message, nint pUserData)
        {
            if (status == WGPURequestAdapterStatus.Success)
            {
                result = candidateAdapter;
            }
            else
            {
                //Log.Error("Could not get WebGPU adapter: " + Interop.GetString(message));
            }
        }
    }

    private static bool CheckIsSupported()
    {
        return true;
    }
}
