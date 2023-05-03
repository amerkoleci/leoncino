// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using Win32.Graphics.Direct3D12;

namespace Leoncino.D3D12;

internal static unsafe class D3DUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CommandListType ToD3D12(this CommandQueue queue)
    {
        switch (queue)
        {
            default:
            case CommandQueue.Graphics:
                return CommandListType.Direct;

            case CommandQueue.Compute:
                return CommandListType.Compute;

            case CommandQueue.Copy:
                return CommandListType.Copy;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QueryHeapType ToD3D12(this QueryType type)
    {
        switch (type)
        {
            default:
            case QueryType.Timestamp:
                return QueryHeapType.Timestamp;

            case QueryType.Occlusion:
            case QueryType.BinaryOcclusion:
                return QueryHeapType.Occlusion;

            case QueryType.PipelineStatistics:
                return QueryHeapType.PipelineStatistics;
        }
    }
}

//unsafe partial class Kernel32
//{
//    [LibraryImport("kernel32")]
//    public static partial void InitializeSRWLock(void* SRWLock);

//    [LibraryImport("kernel32")]
//    public static partial void AcquireSRWLockExclusive(void* SRWLock);

//    [LibraryImport("kernel32")]
//    public static partial void ReleaseSRWLockExclusive(void* SRWLock);
//}
