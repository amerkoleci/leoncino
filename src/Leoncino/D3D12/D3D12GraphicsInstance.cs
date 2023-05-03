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
using System.Diagnostics;
using DxgiFeature = Win32.Graphics.Dxgi.Feature;
using DxgiInfoQueueFilter = Win32.Graphics.Dxgi.InfoQueueFilter;

namespace Leoncino.D3D12;

internal unsafe class D3D12GraphicsInstance : GraphicsInstance
{
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);

    private readonly ComPtr<IDXGIFactory4> _dxgiFactory;

    public static bool IsSupported() => s_isSupported.Value;

    public D3D12GraphicsInstance(in GraphicsInstanceDescriptor descriptor)
        : base(BackendType.D3D12, descriptor.ValidationMode)
    {
        Guard.IsTrue(IsSupported(), nameof(D3D12GraphicsInstance), "Direct3D12 is not supported");

        uint dxgiFactoryFlags = 0u;

        if (ValidationMode != ValidationMode.Disabled)
        {
            dxgiFactoryFlags = DXGI_CREATE_FACTORY_DEBUG;

            using ComPtr<ID3D12Debug> d3d12Debug = default;
            if (D3D12GetDebugInterface(__uuidof<ID3D12Debug>(), d3d12Debug.GetVoidAddressOf()).Success)
            {
                d3d12Debug.Get()->EnableDebugLayer();

                if (ValidationMode == ValidationMode.GPU)
                {
                    using ComPtr<ID3D12Debug1> d3d12Debug1 = default;
                    using ComPtr<ID3D12Debug2> d3d12Debug2 = default;

                    if (d3d12Debug.CopyTo(d3d12Debug1.GetAddressOf()).Success)
                    {
                        d3d12Debug1.Get()->SetEnableGPUBasedValidation(true);
                        d3d12Debug1.Get()->SetEnableSynchronizedCommandQueueValidation(true);
                    }

                    if (d3d12Debug.CopyTo(d3d12Debug2.GetAddressOf()).Success)
                    {
                        d3d12Debug2.Get()->SetGPUBasedValidationFlags(GpuBasedValidationFlags.None);
                    }
                }
            }
            else
            {
                Debug.WriteLine("WARNING: Direct3D Debug Device is not available");
            }

            // DRED
            {
                using ComPtr<ID3D12DeviceRemovedExtendedDataSettings1> pDredSettings = default;
                if (D3D12GetDebugInterface(__uuidof<ID3D12DeviceRemovedExtendedDataSettings1>(), pDredSettings.GetVoidAddressOf()).Success)
                {
                    // Turn on auto - breadcrumbs and page fault reporting.
                    pDredSettings.Get()->SetAutoBreadcrumbsEnablement(DredEnablement.ForcedOn);
                    pDredSettings.Get()->SetPageFaultEnablement(DredEnablement.ForcedOn);
                    pDredSettings.Get()->SetBreadcrumbContextEnablement(DredEnablement.ForcedOn);
                }
            }

#if DEBUG
            using ComPtr<IDXGIInfoQueue> dxgiInfoQueue = default;

            if (DXGIGetDebugInterface1(0u, __uuidof<IDXGIInfoQueue>(), dxgiInfoQueue.GetVoidAddressOf()).Success)
            {
                dxgiInfoQueue.Get()->SetBreakOnSeverity(DXGI_DEBUG_ALL, InfoQueueMessageSeverity.Error, true);
                dxgiInfoQueue.Get()->SetBreakOnSeverity(DXGI_DEBUG_ALL, InfoQueueMessageSeverity.Corruption, true);

                int* hide = stackalloc int[1]
                {
                    80 /* IDXGISwapChain::GetContainingOutput: The swapchain's adapter does not control the output on which the swapchain's window resides. */,
                };

                DxgiInfoQueueFilter filter = new()
                {
                    DenyList = new Win32.Graphics.Dxgi.InfoQueueFilterDescription()
                    {
                        NumIDs = 1,
                        pIDList = hide
                    }
                };

                dxgiInfoQueue.Get()->AddStorageFilterEntries(DXGI_DEBUG_DXGI, &filter);
            }
#endif
        }

        ThrowIfFailed(CreateDXGIFactory2(dxgiFactoryFlags, __uuidof<IDXGIFactory4>(), _dxgiFactory.GetVoidAddressOf()));

        // Determines whether tearing support is available for fullscreen borderless windows.
        {
            Bool32 allowTearing = false;

            using ComPtr<IDXGIFactory5> dxgiFactory5 = default;
            HResult hr = _dxgiFactory.CopyTo(dxgiFactory5.GetAddressOf());

            if (hr.Success)
            {
                hr = dxgiFactory5.Get()->CheckFeatureSupport(DxgiFeature.PresentAllowTearing, &allowTearing, sizeof(Bool32));
            }

            if (hr.Failure || !allowTearing)
            {
                TearingSupported = false;
#if DEBUG
                Debug.WriteLine("WARNING: Variable refresh rate displays not supported");
#endif
            }
            else
            {
                TearingSupported = true;
            }
        }
    }

    public IDXGIFactory4* DXGIFactory => _dxgiFactory;
    public bool TearingSupported { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12GraphicsInstance" /> class.
    /// </summary>
    ~D3D12GraphicsInstance() => Dispose(disposing: false);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dxgiFactory.Dispose();

#if DEBUG
            using ComPtr<IDXGIDebug1> dxgiDebug = default;
            if (DXGIGetDebugInterface1(0u, __uuidof<IDXGIDebug1>(), dxgiDebug.GetVoidAddressOf()).Success)
            {
                dxgiDebug.Get()->ReportLiveObjects(DXGI_DEBUG_ALL, ReportLiveObjectFlags.Summary | ReportLiveObjectFlags.IgnoreInternal);
            }
#endif
        }
    }

    /// <inheritdoc />
    public override Surface CreateSurface(SurfaceSource source) => new D3D12Surface(this, source);

    /// <inheritdoc />
    public override GraphicsAdapter RequestAdapter(Surface? compatibleSurface = default, PowerPreference powerPreference = PowerPreference.HighPerformance)
    {
        using ComPtr<IDXGIFactory6> dxgiFactory6 = default;
        GpuPreference gpuPreference = powerPreference.ToDxgi();
        bool queryByPreference = _dxgiFactory.CopyTo(dxgiFactory6.GetAddressOf()).Success;

        using ComPtr<IDXGIAdapter1> dxgiAdapter = default;
        for (uint i = 0; NextAdapter(i, dxgiAdapter.ReleaseAndGetAddressOf()) != DXGI_ERROR_NOT_FOUND; ++i)
        {
            AdapterDescription1 adapterDesc;
            ThrowIfFailed(dxgiAdapter.Get()->GetDesc1(&adapterDesc));

            // Don't select the Basic Render Driver adapter.
            if ((adapterDesc.Flags & AdapterFlags.Software) != 0u)
            {
                continue;
            }

            using ComPtr<ID3D12Device> tempDevice = default;
            if (D3D12CreateDevice((IUnknown*)dxgiAdapter.Get(), FeatureLevel.Level_11_0,
                __uuidof<ID3D12Device>(), tempDevice.GetVoidAddressOf()).Success)
            {
                return new D3D12GraphicsAdapter(this, dxgiAdapter, tempDevice.Get());
            }
        }

        throw new GraphicsException("No suitable adapters found");

        HResult NextAdapter(uint index, IDXGIAdapter1** ppAdapter)
        {
            if (queryByPreference)
                return dxgiFactory6.Get()->EnumAdapterByGpuPreference(index, gpuPreference, __uuidof<IDXGIAdapter1>(), (void**)ppAdapter);
            else
                return _dxgiFactory.Get()->EnumAdapters1(index, ppAdapter);
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
                if (D3D12CreateDevice((IUnknown*)dxgiAdapter.Get(), FeatureLevel.Level_11_0,
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
