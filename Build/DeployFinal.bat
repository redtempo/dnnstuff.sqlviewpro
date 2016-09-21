@echo off
echo.
set version=%1
set path=C:\Program Files (x86)\MSBuild\14.0\Bin\;%path%
set logtype=normal

Msbuild.exe ModuleSpecific.targets /p:Version=%version% /t:AddAnnouncement /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Deploy_AddAnouncement.log;verbosity=%logtype%

set type=Install
set buildconfig=Trial
set dnnversion=DNN8
Msbuild.exe ModuleSpecific.targets /p:Type=%type%;DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig% /t:DeployTrial /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Deploy_%buildconfig%_%dnnversion%.log;verbosity=%logtype%

set type=Install
set buildconfig=Release
set dnnversion=DNN8
Msbuild.exe ModuleSpecific.targets /p:Type=%type%;DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig% /t:DeployFile /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Deploy_%buildconfig%_%dnnversion%.log;verbosity=%logtype%

set type=Source
set buildconfig=Release
set dnnversion=DNN8
Msbuild.exe ModuleSpecific.targets /p:Type=%type%;DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig% /t:DeploySource /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Deploy_%buildconfig%_%dnnversion%.log;verbosity=%logtype%


set type=Install
set buildconfig=Trial
set dnnversion=DNN7
Msbuild.exe ModuleSpecific.targets /p:Type=%type%;DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig% /t:DeployTrial /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Deploy_%buildconfig%_%dnnversion%.log;verbosity=%logtype%

set type=Install
set buildconfig=Release
set dnnversion=DNN7
Msbuild.exe ModuleSpecific.targets /p:Type=%type%;DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig% /t:DeployFile /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Deploy_%buildconfig%_%dnnversion%.log;verbosity=%logtype%

set type=Source
set buildconfig=Release
set dnnversion=DNN7
Msbuild.exe ModuleSpecific.targets /p:Type=%type%;DNNVersion=%dnnversion%;Version=%version%;Configuration=%buildconfig% /t:DeploySource /l:FileLogger,Microsoft.Build.Engine;logfile=Logs\Deploy_%buildconfig%_%dnnversion%.log;verbosity=%logtype%

:end