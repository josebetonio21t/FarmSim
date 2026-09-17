@echo off
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\run-k6.ps1" %*
exit /b %ERRORLEVEL%
