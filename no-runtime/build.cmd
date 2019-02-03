@set ILCPATH=%DROPPATH%\tools

@del zerosharp.ilexe >nul 2>&1
@del zerosharp.obj >nul 2>&1
@del zerosharp.exe >nul 2>&1
@del zerosharp.map >nul 2>&1
@del zerosharp.pdb >nul 2>&1

@if "%1" == "clean" exit /B

csc /debug:embedded /noconfig /nostdlib /runtimemetadataversion:v4.0.30319 zerosharp.cs /out:zerosharp.ilexe /langversion:latest /unsafe
%ILCPATH%\ilc zerosharp.ilexe -o zerosharp.obj --systemmodule zerosharp --map zerosharp.map -O
link /subsystem:console zerosharp.obj /entry:__managed__Main kernel32.lib /merge:.modules=.pdata /incremental:no
