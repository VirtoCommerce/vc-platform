dotnet restore "%DEPLOYMENT_SOURCE%\VirtoCommerce.Platform.sln"

dotnet publish "%DEPLOYMENT_SOURCE%\src\VirtoCommerce.Platform.Web\VirtoCommerce.Platform.Web.csproj" --output "%DEPLOYMENT_TEMP%" --configuration Release

call :ExecuteCmd "%KUDU_SYNC_CMD%" -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"

:: Execute command routine that will echo out when error 
:ExecuteCmd
setlocal
set _CMD_=%*
echo command=%_CMD_%
call %_CMD_%
if "%ERRORLEVEL%" NEQ "0" echo Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
exit /b %ERRORLEVEL% 
