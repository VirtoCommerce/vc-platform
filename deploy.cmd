::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

echo Virto Commerce Platform deployment.

:: 1. Restore nuget packages
call :ExecuteCmd dotnet restore "%DEPLOYMENT_SOURCE%\VirtoCommerce.Platform.sln"
IF !ERRORLEVEL! NEQ 0 goto error

:: 2. Build and publish
call :ExecuteCmd dotnet publish "%DEPLOYMENT_SOURCE%\src\VirtoCommerce.Platform.Web\VirtoCommerce.Platform.Web.csproj" --output "%DEPLOYMENT_TEMP%" --configuration Release
IF !ERRORLEVEL! NEQ 0 goto error

:: 3. KuduSync
call :ExecuteCmd "%KUDU_SYNC_CMD%" -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"
IF !ERRORLEVEL! NEQ 0 goto error
