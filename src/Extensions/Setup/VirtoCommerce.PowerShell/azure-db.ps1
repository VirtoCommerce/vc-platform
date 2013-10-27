# Script builds and deploys frontend azure package.
Param(  
  	[parameter(Mandatory=$true)]
		$db_servername,
        [parameter(Mandatory=$true)]
		$db_serverlogin,
		[parameter(Mandatory=$true)]
		$db_serverpassword,
		[parameter(Mandatory=$true)]
        $db_databasename,
		[parameter(Mandatory=$true)]
		$selectedSubscription,
        [parameter(Mandatory=$true)]
		$publishSettingsFile,
        [parameter(Mandatory=$true)]
		$setupModulePath
     )

function setup-database
{
    Write-Output "Started SQL Database Setup"
    remove-create-database
    run-database-scripts
    Write-Output "Finished SQL Database Setup"
}

Function remove-create-database
{
    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: setting credentials"
    $servercredential = new-object System.Management.Automation.PSCredential("$db_serverlogin", ("$db_serverpassword"  | ConvertTo-SecureString -asPlainText -Force)) 

    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: setting db context"
    $ctx = New-AzureSqlDatabaseServerContext -ServerName $db_servername -Credential $serverCredential 

    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: checking if database exists"
    $db = Get-AzureSqlDatabase $ctx -DatabaseName "$db_databasename" -ErrorAction SilentlyContinue
    if ($db -ne $null)
    {
        Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: database exists, removing ..."
        Remove-AzureSqlDatabase $ctx -DatabaseName "$db_databasename" -Force
    }
    else
    {
        Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: no database found, creating new"
        $db = New-AzureSqlDatabase $ctx -DatabaseName $db_databasename
    }
}

Function run-database-scripts
{
    $db_connectionstring = "Server=tcp:$db_servername.database.windows.net,1433;Database=$db_databasename;User ID=$db_serverlogin@$db_servername;Password=$db_serverpassword;Trusted_Connection=False;Encrypt=True;Connection Timeout=420;MultipleActiveResultSets=True"
    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: running database scripts"
    . ".\setup-database.ps1" -dbconnection $db_connectionstring -moduleFile $setupModulePath
}


#Write-Output "Running Azure Imports"
#Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\*.psd1"
Import-AzurePublishSettingsFile $publishSettingsFile

Set-AzureSubscription -DefaultSubscription $selectedSubscription

##Set-AzureSubscription -CurrentStorageAccount $storageAccountName -SubscriptionName $selectedSubscription

# Clear out any previous Windows Azure subscription details in the current context (just to be safe).
# Select-AzureSubscription -Clear

# Select (by friendly name entered in the 'Set-AzureSubscription' cmdlet) the Windows Azure subscription to use.
Select-AzureSubscription $selectedSubscription

setup-database
# See all servers in the subscription 
# Get-AzureSqlDatabaseServer


