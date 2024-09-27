// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal unsafe class VulkanGraphicsDevice : GraphicsDevice
{
    private VulkanGraphicsAdapter _adapter;

    public VulkanGraphicsDevice(VulkanGraphicsAdapter adapter, in GraphicsDeviceDescriptor description)
        : base(description.Label)
    {
        _adapter = adapter;
    }

    /// <inheritdoc />
    public override GraphicsAdapter Adapter => _adapter;

    public VkDevice Handle { get; }

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanGraphicsDevice" /> class.
    /// </summary>
    ~VulkanGraphicsDevice() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    /// <inheritdoc />
    protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, void* initialData)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override unsafe Texture CreateTextureCore(in TextureDescriptor descriptor, TextureData* initialData)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override BindGroupLayout CreateBindGroupLayoutCore(in BindGroupLayoutDescriptor descriptor)
    {
        throw new NotImplementedException();
    }
}
