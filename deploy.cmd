@if "%SCM_TRACE_LEVEL%" NEQ "4" @echo off

:: ----------------------
:: KUDU Deployment Script
:: Version: 0.1.11
:: ----------------------

:: Prerequisites
:: -------------

:: Verify node.js installed
where node 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
    echo Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment.
    goto error
)

:: If nuget.exe does not exist in one of the PATH directories, use \Tools\NuGet\nuget.exe
SET NUGET=nuget.exe
where nuget.exe 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
    SET "NUGET=%~dp0Tools\NuGet\nuget.exe"
)

:: Setup
:: -----

setlocal enabledelayedexpansion

SET "ARTIFACTS=%~dp0artifacts"

IF NOT DEFINED DEPLOYMENT_SOURCE (
    SET "DEPLOYMENT_SOURCE=%~dp0."
)

IF NOT DEFINED DEPLOYMENT_TARGET (
    SET "DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot"
)

IF NOT DEFINED NEXT_MANIFEST_PATH (
    SET "NEXT_MANIFEST_PATH=%ARTIFACTS%\manifest"

    IF NOT DEFINED PREVIOUS_MANIFEST_PATH (
        SET "PREVIOUS_MANIFEST_PATH=%ARTIFACTS%\manifest"
    )
)

IF NOT DEFINED KUDU_SYNC_CMD (
    :: Install kudu sync
    echo Installing Kudu Sync
    call npm install kudusync -g --silent
    IF !ERRORLEVEL! NEQ 0 goto error

    :: Locally just running "kuduSync" would also work
    SET "KUDU_SYNC_CMD=%appdata%\npm\kuduSync.cmd"
)
IF NOT DEFINED DEPLOYMENT_TEMP (
    SET "DEPLOYMENT_TEMP=%temp%\_vc_deploy\%random%"
    SET CLEAN_LOCAL_DEPLOYMENT_TEMP=true
)

IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
    IF EXIST "%DEPLOYMENT_TEMP%" rd /s /q "%DEPLOYMENT_TEMP%"
    mkdir "%DEPLOYMENT_TEMP%"
)

IF NOT DEFINED MSBUILD_EXE IF DEFINED MSBUILD_15_DIR IF EXIST "%MSBUILD_15_DIR%\MSBuild.exe" SET "MSBUILD_EXE=%MSBUILD_15_DIR%\MSBuild.exe"
IF NOT DEFINED MSBUILD_EXE IF DEFINED MSBUILD_PATH IF EXIST "%MSBUILD_PATH%" SET "MSBUILD_EXE=%MSBUILD_PATH%"
IF NOT DEFINED ProgramFiles(x86) SET "ProgramFiles(x86)=%ProgramFiles%"
IF NOT DEFINED MSBUILD_EXE IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
    FOR /f "usebackq tokens=*" %%i IN (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) DO (
        IF EXIST "%%i\MSBuild\15.0\Bin\MSBuild.exe" (
            SET "MSBUILD_EXE=%%i\MSBuild\15.0\Bin\MSBuild.exe"
        )
    )
)
IF NOT DEFINED MSBUILD_EXE IF EXIST "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" SET "MSBUILD_EXE=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
IF NOT DEFINED MSBUILD_EXE IF EXIST "%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" SET "MSBUILD_EXE=%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
IF NOT DEFINED MSBUILD_EXE SET "MSBUILD_EXE=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

SET SOLUTION_DIR=%DEPLOYMENT_SOURCE%
SET SOLUTION_FILE=%SOLUTION_DIR%\VirtoCommerce.Platform.sln
SET PUBLISHED_WEBSITES=%DEPLOYMENT_TEMP%\_PublishedWebsites

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

echo Handling .NET Web Application deployment.

:: Build platform
echo Building %SOLUTION_FILE%

:: Restore NuGet packages
call :ExecuteCmd "%NUGET%" restore "%SOLUTION_FILE%"
IF !ERRORLEVEL! NEQ 0 goto error

:: Build to the temporary path
call :ExecuteCmd "%MSBUILD_EXE%" "%SOLUTION_FILE%" /nologo /verbosity:m /t:Build /p:Configuration=Release;Platform="Any CPU";SolutionDir="%SOLUTION_DIR%\\";OutputPath="%DEPLOYMENT_TEMP%" %SCM_BUILD_ARGS%
IF !ERRORLEVEL! NEQ 0 goto error

:: Rename application directory
call :ExecuteCmd rename "%PUBLISHED_WEBSITES%\VirtoCommerce.Platform.Web" platform
IF !ERRORLEVEL! NEQ 0 goto error

:: Clear build output
call :ExecuteCmd "%MSBUILD_EXE%" "%SOLUTION_FILE%" /nologo /verbosity:m /t:Clean /p:Configuration=Release;Platform="Any CPU";SolutionDir="%SOLUTION_DIR%\\" %SCM_BUILD_ARGS%

:: KuduSync
IF /I "%IN_PLACE_DEPLOYMENT%" NEQ "1" (
    call :ExecuteCmd "%KUDU_SYNC_CMD%" -v 50 -f "%PUBLISHED_WEBSITES%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"
    IF !ERRORLEVEL! NEQ 0 goto error
)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:: Post deployment stub
IF DEFINED POST_DEPLOYMENT_ACTION call "%POST_DEPLOYMENT_ACTION%"
IF !ERRORLEVEL! NEQ 0 goto error

goto end

:: Execute command routine that will echo out when error
:ExecuteCmd
setlocal
set _CMD_=%*
echo command=%_CMD_%
call %_CMD_%
if "%ERRORLEVEL%" NEQ "0" echo Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
exit /b %ERRORLEVEL%

:error
endlocal
echo An error has occurred during web site deployment.
call :exitSetErrorLevel
call :exitFromFunction 2>nul

:exitSetErrorLevel
exit /b 1

:exitFromFunction
()

:end
endlocal
echo Finished successfully.
