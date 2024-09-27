// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace Leoncino.Vulkan;

internal unsafe partial class VulkanGraphicsBuffer : GraphicsBuffer
{
    private readonly VulkanGraphicsDevice _device;
    private readonly void* _pMappedData = default;

    public VulkanGraphicsBuffer(VulkanGraphicsDevice device, in BufferDescriptor description)
        : base(description)
    {
        _device = device;
    }

    public VkBuffer Handle { get; }

    public override GraphicsDevice Device => throw new NotImplementedException();

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanGraphicsBuffer" /> class.
    /// </summary>
    ~VulkanGraphicsBuffer() => Dispose(disposing: false);

    /// <inheritdoc />
    protected internal override void Destroy() 
    {
    }

    protected override unsafe void SetDataUnsafe(void* dataPtr, ulong offsetInBytes)
    {
        NativeMemory.Copy(dataPtr, (byte*)_pMappedData + offsetInBytes, (nuint)Size);
    }

    protected override unsafe void GetDataUnsafe(void* destPtr, ulong offsetInBytes) => throw new NotImplementedException();
}
