# C# for systems programming

These samples show how to compile C# to native code using .NET Native AOT technology ([NativeAOT](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT), also known as CoreRT previously).

The samples are for people who would like to use C#, but don't want to be bound by the choices of base class libraries that normally come with C# (in the form that it's bundled in .NET). If you just want to native compile your .NET apps, go to the [NativeAOT](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT) repo/branch instead. Nothing to see for you in this repo.

<B><I>[no-runtime](#)</I></B> is a rather pointless sample that demonstrates how to write code in C# that is directly runnable without a runtime. C# has value types and you can p/invoke into an unmanaged memory allocator, so you can do things with this, but you're so severily limited it's rather pointless. But Hello world ends up being about 8 kB native EXE with no dependencies, so that's rather cool.

<B><I>[with-runtime](#)</I></B> is something that can be actually useful. This includes the full managed and unmanaged runtime - GC, exception handling, and interface dispatch all work. Test.CoreLib used as the class library here is the same Test.CoreLib that you can find in the NativeAOT repo. Don't look for things like `Object.ToString()` because being compatible with .NET is not the point. This sample comes down to about 400 kB, most of which is the C runtime library.

<B><I>[efi-no-runtime](./efi-no-runtime/README.md)</I></B> Is an EFI boot application that lets you run C# on bare metal, without an OS. Similar restrictions to the <B>[no-runtime](#)</B> sample apply. To make a version of this sample with a runtime would require some porting work on the runtime side.

## Building the samples

[.NET 7 SDK](https://dotnet.microsoft.com/download) is a prerequisite for building these on all platforms.

In addition to the .NET 7 SDK, these are needed:
* On Windows: Visual Studio 2022 **with** C++ development support and a Windows SDK
* On Linux: clang
* On macOS (untested): XCode

One you made sure you have the prerequisites, enter the appropriate sample directory and type:

```bash
$ dotnet publish -c Release
```

Some samples also come with a shell script (*.cmd) that pieces together all the tools and avoid MSBuild or dotnet. You need to make sure you have environment set up before running the script. Look at the script for details. The script is redundant with the *.csproj project files.
<br><br>
The efi project comes with some publish profiles(*.pubxml) which are used to allow creation of the vhdx file needed for booting.
