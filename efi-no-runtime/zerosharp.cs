using System;
using System.Runtime.InteropServices;

namespace System
{
    public class Object { public IntPtr m_pEEType; } // The layout of object is a contract with the compiler.
    public struct Void { }
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

        public class RuntimeFeature
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

    class StartupCodeHelpers
    {
        [RuntimeExport("RhpReversePInvoke2")]
        static void RhpReversePInvoke2() { }
        [RuntimeExport("RhpReversePInvokeReturn2")]
        static void RhpReversePInvokeReturn2() { }
        [System.Runtime.RuntimeExport("__fail_fast")]
        static void FailFast() { while (true) ; }
        [System.Runtime.RuntimeExport("RhpPInvoke")]
        static void RphPinvoke() { }
        [System.Runtime.RuntimeExport("RhpPInvokeReturn")]
        static void RphPinvokeReturn() { }
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

    private readonly delegate* unmanaged<void*, char*, void*> _outputString;
    public void OutputString(void* handle, char* str)
    {
        _outputString(handle, str);
    }
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
