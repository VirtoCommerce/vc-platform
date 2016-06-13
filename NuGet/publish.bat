@echo off
setlocal enabledelayedexpansion

if "%1" equ "" (
    echo Pass NuGet API key as first parameter
    pause
    exit /b 1
)

for %%f in (*.nupkg) do (
    set fileName=%%~nf
    if "!fileName!" equ "!fileName:.symbols=!" (
        nuget push %%f -Source nuget.org -ApiKey %1
    )
)

pause
