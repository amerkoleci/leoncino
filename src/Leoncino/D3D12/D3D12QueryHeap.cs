// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using D3DQueryHeapDescription = Win32.Graphics.Direct3D12.QueryHeapDescription;
using static Win32.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12QueryHeap : QueryHeap
{
    private readonly ComPtr<ID3D12QueryHeap> _handle;

    public D3D12QueryHeap(D3D12GraphicsDevice device, in QueryHeapDescription description)
        : base(device, description)
    {
        D3DQueryHeapDescription d3dDesc = new(description.Type.ToD3D12(), (uint)description.Count);

        HResult hr = device.Handle->CreateQueryHeap(&d3dDesc, __uuidof<ID3D12QueryHeap>(), _handle.GetVoidAddressOf());
        if (hr.Failure)
        {
            throw new GraphicsException("D3D12: Failed to create query heap");
        }

        //if (desc.label)
        //{
        //    resource->SetLabel(desc.label);
        //}
    }


    public ID3D12QueryHeap* Handle => _handle;

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12QueryHeap" /> class.
    /// </summary>
    ~D3D12QueryHeap() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            //vkDestroyQueryPool();
        }
    }

    /// <inheritdoc />
    protected override void OnLabelChanged(string newLabel)
    {
    }
}
