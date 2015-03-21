<#
    Script builds and deploys azure packages.

    Requirements
    1. Create new database server and set server name in settings below (make sure to add current ip to the firewall)
    2. Set-ExecutionPolicy RemoteSigned
    3. Update you subscription settings file by running Get-AzurePublishSettingsFile
    4. run the deployment

    Features
    - deploys front end
    - deploys database including sample data
#>
Param(  
  	[parameter(Mandatory=$true)]
		$deploymentdir,
        [parameter(Mandatory=$true)]
		$solutiondir,
        [parameter(Mandatory=$true)]
		$storageaccount,
        [parameter(Mandatory=$true)]
		$subscriptionname,
        [parameter(Mandatory=$true)]
        $search_servicename,
        [parameter(Mandatory=$true)]
        $frontend_servicename,
        [parameter(Mandatory=$true)]
        $scheduler_servicename,
	  	[parameter(Mandatory=$false)]
		$db_recreate = $true,
	  	[parameter(Mandatory=$false)]
		$db_customsqlfolder,
	  	[parameter(Mandatory=$false)]
		$db_sampledata = $true,
        [parameter(Mandatory=$true)]
        $db_servername,
        [parameter(Mandatory=$true)] # account that db scripts will be running under
        $db_serverlogin,
        [parameter(Mandatory=$true)]
        $db_serverpassword,
        [parameter(Mandatory=$false)] # account that frontend will be running under
        $db_serveruserlogin,
        [parameter(Mandatory=$false)]
        $db_serveruserpassword,
        [parameter(Mandatory=$true)]
        $db_databasename,
		$publishsettingsfile,
        $vcfpowershellfile,
        $region = 'West US',
		$slot = 'Production',
		$admin_version = '1.0',
        # controlling parameters, allow deploying subset of features, shown in the order they are executed
		$build = $true,
		$build_params,
		$build_config = 'Release',
        $deploy_database = $true,
        $deploy_search = $true,
        $deploy_scheduler = $true,
        $deploy_frontend = $true,
		$deploy_frontend_website = $false, # deploys frontend as a website instead of cloud service, webjobs are deployed as well
        $deploy_admin = $true
     )

# paremeters that need to be changed often or system specific (not deployment specific)
$common_deploymentdir = $deploymentdir
$build_solutiondir = $solutiondir
$common_subscriptionid = $subscriptionname
$common_subscriptionname = $subscriptionname
$common_storageaccount = $storageaccount
$global:common_storagekey = $null

$search_workerrolehome = "$build_solutiondir\src\Azure\WorkerRoles\Search\ESWorkerRole"
$search_workerroleconfig = "$build_solutiondir\src\Azure\WorkerRoles\Search\ElasticSearch"
$search_elasticsearchdistro = "$build_solutiondir\Tools\ElasticSearch"

$scheduler_workerroleconfig = "$build_solutiondir\src\Azure\WorkerRoles\Scheduler\AzureScheduler"
$frontend_workerroleconfig = "$build_solutiondir\src\Azure\WebRoles\Frontend\CommerceSite"

$build_configuration = $build_config # build configuration to use

#*************** LESS OFTEN CHANGED SETTINGS

if($vcfpowershellfile -eq $null)
{
    $common_vcfpowershellfile = ".\bin\$build_configuration\VirtoCommerce.PowerShell.dll"
}
else
{
    $common_vcfpowershellfile = "$vcfpowershellfile"
}

$build_path = "$common_deploymentdir\VCBuildTemp"
$build_solutionname = "$build_solutiondir\VirtoCommerce.sln"

# db settings
if($db_customsqlfolder -eq $null)
{
    $common_dbcustomfolder = "$solutiondir\DeploymentSQL"
}
else
{
    $common_dbcustomfolder = "$db_customsqlfolder"
}

# admin settings
$admin_blobprefix = "$admin_version/admin"
$admin_installcontainer = "http://$common_storageaccount.blob.core.windows.net/software"

# frontend settings
$frontend_packagename = 'CommerceSite.cspkg'

# scheduler settings
$scheduler_packagename = 'AzureScheduler.cspkg'

