using System;
using System.Runtime.InteropServices;

#region A couple very basic things
namespace System
{
    public class Object
    {
#pragma warning disable 169
        // The layout of object is a contract with the compiler.
        private IntPtr m_pMethodTable;
#pragma warning restore 169
    }
    public struct Void { }

    // The layout of primitive types is special cased because it would be recursive.
    // These really don't need any fields to work.
    public struct Boolean { }
    public struct Char { }
    public struct SByte { }
    public struct Byte { }
    public struct Int16 { }
    public struct UInt16 { }
    public struct Int32 { }
    public struct UInt32 { }
    public struct Int64 { }
    public struct UInt64 { }
    public struct IntPtr { }
    public struct UIntPtr { }
    public struct Single { }
    public struct Double { }

    public abstract class ValueType { }
    public abstract class Enum : ValueType { }

    public struct Nullable<T> where T : struct { }
    
    public sealed class String { public readonly int Length; }
    public abstract class Array { }
    public abstract class Delegate { }
    public abstract class MulticastDelegate : Delegate { }

    public struct RuntimeTypeHandle { }
    public struct RuntimeMethodHandle { }
    public struct RuntimeFieldHandle { }

    public class Attribute { }

    namespace Runtime.CompilerServices
    {
        public class RuntimeHelpers
        {
            public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);
        }

        public static class RuntimeFeature
        {
            public const string UnmanagedSignatureCallingConvention = nameof(UnmanagedSignatureCallingConvention);
        }
    }
}

namespace System.Runtime.InteropServices
{
    public class UnmanagedType { }

    sealed class StructLayoutAttribute : Attribute
    {
        public StructLayoutAttribute(LayoutKind layoutKind)
        {
        }
    }

    internal enum LayoutKind
    {
        Sequential = 0, // 0x00000008,
        Explicit = 2, // 0x00000010,
        Auto = 3, // 0x00000000,
    }

    internal enum CharSet
    {
        None = 1,       // User didn't specify how to marshal strings.
        Ansi = 2,       // Strings should be marshalled as ANSI 1 byte chars.
        Unicode = 3,    // Strings should be marshalled as Unicode 2 byte chars.
        Auto = 4,       // Marshal Strings in the right way for the target system.
    }
}
#endregion

#region Things needed by ILC
namespace System
{
    namespace Runtime
    {
        internal sealed class RuntimeExportAttribute : Attribute
        {
            public RuntimeExportAttribute(string entry) { }
        }
    }

    class Array<T> : Array { }
}

namespace Internal.Runtime.CompilerHelpers
{
    using System.Runtime;

    // A class that the compiler looks for that has helpers to initialize the
    // process. The compiler can gracefully handle the helpers not being present,
    // but the class itself being absent is unhandled. Let's add an empty class.
    class StartupCodeHelpers
    {
        // A couple symbols the generated code will need we park them in this class
        // for no particular reason. These aid in transitioning to/from managed code.
        // Since we don't have a GC, the transition is a no-op.
        [RuntimeExport("RhpReversePInvoke")]
        static void RhpReversePInvoke(IntPtr frame) { }
        [RuntimeExport("RhpReversePInvokeReturn")]
        static void RhpReversePInvokeReturn(IntPtr frame) { }
        [RuntimeExport("RhpPInvoke")]
        static void RhpPInvoke(IntPtr frame) { }
        [RuntimeExport("RhpPInvokeReturn")]
        static void RhpPInvokeReturn(IntPtr frame) { }

        [RuntimeExport("RhpFallbackFailFast")]
        static void RhpFallbackFailFast() { while (true) ; }
    }
}
#endregion

[StructLayout(LayoutKind.Sequential)]
struct EFI_HANDLE
{
    private IntPtr _handle;
}

[StructLayout(LayoutKind.Sequential)]
unsafe readonly struct EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL
{
    private readonly IntPtr _pad;

    public readonly delegate* unmanaged<void*, char*, void*> OutputString;
}

[StructLayout(LayoutKind.Sequential)]
readonly struct EFI_TABLE_HEADER
{
    public readonly ulong Signature;
    public readonly uint Revision;
    public readonly uint HeaderSize;
    public readonly uint Crc32;
    public readonly uint Reserved;
}

[StructLayout(LayoutKind.Sequential)]
unsafe readonly struct EFI_SYSTEM_TABLE
{
    public readonly EFI_TABLE_HEADER Hdr;
    public readonly char* FirmwareVendor;
    public readonly uint FirmwareRevision;
    public readonly EFI_HANDLE ConsoleInHandle;
    public readonly void* ConIn;
    public readonly EFI_HANDLE ConsoleOutHandle;
    public readonly EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL* ConOut;
}

#region Need by new version of ILC
namespace System 
{
    public static partial class AppContext 
    {
        public static void SetData(string name, object? data)
        {
        }
    }
}

namespace System
{
    /* By default, attributes are inherited and multiple attributes are not allowed */
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageAttribute : Attribute
    {
        private readonly AttributeTargets _attributeTarget;
        private bool _allowMultiple;
        private bool _inherited;

        internal static readonly AttributeUsageAttribute Default = new AttributeUsageAttribute(AttributeTargets.All);

        public AttributeUsageAttribute(AttributeTargets validOn)
        {
            _attributeTarget = validOn;
            _inherited = true;
        }

        internal AttributeUsageAttribute(AttributeTargets validOn, bool allowMultiple, bool inherited)
        {
            _attributeTarget = validOn;
            _allowMultiple = allowMultiple;
            _inherited = inherited;
        }

        public AttributeTargets ValidOn => _attributeTarget;

        public bool AllowMultiple
        {
            get => _allowMultiple;
            set => _allowMultiple = value;
        }

        public bool Inherited
        {
            get => _inherited;
            set => _inherited = value;
        }
    }
}

namespace System
{
    // Enum used to indicate all the elements of the
    // VOS it is valid to attach this element to.

    public enum AttributeTargets
    {
        Assembly = 0x0001,
        Module = 0x0002,
        Class = 0x0004,
        Struct = 0x0008,
        Enum = 0x0010,
        Constructor = 0x0020,
        Method = 0x0040,
        Property = 0x0080,
        Field = 0x0100,
        Event = 0x0200,
        Interface = 0x0400,
        Parameter = 0x0800,
        Delegate = 0x1000,
        ReturnValue = 0x2000,
        GenericParameter = 0x4000,

        All = Assembly | Module | Class | Struct | Enum | Constructor |
                        Method | Property | Field | Event | Interface | Parameter |
                        Delegate | ReturnValue | GenericParameter
    }
}
#endregion

unsafe class Program
{
    static void Main() { }

    [System.Runtime.RuntimeExport("EfiMain")]
    static long EfiMain(IntPtr imageHandle, EFI_SYSTEM_TABLE* systemTable)
    {
        string hello = "Hello world!";
        fixed (char* pHello = hello)
        {
            systemTable->ConOut->OutputString(systemTable->ConOut, pHello);
        }

        while (true) ;
    }
}
