// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;

namespace Leoncino;

/// <summary>
/// Base class for graphics objects, that implements <see cref="IDisposable"/> interface.
/// </summary>
public abstract class GraphicsObjectBase : IDisposable
{
    private volatile uint _isDisposed = 0;
    private string? _label;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicsObjectBase" /> class.
    /// </summary>
    /// <param name="label">The label of the object or <c>null</c>.</param>
    protected GraphicsObjectBase(string? label = default)
    {
        _label = label;
        _isDisposed = 0;
    }

    /// <summary>
    /// Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
    /// </summary>
    public bool IsDisposed => _isDisposed != 0;

    /// <summary>
    /// Gets or sets the label that identifies this object.
    /// </summary>
    public string? Label
    {
        get => _label;
        set
        {
            _label = value;
            OnLabelChanged(_label ?? string.Empty);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="disposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool disposing);

    /// <summary>Throws an exception if the object has been disposed.</summary>
    /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ThrowIfDisposed()
    {
        if (_isDisposed != 0)
        {
            throw new ObjectDisposedException(ToString());
        }
    }

    protected virtual void OnLabelChanged(string newLabel)
    {
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        if (string.IsNullOrEmpty(_label))
        {
            return base.ToString();
        }

        return _label;
    }
}