# search settings
$search_packagename = 'ElasticSearch.cspkg'

# common service deployment parameters
# this parameters are common for all services we deploying
$common_configfolder = "$common_deploymentdir\Configs" # folder contains configuration files for the specific azure server, including connection strings

# set pub config folder that will be used to store modified configuration files
$common_frontendpubxml = $common_configfolder

if($publishsettingsfile -eq $null)
{
    $common_publishsettingsfile = "$common_configfolder\VirtoCommerce.publishsettings"
}
else
{
    $common_publishsettingsfile = "$publishsettingsfile"
}

$common_serviceconfig = 'ServiceConfiguration.Cloud.cscfg'
$common_slot = $slot
$common_region = $region
$global:buildexe_path = $null


# EXECUTION CODE, DO NOT MODIFY ANYTHING BELOW
Function Deploy
{
    if($deploy_search -or $deploy_frontend -or $deploy_scheduler -or $deploy_admin)
    {
        create-container -container_name "mydeployments"
        create-container -container_name "software" -permission "Blob"
    }

    if($deploy_database)
    {
       deploy-database
       if (! $?)
       {
         throw "Database Deployment failed"
       }
    }

    if($deploy_search)
    {
       deploy-search
       if (! $?)
       {
         throw "Search Deployment failed"
       }
    }

    if($deploy_scheduler)
    {
       deploy-scheduler
       if (! $?)
       {
         throw "Scheduler Deployment failed"
       }
    }

    if($deploy_frontend)
    {
       deploy-frontend
       if (! $?)
       {
         throw "Frontend Deployment failed"
       }
    }

    if($deploy_admin)
    {
       deploy-admin
       if (! $?)
       {
         throw "Deployment failed"
       }
    }
}

# Get DB Connection String for SQL Azure
Function Get-DBConnectionString
{
	$dbserverlogin = $db_serverlogin
	$dbserverpassword = $db_serverpassword

    $temp_connectionstring = "Server=tcp:$db_servername.database.windows.net,1433;Database=$db_databasename;User ID=$dbserverlogin@$db_servername;Password=$dbserverpassword;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;MultipleActiveResultSets=True" 

    Return $temp_connectionstring
}

Function Get-SearchConnectionString
{
    $temp_connectionstring = "server=$search_servicename.cloudapp.net:9200;scope=default" 

    Return $temp_connectionstring
}

Function Get-StorageConnectionString
{
    Param(
        [Parameter(Mandatory = $false)]
        $secure = $true
    )

	if($secure)
	{
		$temp_connectionstring = "DefaultEndpointsProtocol=https;AccountName=$common_storageaccount;AccountKey=$common_storagekey" 
	}
	else
	{
		$temp_connectionstring = "DefaultEndpointsProtocol=http;AccountName=$common_storageaccount;AccountKey=$common_storagekey" 
	}

    Return $temp_connectionstring
}

# Generate the publish xml which will be used by MSBuild to deploy the project to website.
Function Generate-PublishXml
{
    Param(
        [Parameter(Mandatory = $true)]
        [String]$WebsiteName
    )

	#& xcopy "pubxml.template" "$common_frontendpubxml\" /Y
	$exists = Test-Path $common_frontendpubxml
	if (! $exists)
	{
		New-Item -ItemType directory -Path $common_frontendpubxml
	}
    
    # Get the current subscription you are working on
    $s = Get-AzureSubscription -Current
    # Get the certificate of the current subscription from your local cert store
    $cert = Get-ChildItem ("Cert:\CurrentUser\My\{0}" -f $s.Certificate.Thumbprint)
    $website = Get-AzureWebsite -Name $WebsiteName
    # Compose the REST API URI from which you will get the publish settings info
    $uri = "https://management.core.windows.net:8443/{0}/services/WebSpaces/{1}/sites/{2}/publishxml" -f `
        $s.SubscriptionId, $website.WebSpace, $Website.Name

    # Get the publish settings info from the REST API
    $publishSettings = Invoke-RestMethod -Uri $uri -Certificate $cert -Headers @{"x-ms-version" = "2013-06-01"}

    # Save the publish settings info into a .publishsettings file
    # and read the content as xml
    $publishSettings.InnerXml > ("{0}\{1}.publishsettings" -f $common_frontendpubxml, $WebsiteName)
    [Xml]$xml = Get-Content ("{0}\{1}.publishsettings" -f $common_frontendpubxml, $WebsiteName)

    # Get the publish xml template and generate the .pubxml file
    [String]$template = Get-Content ("pubxml.template")
    ($template -f $website.HostNames[0], $xml.publishData.publishProfile.publishUrl.Get(0), $WebsiteName) `
        | Out-File -Encoding utf8 ("{0}\{1}.pubxml" -f $common_frontendpubxml, $WebsiteName)
}

