# EFI boot application in C#

This sample is a EFI boot application written in C# that displays Hello World. It runs without an OS, on x64 bare metal hardware.

<img src="https://migeel.sk/efisharp.png" >

## Building the program

Refer to the general instructions at the root of the repo.

## Booting the generated program

Note: producing VHDX requires running from an elevated command prompt. It will not work without elevation.

Running `build.cmd` should produce a BOOTX64.EFI file in the current directory. There are multiple ways to run this. QEMU with an EFI firmware should work. I use Hyper-V.

Running `build.cmd vhd` will produce a VHDX file for you that you can run on Hyper-V directly. You need to create a new Gen 2 virtual machine in Hyper-V and attach the generated disk. Make sure to turn off Secure boot in the virtual machine: the EFI image is not signed.

Similarly, adding `-p:BuildVHDX=true` to the `dotnet publish` line (when using the `*.csproj` project) will produce a bootable VHDX.
