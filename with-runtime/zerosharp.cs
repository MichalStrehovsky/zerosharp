using System;
using System.Runtime.InteropServices;

static unsafe class Console
{
    [DllImport("kernel32")]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32")]
    static extern IntPtr WriteConsoleW(IntPtr hConsole, void* lpBuffer, int charsToWrite, out int charsWritten, void* reserved);

    public static void WriteLine(string s)
    {
        IntPtr stdInputHandle = GetStdHandle(-11);
        int charsWritten;

        fixed (char* c = s)
        {
            WriteConsoleW(stdInputHandle, c, s.Length, out charsWritten, null);
        }

        char newLine = '\n';
        WriteConsoleW(stdInputHandle, &newLine, 1, out charsWritten, null);
    }
}

class MyException : Exception { }

interface IFooer
{
    void Foo();
}

struct Fooer : IFooer
{
    public void Foo() => Console.WriteLine("Foo");
}

class Program
{
    static int Main()
    {
        try
        {
            throw new MyException();
            Console.WriteLine("Exception not thrown!");
        }
        catch
        {
            Console.WriteLine("Exception caught");
        }

        IFooer fooer = (IFooer)new Fooer();
        fooer.Foo();

        return 42;
    }
}
