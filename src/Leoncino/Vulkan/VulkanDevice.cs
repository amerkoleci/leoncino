// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal unsafe class VulkanDevice : GPUDevice
{
    private VulkanAdapter _adapter;

    public VulkanDevice(VulkanAdapter adapter)
    {
        _adapter = adapter;
    }

    /// <inheritdoc />
    public override GPUAdapter Adapter => _adapter;

    public VkDevice Handle { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanDevice" /> class.
    /// </summary>
    ~VulkanDevice() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    /// <inheritdoc />
    protected override GPUBuffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override unsafe GPUTexture CreateTextureCore(in TextureDescriptor descriptor, TextureData* initialData)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override BindGroupLayout CreateBindGroupLayoutCore(in BindGroupLayoutDescriptor descriptor)
    {
        throw new NotImplementedException();
    }
}
