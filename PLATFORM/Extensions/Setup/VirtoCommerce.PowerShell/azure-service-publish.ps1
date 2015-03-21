#NOTE: This script is based on the Microsoft provided guidance example at
#      https://www.windowsazure.com/en-us/develop/net/common-tasks/continuous-delivery/.

Param(  
  	[parameter(Mandatory=$true)]
		$serviceName,
        [parameter(Mandatory=$true)]
		$storageAccountName,
		[parameter(Mandatory=$true)]
		$storageAccountKey,
		[parameter(Mandatory=$true)]
        $cloudConfigLocation,
		[parameter(Mandatory=$true)]
		$packageLocation,
        $slot = "Staging",
        $deploymentLabel = $null,
        $timeStampFormat = "g",
        $alwaysDeleteExistingDeployments = 1,
        $enableDeploymentUpgrade = 1,
		[parameter(Mandatory=$true)]		
		$selectedSubscription,
        [parameter(Mandatory=$true)]
		$publishSettingsFile,
		$subscriptionId,
		$location
     )

function SuspendDeployment()
{
    write-progress -id 1 -activity "Suspending Deployment" -status "In progress"
    Write-Output "$(Get-Date –f $timeStampFormat) - Suspending Deployment: In progress"

	$suspend = Set-AzureDeployment -Slot $slot -ServiceName $serviceName -Status Suspended

    write-progress -id 1 -activity "Suspending Deployment" -status $opstat
    Write-Output "$(Get-Date –f $timeStampFormat) - Suspending Deployment: $opstat"
}

function DeleteDeployment()
{
    SuspendDeployment

    write-progress -id 2 -activity "Deleting Deployment" -Status "In progress"
    Write-Output "$(Get-Date –f $timeStampFormat) - Deleting Deployment: In progress"

    $removeDeployment = Remove-AzureDeployment -Slot $slot -ServiceName $serviceName
    Write-Output "$(Get-Date –f $timeStampFormat) - Deleting Deployment: $opstat"

    sleep -Seconds 10
}

function UploadPackage()
{
    $blobFile = Get-ChildItem $packageLocation
    $destContext = New-AzureStorageContext –StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey
    Write-Output $packageLocation
    Write-Output $blobFile.Name
    Set-AzureStorageBlobContent -File $packageLocation -Container "mydeployments" -Blob $blobFile.Name -Verbose -ConcurrentTaskCount 1 -Force
}

function Publish()
{	
	CreateService
	
    $deployment = Get-AzureDeployment -ServiceName $serviceName -Slot $slot -ErrorVariable a -ErrorAction silentlycontinue 
 
    if ($deployment -eq $null)
    {
        write-host "No deployment is detected. Creating a new deployment. "
    }
	
    #check for existing deployment and then either upgrade, delete + deploy, or cancel according to $alwaysDeleteExistingDeployments and $enableDeploymentUpgrade boolean variables
    if ($deployment.Name -ne $null)
    {
        switch ($alwaysDeleteExistingDeployments)
        {
            1 
            {		
				UploadPackage
			
                switch ($enableDeploymentUpgrade)
                {
                    1
                    {
                        Write-Output "$(Get-Date –f $timeStampFormat) - Deployment exists in $servicename.  Upgrading deployment."
                        UpgradeDeployment
                    }
                    0  #Delete then create new deployment
                    {
                        Write-Output "$(Get-Date –f $timeStampFormat) - Deployment exists in $servicename.  Deleting deployment."
                        DeleteDeployment
                        CreateNewDeployment

                    }
                } # switch ($enableDeploymentUpgrade)
            }
            0
            {
                Write-Output "$(Get-Date –f $timeStampFormat) - ERROR: Deployment exists in $servicename.  Script execution cancelled."
                exit
            }
        }
    } else
	{
			UploadPackage
            CreateNewDeployment
    }
}

function CreateNewDeployment()
{
    write-progress -id 3 -activity "Creating New Deployment" -Status "In progress"
    Write-Output "$(Get-Date –f $timeStampFormat) - Creating New Deployment: In progress"

    $blobFile = Get-ChildItem $packageLocation
    $blob_name = $blobFile.Name
    $blob_url = "http://$storageAccountName.blob.core.windows.net/mydeployments/$blob_name"

    $newdeployment = New-AzureDeployment -Verbose -Slot $slot -Package $blob_url -Configuration $cloudConfigLocation -label $deploymentLabel -ServiceName $serviceName -ErrorVariable err -ErrorAction continue
	if ($err.count -ne 0)
	{
		Write-Error "$(Get-Date –f $timeStampFormat) - ERROR: Deployment creating new deployment in $servicename. Script execution cancelled."
        exit 1
	}
	
    StartInstances
}

function CreateService()
{
	$svc = Get-AzureService -ServiceName $servicename -ErrorVariable a -ErrorAction silentlycontinue 
	if ($svc -eq $null)
	{
		New-AzureService -ServiceName $servicename -Location $location
	}
}

function UpgradeDeployment()
{
    write-progress -id 3 -activity "Upgrading Deployment" -Status "In progress"
    Write-Output "$(Get-Date –f $timeStampFormat) - Upgrading Deployment: In progress"

    $blobFile = Get-ChildItem $packageLocation
    $blob_name = $blobFile.Name
    $blob_url = "http://$storageAccountName.blob.core.windows.net/mydeployments/$blob_name"

	Set-AzureDeployment -Upgrade -ServiceName $serviceName -Mode Auto -Label $deploymentLabel -Package $blob_url -Configuration $cloudConfigLocation -Slot $slot -Force
	
	StartInstances
}