Function update-config
{
    Param(
		$configuration,
        [Parameter(Mandatory = $false)]
        $secure = $true
    )

	$dbserverlogin = $db_serverlogin
	$dbserverpassword = $db_serverpassword

	if($db_serveruserlogin -ne $null)
	{
		Write-Output "$(Get-Date -f $timeStampFormat) - Found user login/password for database, using them instead"
		$dbserverlogin = $db_serveruserlogin
		$dbserverpassword = $db_serveruserpassword
	}

	$searchConnectionString = Get-SearchConnectionString
	$storageConnectionString = Get-StorageConnectionString -secure $secure

    Write-Output "loading config from $configuration"
    [xml]$temp_serviceConfig = Get-Content $configuration

    # set database connection string
    $temp_connectionstring = Get-DBConnectionString
    $temp_serviceConfig.ServiceConfiguration.Role.ConfigurationSettings.Setting |
        ? { $_.name -eq 'VirtoCommerce' } |
        % { if($_ -ne $null) {$_.value = "$temp_connectionstring"} }

    # update search url
    $temp_serviceConfig.ServiceConfiguration.Role.ConfigurationSettings.Setting |
        ? { $_.name -eq 'SearchConnectionString' } |
        % { if($_ -ne $null) { $_.value = $searchConnectionString} }

    # update storage url
    $temp_serviceConfig.ServiceConfiguration.Role.ConfigurationSettings.Setting |
        ? { $_.name -eq 'Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString' } |
        % { if($_ -ne $null) { $_.value = $storageConnectionString} }

    # update storage url
    $temp_serviceConfig.ServiceConfiguration.Role.ConfigurationSettings.Setting |
        ? { $_.name -eq 'DataConnectionString' } |
        % { if($_ -ne $null) {$_.value = $storageConnectionString} }

    $temp_serviceConfig.Save("$configuration")
}

Function deploy-database
{	
    write-progress -id 1 -activity "SQL Database Deployment" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - SQL Database Deployment: In progress"

    . ".\azure-db.ps1" -db_recreate $db_recreate -db_customFolder $common_dbcustomfolder -db_servername $db_servername -db_serverlogin $db_serverlogin -db_serverpassword $db_serverpassword -db_databasename $db_databasename -selectedSubscription $common_subscriptionname -publishSettingsFile $common_publishsettingsfile -setupModulePath $common_vcfpowershellfile -db_sampledata $db_sampledata

    write-progress -id 1 -activity "SQL Database Deployment" -status "Finished"
    Write-Output "$(Get-Date -f $timeStampFormat) - SQL Database Deployment: Finished"
}

