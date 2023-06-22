// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using Win32;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using static Win32.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12GraphicsDevice : GraphicsDevice
{
    private readonly D3D12GraphicsAdapter _adapter;
    private readonly ComPtr<ID3D12Device5> _handle;
    private readonly D3D12CommandQueue[] _queues = new D3D12CommandQueue[(int)CommandQueue.Count];

    public D3D12GraphicsDevice(D3D12GraphicsAdapter adapter, in ComPtr<ID3D12Device5> device)
    {
        _adapter = adapter;
        _handle = device.Move();

        // Create command queue's
        for (int i = 0; i < (int)CommandQueue.Count; i++)
        {
            CommandQueue queue = (CommandQueue)i;
            _queues[i] = new D3D12CommandQueue(this, queue);
        }
    }

    /// <inheritdoc />
    public override GraphicsAdapter Adapter => _adapter;

    public IDXGIFactory2* DXGIFactory => _adapter.DXGIFactory;
    public bool TearingSupported => _adapter.TearingSupported;
    public ID3D12Device5* Handle => _handle;
    public ID3D12CommandQueue* D3D12GraphicsQueue => _queues[(int)CommandQueue.Graphics].Handle;
    public D3D12CommandQueue GetQueue(CommandQueue queue = CommandQueue.Graphics) => _queues[(int)queue];

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12GraphicsDevice" /> class.
    /// </summary>
    ~D3D12GraphicsDevice() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Destroy CommandQueue's
            for (int i = 0; i < (int)CommandQueue.Count; i++)
            {
                _queues[i].Dispose();
            }

#if DEBUG
            uint refCount = _handle.Get()->Release();
            if (refCount > 0)
            {
                Debug.WriteLine($"Direct3D12: There are {refCount} unreleased references left on the device");

                using ComPtr<ID3D12DebugDevice> debugDevice = default;

                if (_handle.CopyTo(debugDevice.GetAddressOf()).Success)
                {
                    debugDevice.Get()->ReportLiveDeviceObjects(ReportLiveDeviceObjectFlags.Detail | ReportLiveDeviceObjectFlags.IgnoreInternal);
                }
            }
#else
            _handle.Dispose();
#endif
            _adapter.Dispose();
        }
    }

    /// <inheritdoc />
    public override void WaitIdle()
    {
        for (int i = 0; i < (int)CommandQueue.Count; i++)
        {
            _queues[i].WaitForIdle();
        }
    }

    /// <inheritdoc />
    protected override Buffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData)
    {
        return new D3D12Buffer(this, descriptor, initialData);
    }

    /// <inheritdoc />
    protected override Texture CreateTextureCore(in TextureDescriptor descriptor, void* initialData)
    {
        return new D3D12Texture(this, descriptor, initialData);
    }

    /// <inheritdoc />
    protected override QueryHeap CreateQueryHeapCore(in QueryHeapDescription description)
    {
        return new D3D12QueryHeap(this, in description);
    }

    /// <inheritdoc />
    protected override SwapChain CreateSwapChainCore(Surface surface, in SwapChainDescriptor descriptor)
    {
        return new D3D12SwapChain(this, surface, descriptor);
    }
}
