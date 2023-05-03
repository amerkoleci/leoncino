// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public enum GraphicsAdapterType
{
    /// <summary>
    /// Other or Unknown.
    /// </summary>
    Other,
    /// <summary>
    /// Integrated GPU with shared CPU/GPU memory.
    /// </summary>
    IntegratedGpu,
    /// <summary>
    /// Discrete GPU with separate CPU/GPU memory.
    /// </summary>
    DiscreteGpu,
    /// <summary>
    /// Virtual / Hosted.
    /// </summary>
    VirtualGpu,
    /// <summary>
    /// Cpu / Software / WARP Rendering.
    /// </summary>
    Cpu,
}
