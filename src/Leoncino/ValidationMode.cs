// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Type of validation during creation of <see cref="GraphicsFactory"/>.
/// </summary>
/// <remarks>See <see cref="GraphicsFactoryDescription.ValidationMode"/></remarks>
public enum ValidationMode
{
    /// <summary>
    /// No validation is enabled.
    /// </summary>
    Disabled,
    /// <summary>
    /// Print warnings and errors.
    /// </summary>
    Enabled,
    /// <summary>
    /// Print all warnings, errors and info messages.
    /// </summary>
    Verbose,
    /// Enable GPU-based validation
    GPU
}
