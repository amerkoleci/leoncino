// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="GraphicsFactory"/>.
/// </summary>
public readonly record struct GraphicsFactoryDescription
{
    public GraphicsFactoryDescription()
    {
    }

    /// <summary>
    /// Gets or sets the preferred backend to create.
    /// </summary>
    public GraphicsBackend PreferredBackend { get; init; } = GraphicsBackend.Count;

    /// <summary>
    /// Gets or sets the <see cref="GraphicsFactory"/> validation mode.
    /// </summary>
    public ValidationMode ValidationMode { get; init; } = ValidationMode.Disabled;

    /// <summary>
    /// Gets or sets the label of <see cref="GraphicsFactory"/>.
    /// </summary>
    public string? Label { get; init; } = default;
}
