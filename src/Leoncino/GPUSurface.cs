// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

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
}
