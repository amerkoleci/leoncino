// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// A bitmask indicating how a <see cref="Texture"/> is permitted to be used.
/// </summary>
[Flags]
public enum TextureUsage
{
    /// <summary>
    /// None usage.
    /// </summary>
    None = 0,
    /// <summary>
    /// Supports shader read access.
    /// </summary>
    ShaderRead = 1 << 0,
    /// <summary>
    /// Supports write read access.
    /// </summary>
    ShaderWrite = 1 << 1,
    /// <summary>
    /// Supports shader read and write access.
    /// </summary>
    ShaderReadWrite = ShaderRead | ShaderWrite,
    /// <summary>
    /// Supports usage as render target / depth stencil target.
    /// </summary>
    RenderTarget = 1 << 2,
    /// <summary>
    /// Supports transient memory usage.
    /// </summary>
    Transient = 1 << 3,
    ShadingRate = 1 << 4,
}
