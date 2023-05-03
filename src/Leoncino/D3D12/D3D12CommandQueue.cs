// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using static Win32.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12CommandQueue : IDisposable
{
    private readonly D3D12GraphicsDevice _device;
    private readonly ComPtr<ID3D12CommandQueue> _handle;
    private readonly ComPtr<ID3D12Fence> _fence;
    private ulong _nextFenceValue;
    private ulong _lastCompletedFenceValue;

    public D3D12CommandQueue(D3D12GraphicsDevice device, CommandQueue type)
    {
        _device = device;

        CommandListType commandListType = type.ToD3D12();
        _nextFenceValue = (ulong)commandListType << 56 | 1;
        _lastCompletedFenceValue = (ulong)commandListType << 56;

        CommandQueueDescription d3dDesc = new(type.ToD3D12(), CommandQueuePriority.Normal);
        HResult hr = device.Handle->CreateCommandQueue(&d3dDesc, __uuidof<ID3D12CommandQueue>(), _handle.GetVoidAddressOf());
        if (hr.Failure)
        {
            throw new GraphicsException("D3D12: Failed to create CommandQueue");
        }

        hr = device.Handle->CreateFence(0, FenceFlags.Shared, __uuidof<ID3D12Fence>(), _fence.GetVoidAddressOf());
        if (hr.Failure)
        {
            throw new GraphicsException("D3D12: Failed to create Fence");
        }
        ThrowIfFailed(_fence.Get()->Signal(_lastCompletedFenceValue));

        switch (type)
        {
            case CommandQueue.Graphics:
                _handle.Get()->SetName("Graphics Queue");
                _fence.Get()->SetName("GraphicsQueue - Fence");
                break;
            case CommandQueue.Compute:
                _handle.Get()->SetName("Compute Queue");
                _fence.Get()->SetName("ComputeQueue - Fence");
                break;
            case CommandQueue.Copy:
                _handle.Get()->SetName("CopyQueue");
                _fence.Get()->SetName("CopyQueue - Fence");
                break;
        }
    }

    public ID3D12CommandQueue* Handle => _handle;

    /// <inheritdoc />
    public void Dispose()
    {
        _fence.Dispose();
        _handle.Dispose();
    }

    public ulong IncrementFence()
    {
        //std::lock_guard<std::mutex> LockGuard(m_FenceMutex);
        _handle.Get()->Signal(_fence.Get(), _nextFenceValue);
        return _nextFenceValue++;
    }

    public bool IsFenceComplete(ulong fenceValue)
    {
        // Avoid querying the fence value by testing against the last one seen.
        // The max() is to protect against an unlikely race condition that could cause the last
        // completed fence value to regress.
        if (fenceValue > _lastCompletedFenceValue)
        {
            _lastCompletedFenceValue = Math.Max(_lastCompletedFenceValue, _fence.Get()->GetCompletedValue());
        }

        return fenceValue <= _lastCompletedFenceValue;
    }

    public void WaitForFence(ulong fenceValue)
    {
        if (IsFenceComplete(fenceValue))
            return;

        {
            //std::lock_guard<std::mutex> LockGuard(m_EventMutex);

            // NULL event handle will simply wait immediately:
            // https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nf-d3d12-id3d12fence-seteventoncompletion#remarks
            _fence.Get()->SetEventOnCompletion(fenceValue, Win32.Handle.Null);
            _lastCompletedFenceValue = fenceValue;
        }
    }

    public void WaitForIdle()
    {
        WaitForFence(IncrementFence());
    }
}
