@echo off
setlocal

if "%SIGN_OMNIFACTOTUM_WPF%" neq "1" goto SKIP

echo * SIGNING "%~f1"...
sn.exe -Rca "%~f1" Omnifactotum.Wpf
if errorlevel 1 goto ERROR

goto :EOF
goto ERROR

:SKIP
echo ** SIGNING HAS BEEN SKIPPED.
exit /b 0

:ERROR
echo ***** ERROR *****
exit /b 1
