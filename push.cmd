@echo off

nuget push .\Acr\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
nuget push .\Acr\bin\Release\*.nupkg -Source https://www.myget.org/F/acr/api/v2/package
del .\Acr\bin\Release\*.nupkg

rem nuget push .\Acr.EfCore\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
rem nuget push .\Acr.EfCore\bin\Release\*.nupkg -Source https://www.myget.org/F/acr/api/v2/package
rem del .\Acr.EfCore\bin\Release\*.nupkg

nuget push .\Acr.XamForms\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
nuget push .\Acr.XamForms\bin\Release\*.nupkg -Source https://www.myget.org/F/acr/api/v2/package
del .\Acr.XamForms\bin\Release\*.nupkg
pause