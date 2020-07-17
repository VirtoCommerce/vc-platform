:: 1. Restore nuget packages
dotnet restore "%DEPLOYMENT_SOURCE%\VirtoCommerce.Platform.sln"

:: 2. Build and publish
dotnet publish "%DEPLOYMENT_SOURCE%\src\VirtoCommerce.Platform.Web\VirtoCommerce.Platform.Web.csproj" --output "%DEPLOYMENT_TEMP%" --configuration Release

:: 3. KuduSync
call :ExecuteCmd "%KUDU_SYNC_CMD%" -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd" 
