// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Dxgi;
using static Win32.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12SwapChain : SwapChain
{
    private readonly ComPtr<IDXGISwapChain3> _handle;

    public D3D12SwapChain(D3D12GraphicsDevice device, Surface surface, in SwapChainDescriptor descriptor)
        : base(device, surface, descriptor)
    {
        SwapChainDescription1 swapChainDesc = new()
        {
            Width = (uint)descriptor.Width,
            Height = (uint)descriptor.Height,
            Format = descriptor.Format.ToDxgiSwapChainFormat(),
            Stereo = false,
            SampleDesc = new(1, 0),
            BufferUsage = Usage.RenderTargetOutput,
            BufferCount = descriptor.PresentMode.ToBufferCount(),
            Scaling = Scaling.Stretch,
            SwapEffect = SwapEffect.FlipDiscard,
            AlphaMode = Win32.Graphics.Dxgi.Common.AlphaMode.Ignore,
            Flags = device.TearingSupported ? SwapChainFlags.AllowTearing : SwapChainFlags.None
        };

        using ComPtr<IDXGISwapChain1> tempSwapChain = default;
        switch (surface.Source)
        {
            case Win32SurfaceSource win32SurfaceSource:
                SwapChainFullscreenDescription fsSwapChainDesc = new()
                {
                    Windowed = !descriptor.IsFullscreen
                };

                ThrowIfFailed(device.DXGIFactory->CreateSwapChainForHwnd(
                    (IUnknown*)device.D3D12GraphicsQueue,
                    win32SurfaceSource.Hwnd,
                    &swapChainDesc,
                    &fsSwapChainDesc,
                    null,
                    tempSwapChain.GetAddressOf())
                    );

                // This class does not support exclusive full-screen mode and prevents DXGI from responding to the ALT+ENTER shortcut
                ThrowIfFailed(device.DXGIFactory->MakeWindowAssociation(win32SurfaceSource.Hwnd, WindowAssociationFlags.NoAltEnter));
                break;
        }

        ThrowIfFailed(tempSwapChain.CopyTo(_handle.GetAddressOf()));
    }

    public IDXGISwapChain3* Handle => _handle;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Device.WaitIdle();

            _handle.Dispose();
        }
    }
}
