@echo off
call PackPackage
nuget push *.nupkg
pause