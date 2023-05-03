// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32.Graphics.Direct3D12;
using D3D12Feature = Win32.Graphics.Direct3D12.Feature;

namespace Leoncino.D3D12;

internal unsafe class D3D12Features
{
    // Feature support data structs
    private readonly FeatureDataD3D12Options _options;
    private readonly FeatureDataGpuVirtualAddressSupport _gpuVASupport;
    private readonly FeatureDataD3D12Options1 _options1;

    private readonly FeatureDataProtectedResourceSessionSupport[] _protectedResourceSessionSupport;
    private readonly FeatureDataArchitecture1[] _architecture1;

    public D3D12Features(ID3D12Device* device)
    {
        // Initialize static feature support data structures
        if (device->CheckFeatureSupport(D3D12Feature.Options, ref _options).Failure)
        {
            _options = default;
        }

        if (device->CheckFeatureSupport(D3D12Feature.GpuVirtualAddressSupport, ref _gpuVASupport).Failure)
        {
            _gpuVASupport = default;
        }

        if (device->CheckFeatureSupport(D3D12Feature.Options1, ref _options1).Failure)
        {
            _options1 = default;
        }

        // Initialize per-node feature support data structures
        NodeCount = device->GetNodeCount();
        _protectedResourceSessionSupport = new FeatureDataProtectedResourceSessionSupport[NodeCount];
        _architecture1 = new FeatureDataArchitecture1[NodeCount];

        for (uint nodeIndex = 0; nodeIndex < NodeCount; nodeIndex++)
        {
            _protectedResourceSessionSupport[nodeIndex].NodeIndex = nodeIndex;
            if (device->CheckFeatureSupport(D3D12Feature.ProtectedResourceSessionSupport, ref _protectedResourceSessionSupport[nodeIndex]).Failure)
            {
                _protectedResourceSessionSupport[nodeIndex].Support = ProtectedResourceSessionSupportFlags.None;
            }

            _architecture1[nodeIndex].NodeIndex = nodeIndex;
            if (device->CheckFeatureSupport(D3D12Feature.Architecture1, ref _architecture1[nodeIndex]).Failure)
            {
                FeatureDataArchitecture archLocal = new();
                archLocal.NodeIndex = nodeIndex;
                if (device->CheckFeatureSupport(D3D12Feature.Architecture, ref archLocal).Failure) 
                {
                    archLocal.TileBasedRenderer = false;
                    archLocal.UMA = false;
                    archLocal.CacheCoherentUMA = false;
                }

                _architecture1[nodeIndex].TileBasedRenderer = archLocal.TileBasedRenderer;
                _architecture1[nodeIndex].UMA = archLocal.UMA;
                _architecture1[nodeIndex].CacheCoherentUMA = archLocal.CacheCoherentUMA;
                _architecture1[nodeIndex].IsolatedMMU = false;
            }
        }
    }

    public uint NodeCount { get; }

    public bool TileBasedRenderer(uint nodeIndex = 0) => _architecture1[nodeIndex].TileBasedRenderer;
    public bool UMA(uint nodeIndex = 0) => _architecture1[nodeIndex].UMA;
    public bool CacheCoherentUMA(uint nodeIndex = 0) => _architecture1[nodeIndex].CacheCoherentUMA;
    public bool IsolatedMMU(uint nodeIndex = 0) => _architecture1[nodeIndex].IsolatedMMU;
}
