// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using WebGPU;
using static WebGPU.WebGPU;

namespace Leoncino.WebGPU;

internal unsafe class WebGPUBindGroupLayout : BindGroupLayout
{
    private readonly WebGPUDevice _device;
    public readonly void* pMappedData = default;

    public WebGPUBindGroupLayout(WebGPUDevice device, in BindGroupLayoutDescriptor descriptor)
        : base(descriptor)
    {
        _device = device;

        LayoutBindingCount = (uint)descriptor.Entries.Length;
        WGPUBindGroupLayoutEntry* entries = stackalloc WGPUBindGroupLayoutEntry[(int)LayoutBindingCount];

        for (int i = 0; i < LayoutBindingCount; i++)
        {
            ref readonly BindGroupLayoutEntry entry = ref descriptor.Entries[i];

            entries[i].nextInChain = null;
            entries[i].binding = (uint)i;
            entries[i].visibility = entry.Visibility.ToWGPU();

            switch (entry.BindingType)
            {
                case BindingInfoType.Buffer:
                    entries[i].buffer = new WGPUBufferBindingLayout()
                    {
                        type = entry.Buffer.Type.ToWGPU(),
                        hasDynamicOffset = entry.Buffer.HasDynamicOffset,
                        minBindingSize = entry.Buffer.MinBindingSize,
                    };
                    break;

                case BindingInfoType.Sampler:
                    entries[i].sampler = new WGPUSamplerBindingLayout()
                    {
                        type = entry.Sampler.Type.ToWGPU(),
                    };
                    break;

                case BindingInfoType.Texture:
                    entries[i].texture = new WGPUTextureBindingLayout()
                    {
                        sampleType = entry.Texture.SampleType.ToWGPU(),
                        viewDimension = WGPUTextureViewDimension._2D,
                        multisampled = entry.Texture.Multisampled,
                    };
                    break;

                case BindingInfoType.StorageTexture:
                    entries[i].storageTexture = new WGPUStorageTextureBindingLayout()
                    {
                        access = entry.StorageTexture.Access.ToWGPU(),
                        format = entry.StorageTexture.Format.ToWGPU(),
                        viewDimension = WGPUTextureViewDimension._2D,
                    };
                    break;
            }
        }

        fixed (byte* pLabel = descriptor.Label.GetUtf8Span())
        {
            WGPUBindGroupLayoutDescriptor wgpuDescriptor = new()
            {
                nextInChain = null,
                label = pLabel,
                entryCount = LayoutBindingCount,
                entries = entries
            };

            Handle = wgpuDeviceCreateBindGroupLayout(device.Handle, &wgpuDescriptor);
        }
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="WebGPUBindGroupLayout" /> class.
    /// </summary>
    ~WebGPUBindGroupLayout() => Dispose(disposing: false);

    public uint LayoutBindingCount { get; }

    public WGPUBindGroupLayout Handle { get; }

    /// <inheritdoc />
    public override GraphicsDevice Device => _device;

    /// <inheritdoc />
    protected internal override void Destroy()
    {
        wgpuBindGroupLayoutRelease(Handle);
    }

    /// <inheritdoc />
    protected override void OnLabelChanged(string newLabel)
    {
        wgpuBindGroupLayoutSetLabel(Handle, newLabel);
    }
}
