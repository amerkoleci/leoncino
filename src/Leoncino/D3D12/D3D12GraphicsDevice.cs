// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using static Win32.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12GraphicsDevice : GraphicsDevice
{
    private readonly D3D12GraphicsAdapter _adapter;
    private readonly ComPtr<ID3D12Device5> _handle;

    public D3D12GraphicsDevice(D3D12GraphicsAdapter adapter, in ComPtr<ID3D12Device5> device)
    {
        _adapter = adapter;
        _handle = device.Move();
    }

    /// <inheritdoc />
    public override GraphicsAdapter Adapter => throw new NotImplementedException();

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12GraphicsDevice" /> class.
    /// </summary>
    ~D3D12GraphicsDevice() => Dispose(disposing: false);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _handle.Dispose();
        }
    }
}
