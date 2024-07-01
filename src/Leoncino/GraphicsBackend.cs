// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// The backend used by the <see cref="GraphicsFactory"/>.
/// </summary>
public enum GraphicsBackend
{
    /// <summary>
    /// Vulkan
    /// </summary>
    Vulkan,
    /// <summary>
    /// Direct3D 12
    /// </summary>
    Direct3D12,
    /// <summary>
    /// Metal
    /// </summary>
    Metal,
    /// <summary>
    /// WebGPU
    /// </summary>
    WebGPU,

    Count,
}
