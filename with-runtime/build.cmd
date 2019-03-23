@set ILCPATH=%DROPPATH%\tools
@set SDKPATH=%DROPPATH%\sdk
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

csc /debug /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 zerosharp.cs /out:zerosharp.ilexe /langversion:latest /unsafe /r:Test.CoreLib.dll
"%ILCPATH%\ilc" zerosharp.ilexe -r:Test.CoreLib.dll -o zerosharp.obj --systemmodule Test.CoreLib --map zerosharp.map -O --noscan -g
link zerosharp.obj "%SDKPATH%\bootstrapper.lib" "%SDKPATH%\Runtime.lib" kernel32.lib ole32.lib advapi32.lib /incremental:no /debug /opt:ref
