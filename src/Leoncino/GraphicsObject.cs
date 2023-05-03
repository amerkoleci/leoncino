// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Reflection;
using CommunityToolkit.Diagnostics;

namespace Leoncino;

public abstract class GraphicsObject : DisposableObject
{
    private string _label;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphicsObject" /> class.
    /// </summary>
    /// <param name="device">The device object that created the object.</param>
    /// <param name="label">The label of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    protected GraphicsObject(GraphicsDevice device, string? label = default)
    {
        Guard.IsNotNull(device, nameof(device));

        Device = device;
        _label = label ?? GetType().Name;
    }

    /// <summary>
    /// Get the <see cref="GraphicsDevice"/> object that created this object.
    /// </summary>
    public GraphicsDevice Device { get; }

    /// <summary>
    /// Gets or sets the label that identifies this object.
    /// </summary>
    public string Label
    {
        get => _label;
        set
        {
            _label = value ?? GetType().Name;
            OnLabelChanged(_label);
        }
    }

    protected virtual void OnLabelChanged(string newLabel)
    {
    }

    /// <inheritdoc />
    public override string ToString() => _label;
}
