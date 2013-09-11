@echo off
.nuget\NuGet.exe install SmartSite.Core -ConfigFile .nuget/NuGet.Config -OutputDirectory packages 
.nuget\NuGet.exe update Web/packages.config
.nuget\NuGet.exe restore
