// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;
using Win32;
using Win32.Graphics.Direct3D;
using Win32.Graphics.Dxgi;
using static Win32.Apis;
using static Win32.Graphics.Dxgi.Apis;
using static Win32.Graphics.Direct3D12.Apis;
using Win32.Graphics.Direct3D12;

namespace Leoncino.D3D12;

internal unsafe class D3D12GraphicsInstance : GraphicsInstance
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);

    public static bool IsSupported() => s_isSupported.Value;

    public D3D12GraphicsInstance(in GraphicsInstanceDescriptor descriptor)
        : base(BackendType.D3D12)
    {
        Guard.IsTrue(IsSupported(), nameof(D3D12GraphicsInstance), "Direct3D12 is not supported");
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12GraphicsInstance" /> class.
    /// </summary>
    ~D3D12GraphicsInstance() => Dispose(disposing: false);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {

        }
    }

    private static bool CheckIsSupported()
    {
        try
        {
#if NET6_0_OR_GREATER
            if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 19041))
            {
                return false;
            }
#endif

            using ComPtr<IDXGIFactory4> dxgiFactory = default;
            using ComPtr<IDXGIAdapter1> dxgiAdapter = default;

            ThrowIfFailed(CreateDXGIFactory1(__uuidof<IDXGIFactory2>(), dxgiFactory.GetVoidAddressOf()));

            bool foundCompatibleDevice = false;
            for (uint adapterIndex = 0;
                dxgiFactory.Get()->EnumAdapters1(adapterIndex, dxgiAdapter.ReleaseAndGetAddressOf()).Success;
                adapterIndex++)
            {
                AdapterDescription1 adapterDesc;
                ThrowIfFailed(dxgiAdapter.Get()->GetDesc1(&adapterDesc));

                if ((adapterDesc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                {
                    // Don't select the Basic Render Driver adapter.
                    continue;
                }

                // Check to see if the adapter supports Direct3D 12, but don't create the actual device.
                if (D3D12CreateDevice((IUnknown*)dxgiAdapter.Get(), FeatureLevel.Level_12_0,
                     __uuidof<ID3D12Device>(), null).Success)
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
