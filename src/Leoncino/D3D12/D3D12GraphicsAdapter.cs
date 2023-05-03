// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using Win32;
using Win32.Graphics.Direct3D;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using static Win32.Apis;
using static Win32.Graphics.Direct3D12.Apis;
using static Win32.Graphics.Dxgi.Apis;
using DxgiFeature = Win32.Graphics.Dxgi.Feature;
using DxgiInfoQueueFilter = Win32.Graphics.Dxgi.InfoQueueFilter;

namespace Leoncino.D3D12;

internal unsafe class D3D12GraphicsAdapter : GraphicsAdapter
{
    private readonly ComPtr<IDXGIAdapter1> _adapter;

    public D3D12GraphicsAdapter(in ComPtr<IDXGIAdapter1> adapter)
    {
        _adapter = adapter.Move();
    }

    public IDXGIAdapter1* Handle => _adapter;

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12GraphicsAdapter" /> class.
    /// </summary>
    ~D3D12GraphicsAdapter() => Dispose(disposing: false);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _adapter.Dispose();
        }
    }
}
