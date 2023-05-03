// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Operation to perform on the stencil value.
/// </summary>
public enum StencilOperation
{
    /// <summary>
    /// Keep stencil value unchanged.
    /// </summary>
    Keep,
    /// <summary>
    /// Set stencil value to zero.
    /// </summary>
    Zero,
    /// <summary>
    /// /// Replace stencil value with value provided in most recent call to SetStencilReference
    /// </summary>
    Replace,
    /// <summary>
    /// /// Increments stencil value by one, clamping on overflow.
    /// </summary>
    IncrementClamp,
    /// <summary>
    /// /// Decrements stencil value by one, clamping on underflow.
    /// </summary>
    DecrementClamp,
    /// <summary>
    /// Bitwise inverts stencil value.
    /// </summary>
    Invert,
    /// <summary>
    /// Increments stencil value by one, wrapping on overflow.
    /// </summary>
    IncrementWrap,
    /// <summary>
    /// Decrements stencil value by one, wrapping on underflow.
    /// </summary>
    DecrementWrap,
}
