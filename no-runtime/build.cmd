::
:: "Manual" build script that bypasses MSBuild and directly invokes the necessary tools.
:: Good to show how things get hooked up together, but redundant with the project file.
::
:: The tools are:
::
:: * CSC, the C# compiler
::   Opening a "x64 Native Tools Command Prompt for VS 2019" will place csc.exe on your PATH.
:: * ILC, the Native AOT compiler
::   If you use the project file to build this sample at least once, you can find ILC
::   in your NuGet cache. It will be somewhere like
::   C:\Users\username\.nuget\packages\runtime.win-x64.microsoft.dotnet.ilcompiler\7.0.0-alpha.1.21430.2
:: * Linker
::   This is the platform linker. "x64 Native Tools Command Prompt for VS 2019" will place
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

@del zerosharp.ilexe >nul 2>&1
@del zerosharp.obj >nul 2>&1
@del zerosharp.exe >nul 2>&1
@del zerosharp.map >nul 2>&1
@del zerosharp.pdb >nul 2>&1

@if "%1" == "clean" exit /B

csc /define:WINDOWS /debug:embedded /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 zerosharp.cs /out:zerosharp.ilexe /langversion:latest /unsafe || goto Error
%ILCPATH%\ilc zerosharp.ilexe -g -o zerosharp.obj --systemmodule zerosharp --map zerosharp.map -O --directpinvoke:kernel32 || goto Error
link /debug /subsystem:console zerosharp.obj /entry:__managed__Main kernel32.lib /incremental:no || goto Error

@goto :EOF

:Error
@echo Tool failed.
exit /B 1
