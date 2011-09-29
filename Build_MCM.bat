@echo off

rem Needed to update variable in loop
setlocal enabledelayedexpansion

for %%i in (.\bin\MCMExtension\*.dll) do (set files=!files!%%~i )

echo Merging MCMExtension

tools\ILMerge\ILMerge.exe /out:build\Geckon.MCM.Extension.dll /lib:C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /lib:lib\ %files%

set files=

for %%i in (.\bin\MCMModule\*.dll) do (set files=!files!%%~i )

echo Merging MCMModule

tools\ILMerge\ILMerge.exe /out:build\Geckon.MCM.Module.dll /lib:C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /lib:lib\ %files%

echo Done

pause