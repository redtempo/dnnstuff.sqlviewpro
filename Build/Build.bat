@echo off
echo.
set version=%1
set path=C:\Program Files (x86)\MSBuild\14.0\Bin\;%path%

mkdir logs

set buildconfig=Release

REM restore packages
nuget.exe restore ..\

REM build install zip file
Msbuild.exe ModuleSpecific.targets /p:VisualStudioVersion=14.0;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.5 /t:Install /l:FileLogger,Microsoft.Build.Engine;logfile=logs\Build_%buildconfig%.log;verbosity=diagnostic
if ERRORLEVEL 1 goto end

:end