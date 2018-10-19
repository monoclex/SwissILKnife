@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

if not "%~1" == "" goto :collect

CALL :task "%~0" "SwissILKnife"

exit /b 0

:collect

set "proj=%~1"

echo.Running tests for '%proj%'

dotnet test %proj%.Tests\ /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

echo.Collecting Results

packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe ^
	-reports:%proj%.Tests\coverage.opencover.xml ^
	-targetdir:Reports\%proj%\

exit

:task
shift

echo. Starting task for "%~1"
start %~0 %~1
echo. Task started

exit /b 0