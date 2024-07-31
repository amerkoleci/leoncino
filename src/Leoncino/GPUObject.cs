// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// An base graphics object that was created by <see cref="GraphicsDevice"/>
/// </summary>
public abstract class GPUObject : GraphicsObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GPUObject" /> class.
    /// </summary>
    /// <param name="label">The label of the object or <c>null</c>.</param>
    protected GPUObject(string? label = default)
        : base(label)
    {
    }

    /// <summary>
    /// Get the <see cref="GraphicsDevice"/> object that created this object.
    /// </summary>
    public abstract GraphicsDevice Device { get; }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Device.QueueDestroy(this);
        }
    }

    /// <summary>
    /// The safe moment to actually destroy object.
    /// </summary>
    protected internal abstract void Destroy();
}
