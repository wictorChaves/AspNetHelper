@echo off
set currentDirectory=%~dp0

set executable="C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\Microsoft\Azure Storage Emulator\azurite.exe"
set host=127.0.0.1
set folder=%currentDirectory%
set debugFile=%currentDirectory%\debug.log

%executable% %parameters% --queueHost %host% --location %folder% --debug %debugFile%