function StartInstances()
{
#    write-progress -id 4 -activity "Starting Instances" -status "In progress"
#    Write-Output "$(Get-Date –f $timeStampFormat) - Starting Instances: In progress"
#
#    $run = Set-AzureDeployment -Slot $slot -ServiceName $serviceName -Status Running
    $deployment = Get-AzureDeployment -ServiceName $serviceName -Slot $slot
    $oldStatusStr = @("") * $deployment.RoleInstanceList.Count

    while (-not(AllInstancesRunning($deployment.RoleInstanceList)))
    {
        $i = 1
        foreach ($roleInstance in $deployment.RoleInstanceList)
        {
            $instanceName = $roleInstance.InstanceName
            $instanceStatus = $roleInstance.InstanceStatus

			# Did the status change?
            if ($oldStatusStr[$i - 1] -ne $roleInstance.InstanceStatus)
            {
                $oldStatusStr[$i - 1] = $roleInstance.InstanceStatus
                Write-Output "$(Get-Date –f $timeStampFormat) - Starting Instance '$instanceName': $instanceStatus"
            }

            write-progress -id (4 + $i) -activity "Starting Instance '$instanceName'" -status "$instanceStatus"
            $i = $i + 1
        }

        sleep -Seconds 1

        $deployment = Get-AzureDeployment -ServiceName $serviceName -Slot $slot
    }

    $i = 1
    foreach ($roleInstance in $deployment.RoleInstanceList)
    {
        $instanceName = $roleInstance.InstanceName
        $instanceStatus = $roleInstance.InstanceStatus

        if ($oldStatusStr[$i - 1] -ne $roleInstance.InstanceStatus)
        {
            $oldStatusStr[$i - 1] = $roleInstance.InstanceStatus
            Write-Output "$(Get-Date –f $timeStampFormat) - Starting Instance '$instanceName': $instanceStatus"
        }

        write-progress -id (4 + $i) -activity "Starting Instance '$instanceName'" -status "$instanceStatus"
        $i = $i + 1
    }

	$deployment = Get-AzureDeployment -ServiceName $serviceName -Slot $slot
    $opstat = $deployment.Status

    write-progress -id 4 -activity "Starting Instances" -status $opstat
    Write-Output "$(Get-Date –f $timeStampFormat) - Starting Instances: $opstat"
}

function AllInstancesRunning($roleInstanceList)
{
    foreach ($roleInstance in $roleInstanceList)
    {
		if ($roleInstance.InstanceStatus -ne "ReadyRole")
        {
            return $false
        }
    }

    return $true
}

#cls
Write-Output "$(Get-Date –f $timeStampFormat) - Windows Azure Cloud App deploy script started."

Write-Host "Service Name = $serviceName"
Write-Host "Storage Account = $storageAccountName"
Write-Host "Storage Account Key = $storageAccountKey"
Write-Host "Configuration File = $cloudConfigLocation"
Write-Host "Package File = $packageLocation"
Write-Host "Deployment Slot = $slot"
Write-Host "Label = $deploymentLabel"
Write-Host "Timestamp Format = $timeStampFormat"
Write-Host "Delete Existing Deployment = $alwaysDeleteExistingDeployments"
Write-Host "Perform Upgrade = $enableDeploymentUpgrade"
Write-Host "Subscription Name = $selectedSubscription"
Write-Host "Subscription Id = $subscriptionId"
Write-Host "Management Certificate Thumbprint = $thumbprint"
Write-Host "Deployment Region = $location"


#Write-Output "Running Azure Imports"
#Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\*.psd1"
Import-AzurePublishSettingsFile $publishSettingsFile

# Set-AzureSubscription -DefaultSubscription $selectedSubscription

Set-AzureSubscription -CurrentStorageAccount $storageAccountName -SubscriptionName $selectedSubscription

# Clear out any previous Windows Azure subscription details in the current context (just to be safe).
# Select-AzureSubscription -Clear

# Select (by friendly name entered in the 'Set-AzureSubscription' cmdlet) the Windows Azure subscription to use.
Select-AzureSubscription $selectedSubscription


# Build the label for the deployment. Currently using the current time. Can be pretty much anything.
if ($deploymentLabel -eq $null)
{
	$currentDate = Get-Date
	$deploymentLabel = $serviceName + " - v" + $currentDate.ToUniversalTime().ToString("yyyyMMdd.HHmmss")
}
Write-Output "$(Get-Date –f $timeStampFormat) - Preparing deployment of $deploymentLabel for Subscription ID $subscriptionId."

# Execute the steps to publish the package.
Publish

$deployment = Get-AzureDeployment -ServiceName $serviceName -Slot $slot -ErrorVariable a -ErrorAction silentlycontinue 
$deploymentUrl = $deployment.Url
$deploymentId = $deployment.DeploymentId

Write-Output "$(Get-Date –f $timeStampFormat) - Creating New Deployment, Deployment ID: $deploymentId."
Write-Output "$(Get-Date –f $timeStampFormat) - Created Cloud App with URL $deploymentUrl."
Write-Output "$(Get-Date –f $timeStampFormat) - Windows Azure Cloud App deploy script finished."