// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using static Win32.Apis;
using static Win32.Graphics.Dxgi.Apis;
using static Win32.Graphics.Direct3D12.Apis;
using InfoQueueFilter = Win32.Graphics.Direct3D12.InfoQueueFilter;
using MessageId = Win32.Graphics.Direct3D12.MessageId;
using Win32.Graphics.Direct3D;

namespace Leoncino.D3D12;

internal unsafe class D3D12GraphicsAdapter : GraphicsAdapter, IDisposable
{
    private D3D12GraphicsInstance _instance;
    private readonly ComPtr<IDXGIAdapter1> _adapter;

    public D3D12GraphicsAdapter(D3D12GraphicsInstance instance, IDXGIAdapter1* adapter, ID3D12Device* tempDevice)
    {
        _instance = instance;
        _adapter = new(adapter);

        AdapterDescription1 adapterDesc;
        ThrowIfFailed(_adapter.Get()->GetDesc1(&adapterDesc));

        D3D12Features features = new(tempDevice);

        VendorId = adapterDesc.VendorId;
        DeviceId = adapterDesc.DeviceId;
        Name = new string((char*)adapterDesc.Description);

        // Detect adapter type.
        if ((adapterDesc.Flags & AdapterFlags.Software) != AdapterFlags.None)
        {
            AdapterType = GraphicsAdapterType.Cpu;
        }
        else
        {
            AdapterType = features.UMA() ? GraphicsAdapterType.IntegratedGpu : GraphicsAdapterType.DiscreteGpu;
        }

        // Convert the adapter's D3D12 driver version to a readable string like "24.21.13.9793".
        long umdVersion;
        if (_adapter.Get()->CheckInterfaceSupport(__uuidof<IDXGIDevice>(), &umdVersion) != DXGI_ERROR_UNSUPPORTED)
        {
            string driverDescription = "D3D12 driver version ";

            for (int i = 0; i < 4; ++i)
            {
                ushort driverVersionPart = (ushort)((umdVersion >> (48 - 16 * i)) & 0xFFFF);
                driverDescription += $"{driverVersionPart}.";
            }

            DriverDescription = driverDescription;
        }
        else
        {
            DriverDescription = string.Empty;
        }
    }

    public IDXGIFactory2* DXGIFactory => (IDXGIFactory2*)_instance.DXGIFactory;
    public bool TearingSupported => _instance.TearingSupported;
    public IDXGIAdapter1* Handle => _adapter;

    /// <inheritdoc />
    public override uint VendorId { get; }

    /// <inheritdoc />
    public override uint DeviceId { get; }

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override string DriverDescription { get; }

    /// <inheritdoc />
    public override GraphicsAdapterType AdapterType { get; }

    /// <inheritdoc />
    public override BackendType Backend => BackendType.D3D12;

    /// <inheritdoc />
    public void Dispose()
    {
        _adapter.Dispose();
    }

    /// <inheritdoc />
    public override PixelFormat GetPreferredFormat(Surface surface) => PixelFormat.Bgra8UnormSrgb;

    public override GraphicsDevice CreateDevice(in GraphicsDeviceDescriptor descriptor)
    {
        ComPtr<ID3D12Device5> device = default;
        HResult result = D3D12CreateDevice((IUnknown*)_adapter.Get(), FeatureLevel.Level_11_0, __uuidof<ID3D12Device5>(), device.GetVoidAddressOf());
        if (result.Failure)
        {
            throw new GraphicsException();
        }

        if (!string.IsNullOrEmpty(descriptor.Label))
        {
            device.Get()->SetName(descriptor.Label);
        }

        if (_instance.ValidationMode != ValidationMode.Disabled)
        {
            using ComPtr<ID3D12DebugDevice1> debugDevice = default;
            if (device.CopyTo(debugDevice.GetAddressOf()).Success)
            {
                const bool g_D3D12DebugLayer_AllowBehaviorChangingDebugAids = true;
                const bool g_D3D12DebugLayer_ConservativeResourceStateTracking = true;
                const bool g_D3D12DebugLayer_DisableVirtualizedBundlesValidation = false;

                DebugFeature featureFlags = DebugFeature.None;
                if (g_D3D12DebugLayer_AllowBehaviorChangingDebugAids)
                    featureFlags |= DebugFeature.AllowBehaviorChangingDebugAids;
                if (g_D3D12DebugLayer_ConservativeResourceStateTracking)
                    featureFlags |= DebugFeature.ConservativeResourceStateTracking;
                if (g_D3D12DebugLayer_DisableVirtualizedBundlesValidation)
                    featureFlags |= DebugFeature.DisableVirtualizedBundlesValidation;

                ThrowIfFailed(debugDevice.Get()->SetDebugParameter(DebugDeviceParameterType.FeatureFlags, &featureFlags, sizeof(DebugFeature)));
            }

            // Configure debug device (if active).
            using ComPtr<ID3D12InfoQueue> infoQueue = default;
            if (device.CopyTo(infoQueue.GetAddressOf()).Success)
            {
                infoQueue.Get()->SetBreakOnSeverity(MessageSeverity.Corruption, true);
                infoQueue.Get()->SetBreakOnSeverity(MessageSeverity.Error, true);

                // These severities should be seen all the time
                uint enabledSeveritiesCount = (_instance.ValidationMode == ValidationMode.Verbose) ? 5u : 4u;
                MessageSeverity* enabledSeverities = stackalloc MessageSeverity[5]
                {
                    MessageSeverity.Corruption,
                    MessageSeverity.Error,
                    MessageSeverity.Warning,
                    MessageSeverity.Message,
                    MessageSeverity.Info
                };

                const int disabledMessagesCount = 9;
                MessageId* disabledMessages = stackalloc MessageId[disabledMessagesCount]
                {
                    MessageId.ClearRenderTargetViewMismatchingClearValue,
                    MessageId.ClearDepthStencilViewMismatchingClearValue,
                    MessageId.MapInvalidNullRange,
                    MessageId.UnmapInvalidNullRange,
                    MessageId.ExecuteCommandListsWrongSwapchainBufferReference,
                    MessageId.ResourceBarrierMismatchingCommandListType,
                    MessageId.ExecuteCommandListsGpuWrittenReadbackResourceMapped,
                    MessageId.LoadPipelineNameNotFound,
                    MessageId.StorePipelineDuplicateName
                };

                InfoQueueFilter filter = new();
                filter.AllowList.NumSeverities = enabledSeveritiesCount;
                filter.AllowList.pSeverityList = enabledSeverities;
                filter.DenyList.NumIDs = disabledMessagesCount;
                filter.DenyList.pIDList = disabledMessages;

                // Clear out the existing filters since we're taking full control of them
                _ = infoQueue.Get()->PushEmptyStorageFilter();

                ThrowIfFailed(infoQueue.Get()->AddStorageFilterEntries(&filter));
            }
        }

        return new D3D12GraphicsDevice(this, device);
    }
}
