// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Leoncino;

public abstract class Surface
{
    protected Surface(SurfaceSource source)
    {
        Guard.IsNotNull(source, nameof(source));

        Source = source;
    }

    public SurfaceSource Source { get; }
}
