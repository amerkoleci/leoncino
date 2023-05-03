// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="Buffer"/>.
/// </summary>
public record struct BufferDescriptor
{
    public BufferDescriptor(
        ulong size,
        BufferUsage usage = BufferUsage.ShaderReadWrite,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        Usage = usage;
        Size = size;
        CpuAccess = access;
        Label = label;
    }

    /// <summary>
    /// Size in bytes of <see cref="Buffer"/>
    /// </summary>
    public ulong Size { get; init; }

    /// <summary>
    /// Gets or Sets the <see cref="BufferUsage"/> of <see cref="Buffer"/>.
    /// </summary>
    public BufferUsage Usage { get; init; }

    /// <summary>
    /// CPU access of the <see cref="Buffer"/>.
    /// </summary>
    public CpuAccessMode CpuAccess { get; init; }

    // <summary>
    /// Gets or sets the label of <see cref="Buffer"/>.
    /// </summary>
    public string? Label { get; init; }
}
