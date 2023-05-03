// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="GraphicsInstance"/>.
/// </summary>
public readonly record struct GraphicsInstanceDescriptor
{
    public GraphicsInstanceDescriptor()
    {
    }

    /// <summary>
    /// Gets or sets the preferred backend to creates.
    /// </summary>
    public BackendType PreferredBackend { get; init; } = BackendType.Count;

    /// <summary>
    /// Gets or sets the validation mode.
    /// </summary>
    public ValidationMode ValidationMode { get; init; } = ValidationMode.Disabled;

    // <summary>
    /// Gets or sets the label of <see cref="GraphicsInstance"/>.
    /// </summary>
    public string? Label { get; init; } = default;
}
