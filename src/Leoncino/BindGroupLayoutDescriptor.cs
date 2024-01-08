// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Leoncino;

/// <summary>
/// Structure that describes a <see cref="BindGroupLayout"/>.
/// </summary>
public readonly record struct BindGroupLayoutDescriptor
{
    public BindGroupLayoutDescriptor(params BindGroupLayoutEntry[] entries)
    {
        Entries = entries;
    }

    public BindGroupLayoutEntry[] Entries { get; init; }

    /// <summary>
    /// The label of <see cref="BindGroupLayout"/>.
    /// </summary>
    public string? Label { get; init; }
}
