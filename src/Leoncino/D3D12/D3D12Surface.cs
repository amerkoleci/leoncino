// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino.D3D12;

internal unsafe class D3D12Surface : Surface
{
    private readonly D3D12GraphicsInstance _instance;

    public D3D12Surface(D3D12GraphicsInstance instance, in SurfaceSource source)
        : base(source)  
    {
        _instance = instance;
    }
}
