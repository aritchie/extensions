@echo off

rem nuget push .\Acr\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
rem del .\Acr\bin\Release\*.nupkg

nuget push .\Acr.EfCore\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
del .\Acr.EfCore\bin\Release\*.nupkg

rem nuget push .\Acr.XamForms\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
rem del .\Acr.XamForms\bin\Release\*.nupkg
pause