// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes a <see cref="BindGroupLayout"/>.
/// </summary>
public readonly ref struct BindGroupLayoutDescription
{
    public BindGroupLayoutDescription(ReadOnlySpan<BindGroupLayoutEntry> entries)
    {
        Entries = entries;
    }

    public ReadOnlySpan<BindGroupLayoutEntry> Entries { get; init; }

    /// <summary>
    /// The label of <see cref="BindGroupLayout"/>.
    /// </summary>
    public string? Label { get; init; }
}
