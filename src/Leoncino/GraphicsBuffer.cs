// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Leoncino;

/// <summary>
/// Defines a graphics buffer that holds data.
/// </summary>
public abstract class GraphicsBuffer : GPUObject
{
    protected GraphicsBuffer(in BufferDescriptor descriptor)
        : base(descriptor.Label)
    {
        Size = descriptor.Size;
        Usage = descriptor.Usage;
        CpuAccess = descriptor.CpuAccess;
    }

    /// <summary>
    /// The total size, in bytes, of the buffer.
    /// </summary>
    public ulong Size { get; }

    /// <summary>
    /// A bitmask indicating this buffer usage.
    /// </summary>
    public BufferUsage Usage { get; }

    /// <summary>
    /// Getsthe CPU access of the <see cref="Buffer"/>.
    /// </summary>
    public CpuAccessMode CpuAccess { get; }

    public unsafe void SetData<T>(in T data, ulong offsetInBytes = 0) where T : unmanaged
    {
        fixed (T* pointer = &data)
        {
            SetData(new ReadOnlySpan<T>(pointer, 1), offsetInBytes);
        }
    }

    public unsafe void SetData<T>(ReadOnlySpan<T> data, ulong offsetInBytes = 0) where T : unmanaged
    {
        Debug.Assert(CpuAccess == CpuAccessMode.Write);

#if VALIDATE_USAGE
        if (CpuAccess != CpuAccessMode.Write)
        {
            throw new GraphicsException("Cannot set buffer data for not writeable buffer");
        }
#endif

        fixed (T* dataPtr = data)
        {
            SetDataUnsafe(dataPtr, offsetInBytes);  
        }
    }

    public T GetData<T>(ulong offsetInBytes = 0) where T : unmanaged
    {
        T data = new();
        GetData(ref data, offsetInBytes);

        return data;
    }

    public unsafe void GetData<T>(ref T data, ulong offsetInBytes = 0) where T : unmanaged
    {
        Debug.Assert(CpuAccess != CpuAccessMode.None);

        fixed (T* destPtr = &data)
        {
            GetDataUnsafe(destPtr, offsetInBytes);
        }
    }

    public void GetData<T>(Span<T> destination, ulong offsetInBytes = 0) where T : unmanaged
    {
        GetData(ref MemoryMarshal.GetReference(destination), offsetInBytes);
    }

    protected unsafe abstract void SetDataUnsafe(void* dataPtr, ulong offsetInBytes);
    protected unsafe abstract void GetDataUnsafe(void* destPtr, ulong offsetInBytes);
}
