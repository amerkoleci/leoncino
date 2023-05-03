// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Type of query contained in a <see cref="QueryHeap"/>.
/// </summary>
public enum QueryType
{
    /// Create a heap to contain timestamp queries.
    Timestamp,
    /// Used for occlusion query heap or occlusion queries.
    Occlusion,
    /// Can be used in the same heap as occlusion.
    BinaryOcclusion,
    /// Create a heap to contain a structure of `PipelineStatistics`
    PipelineStatistics,
}
