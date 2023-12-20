// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Leoncino;

/// <summary>
/// Defines a platform-specific GPU surface
/// </summary>
public abstract class GPUSurface : GPUObjectBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Surface" /> class.
    /// </summary>
    /// <param name="descriptor">The <see cref="SurfaceDescriptor"/>.</param>
    protected GPUSurface(in SurfaceDescriptor descriptor)
        : base(descriptor.Label)
    {
    }

    public bool IsConfigured { get; private set; }

    public void Configure(GPUDevice device, in SurfaceConfiguration configuration)
    {
        Guard.IsNotNull(device, nameof(device));
        Guard.IsFalse(IsConfigured, nameof(IsConfigured));
        Guard.IsTrue(configuration.Format != PixelFormat.Undefined, nameof(configuration.Format));
        Guard.IsGreaterThanOrEqualTo(configuration.Width, 1, nameof(configuration.Width));
        Guard.IsGreaterThanOrEqualTo(configuration.Height, 1, nameof(configuration.Height));

        ConfigureCore(device, configuration);
        IsConfigured = true;
    }

    protected abstract void ConfigureCore(GPUDevice device, in SurfaceConfiguration configuration);
}
