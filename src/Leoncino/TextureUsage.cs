// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// A bitmask indicating how a <see cref="GPUTexture"/> is permitted to be used.
/// </summary>
[Flags]
public enum TextureUsage
{
    /// <summary>
    /// None usage.
    /// </summary>
    None = 0,
    /// <summary>
    /// Supports reading or sampling from the texture in a shader.
    /// </summary>
    ShaderRead = 1 << 0,
    /// <summary>
    /// Supports writing to the texture in a shader.
    /// </summary>
    ShaderWrite = 1 << 1,
    /// <summary>
    /// Supports reading or sampling from the texture or writing to the texture from shader.
    /// </summary>
    ShaderReadWrite = ShaderRead | ShaderWrite,
    /// <summary>
    /// Supports for rendering to the texture in a render pass (color or depth attachment).
    /// </summary>
    RenderTarget = 1 << 2,
    /// <summary>
    /// Specifies transient usage.
    /// </summary>
    Transient = 1 << 3,
    /// <summary>
    /// Supports shading rate access.
    /// </summary>
    ShadingRate = 1 << 4,
    /// <summary>
    /// Supports shared access.
    /// </summary>
    Shared = 1 << 5,
}
