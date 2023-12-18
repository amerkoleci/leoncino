// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public sealed class GraphicsException : Exception
{
    public GraphicsException(string message)
        : base(message)
    {
        //Log.Error(message);
    }
}
