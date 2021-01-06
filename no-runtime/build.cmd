@set ILCPATH=%DROPPATH%\tools
@if not exist %ILCPATH%\ilc.exe (
  echo The DROPPATH environment variable not set. Refer to README.md.
  exit /B
)
@where csc >nul 2>&1
@if ERRORLEVEL 1 (
  echo CSC not on the PATH. Refer to README.md.
  exit /B
)

@del zerosharp.ilexe >nul 2>&1
@del zerosharp.obj >nul 2>&1
@del zerosharp.exe >nul 2>&1
@del zerosharp.map >nul 2>&1
@del zerosharp.pdb >nul 2>&1

@if "%1" == "clean" exit /B

csc /debug:embedded /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 zerosharp.cs /out:zerosharp.ilexe /langversion:latest /unsafe
%ILCPATH%\ilc zerosharp.ilexe -o zerosharp.obj --systemmodule zerosharp --map zerosharp.map -O --directpinvoke:kernel32
link /subsystem:console zerosharp.obj /entry:__managed__Main kernel32.lib /incremental:no
