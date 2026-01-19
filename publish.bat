@echo off
echo Building Click2Fetch...

cd /d "%~dp0src\Click2Fetch"

echo.
echo Publishing Windows x64...
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -o "..\..\publish\win-x64"

echo.
echo Done!
echo Output: publish\win-x64\Click2Fetch.exe
pause
