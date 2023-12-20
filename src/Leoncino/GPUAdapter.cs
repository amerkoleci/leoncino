// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a GPU (physical device) adapter
/// </summary>
public abstract class GPUAdapter : GPUObjectBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GPUAdapter" /> class.
    /// </summary>
    protected GPUAdapter()
    {
    }

    public abstract PixelFormat GetSurfacePreferredFormat(GPUSurface surface);

    public ValueTask<GPUDevice> CreateDeviceAsync(in DeviceDescriptor descriptor)
    {
        return CreateDeviceAsyncCore(in descriptor);
    }

    protected abstract ValueTask<GPUDevice> CreateDeviceAsyncCore(in DeviceDescriptor descriptor);
}
