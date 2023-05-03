// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using NUnit.Framework;

namespace Leoncino.Tests;

[TestFixture(TestOf = typeof(GraphicsInstance))]
public class GraphicsInstanceTests
{
    [TestCase]
    public void Default_IsValid()
    {
        using GraphicsInstance instance = GraphicsInstance.CreateDefault(new GraphicsInstanceDescriptor());
        Assert.IsNotNull(instance);
    }
}
