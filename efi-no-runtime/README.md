# EFI boot application in C#

This sample is a EFI boot application written in C# that displays Hello World. It runs without an OS, on Multiple Target bare metal hardwares.

<img src="https://migeel.sk/efisharp.png" >

## Building the program

Refer to the general instructions at the root of the repo.

### Using Publish

To create the needed vhdx file we need to publish out project.
- Firstly right click on the project and go down to publish.
- Next Select one of the Following Targets to build

    | Publish Target | .Net Target | Boot File Name |
    |----------------|-------------|----------------|
    | X86Debug       | win-x86     |  BOOTIA32.EFI  |
    | X64Debug       | win-x64     |  BOOTX64.EFI   |
    | ARM64Debug     | win-arm64   |  BOOTAA64.EFI  |
    | ARMDebug       | win-arm     |  BOOTARM.EFI   |
    | X86Release     | win-x86     |  BOOTIA32.EFI  |
    | X64Release     | win-x64     |  BOOTX64.EFI   |
    | ARM64Release   | win-arm64   |  BOOTAA64.EFI  |
    | ARMRelease     | win-arm     |  BOOTARM.EFI   |

- Now Click publish and wait as Visual Studios Builds you the file, To produce the VHDX file
requires it requires that Visual Studios is running as an administrator.
- Once completed the file can be found under the Project Folder's Publish Directory under the the Target Architecture's
Folder.

This File can be used under [HyperV](###Using-HyperV) or [Virtual Box](###Using-VirtualBox)

NOTE: This is a Replacement for the '[Build.cmd](###-Using-Build.cmd)'

### Using Build.cmd [Deprecated]

Note: producing VHDX requires running from an elevated command prompt. It will not work without elevation.

Running `build.cmd` should produce a BOOTX64.EFI file in the current directory. There are multiple ways to run this. QEMU with an EFI firmware should work. I use Hyper-V.

Running `build.cmd vhd` will produce a VHDX file for you that you can run on Hyper-V directly. You need to create a new Gen 2 virtual machine in Hyper-V and attach the generated disk. Make sure to turn off Secure boot in the virtual machine: the EFI image is not signed.

Similarly, adding `-p:BuildVHDX=true` to the `dotnet publish` line (when using the `*.csproj` project) will produce a bootable VHDX.

## Booting The VHDX

### Using VirtualBox

To boot it in VirtualBox, we need to get a `vdi` file. First, [Create the VHDX](###-Using-Publish).<br> Then, you can use VB's built-in tool to convert the `vhdx` file into a `vdi` one:
```
VBoxManage.exe clonemedium disk zerosharp.vhdx zerosharp.vdi
```

Now, go to VirtualBox, create an empty OS, load the `vdi` file as a hard drive, and it should work.

### Using HyperV

If you have HyperV you can use the vhdx file created during the [Publish Process](###-Using-Publish) to create a virtual machine
which can then be used to test your OS.<br><br>
Note: Requires HyperV Installation Which in Windows Requires At Least The Pro Edition, and the Feature HyperV with the HyperV
Management Tools and HyperV Platform to be turn on.<br><br>
Note: HyperV under x86_64 requires use of Gen2 Virtual Machines, which will limit testing to x64 targets.
It is recomended to use an alternative virtual machine software like VirtualBox or Qemu for architectures not supported under HyperV.

