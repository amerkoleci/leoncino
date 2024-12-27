// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino.Vulkan;

internal struct VulkanPhysicalDeviceVideoExtensions
{
    public bool Queue;
    public bool DecodeQueue;
    public bool DecodeH264;
    public bool DecodeH265;
    public bool EncodeQueue;
    public bool EncodeH264;
    public bool EncodeH265;
}

internal struct VulkanPhysicalDeviceExtensions
{
    // Core 1.3
    public bool Maintenance4;
    public bool DynamicRendering;
    public bool Synchronization2;
    public bool ExtendedDynamicState;
    public bool ExtendedDynamicState2;
    public bool PipelineCreationCacheControl;
    public bool FormatFeatureFlags2;

    // Core 1.4
    public bool PushDescriptor;

    // Extensions
    public bool Swapchain;
    public bool Maintenance5;
    public bool Maintenance6;
    public bool DepthClipEnable;
    public bool MemoryBudget;
    public bool AMD_DeviceCoherentMemory;
    public bool MemoryPriority;

    public bool ExternalMemory;
    public bool ExternalSemaphore;
    public bool ExternalFence;

    public bool PerformanceQuery;
    public bool HostQueryReset;
    public bool DeferredHostOperations;
    public bool PortabilitySubset;
    public bool TextureCompressionAstcHdr;
    public bool ShaderViewportIndexLayer;
    public bool ConservativeRasterization;

    public bool AccelerationStructure;
    public bool RaytracingPipeline;
    public bool RayQuery;
    public bool FragmentShadingRate;
    public bool MeshShader;
    public bool ConditionalRendering;
    public bool win32_full_screen_exclusive;

    public VulkanPhysicalDeviceVideoExtensions Video;
}
