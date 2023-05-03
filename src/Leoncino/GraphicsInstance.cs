// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Instance that enumerate graphics adapters and creates surface.
/// </summary>
public abstract class GraphicsInstance : DisposableObject
{
    protected GraphicsInstance(BackendType backend)
    {
        Backend = backend;
    }

    public BackendType Backend { get; }

    public abstract IReadOnlyList<GraphicsAdapter> EnumerateAdapters(PowerPreference powerPreference = PowerPreference.HighPerformance);

    public static GraphicsInstance CreateDefault(in GraphicsInstanceDescriptor descriptor)
    {
        BackendType backend = descriptor.PreferredBackend;
        if (backend == BackendType.Count)
        {
            backend = BackendType.D3D12;
        }

        switch (backend)
        {
            case BackendType.Null:
                break;
            case BackendType.Vulkan:
                break;
#if !EXCLUDE_D3D12_BACKEND
            case BackendType.D3D12:
                return new D3D12.D3D12GraphicsInstance(descriptor);
#endif

            case BackendType.D3D11:
                break;
            case BackendType.Metal:
                break;
            default:
                throw new GraphicsException();
        }

        throw new GraphicsException();
    }
}
