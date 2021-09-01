# C# for systems programming

These samples show how to compile C# to native code using the .NET Native AOT technology ([NativeAOT](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT), also known as CoreRT previously).

`no-runtime` is a rather pointless sample that demonstrates how to write code in C# that is directly runnable without a runtime. C# has value types and you can p/invoke into an unmanaged memory allocator, so you can do things with this, but you're so severily limited it's rather pointless. But Hello world ends up being about 4-5 kB native EXE with no dependencies, so that's rather cool.

`with-runtime` is something that can be actually useful. This includes the full managed and unmanaged runtime - GC, exception handling, and interface dispatch all work. Test.CoreLib used as the class library here is the same Test.CoreLib that you can find in the NativeAOT repo. Don't look for things like `Object.ToString()` because being compatible with .NET is not the point. This sample comes down to about 400 kB, most of which is the C runtime library.

`efi-no-runtime` is an EFI boot application that lets you run C# on bare metal, without an OS. Similar restrictions to the `no-runtime` sample apply. Making a version of this sample with a runtime would require some porting work on the runtime side.

## Building the samples

[.NET 6 SDK](https://dotnet.microsoft.com/download) is a prerequisite for building these on all platforms.

In addition to the .NET 6 SDK, these are needed:
* On Windows: Visual Studio 2019 **with** C++ development support and a Windows SDK
* On Linux: clang
* On macOS (untested): XCode

One you made sure you have the prerequisites, enter the appropriate sample directory and type:

```bash
$ dotnet publish -r [RID] -c Release
```

Where `[RID]` is the RID you're building for (one of `linux-x64` (Linux with glibc), `linux-musl-x64` (Linux with musl libc), `linux-arm64`, `windows-x64`, `windows-arm64`, `osx-x64`). Cross-building is possible but requires extra steps, so just use the platform you're running on to avoid errors.

Some samples also come with a shell script (*.cmd) that pieces together all the tools and avoid MSBuild or dotnet. You need to make sure you have environment set up before running the script. Look at the script for details. The script is redundant with the *.csproj project files.
