// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Represents errors and exceptions that occurs in the Leoncino library.
/// </summary>
public sealed class GraphicsException : Exception
{
    /// <summary>
    /// Create a new instance of a new <see cref="GraphicsException"/> class.
    /// </summary>
    public GraphicsException()
    {
    }


    /// <summary>
    /// Create a new instance of <see cref="GraphicsException"/> class with the given message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public GraphicsException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Create a new instance of <see cref="GraphicsException"/> class with the given message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public GraphicsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
