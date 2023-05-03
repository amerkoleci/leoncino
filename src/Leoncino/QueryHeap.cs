// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a query heap.
/// </summary>
public abstract class QueryHeap : GraphicsObject
{
    protected QueryHeap(GraphicsDevice device, in QueryHeapDescriptor descriptor)
        : base(device, descriptor.Label)
    {
        Type = descriptor.Type;
        Count = descriptor.Count;
    }

    public QueryType Type { get; }
    public int Count { get; }
}
