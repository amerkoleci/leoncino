// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="GraphicsDevice"/>.
/// </summary>
public readonly record struct DeviceDescriptor
{
    public DeviceDescriptor()
    {
    }

    /// <summary>
    /// Gets or sets the label of <see cref="Instance"/>.
    /// </summary>
    public string? Label { get; init; } = default;
}
