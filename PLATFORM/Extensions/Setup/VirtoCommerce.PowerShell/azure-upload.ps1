<#
    Script uploads specified folder to the blob storage

    This script is useful when uploading single file to azure.
#>
Param(  
  	[parameter(Mandatory=$true)]
		$deploymentdir,
        [parameter(Mandatory=$true)]
		$storageaccount,
        [parameter(Mandatory=$true)]
		$subscriptionname,
        [parameter(Mandatory=$true)]
		$container,
        [parameter(Mandatory=$true)]
		$folder,
        $region = 'West US'
     )

# paremeters that need to be changed often or system specific (not deployment specific)
$common_deploymentdir = $deploymentdir
$common_subscriptionid = $subscriptionname
$common_subscriptionname = $subscriptionname
$common_storageaccount = $storageaccount
$global:common_storagekey = $null


#*************** LESS OFTEN CHANGED SETTINGS

# common service deployment parameters
$common_configfolder = "$common_deploymentdir\Configs" # folder contains configuration files for the specific azure server, including connection strings
$common_publishsettingsfile = "$common_configfolder\VirtoCommerce.publishsettings"
$common_slot = $slot
$common_region = $region

# EXECUTION CODE, DO NOT MODIFY ANYTHING BELOW
Function Deploy
{
    create-container -container_name $container -permission "Blob"

       deploy-folder
       if (! $?)
       {
         throw "Deployment failed"
       }
}

Function create-storage
{
    write-progress -id 1 -activity "Checking storage account $common_storageaccount" -status "In progress"
    Write-Output "$(Get-Date –f $timeStampFormat) - Checking storage account {$common_storageaccount}: In progress"

    $temp_storageaccount = Get-AzureStorageAccount –StorageAccountName $common_storageaccount -ErrorAction SilentlyContinue
    if($temp_storageaccount -eq $null)
    {
        write-progress -id 1 -activity "Checking container $storageaccount" -status "Creating new"
        Write-Output "$(Get-Date –f $timeStampFormat) - Checking storage account {$common_storageaccount}: Creating new"

        New-AzureStorageAccount -StorageAccountName $common_storageaccount -Label $common_storageaccount -Location $common_region
        if ($LASTEXITCODE -ne 0)
        {
          throw "Build Failed"
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
    Write-Output "$(Get-Date –f $timeStampFormat) - Checking container {$container_name}: In progress"

    create-storage

    Write-Output $common_storageaccount
    $destContext = New-AzureStorageContext –StorageAccountName $common_storageaccount -StorageAccountKey $common_storagekey

    $temp_container = Get-AzureStorageContainer -Name $container_name -Context $destContext -ErrorAction SilentlyContinue
    if($temp_container -eq $null)
    {
        write-progress -id 1 -activity "Checking container $container_name" -status "Creating new"
        Write-Output "$(Get-Date –f $timeStampFormat) - Checking container {$container_name}: Creating new"       
        New-AzureStorageContainer -Name $container_name -Permission $permission -Context $destContext
    }

    write-progress -id 1 -activity "Checking container" -status "Finished"
    Write-Output "$(Get-Date –f $timeStampFormat) - Checking container {$container_name}: Finished"
}

Function deploy-folder
{
    write-progress -id 1 -activity "File Deployment" -status "In progress"
    Write-Output "$(Get-Date –f $timeStampFormat) - File Deployment: In progress"

    $temp_application = "$folder"

    ls "$temp_application" -File -Recurse | Set-AzureStorageBlobContent -Container $container -Verbose -ConcurrentTaskCount 2 -Force   

    write-progress -id 1 -activity "File Deployment" -status "Finished"
    Write-Output "$(Get-Date –f $timeStampFormat) - File Deployment: Finished"
}   


#Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\*.psd1"
Import-AzurePublishSettingsFile $common_publishsettingsfile

Set-AzureSubscription -DefaultSubscription $common_subscriptionname

Set-AzureSubscription -CurrentStorageAccount $common_storageaccount -SubscriptionName $common_subscriptionname

# Clear out any previous Windows Azure subscription details in the current context (just to be safe).
# Select-AzureSubscription -Clear

# Select (by friendly name entered in the 'Set-AzureSubscription' cmdlet) the Windows Azure subscription to use.
Select-AzureSubscription $common_subscriptionname

Deploy