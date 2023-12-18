// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Structure that describes the <see cref="GraphicsAdapter"/> request.
/// </summary>
public readonly record struct RequestAdapterOptions
{
    public RequestAdapterOptions()
    {
    }

    public Surface? CompatibleSurface { get; init; }

    /// <summary>
    /// Gets or sets the GPU adapter selection power preference.
    /// </summary>
    public PowerPreference PowerPreference { get; init; } = PowerPreference.Undefined;

    /// <summary>
    /// Gets or sets the label of <see cref="Instance"/>.
    /// </summary>
    public string? Label { get; init; } = default;
}