Function deploy-frontend
{
    write-progress -id 1 -activity "Frontend Deployment" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Frontend Deployment: In progress"

    if(!$deploy_frontend_website) # deploy using cloud service
    {
	    & xcopy "$frontend_workerroleconfig\$common_serviceconfig" "$common_configfolder\CommerceSite\" /Y
        if ($LASTEXITCODE -ne 0)
        {
           throw "XCOPY failed"
        }

        update-config -configuration $common_configfolder\CommerceSite\$common_serviceconfig

        Write-Host "*** Starting Windows CommerceSite Azure deployment process ***"
        . ".\azure-service-publish.ps1" -serviceName $frontend_servicename -storageAccountName $common_storageaccount -storageAccountKey $common_storagekey -cloudConfigLocation $common_configfolder\CommerceSite\$common_serviceconfig -packageLocation $build_path\$frontend_packagename -selectedSubscription $common_subscriptionname -publishSettingsFile $common_publishsettingsfile -subscriptionId $common_subscriptionid -slot $common_slot -location $common_region
            if (! $?)	
            {
              throw "Frontend deployment failed"
            }
    }
	else # deploy as a website
	{
		$global:buildexe_path = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" -Name MSBuildToolsPath).MSBuildToolsPath
		Write-Host "MSBUILD 4.0 Path = $buildexe_path"

		# Create a new website
		Write-Verbose ("[Start] Checking if website {0} exists in slot {1}" -f $frontend_servicename, $common_slot)
		$website = Get-AzureWebsite -Name $frontend_servicename -ErrorAction SilentlyContinue #-Slot $common_slot 
		Write-Verbose ("[Finished] Checking if website {0} exists in slot {1}" -f $frontend_servicename, $common_slot)

		if (! $website)
		{
			Write-Verbose ("[Start] creating website {0} in location {1} and slot {2}" -f $frontend_servicename, $common_region, $common_slot)
			$website = New-AzureWebsite -Name $frontend_servicename -Location $common_region -Verbose #-slot $slot
			Write-Verbose ("[Finish] creating website {0} in location {1} and slot {2}" -f $frontend_servicename, $common_region, $common_slot)
		}

		$dbconnectionstring = Get-DBConnectionString
		$searchConnectionString = Get-SearchConnectionString
		$storageConnectionString = Get-StorageConnectionString

		# Configure app settings for storage account
		$appSettings = @{ `
			"SearchConnectionString" = $searchConnectionString; `
			"DataConnectionString" = $storageConnectionString; `
			"Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" = $storageConnectionString; `
			"AzureWebJobsDashboard" = $storageConnectionString; `
		}

		# Configure connection strings for appdb and ASP.NET member db
		$connectionStrings = ( `
			@{Name = "VirtoCommerce"; Type = "SQLAzure"; ConnectionString = $dbconnectionstring} 
		)

		# Add the connection string and storage account name/key to the website
		Set-AzureWebsite -Name $frontend_servicename -AppSettings $appSettings -ConnectionStrings $connectionStrings

		# Generate the .pubxml file which will be used by webdeploy later
		Write-Verbose ("[Begin] generating {0}.pubxml file" -f $frontend_servicename)
		Generate-PublishXml -Website $frontend_servicename
		Write-Verbose ("{0}\{1}.pubxml" -f $common_frontendpubxml, $frontend_servicename )
		Write-Verbose ("[Finish] generating {0}\{1}.pubxml file" -f $common_frontendpubxml, $frontend_servicename)

		# Read from the publish settings file to get the deploy password
		$publishXmlFile = "{0}\{1}.pubxml" -f $common_frontendpubxml, $frontend_servicename
		[Xml]$xml = Get-Content ("{0}\{1}.publishsettings" -f $common_frontendpubxml, $frontend_servicename)
		$password = $xml.publishData.publishProfile.userPWD.get(0)

		$FrontendProjectFile = "$build_solutiondir\src\Presentation\FrontEnd\StoreWebApp\StoreWebApp.csproj"
		# Run MSBuild to publish the project
		& "$global:buildexe_path\MSBuild.exe" $FrontendProjectFile `
			/p:VisualStudioVersion=12.0 `
			/p:DeployOnBuild=true `
			/p:PublishProfile=$publishXmlFile `
			/p:Password=$password

		Write-Verbose ("[Finish] deploying to Windows Azure website {0}" -f $frontend_servicename)
	}

    write-progress -id 1 -activity "Frontend Deployment" -status "Finished"
    Write-Output "$(Get-Date -f $timeStampFormat) - Frontend Deployment: Finished"
}

