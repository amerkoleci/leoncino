// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a platform-specific graphics surface.
/// </summary>
public abstract class GraphicsSurface : GraphicsObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicsSurface" /> class.
    /// </summary>
    /// <param name="descriptor">The <see cref="SurfaceDescriptor"/>.</param>
    protected GraphicsSurface(in SurfaceDescriptor descriptor)
        : base(descriptor.Label)
    {
    }

    public bool IsConfigured { get; private set; }

    public void Configure(GraphicsDevice device, in SurfaceConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(device, nameof(device));

        //Guard.IsFalse(IsConfigured, nameof(IsConfigured));
        //Guard.IsTrue(configuration.Format != PixelFormat.Undefined, nameof(configuration.Format));
        //Guard.IsGreaterThanOrEqualTo(configuration.Width, 1, nameof(configuration.Width));
        //Guard.IsGreaterThanOrEqualTo(configuration.Height, 1, nameof(configuration.Height));

        ConfigureCore(device, configuration);
        IsConfigured = true;
    }

    protected abstract void ConfigureCore(GraphicsDevice device, in SurfaceConfiguration configuration);
}
