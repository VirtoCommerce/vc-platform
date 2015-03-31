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
	SET DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot
)

IF NOT DEFINED NEXT_MANIFEST_PATH (
	SET NEXT_MANIFEST_PATH=%ARTIFACTS%\manifest

	IF NOT DEFINED PREVIOUS_MANIFEST_PATH (
		SET PREVIOUS_MANIFEST_PATH=%ARTIFACTS%\manifest
	)
)

IF NOT DEFINED KUDU_SYNC_CMD (
	:: Install kudu sync
	echo Installing Kudu Sync
	call npm install kudusync -g --silent
	IF !ERRORLEVEL! NEQ 0 goto error

	:: Locally just running "kuduSync" would also work
	SET KUDU_SYNC_CMD=%appdata%\npm\kuduSync.cmd
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
SET STORE_SOLUTION_DIR=%DEPLOYMENT_SOURCE%\STOREFRONT
SET STORE_SOLUTION_FILE=%STORE_SOLUTION_DIR%\VirtoCommerce.Website.sln
SET PUBLISHED_WEBSITES=%DEPLOYMENT_TEMP%\_PublishedWebsites
SET PUBLISHED_MODULES=%PUBLISHED_WEBSITES%\Modules

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

echo Handling .NET Web Application deployment.

IF /I "%APPSETTING_VirtoCommerce_DeployApplications%" NEQ "Web Admin Only" (
    :: Build storefront
    echo Building %STORE_SOLUTION_FILE%

    call :ExecuteCmd "%NUGET%" restore "%STORE_SOLUTION_FILE%"
    IF !ERRORLEVEL! NEQ 0 goto error

    call :ExecuteCmd "%MSBUILD_PATH%" "%STORE_SOLUTION_FILE%" /nologo /verbosity:m /t:Build /p:Configuration=Release;DebugType=none;AllowedReferenceRelatedFileExtensions=":";SolutionDir="%STORE_SOLUTION_DIR%\.\\";OutputPath="%DEPLOYMENT_TEMP%" %SCM_BUILD_ARGS%
    IF !ERRORLEVEL! NEQ 0 goto error

    call :ExecuteCmd rename "%PUBLISHED_WEBSITES%\VirtoCommerce.Website" store
    IF !ERRORLEVEL! NEQ 0 goto error
)

:: Build platform
echo Building %ADMIN_SOLUTION_FILE%

:: 1. Restore NuGet packages
call :ExecuteCmd "%NUGET%" restore "%ADMIN_SOLUTION_FILE%"
IF !ERRORLEVEL! NEQ 0 goto error

:: 2. Build to the temporary path
call :ExecuteCmd "%MSBUILD_PATH%" "%ADMIN_SOLUTION_FILE%" /nologo /verbosity:m /t:Build /p:Configuration=Release;DebugType=none;AllowedReferenceRelatedFileExtensions=":";SolutionDir="%ADMIN_SOLUTION_DIR%\.\\";OutputPath="%DEPLOYMENT_TEMP%";VCModulesOutputDir="%PUBLISHED_MODULES%" %SCM_BUILD_ARGS%
IF !ERRORLEVEL! NEQ 0 goto error

call :ExecuteCmd rename "%PUBLISHED_WEBSITES%\VirtoCommerce.Platform.Web" admin
IF !ERRORLEVEL! NEQ 0 goto error

:: Move modules inside WebAdmin
IF EXIST "%PUBLISHED_MODULES%" (
    call :ExecuteCmd move /Y "%PUBLISHED_MODULES%" "%PUBLISHED_WEBSITES%\admin\Modules"
    IF !ERRORLEVEL! NEQ 0 goto error
)

:: Clear build output
call :ExecuteCmd "%MSBUILD_PATH%" "%ADMIN_SOLUTION_FILE%" /nologo /verbosity:m /t:Clean /p:Configuration=Release;SolutionDir="%ADMIN_SOLUTION_DIR%\.\\" %SCM_BUILD_ARGS%

:: 3. KuduSync
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
