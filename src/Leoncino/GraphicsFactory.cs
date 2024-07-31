// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a graphics factory for enumerating adapters and surface creation
/// </summary>
public abstract class GraphicsFactory : GraphicsObject
{
    protected GraphicsFactory(in GraphicsFactoryDescription description)
        : base(description.Label)
    {
    }

    /// <summary>
    /// Gets a value identifying the specific graphics backend used by this factory.
    /// </summary>
    public abstract GraphicsBackend BackendType { get; }

    public GraphicsSurface CreateSurface(in SurfaceDescription descriptor)
    {
#if VALIDATE_USAGE
        if (descriptor.Source is null)
        {
            throw new GraphicsException("SurfaceDescriptor.Source must be valid.");
        }
#endif

        return CreateSurfaceCore(in descriptor);
    }

    public GraphicsAdapter RequestAdapter(in RequestAdapterOptions options)
    {
        return RequestAdapterCore(in options);
    }

    protected abstract GraphicsSurface CreateSurfaceCore(in SurfaceDescription descriptor);

    protected abstract GraphicsAdapter RequestAdapterCore(in RequestAdapterOptions options);

    public static bool IsBackendSupport(GraphicsBackend backend)
    {
        //Guard.IsTrue(backend != GraphicsBackendType.Count, nameof(backend), "Invalid backend");

        switch (backend)
        {
#if !EXCLUDE_VULKAN_BACKEND
            case GraphicsBackend.Vulkan:
                return Vulkan.VulkanGraphicsFactory.IsSupported();
#endif

#if !EXCLUDE_D3D12_BACKEND
            case GraphicsBackend.Direct3D12:
                return D3D12.D3D12GraphicsFactory.IsSupported();
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

    public static GraphicsFactory Create(in GraphicsFactoryDescription description)
    {
        GraphicsBackend backend = description.PreferredBackend;
        if (backend == GraphicsBackend.Count)
        {
            if (IsBackendSupport(GraphicsBackend.Direct3D12))
            {
                backend = GraphicsBackend.Direct3D12;
            }
            else if (IsBackendSupport(GraphicsBackend.Metal))
            {
                backend = GraphicsBackend.Metal;
            }
            else if (IsBackendSupport(GraphicsBackend.Vulkan))
            {
                backend = GraphicsBackend.Vulkan;
            }
            else if (IsBackendSupport(GraphicsBackend.WebGPU))
            {
                backend = GraphicsBackend.WebGPU;
            }
        }

        GraphicsFactory? instance = default;
        switch (backend)
        {
#if !EXCLUDE_VULKAN_BACKEND
            case GraphicsBackend.Vulkan:
                if (Vulkan.VulkanGraphicsFactory.IsSupported())
                {
                    instance = new Vulkan.VulkanGraphicsFactory(in description);
                }
                break;
#endif

#if !EXCLUDE_D3D12_BACKEND 
            case GraphicsBackend.Direct3D12:
                if (D3D12.D3D12GraphicsFactory.IsSupported())
                {
                    instance = new D3D12.D3D12GraphicsFactory(in description);
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
