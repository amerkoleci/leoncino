// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;

namespace Leoncino;

/// <summary>
/// An object which is disposable.
/// </summary>
public abstract class DisposableObject : IDisposable
{
#if NET6_0_OR_GREATER
    private volatile uint _isDisposed;
#else
    private volatile int _isDisposed;
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableObject" /> class.
    /// </summary>
    protected DisposableObject()
    {
        _isDisposed = 0;
    }

    /// <summary>
    /// Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
    /// </summary>
    public bool IsDisposed => _isDisposed != 0;

    /// <inheritdoc />
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>Asserts that the object has not been disposed.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void AssertNotDisposed() => Guard.IsTrue(_isDisposed == 0);

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);

    /// <summary>Sets the name of the object.</summary>
    /// <param name="value">The new name of the object.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void SetNameUnsafe(string value);

    /// <summary>Throws an exception if the object has been disposed.</summary>
    /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ThrowIfDisposed()
    {
        if (_isDisposed != 0)
        {
            throw new ObjectDisposedException(GetType().ToString());
        }
    }

    /// <summary>Marks the object as being disposed.</summary>
    protected void MarkDisposed() => Interlocked.Exchange(ref _isDisposed, 1);
}
