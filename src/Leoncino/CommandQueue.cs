// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines the type of <see cref="CommandQueue"/>.
/// </summary>
public enum CommandQueue
{
    Graphics = 0,
    Compute,
    Copy,

    Count
}
