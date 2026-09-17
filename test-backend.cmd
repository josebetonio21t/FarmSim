@echo off
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\test-backend.ps1" %*
exit /b %ERRORLEVEL%
