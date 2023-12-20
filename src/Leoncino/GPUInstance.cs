// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a global GPU instance/factory for enumerating adapters and surface creation
/// </summary>
public abstract class GPUInstance : GPUObjectBase
{
    protected GPUInstance(in InstanceDescriptor descriptor)
        : base(descriptor.Label)
    {
    }

    public GPUSurface CreateSurface(in SurfaceDescriptor descriptor)
    {
#if VALIDATE_USAGE
        if (descriptor.Source is null)
        {
            throw new GPUException("SurfaceDescriptor.Source must be valid.");
        }
#endif

        return CreateSurfaceCore(in descriptor);
    }

    public ValueTask<GPUAdapter> RequestAdapterAsync(in RequestAdapterOptions options)
    {
        return RequestAdapterAsyncCore(in options);
    }

    protected abstract GPUSurface CreateSurfaceCore(in SurfaceDescriptor descriptor);

    protected abstract ValueTask<GPUAdapter> RequestAdapterAsyncCore(in RequestAdapterOptions options);

    public static bool IsBackendSupport(GraphicsBackendType backend)
    {
        //Guard.IsTrue(backend != GraphicsBackendType.Count, nameof(backend), "Invalid backend");

        switch (backend)
        {
#if !EXCLUDE_VULKAN_BACKEND
            case GraphicsBackendType.Vulkan:
                return Vulkan.VulkanGraphicsDevice.IsSupported();
#endif

#if !EXCLUDE_D3D12_BACKEND
            case GraphicsBackendType.D3D12:
                return D3D12.D3D12GraphicsDevice.IsSupported();
#endif

#if !EXCLUDE_METAL_BACKEND
            case GraphicsBackendType.Metal:
                return false;
#endif

#if !EXCLUDE_WEBGPU_BACKEND
            case GraphicsBackendType.WebGPU:
                return WebGPU.WebGPUInstance.IsSupported();
#endif

            default:
                return false;
        }
    }

    public static GPUInstance Create(in InstanceDescriptor descriptor)
    {
        GraphicsBackendType backend = descriptor.PreferredBackend;
        if (backend == GraphicsBackendType.Count)
        {
            if (IsBackendSupport(GraphicsBackendType.D3D12))
            {
                backend = GraphicsBackendType.D3D12;
            }
            else if (IsBackendSupport(GraphicsBackendType.Metal))
            {
                backend = GraphicsBackendType.Metal;
            }
            else if (IsBackendSupport(GraphicsBackendType.Vulkan))
            {
                backend = GraphicsBackendType.Vulkan;
            }
            else if (IsBackendSupport(GraphicsBackendType.WebGPU))
            {
                backend = GraphicsBackendType.WebGPU;
            }
        }

        GPUInstance? instance = default;
        switch (backend)
        {
#if !EXCLUDE_VULKAN_BACKEND
            case GraphicsBackendType.Vulkan:
                if (Vulkan.VulkanGraphicsDevice.IsSupported())
                {
                    instance = new Vulkan.VulkanGraphicsDevice(in description);
                }
                break;
#endif

#if !EXCLUDE_D3D12_BACKEND 
            case GraphicsBackendType.D3D12:
                if (D3D12.D3D12GraphicsDevice.IsSupported())
                {
                    instance = new D3D12.D3D12GraphicsDevice(in description);
                }
                break;
#endif

#if !EXCLUDE_METAL_BACKEND
            case GraphicsBackendType.Metal:
                break;
#endif

#if !EXCLUDE_WEBGPU_BACKEND
            case GraphicsBackendType.WebGPU:
                if (WebGPU.WebGPUInstance.IsSupported())
                {
                    instance = new WebGPU.WebGPUInstance(in descriptor);
                }
                break;
#endif

            default:
                break;
        }

        if (instance == null)
        {
            throw new GPUException($"{backend} is not supported");
        }

        return instance!;
    }
}
