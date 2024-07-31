// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that configures the <see cref="GraphicsSurface"/>.
/// </summary>
public readonly record struct SurfaceConfiguration
{
    public SurfaceConfiguration()
    {

    }

    public PixelFormat Format { get; init; } = PixelFormat.Undefined;
    public TextureUsage Usage { get; init; } = TextureUsage.RenderTarget;

    public int Width { get; init; }
    public int Height { get; init; }
    public PresentMode PresentMode { get; init; } = PresentMode.Fifo;
    public bool IsFullscreen { get; init; } = false;

    public string? Label { get; init; } = default;
}