Function Build
{
    if($build)
    {
		write-progress -id 1 -activity "Solution Build" -status "Restoring packages"
		& "$build_solutiondir\.nuget\nuget.exe" restore $build_solutionname
        if ($LASTEXITCODE -ne 0)
        {
          throw "Failed to restore packages"
        }

		# set the msbuild path, vs.net 2012 is needed
		$global:buildexe_path = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\12.0" -Name MSBuildToolsPath).MSBuildToolsPath
		Write-Host "MSBUILD 4.0 Path = $buildexe_path"

        write-progress -id 1 -activity "Solution Build" -status "In progress"
        Write-Output "$(Get-Date -f $timeStampFormat) - Solution Build: In progress"

        $now = Get-Date
        $PublishApplicationVersion = "$admin_version.$($now.DayOfYear).$($now.Hour)$($now.Minute)" # get latest checkin version and put it as a version number or a datetime
        Write-Host "Version: $PublishApplicationVersion"

        # clean the build directory
        write-progress -id 1 -activity "Solution Build" -status "Removing previous builds"
        Remove-Item -Path $build_path\* -Recurse -Force -ErrorAction SilentlyContinue

        # build
        write-progress -id 1 -activity "Solution Build" -status "Cleaning in Progress"
        & "$global:buildexe_path\MSBuild.exe" $build_solutionname /m /t:clean $build_params /p:Configuration="$build_configuration"
        if ($LASTEXITCODE -ne 0)
        {
          throw "Build Failed"
        }

        write-progress -id 1 -activity "Solution Build" -status "Building in Progress"
        & "$global:buildexe_path\MSBuild.exe" $build_solutionname /m /t:build /p:Configuration="$build_configuration" /Property:ApplicationVersion=$PublishApplicationVersion /Property:MinimumRequiredVersion=$PublishApplicationVersion $build_params
        if ($LASTEXITCODE -ne 0)
        {
          throw "Build Failed"
        }

        write-progress -id 1 -activity "Solution Build" -status "Publishing in Progress"
        & "$global:buildexe_path\MSBuild.exe" $build_solutionname /m /t:Publish /p:InstallUrl=$admin_installcontainer/$admin_blobprefix/ /p:Configuration="$build_configuration" /p:TargetProfile=Cloud /Property:PublishDir="$build_path\" /Property:ApplicationVersion=$PublishApplicationVersion /Property:MinimumRequiredVersion=$PublishApplicationVersion $build_params
        if ($LASTEXITCODE -ne 0)
        {
          throw "Build Failed"
        }

        build-search

        write-progress -id 1 -activity "Solution Build" -status "Finished"
        Write-Output "$(Get-Date -f $timeStampFormat) - Solution Build: Finished"
    }    
}

Function build-search
{
	# set the msbuild path, vs.net 2012 is needed
	$global:buildexe_path = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" -Name MSBuildToolsPath).MSBuildToolsPath
	Write-Host "MSBUILD 4.0 Path = $buildexe_path"

    write-progress -id 1 -activity "Search Build" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Search Build: In progress"

	# set the azure sdk 2.4 home folder
	$azureSDKPath = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Microsoft SDKs\ServiceHosting\v2.4" -Name InstallPath).InstallPath
	Write-Host "Azure SDK 2.4 Path = $azureSDKPath"

    & "$global:buildexe_path\MSBuild.exe" $search_workerrolehome\ESWorkerRole.csproj /p:Configuration=$build_configuration
    if ($LASTEXITCODE -ne 0)
    {
       throw "Deployment failed"
    }

    write-progress -id 1 -activity "Search Build" -status "Copy JRE7"
    Write-Output "$(Get-Date -f $timeStampFormat) - Search Build: Copy JRE7 defined at ""$Env:JAVA_HOME"""

    $JRE_HOME = $Env:JAVA_HOME

    Write-Host "xcopy ""$JRE_HOME\*.*"" ""$search_workerrolehome\es\jre7\"" /E /Y /Q"
    #xcopy "$JRE_HOME\*.*" "$search_workerrolehome\es\jre7\" /E /Y /Q

    Write-Host "xcopy ""$JRE_HOME\*.*"" ""$search_workerrolehome\bin\$build_configuration\es\jre7\"" /E /Y /Q"
    & "xcopy" "$JRE_HOME\*.*" "$search_workerrolehome\bin\$build_configuration\es\jre7\" /E /Y /Q
    if ($LASTEXITCODE -ne 0)
    {
       throw "XCOPY failed"
    }

    Write-Output "$(Get-Date -f $timeStampFormat) - Search Build: Copy ES Distribution"
    write-progress -id 1 -activity "Search Build" -status "Copy ES Distribution"
	Write-Host "xcopy ""$search_elasticsearchdistro\*.*"" ""$search_workerrolehome\bin\$build_configuration\es\"" /E /Q"
    echo "N" | xcopy "$search_elasticsearchdistro\*.*" "$search_workerrolehome\bin\$build_configuration\es\" /E /Q

    Write-Output "$(Get-Date -f $timeStampFormat) - Search Build: CS Packing"
    write-progress -id 1 -activity "Search Build" -status "cspack running"
    & "$azureSDKPath\bin\cspack.exe" $search_workerroleconfig\ServiceDefinition.csdef "/role:ESWorkerRole;$search_workerrolehome/bin/$build_configuration;VirtoCommerce.Azure.ESWorkerRole.dll" "/rolePropertiesFile:ESWorkerRole;$search_workerrolehome\AzureRoleProperties.txt" "/out:$build_path/ElasticSearch.cspkg"
    if ($LASTEXITCODE -ne 0)
    {
      throw "CSPACK Failed"
    }

    write-progress -id 1 -activity "Search Build" -status "Finished"
    Write-Output "$(Get-Date -f $timeStampFormat) - Search Build: Finished"
}

