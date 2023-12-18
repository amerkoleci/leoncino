// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="Instance"/>.
/// </summary>
public readonly record struct InstanceDescriptor
{
    public InstanceDescriptor()
    {
    }

    /// <summary>
    /// Gets or sets the preferred backend to creates.
    /// </summary>
    public GraphicsBackendType PreferredBackend { get; init; } = GraphicsBackendType.Count;

    /// <summary>
    /// Gets or sets the <see cref="GraphicsDevice"/> validation mode.
    /// </summary>
    public ValidationMode ValidationMode { get; init; } = ValidationMode.Disabled;

    /// <summary>
    /// Gets or sets the label of <see cref="Instance"/>.
    /// </summary>
    public string? Label { get; init; } = default;
}
