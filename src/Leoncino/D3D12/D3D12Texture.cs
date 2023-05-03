// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using static Win32.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12Texture : Texture
{
    private readonly ComPtr<ID3D12Resource> _handle;

    public D3D12Texture(D3D12GraphicsDevice device, in TextureDescriptor descriptor, void* initialData)
        : base(device, descriptor)
    {
        
    }

    public ID3D12Resource* Handle => _handle;

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12Texture" /> class.
    /// </summary>
    ~D3D12Texture() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    /// <inheritdoc />
    protected override void OnLabelChanged(string newLabel)
    {
    }
}
