@echo off
echo.
set version=%1
set path=C:\Program Files (x86)\MSBuild\14.0\Bin\;%path%

REM *** SOURCE ***
set buildconfig=Release
Msbuild.exe ModuleSpecific.targets /p:DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.0 /t:Source /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Build_%buildconfig%_%dnnversion%.log;verbosity=diagnostic
if ERRORLEVEL 1 goto end

set buildconfig=Trial
set dnnversion=DNN8
Msbuild.exe ModuleSpecific.targets /p:DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.5 /t:Install /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Build_%buildconfig%_%dnnversion%.log;verbosity=diagnostic
if ERRORLEVEL 1 goto end

set buildconfig=Release
set dnnversion=DNN8
Msbuild.exe ModuleSpecific.targets /p:DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.5 /t:Install /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Build_%buildconfig%_%dnnversion%.log;verbosity=diagnostic
if ERRORLEVEL 1 goto end

set buildconfig=Trial
set dnnversion=DNN7
Msbuild.exe ModuleSpecific.targets /p:DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.0 /t:Install /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Build_%buildconfig%_%dnnversion%.log;verbosity=diagnostic
if ERRORLEVEL 1 goto end

set buildconfig=Release
set dnnversion=DNN7
Msbuild.exe ModuleSpecific.targets /p:DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.0 /t:Install /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Build_%buildconfig%_%dnnversion%.log;verbosity=diagnostic
if ERRORLEVEL 1 goto end

:end