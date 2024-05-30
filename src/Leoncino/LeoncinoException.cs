// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Represents exceptions that occurs in the Leoncino library.
/// </summary>
public sealed class LeoncinoException : Exception
{
    /// <summary>
    /// Create a new instance of <see cref="LeoncinoException"/> with the given message 
    /// </summary>
    /// <param name="message">The exception message.</param>
    public LeoncinoException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Create a new instance of <see cref="LeoncinoException"/> with the given message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public LeoncinoException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
