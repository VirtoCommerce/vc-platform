@echo off

:: ----------------------
:: Modules Packaging Script
:: Version: 1
:: ----------------------

:: Prerequisites
:: -------------

:: If nuget.exe does not exist in one of the PATH directories, use \Tools\NuGet\nuget.exe
SET NUGET=nuget.exe
where nuget.exe 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
    SET NUGET=%~dp0%Tools\NuGet\nuget.exe
)

:: Setup
:: -----

setlocal enabledelayedexpansion

SET ARTIFACTS=%~dp0%artifacts

IF NOT DEFINED DEPLOYMENT_SOURCE (
	SET DEPLOYMENT_SOURCE=%~dp0%.
)

IF NOT DEFINED DEPLOYMENT_TARGET (
	SET DEPLOYMENT_TARGET=%ARTIFACTS%\modules
)

IF NOT DEFINED DEPLOYMENT_TEMP (
	SET DEPLOYMENT_TEMP=%temp%\___deployTemp%random%
	SET CLEAN_LOCAL_DEPLOYMENT_TEMP=true
)

IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
	IF EXIST "%DEPLOYMENT_TEMP%" rd /s /q "%DEPLOYMENT_TEMP%"
	mkdir "%DEPLOYMENT_TEMP%"
)

IF NOT DEFINED MSBUILD_PATH (
	SET MSBUILD_PATH=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
)

SET ADMIN_SOLUTION_DIR=%DEPLOYMENT_SOURCE%\PLATFORM
SET ADMIN_SOLUTION_FILE=%ADMIN_SOLUTION_DIR%\VirtoCommerce.WebPlatform.sln
SET PUBLISHED_WEBSITES=%DEPLOYMENT_TEMP%\_PublishedWebsites
SET PUBLISHED_MODULES=%PUBLISHED_WEBSITES%
SET PUBLISHED_PACKAGES=%PUBLISHED_MODULES%

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

:: Build platform
echo Building %ADMIN_SOLUTION_FILE%

:: 1. Restore NuGet packages
call :ExecuteCmd "%NUGET%" restore "%ADMIN_SOLUTION_FILE%"
IF !ERRORLEVEL! NEQ 0 goto error

:: 2. Build to the temporary path
call :ExecuteCmd "%MSBUILD_PATH%" "%ADMIN_SOLUTION_FILE%" /nologo /verbosity:m /t:Build /p:Configuration=Release;DebugType=none;AllowedReferenceRelatedFileExtensions=":";SolutionDir="%ADMIN_SOLUTION_DIR%\.\\";OutputPath="%DEPLOYMENT_TEMP%";VCModulesOutputDir="%PUBLISHED_MODULES%";VCModulesZipDir="%PUBLISHED_PACKAGES%" %SCM_BUILD_ARGS%
IF !ERRORLEVEL! NEQ 0 goto error

:: Move modules to target directory
call :ExecuteCmd mkdir "%DEPLOYMENT_TARGET%"
call :ExecuteCmd move /Y "%PUBLISHED_PACKAGES%\*.zip" "%DEPLOYMENT_TARGET%\"
IF !ERRORLEVEL! NEQ 0 goto error

:: Clear build output
call :ExecuteCmd "%MSBUILD_PATH%" "%ADMIN_SOLUTION_FILE%" /nologo /verbosity:m /t:Clean /p:Configuration=Release;SolutionDir="%ADMIN_SOLUTION_DIR%\.\\" %SCM_BUILD_ARGS%

:: Remove temporary directory
IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
	IF EXIST "%DEPLOYMENT_TEMP%" rd /s /q "%DEPLOYMENT_TEMP%"
)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

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
