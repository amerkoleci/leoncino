// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public abstract class Instance : GraphicsObjectBase
{
    protected Instance(in InstanceDescriptor descriptor)
        : base(descriptor.Label)
    {
    }

    public Surface CreateSurface(in SurfaceDescriptor descriptor)
    {
#if VALIDATE_USAGE
        if (descriptor.Source is null)
        {
            throw new GraphicsException("SurfaceDescriptor.Source must be valid.");
        }
#endif

        return CreateSurfaceCore(in descriptor);
    }

    // TODO: Async
    public GraphicsAdapter RequestAdapter(in RequestAdapterOptions options)
    {
        return RequestAdapterCore(in options);
    }

    protected abstract Surface CreateSurfaceCore(in SurfaceDescriptor descriptor);

    protected abstract GraphicsAdapter RequestAdapterCore(in RequestAdapterOptions options);

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

    public static Instance Create(in InstanceDescriptor descriptor)
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

        Instance? instance = default;
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
            throw new GraphicsException($"{backend} is not supported");
        }

        return instance!;
    }
}
