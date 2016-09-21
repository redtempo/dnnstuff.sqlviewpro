@echo off
echo.
set version=%1
set buildconfig=%2
set path=C:\Program Files (x86)\MSBuild\14.0\Bin\;%path%
set dnnversion=DNN8
Msbuild.exe ModuleSpecific.targets /p:DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.5;OutputPath="./Build/Output/%dnnversion%" /t:Install /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Build_%buildconfig%_%dnnversion%.log;verbosity=detailed
if ERRORLEVEL 1 goto end

set dnnversion=DNN7
Msbuild.exe ModuleSpecific.targets /p:DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig%;TargetFrameworkVersion=v4.0;OutputPath="./Build/Output/%dnnversion%" /t:Install /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Build_%buildconfig%_%dnnversion%.log;verbosity=diagnostic

:end