Function deploy-search
{
    build-search

    write-progress -id 1 -activity "Search Deployment" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Search Deployment: In progress"

	& xcopy "$search_workerroleconfig\$common_serviceconfig" "$common_configfolder\Search\" /Y
    if ($LASTEXITCODE -ne 0)
    {
       throw "XCOPY failed"
    }

    update-config -configuration $common_configfolder\Search\$common_serviceconfig -secure $false # sicne search relies on cloud drive, it can't support https

    . ".\azure-service-publish.ps1" -serviceName $search_servicename -storageAccountName $common_storageaccount -storageAccountKey $common_storagekey -cloudConfigLocation $common_configfolder\Search\$common_serviceconfig -packageLocation $build_path\$search_packagename -selectedSubscription $common_subscriptionname -publishSettingsFile $common_publishsettingsfile -subscriptionId $common_subscriptionid -slot $common_slot -location $common_region
        if ($LASTEXITCODE -ne 0)
        {
          throw "Deployment failed"
        }

    write-progress -id 1 -activity "Search Deployment" -status "Finished"
    Write-Output "$(Get-Date -f $timeStampFormat) - Search Deployment: Finished"
}


Function deploy-scheduler
{
    write-progress -id 1 -activity "Scheduler Deployment" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Scheduler Deployment: In progress"

	& xcopy "$scheduler_workerroleconfig\$common_serviceconfig" "$common_configfolder\Scheduler\" /Y
    update-config -configuration $common_configfolder\Scheduler\$common_serviceconfig

    . ".\azure-service-publish.ps1" -serviceName $scheduler_servicename -storageAccountName $common_storageaccount -storageAccountKey $common_storagekey -cloudConfigLocation $common_configfolder\Scheduler\$common_serviceconfig -packageLocation $build_path\$scheduler_packagename -selectedSubscription $common_subscriptionname -publishSettingsFile $common_publishsettingsfile -subscriptionId $common_subscriptionid -slot $common_slot -location $common_region
        if (! $?)
        {
          throw "Deployment failed"
        }

    write-progress -id 1 -activity "Scheduler Deployment" -status "Finished"
    Write-Output "$(Get-Date -f $timeStampFormat) - Scheduler Deployment: Finished"
}


