// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

public abstract class BindGroupLayout : GPUObject
{
    protected BindGroupLayout(in BindGroupLayoutDescriptor descriptor)
        : base(descriptor.Label)
    {
    }
}
