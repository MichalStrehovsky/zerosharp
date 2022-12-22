::
:: "Manual" build script that bypasses MSBuild and directly invokes the necessary tools.
:: Good to show how things get hooked up together, but redundant with the project file.
::
:: The tools are:
::
:: * CSC, the C# compiler
::   Opening a "Arm64 Native Tools Command Prompt for VS 2019" will place csc.exe on your PATH.
:: * ILC, the Native AOT compiler
::   If you use the project file to build this sample at least once, you can find ILC
::   in your NuGet cache. It will be somewhere like
::   C:\Users\username\.nuget\packages\runtime.win-arm64.microsoft.dotnet.ilcompiler\7.0.0-alpha.1.21430.2
:: * Linker
::   This is the platform linker. "Arm64 Native Tools Command Prompt for VS 2019" will place
::   the linker on your PATH.
::

@set ILCPATH=%DROPPATH%\tools
@if not exist %ILCPATH%\ilc.exe (
  echo The DROPPATH environment variable not set.
  exit /B
)
@where csc >nul 2>&1
@if ERRORLEVEL 1 (
  echo CSC not on the PATH.
  exit /B
)

@set VHD=%CD%\zerosharp.vhdx
@set VHD_SCRIPT=%CD%\diskpart.txt
@del %VHD% >nul 2>&1
@del %VHD_SCRIPT% >nul 2>&1
@del zerosharp.ilexe >nul 2>&1
@del zerosharp.obj >nul 2>&1
@del zerosharp.map >nul 2>&1
@del zerosharp.pdb >nul 2>&1
@del BOOTAA64.EFI >nul 2>&1

@if "%1" == "clean" exit /B

csc /nologo /debug:embedded /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 zerosharp.cs /out:zerosharp.ilexe /langversion:latest /unsafe
%ILCPATH%\ilc zerosharp.ilexe -o zerosharp.obj --systemmodule zerosharp --map zerosharp.map -O
link /nologo /subsystem:EFI_APPLICATION zerosharp.obj /entry:EfiMain /incremental:no /out:BOOTAA64.EFI

@rem Build a VHD if requested

@if not "%1" == "vhd" exit /B

@(
echo create vdisk file=%VHD% maximum=500
echo select vdisk file=%VHD%
echo attach vdisk
echo convert gpt
echo create partition efi size=100
echo format quick fs=fat32 label="System"
echo assign letter="X"
echo exit
)>%VHD_SCRIPT%

diskpart /s %VHD_SCRIPT%

xcopy BOOTAA64.EFI X:\EFI\BOOT\

@(
echo select vdisk file=%VHD%
echo select partition 2
echo remove letter=X
echo detach vdisk
echo exit
)>%VHD_SCRIPT%

diskpart /s %VHD_SCRIPT%
