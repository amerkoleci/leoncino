// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using static Win32.Apis;
using static Win32.Graphics.Direct3D12.Apis;
using static Win32.Graphics.Dxgi.Apis;

namespace Leoncino.D3D12;

internal unsafe class D3D12GraphicsFactory : GraphicsFactory
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);
    private readonly ComPtr<IDXGIFactory4> _dxgiFactory;

    public static bool IsSupported() => s_isSupported.Value;

    public D3D12GraphicsFactory(in GraphicsFactoryDescription description)
        : base(description)
    {
    }

    /// <inheritdoc />
    public override GraphicsBackend BackendType => GraphicsBackend.Direct3D12;

    public IDXGIFactory4* Handle => _dxgiFactory;

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12GraphicsFactory" /> class.
    /// </summary>
    ~D3D12GraphicsFactory() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dxgiFactory.Dispose();
        }
    }

    /// <inheritdoc />
    protected override GPUSurface CreateSurfaceCore(in SurfaceDescriptor descriptor)
    {
        throw new NotImplementedException();
    }

    protected override ValueTask<GPUAdapter> RequestAdapterAsyncCore(in RequestAdapterOptions options)
    {
        throw new NotImplementedException();
    }

    private static bool CheckIsSupported()
    {
        try
        {
            if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19041))
            {
                return false;
            }

            using ComPtr<IDXGIFactory4> dxgiFactory = default;
            using ComPtr<IDXGIAdapter1> dxgiAdapter = default;

            ThrowIfFailed(CreateDXGIFactory1(__uuidof<IDXGIFactory4>(), dxgiFactory.GetVoidAddressOf()));

            bool foundCompatibleDevice = false;
            for (uint adapterIndex = 0;
                dxgiFactory.Get()->EnumAdapters1(adapterIndex, dxgiAdapter.ReleaseAndGetAddressOf()).Success;
                adapterIndex++)
            {
                AdapterDescription1 adapterDesc;
                ThrowIfFailed(dxgiAdapter.Get()->GetDesc1(&adapterDesc));

                if ((adapterDesc.Flags & AdapterFlags.Software) != 0)
                {
                    // Don't select the Basic Render Driver adapter.
                    continue;
                }

                // Check to see if the adapter supports Direct3D 12, but don't create the actual device.
                if (D3D12CreateDevice((IUnknown*)dxgiAdapter.Get(), FeatureLevel.Level_12_0, __uuidof<ID3D12Device>(), null).Success)
                {
                    foundCompatibleDevice = true;
                    break;
                }
            }

            if (!foundCompatibleDevice)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