Function create-storage
{
    write-progress -id 1 -activity "Checking storage account $common_storageaccount" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Checking storage account {$common_storageaccount}: In progress"

    $temp_storageaccount = Get-AzureStorageAccount -StorageAccountName $common_storageaccount -ErrorAction SilentlyContinue
    if($temp_storageaccount -eq $null)
    {
        write-progress -id 1 -activity "Checking container $storageaccount" -status "Creating new"
        Write-Output "$(Get-Date -f $timeStampFormat) - Checking storage account {$common_storageaccount}: Creating new"

        New-AzureStorageAccount -StorageAccountName $common_storageaccount -Label $common_storageaccount -Location $common_region
        if ($LASTEXITCODE -ne 0)
        {
          throw "Failed to create storage account " + $common_storageaccount
        }
    }

    $global:common_storagekey = (Get-AzureStorageKey -StorageAccountName $common_storageaccount).Primary
}

Function create-container
{
    Param(  
  	    [parameter(Mandatory=$true)]
		$container_name,
		$permission = "Off"
    )
    write-progress -id 1 -activity "Checking container $container_name" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Checking container {$container_name}: In progress"

    create-storage

    Write-Output $common_storageaccount
    $destContext = New-AzureStorageContext -StorageAccountName $common_storageaccount -StorageAccountKey $common_storagekey

    $temp_container = Get-AzureStorageContainer -Name $container_name -Context $destContext -ErrorAction SilentlyContinue
    if($temp_container -eq $null)
    {
        write-progress -id 1 -activity "Checking container $container_name" -status "Creating new"
        Write-Output "$(Get-Date -f $timeStampFormat) - Checking container {$container_name}: Creating new"       
        New-AzureStorageContainer -Name $container_name -Permission $permission -Context $destContext
    }

    write-progress -id 1 -activity "Checking container" -status "Finished"
    Write-Output "$(Get-Date -f $timeStampFormat) - Checking container {$container_name}: Finished"
}

Function deploy-admin
{
    write-progress -id 1 -activity "Admin Deployment" -status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Admin Deployment: In progress"

    $wpf_applicationfiles = "$build_path\Application Files"
    $temp_application = "$build_path\Application"
    # create directory structure for the upload
    Write-Output "Remove-Item -Path ""$temp_application"" -Recurse -Force"
    Remove-Item -Path "$temp_application" -Recurse -Force -ErrorAction Ignore
    New-Item -ItemType directory -Path $temp_application
    New-Item -ItemType directory -Path $temp_application/$admin_version
    New-Item -ItemType directory -Path $temp_application/$admin_version/admin
    
    Write-Output "XCopy ""$build_path\Application Files"" ""$temp_application\$admin_version\admin"" /E /Y /Q"

    XCopy "$build_path\Application Files\*" "$temp_application\$admin_version\admin\Application Files\" /E /Y /Q
    
    Copy-Item -Path "$build_path\setup.exe" -Destination $temp_application/$admin_version/admin
    Copy-Item -Path "$build_path\VirtoCommerce.application" -Destination $temp_application/$admin_version/admin
    Copy-Item -Path "$build_path\VirtoCommerce.application" -Destination $temp_application

    ls "$temp_application" -File -Recurse | Set-AzureStorageBlobContent -Container "software" -Verbose -ConcurrentTaskCount 2 -Force
    
    #Write-Output "Remove temp application_files directory"
    #Remove-Item -Path "$wpf_applicationfiles" -Recurse -Force -ErrorAction Ignore

    write-progress -id 1 -activity "Admin Deployment" -status "Finished"
    Write-Output "$(Get-Date -f $timeStampFormat) - Admin Deployment: Finished"
}   


#Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\*.psd1"
Import-AzurePublishSettingsFile $common_publishsettingsfile
#if ($LASTEXITCODE -ne 0)
#{
#	throw "Failed to import publish settings file"
#}

Set-AzureSubscription -DefaultSubscription $common_subscriptionname

Set-AzureSubscription -CurrentStorageAccount $common_storageaccount -SubscriptionName $common_subscriptionname

# Clear out any previous Windows Azure subscription details in the current context (just to be safe).
# Select-AzureSubscription -Clear

# Select (by friendly name entered in the 'Set-AzureSubscription' cmdlet) the Windows Azure subscription to use.
Select-AzureSubscription $common_subscriptionname

Build
Deploy
