@set ILCPATH=%DROPPATH%\tools
@set SDKPATH=%DROPPATH%\sdk

@del zerosharp.ilexe >nul 2>&1
@del zerosharp.obj >nul 2>&1
@del zerosharp.exe >nul 2>&1
@del zerosharp.map >nul 2>&1
@del zerosharp.pdb >nul 2>&1

@if "%1" == "clean" exit /B

csc /debug:embedded /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 zerosharp.cs /out:zerosharp.ilexe /langversion:latest /unsafe /r:Test.CoreLib.dll
"%ILCPATH%\ilc" zerosharp.ilexe -r:Test.CoreLib.dll -o zerosharp.obj --systemmodule Test.CoreLib --map zerosharp.map -O --noscan
link zerosharp.obj "%SDKPATH%\bootstrapper.lib" "%SDKPATH%\Runtime.lib" kernel32.lib ole32.lib advapi32.lib /incremental:no
