// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="GraphicsSurface"/>.
/// </summary>
public readonly record struct SurfaceDescriptor
{
    public SurfaceDescriptor()
    {
    }

    /// <summary>
    /// Gets or sets the surface source.
    /// </summary>
    public required SurfaceSource Source { get; init; }

    /// <summary>
    /// Gets or sets the label of <see cref="GraphicsSurface"/>.
    /// </summary>
    public string? Label { get; init; } = default;
}
