// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public ref struct SurfaceCapabilities
{
    public PixelFormat PreferredFormat;
    public TextureUsage SupportedUsage;
    public ReadOnlySpan<PixelFormat> Formats;
    public ReadOnlySpan<PresentMode> PresentModes;
}
