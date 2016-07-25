@echo off
nuget pack -build -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=../../Bin/Release" -OutputDirectory "../../Package" 
pause