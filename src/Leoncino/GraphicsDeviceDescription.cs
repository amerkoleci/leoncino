// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="GraphicsDevice"/>.
/// </summary>
public readonly record struct GraphicsDeviceDescription
{
    public GraphicsDeviceDescription()
    {
    }

    /// <summary>
    /// Gets or sets the label.
    /// </summary>
    public string? Label { get; init; } = default;
}
