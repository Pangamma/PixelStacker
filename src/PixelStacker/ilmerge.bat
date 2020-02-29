@echo off
:: this script needs https://www.nuget.org/packages/ilmerge

:: set your target executable name (typically [projectname].exe)
SET APP_NAME=PixelStacker.exe
::SET PROJECT_DIR=%1
SET PROJECT_DIR=D:\git\PixelStacker\src\PixelStacker\
echo ProjectDir=%PROJECT_DIR%

:: Set build, used for directory. Typically Release or Debug
SET ILMERGE_BUILD=Debug

:: set your NuGet ILMerge Version, this is the number from the package manager install, for example:
:: PM> Install-Package ilmerge -Version 3.0.29
:: to confirm it is installed for a given project, see the packages.config file
SET ILMERGE_VERSION=3.0.29

:: the full ILMerge should be found here:
SET ILMERGE_PATH=%USERPROFILE%\.nuget\packages\ilmerge\%ILMERGE_VERSION%\tools\net452
:: dir "%ILMERGE_PATH%"\ILMerge.exe

echo Merging %APP_NAME% ...
dir ..\..\
:: add project DLL's starting with replacing the FirstLib with this project's DLL
"%ILMERGE_PATH%"\ILMerge.exe %PROJECT_DIR%bin\Debug\%APP_NAME%  ^
  /lib:%PROJECT_DIR%bin\%ILMERGE_BUILD%\ ^
  /out:%PROJECT_DIR%bin\merged-%APP_NAME% ^
  ColorMine.dll ^
  FastBitmapLib.dll ^
  fNbt.dll ^
  Newtonsoft.Json.dll
    
::  \otherlibdir\otherlib.dll 

:Done