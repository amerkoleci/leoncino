// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using static Win32.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12Buffer : Buffer
{
    private readonly ComPtr<ID3D12Resource> _handle;

    public D3D12Buffer(D3D12GraphicsDevice device, in BufferDescriptor descriptor, void* initialData)
        : base(device, descriptor)
    {
        ulong alignedSize = descriptor.Size;
        //if (CheckBitsAny(desc.usage, BufferUsage::Constant))
        //{
        //    alignedSize = BitOperations::AlignUp(alignedSize, D3D12_CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT);
        //}

        ResourceFlags resourceFlags = ResourceFlags.None;
        ResourceDescription resourceDesc = ResourceDescription.Buffer(alignedSize, resourceFlags);
        HeapProperties heapProps = D3D12Utils.DefaultHeapProps;
        HResult hr = device.Handle->CreateCommittedResource(&heapProps,
            HeapFlags.None,
            &resourceDesc,
            ResourceStates.Common,
            null,
            __uuidof<ID3D12Resource>(), _handle.GetVoidAddressOf()
            );
        ThrowIfFailed(hr);
    }

    public ID3D12Resource* Handle => _handle;

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12Buffer" /> class.
    /// </summary>
    ~D3D12Buffer() => Dispose(disposing: false);

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
