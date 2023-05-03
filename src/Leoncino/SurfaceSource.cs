// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Leoncino;

/// <summary>
/// Defines a platform specific surface source used for <see cref="Surface"/> creation.
/// </summary>
public abstract class SurfaceSource
{
    protected SurfaceSource()
    {

    }

    public abstract SurfaceSourceType Type { get; }

    /// <summary>
    /// Creates a new <see cref="SurfaceSource"/> for a Win32 window.
    /// </summary>
    /// <param name="hinstance">The Win32 window hinstance.</param>
    /// <param name="hwnd">The Win32 window handle.</param>
    /// <returns>A new <see cref="SurfaceSource"/> which can be used to create a <see cref="Surface"/> for the given Win32 window.
    /// </returns>
    public static SurfaceSource CreateWin32(IntPtr hinstance, IntPtr hwnd) => new Win32SurfaceSource(hinstance, hwnd);
}

internal sealed class Win32SurfaceSource : SurfaceSource
{
    public Win32SurfaceSource(IntPtr hinstance, IntPtr hwnd)
    {
        HInstance = hinstance;
        Hwnd = hwnd;
    }

    public IntPtr HInstance { get; }
    public IntPtr Hwnd { get; }

    /// <inheritdoc />
    public override SurfaceSourceType Type => SurfaceSourceType.Win32;
}
