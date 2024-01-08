// Copyright (c) Amer Koleci and contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace Leoncino;

/// <summary>
/// Represents a floating-point viewport struct.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public record struct Viewport
{
    /// <summary>
    /// Position of the pixel coordinate of the upper-left corner of the viewport.
    /// </summary>
    public float X;

    /// <summary>
    /// Position of the pixel coordinate of the upper-left corner of the viewport.
    /// </summary>
    public float Y;

    /// <summary>
    /// Width dimension of the viewport.
    /// </summary>
    public float Width;

    /// <summary>
    /// Height dimension of the viewport.
    /// </summary>
    public float Height;

    /// <summary>
    /// Gets or sets the minimum depth of the clip volume.
    /// </summary>
    public float MinDepth;

    /// <summary>
    /// Gets or sets the maximum depth of the clip volume.
    /// </summary>
    public float MaxDepth;

    /// <summary>
    /// Initializes a new instance of the <see cref="Viewport"/> struct.
    /// </summary>
    /// <param name="width">The width of the viewport in pixels.</param>
    /// <param name="height">The height of the viewport in pixels.</param>
    public Viewport(float width, float height)
    {
        X = 0.0f;
        Y = 0.0f;
        Width = width;
        Height = height;
        MinDepth = 0.0f;
        MaxDepth = 1.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Viewport"/> struct.
    /// </summary>
    /// <param name="x">The x coordinate of the upper-left corner of the viewport in pixels.</param>
    /// <param name="y">The y coordinate of the upper-left corner of the viewport in pixels.</param>
    /// <param name="width">The width of the viewport in pixels.</param>
    /// <param name="height">The height of the viewport in pixels.</param>
    public Viewport(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        MinDepth = 0.0f;
        MaxDepth = 1.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Viewport"/> struct.
    /// </summary>
    /// <param name="x">The x coordinate of the upper-left corner of the viewport in pixels.</param>
    /// <param name="y">The y coordinate of the upper-left corner of the viewport in pixels.</param>
    /// <param name="width">The width of the viewport in pixels.</param>
    /// <param name="height">The height of the viewport in pixels.</param>
    /// <param name="minDepth">The minimum depth of the clip volume.</param>
    /// <param name="maxDepth">The maximum depth of the clip volume.</param>
    public Viewport(float x, float y, float width, float height, float minDepth, float maxDepth)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        MinDepth = minDepth;
        MaxDepth = maxDepth;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Viewport"/> struct.
    /// </summary>
    /// <param name="bounds">A <see cref="RectangleF"/> that defines the location and size of the viewport in a render target.</param>
    public Viewport(RectangleF bounds)
    {
        X = bounds.X;
        Y = bounds.Y;
        Width = bounds.Width;
        Height = bounds.Height;
        MinDepth = 0.0f;
        MaxDepth = 1.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Viewport"/> struct.
    /// </summary>
    /// <param name="bounds">A <see cref="Rectangle"/> that defines the location and size of the viewport in a render target.</param>
    public Viewport(Rectangle bounds)
    {
        X = bounds.X;
        Y = bounds.Y;
        Width = bounds.Width;
        Height = bounds.Height;
        MinDepth = 0.0f;
        MaxDepth = 1.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Viewport"/> struct.
    /// </summary>
    /// <param name="bounds">A <see cref="Vector4"/> that defines the location and size of the viewport in a render target.</param>
    public Viewport(in Vector4 bounds)
    {
        X = bounds.X;
        Y = bounds.Y;
        Width = bounds.Z;
        Height = bounds.W;
        MinDepth = 0.0f;
        MaxDepth = 1.0f;
    }

    /// <summary>
    /// Gets or sets the bounds of the viewport.
    /// </summary>
    /// <value>The bounds.</value>
    public readonly RectangleF Bounds => new(X, Y, Width, Height);

    /// <summary>
    /// Gets the aspect ratio used by the viewport.
    /// </summary>
    /// <value>The aspect ratio.</value>
    public readonly float AspectRatio
    {
        get
        {
            if (Width == 0.0f || Height == 0.0f)
                return 0.0f;

            return Width / Height;
        }
    }
}
