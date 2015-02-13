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

:: Setup
:: -----

setlocal enabledelayedexpansion

SET ARTIFACTS=%~dp0%..\artifacts

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

IF NOT DEFINED VCPS (
	SET VCPS=%DEPLOYMENT_SOURCE%\src\Extensions\Setup\VirtoCommerce.PowerShell
)

IF /I "%APPSETTING_insertSampleData%" EQU "True" (
	SET INSERT_SAMPLE_DATA=$true
) ELSE (
	SET INSERT_SAMPLE_DATA=$false
)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

echo Handling .NET Web Application deployment.

:: 1. Restore NuGet packages
IF /I "VirtoCommerce.WebPlatform.sln" NEQ "" (
	call :ExecuteCmd nuget restore "%DEPLOYMENT_SOURCE%\VirtoCommerce.WebPlatform.sln"
	IF !ERRORLEVEL! NEQ 0 goto error
)

:: 2. Build to the temporary path
IF /I "%IN_PLACE_DEPLOYMENT%" NEQ "1" (
	echo Building VirtoCommerce.WebPlatform.sln
	call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\VirtoCommerce.WebPlatform.sln" /nologo /verbosity:m /t:Build /p:Configuration=Release;SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
	IF !ERRORLEVEL! NEQ 0 goto error

	echo Building VirtoCommerce.Platform.Web.csproj
	call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\src\Presentation\WebAdmin\VirtoCommerce.Platform.Web\VirtoCommerce.Platform.Web.csproj" /nologo /verbosity:m /t:Build /t:pipelinePreDeployCopyAllFilesToOneFolder /p:_PackageTempDir="%DEPLOYMENT_TEMP%";AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release /p:SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
	IF !ERRORLEVEL! NEQ 0 goto error
) ELSE (
	echo Building VirtoCommerce.WebPlatform.sln
	call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\VirtoCommerce.WebPlatform.sln" /nologo /verbosity:m /t:Build /p:Configuration=Release;SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
	IF !ERRORLEVEL! NEQ 0 goto error

	echo Building VirtoCommerce.Platform.Web.csproj
	call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\src\Presentation\WebAdmin\VirtoCommerce.Platform.Web\VirtoCommerce.Platform.Web.csproj" /nologo /verbosity:m /t:Build /p:AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release /p:SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
	IF !ERRORLEVEL! NEQ 0 goto error
)

:: Clear build output
call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\VirtoCommerce.WebPlatform.sln" /nologo /verbosity:m /t:Clean /p:Configuration=Release;SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%

:: 3. KuduSync
IF /I "%IN_PLACE_DEPLOYMENT%" NEQ "1" (
	call :ExecuteCmd "%KUDU_SYNC_CMD%" -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"
	IF !ERRORLEVEL! NEQ 0 goto error
)

:: Initialize database for first deployment
:: If PREVIOUS_MANIFEST_PATH ends with firstDeploymentManifest then initialize database

echo(!PREVIOUS_MANIFEST_PATH!|findstr /r /i /c:"firstDeploymentManifest$" >nul && %SQLAZURECONNSTR_DefaultConnection% >null && (
	echo First deployment. Need to initialize database. InsertSampleData = %APPSETTING_insertSampleData%

	IF /I "%SQLAZURECONNSTR_DefaultConnection%" EQU "" (
		echo Connection string is empty. Skipping database initialization.
	) ELSE (
		IF EXIST "%VCPS%\VirtoCommerce.PowerShell.csproj" (
			echo Building %VCPS%\VirtoCommerce.PowerShell.csproj
			call :ExecuteCmd "%MSBUILD_PATH%" "%VCPS%\VirtoCommerce.PowerShell.csproj" /nologo /verbosity:m /t:Build /p:Configuration=Release;SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
			IF !ERRORLEVEL! NEQ 0 goto error
		) ELSE (
			echo %VCPS%\VirtoCommerce.PowerShell.csproj does not exist.
		)
	
		IF EXIST "%VCPS%\setup-database.ps1" (
			echo Executing %VCPS%\setup-database.ps1
			call :ExecuteCmd PowerShell -ExecutionPolicy Bypass -Command "%VCPS%\setup-database.ps1" -dbconnection '%SQLAZURECONNSTR_DefaultConnection%' -datafolder "%VCPS%" -moduleFile "%VCPS%\bin\Release\VirtoCommerce.PowerShell.dll" -useSample %INSERT_SAMPLE_DATA% -reducedSample $false
			IF !ERRORLEVEL! NEQ 0 goto error
		) ELSE (
			echo %VCPS%\setup-database.ps1 does not exist.
		)
	
		:: Clear build output
		call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\VirtoCommerce.sln" /nologo /verbosity:m /t:Clean /p:Configuration=Release;SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
	)
) || (
	echo Not first deployment
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
