@echo off

rem Needed to update variable in loop
REM setlocal enabledelayedexpansion

echo Getting list of files

REM for %%i in (.\bin\AnyCPU\*.dll) do (set files=!files!%%~i )

REM echo Merging files
REM %files%

mkdir build
Tools\ILMerge\ILMerge.exe /out:build\Geckon.MCM.Module.dll /lib:"c:\Program Files (x86)\Microsoft ASP.NET\ASP.NET MVC 3\Assemblies" /lib:C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /lib:lib\ src\app\Geckon.MCM.Data.Linq\bin\Debug\Geckon.MCM.Data.Linq.dll src\app\Geckon.MCM.Module.Standard\bin\Debug\Geckon.MCM.Module.Standard.dll src\app\Geckon.MCM.Core.Exception\bin\Debug\Geckon.MCM.Core.Exception.dll

echo Done

pause