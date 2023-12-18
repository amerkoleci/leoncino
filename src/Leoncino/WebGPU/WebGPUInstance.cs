// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe partial class WebGPUInstance : Instance
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);

    public WebGPUInstance(in InstanceDescriptor descriptor)
        : base(descriptor)
    {
        WGPUInstanceDescriptor instanceDescriptor = new()
        {
            nextInChain = null
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

    private static bool CheckIsSupported()
    {
        return true;
    }
}
