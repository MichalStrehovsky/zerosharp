using System;
using System.Runtime;
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

    public enum AttributeTargets { }

    public sealed class AttributeUsageAttribute : Attribute
    {
        public AttributeUsageAttribute(AttributeTargets validOn) { }
        public bool AllowMultiple { get; set; }
        public bool Inherited { get; set; }
    }

    public class AppContext
    {
        public static void SetData(string s, object o) { }
    }

    namespace Runtime.CompilerServices
    {
        public class RuntimeHelpers
        {
            public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);
        }
    }
}
namespace System.Runtime.InteropServices
{
    public sealed class DllImportAttribute : Attribute
    {
        public DllImportAttribute(string dllName) { }
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

unsafe class Program
{
    [DllImport("libc")]
    static extern int printf(byte* fmt);

    [DllImport("kernel32")]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32")]
    static extern IntPtr WriteConsoleW(IntPtr hConsole, void* lpBuffer, int charsToWrite, out int charsWritten, void* reserved);

#if !WINDOWS
    // Export this as "main" so that we can link with the C runtime library properly.
    // If the C runtime library is not initialized we can't even printf.
    // This is not needed on Windows because we don't call the C runtime.
    [RuntimeExport("main")]
#endif
    static int Main()
    {
        string hello = "Hello world!\n";
        fixed (char* pHello = hello)
        {
#if WINDOWS
            WriteConsoleW(GetStdHandle(-11), pHello, hello.Length, out int _, null);
#else
            // Once C# has support for UTF-8 string literals, this can be simplified.
            // https://github.com/dotnet/csharplang/issues/2911
            // Since we don't have that, convert from UTF-16 to ASCII.
            byte* pHelloASCII = stackalloc byte[hello.Length + 1];
            for (int i = 0; i < hello.Length; i++)
                pHelloASCII[i] = (byte)pHello[i];

            printf(pHelloASCII);
#endif
        }

        return 42;
    }
}
