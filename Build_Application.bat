@echo off
ECHO This utility tool will build Lastgarriz application for you using dotnet command line interface.
ECHO.
ECHO Prerequisite : .NET 7 SDK need to be installed first !
ECHO.
ECHO Microsoft installer can be found at : https://dotnet.microsoft.com/en-us/download/dotnet/7.0 (choose Windows x64)
ECHO.
ECHO Caution, please note :
ECHO - Close this window if you don't have .NET 7 SDK installed or proceed.
ECHO - All sources and temporary files will be deleted automatically after completion.
ECHO - Do not share your generated binary to public domain.
ECHO.
pause
dotnet publish Lastgarriz -c Release -r win-x64 --self-contained --output . -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:PublishProtocol=FileSystem
RMDIR /s /q Lastgarriz
RMDIR /s /q .vs
RMDIR /s /q x86
DEL .gitignore
DEL Lastgarriz.sln
DEL LICENSE
DEL README.md
DEL VERSION
DEL Build_Application.bat
