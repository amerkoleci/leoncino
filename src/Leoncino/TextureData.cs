// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Leoncino;

/// <summary>
/// Structure that describes data for <see cref="GPUTexture"/>.
/// </summary>
public readonly struct TextureData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextureData"/> struct.
    /// </summary>
    /// <param name="dataPointer">The data pointer.</param>
    /// <param name="rowPitch">The row pitch.</param>
    /// <param name="slicePitch">The slice pitch.</param>
    [SetsRequiredMembers]
    public unsafe TextureData(void* dataPointer, uint rowPitch, uint slicePitch)
    {
        DataPointer = dataPointer;
        RowPitch = rowPitch;
        SlicePitch = slicePitch;
    }

    /// <summary>
    /// Pointer to the data.
    /// </summary>
    public required unsafe void* DataPointer { get; init; }

    /// <summary>
    /// Gets the number of bytes per row.
    /// </summary>
    public required uint RowPitch { get; init; }

    /// <summary>
    /// Gets the number of bytes per slice (for a 3D texture, a slice is a 2D image)
    /// </summary>
    public required uint SlicePitch { get; init; }
}
