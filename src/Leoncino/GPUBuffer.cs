// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Leoncino;

/// <summary>
/// Defines a GPU buffer
/// </summary>
public abstract class GPUBuffer : GPUObject
{
    protected GPUBuffer(in BufferDescriptor descriptor)
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

    public unsafe void SetData<T>(in T data, int offsetInBytes = 0) where T : unmanaged
    {
        fixed (T* pointer = &data)
        {
            SetData(new Span<T>(pointer, 1), offsetInBytes);
        }
    }

    public unsafe void SetData<T>(Span<T> data, int offsetInBytes = 0) where T : unmanaged
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

    public T GetData<T>(int offsetInBytes = 0) where T : unmanaged
    {
        T data = new();
        GetData(ref data, offsetInBytes);

        return data;
    }

    public unsafe T[] GetArray<T>(int offsetInBytes = 0) where T : unmanaged
    {
        Debug.Assert(CpuAccess != CpuAccessMode.None);

        T[] data = new T[((int)Size / sizeof(T)) - offsetInBytes];
        GetData(data.AsSpan(), offsetInBytes);

        return data;
    }


    public unsafe void GetData<T>(ref T data, int offsetInBytes = 0) where T : unmanaged
    {
        Debug.Assert(CpuAccess != CpuAccessMode.None);

        fixed (T* destPtr = &data)
        {
            GetDataUnsafe(destPtr, offsetInBytes);
        }
    }

    public unsafe void GetData<T>(T[] destination, int offsetInBytes = 0) where T : unmanaged
    {
        Debug.Assert(CpuAccess != CpuAccessMode.None);

        fixed (T* destPtr = destination)
        {
            GetDataUnsafe(destPtr, offsetInBytes);
        }
    }

    public void GetData<T>(Span<T> destination, int offsetInBytes = 0) where T : unmanaged
    {
        GetData(ref MemoryMarshal.GetReference(destination), offsetInBytes);
    }

    protected unsafe abstract void SetDataUnsafe(void* dataPtr, int offsetInBytes);
    protected unsafe abstract void GetDataUnsafe(void* destPtr, int offsetInBytes);
}
