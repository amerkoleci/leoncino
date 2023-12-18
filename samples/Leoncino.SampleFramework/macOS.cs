// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.CompilerServices;
using static Leoncino.Samples.ObjectiveCRuntime;

namespace Leoncino.Samples;

#pragma warning disable IDE1006 // Naming Styles

internal static unsafe partial class ObjectiveCRuntime
{
    private const string ObjCLibrary = "/usr/lib/libobjc.A.dylib";

    [LibraryImport(ObjCLibrary)]
    public static partial IntPtr sel_registerName(byte* namePtr);

    [LibraryImport(ObjCLibrary)]
    public static partial byte* sel_getName(nint selector);
    [LibraryImport(ObjCLibrary)]
    public static partial IntPtr objc_getClass(byte* namePtr);

    [LibraryImport(ObjCLibrary, EntryPoint = "objc_msgSend")]
    public static partial void objc_msgSend(nint receiver, Selector selector);
    [LibraryImport(ObjCLibrary, EntryPoint = "objc_msgSend")]
    public static partial void objc_msgSend(nint receiver, Selector selector, Bool8 b);
    [LibraryImport(ObjCLibrary, EntryPoint = "objc_msgSend")]
    public static partial void objc_msgSend(nint receiver, Selector selector, nint b);

    [LibraryImport(ObjCLibrary, EntryPoint = "objc_msgSend")]
    public static partial Bool8 bool8_objc_msgSend(nint receiver, Selector selector);

    [LibraryImport(ObjCLibrary, EntryPoint = "objc_msgSend")]
    public static partial nint IntPtr_objc_msgSend(nint receiver, Selector selector);

    public static ref T objc_msgSend<T>(nint receiver, Selector selector) where T : struct
    {
        nint value = IntPtr_objc_msgSend(receiver, selector);
        return ref Unsafe.AsRef<T>(&value);
    }

    public static unsafe string GetUtf8String(byte* stringStart)
    {
        int characters = 0;
        while (stringStart[characters] != 0)
        {
            characters++;
        }

        return Encoding.UTF8.GetString(stringStart, characters);
    }
}

internal unsafe readonly struct Selector
{
    public readonly IntPtr NativePtr;

    public Selector(IntPtr ptr)
    {
        NativePtr = ptr;
    }

    public Selector(string name)
    {
        int byteCount = Encoding.UTF8.GetMaxByteCount(name.Length);
        byte* utf8BytesPtr = stackalloc byte[byteCount];
        fixed (char* namePtr = name)
        {
            Encoding.UTF8.GetBytes(namePtr, name.Length, utf8BytesPtr, byteCount);
        }

        NativePtr = sel_registerName(utf8BytesPtr);
    }

    public string Name
    {
        get
        {
            byte* name = sel_getName(NativePtr);
            return GetUtf8String(name);
        }
    }

    public static implicit operator Selector(string s) => new(s);
}

internal static class Selectors
{

    internal static readonly Selector alloc = "alloc";

    internal static readonly Selector init = "init";
}

internal unsafe readonly struct ObjCClass
{
    public readonly IntPtr NativePtr;
    public static implicit operator IntPtr(ObjCClass c) => c.NativePtr;

    public ObjCClass(string name)
    {
        int byteCount = Encoding.UTF8.GetMaxByteCount(name.Length);
        byte* utf8BytesPtr = stackalloc byte[byteCount];
        fixed (char* namePtr = name)
        {
            Encoding.UTF8.GetBytes(namePtr, name.Length, utf8BytesPtr, byteCount);
        }

        NativePtr = objc_getClass(utf8BytesPtr);
    }

    public T AllocInit<T>() where T : struct
    {
        IntPtr value = IntPtr_objc_msgSend(NativePtr, Selectors.alloc);
        objc_msgSend(value, Selectors.init);
        return Unsafe.AsRef<T>(&value);
    }
}


internal readonly struct Bool8
{
    public readonly byte Value;

    public Bool8(byte value)
    {
        Value = value;
    }

    public Bool8(bool value)
    {
        Value = value ? (byte)1 : (byte)0;
    }

    public static implicit operator bool(Bool8 b) => b.Value != 0;
    public static implicit operator byte(Bool8 b) => b.Value;
    public static implicit operator Bool8(bool b) => new(b);
}

internal readonly struct NSView(nint ptr)
{
    internal static readonly Selector setWantsLayer = "setWantsLayer:";
    internal static readonly Selector setLayer = "setLayer:";

    public readonly nint NativePtr = ptr;
    public static implicit operator nint(NSView nsView) => nsView.NativePtr;

    public Bool8 wantsLayer
    {
        get => bool8_objc_msgSend(NativePtr, "wantsLayer");
        set => objc_msgSend(NativePtr, setWantsLayer, value);
    }

    public nint layer
    {
        get => IntPtr_objc_msgSend(NativePtr, "layer");
        set => objc_msgSend(NativePtr, setLayer, value);
    }

    //public CGRect frame
    //{
    //    get
    //    {
    //        return RuntimeInformation.ProcessArchitecture == Architecture.Arm64
    //            ? CGRect_objc_msgSend(NativePtr, "frame")
    //            : objc_msgSend_stret<CGRect>(NativePtr, "frame");
    //    }
    //}
}

internal readonly struct NSWindow(IntPtr ptr)
{
    public readonly IntPtr NativePtr = ptr;

    public ref NSView contentView => ref objc_msgSend<NSView>(NativePtr, "contentView");
}

internal readonly struct CAMetalLayer(nint ptr)
{
    public readonly nint Handle = ptr;

    public static CAMetalLayer New() => s_class.AllocInit<CAMetalLayer>();

    private static readonly ObjCClass s_class = new(nameof(CAMetalLayer));
}

#pragma warning restore IDE1006 // Naming Styles
