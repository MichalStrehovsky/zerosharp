# C# for systems programming

These samples show how to compile C# to native code using the .NET Native AOT technology ([NativeAOT](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT), also known as CoreRT previously).

`no-runtime` is a rather pointless sample that demonstrates how to write code in C# that is directly runnable without a runtime. C# has value types and you can p/invoke into an unmanaged memory allocator, so you can do things with this, but you're so severily limited it's rather pointless. But Hello world ends up being about 4-5 kB native EXE, so that's rather cool.

`with-runtime` is something that can be actually useful. This includes the full managed and unmanaged runtime - GC, exception handling, and interface dispatch all work. Test.CoreLib used as the class library here is the same Test.CoreLib that you can find in the NativeAOT repo. Don't look for things like `Object.ToString()` because being compatible with .NET is not the point. This sample comes down to about 400 kB, most of which is the C runtime library.

`efi-no-runtime` is an EFI boot application that lets you run C# on bare metal, without an OS. Similar restrictions to the `no-runtime` sample apply. Making a version of this sample with a runtime would require some porting work on the runtime side.

## Building the samples

There are some prerequisites for running the build scripts:
* A NativeAOT drop
* Visual Studio 2019 with C++ support and a Windows SDK

You can run these without building NativeAOT yourself. Just point the `DROPPATH` environment variable to the ILCompiler package in your NuGet package cache (if you ever played with using NativeAOT, it will be somewhere like `C:\Users\{user}\.nuget\packages\runtime.win-x64.microsoft.dotnet.ilcompiler\6.0.0-alpha.1.21055.3`). If you haven't tried NativeAOT for your normal .NET Core projects yet, [go try it out](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT/samples/HelloWorld), it's cool.

The `build.cmd` script needs to be executed from a "x64 Native Tools Command Prompt for VS 2019" - it's in your Start menu. Also make sure `DROPPATH` is set as above.

