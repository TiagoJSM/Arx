set "dllExtension=.dll"
set "pdbExtension=.pdb"

echo.Copying file %~1%~2%dllExtension% to %~dp0%~3
echo.Copying file %~1%~2%pdbExtension% to %~dp0%~3

if not exist "%~dp0%~3" mkdir "%~dp0%~3"

COPY /y "%~1%~2%dllExtension%" "%~dp0%~3%~2%dllExtension%"
COPY /y "%~1%~2%pdbExtension%" "%~dp0%~3%~2%pdbExtension%"

