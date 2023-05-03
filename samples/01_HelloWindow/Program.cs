// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Numerics;
using Leoncino.SampleFramework;

namespace Leoncino.Samples;

public static class Program
{
    class TestApp : Application
    {
        public override string Name => "01_HelloWindow";

        public TestApp(ValidationMode validationMode)
            : base(validationMode)
        {

        }

        protected override void OnTick()
        {
        }
    }

    public static void Main()
    {
        ValidationMode validationMode = ValidationMode.Disabled;
#if DEBUG
        validationMode = ValidationMode.Enabled;
#endif

        using TestApp testApp = new(validationMode);
        testApp.Run();
    }
}
