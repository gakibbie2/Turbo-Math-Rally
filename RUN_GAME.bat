@echo off
cd /d "%~dp0"
if exist "src\bin\Release\net9.0-windows\TurboMathRally.exe" (
    start "" "src\bin\Release\net9.0-windows\TurboMathRally.exe"
) else (
    start "" "src\bin\Debug\net9.0-windows\TurboMathRally.exe"
)
