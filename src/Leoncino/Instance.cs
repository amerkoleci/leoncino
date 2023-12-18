// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public abstract class Instance : GraphicsObjectBase
{
    protected Instance(in InstanceDescriptor descriptor)
        : base(descriptor.Label)
    {
    }
}
