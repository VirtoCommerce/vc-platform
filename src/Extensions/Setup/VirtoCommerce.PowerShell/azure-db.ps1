# Script builds and deploys frontend azure package.
Param(  
	  	[parameter(Mandatory=$false)]
		$db_recreate = $true,
	  	[parameter(Mandatory=$true)]
		$db_customFolder,
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
		$setupModulePath,
	  	[parameter(Mandatory=$false)]
		$db_sampledata = $true
     )

function setup-database
{
    Write-Output "$(Get-Date -f $timeStampFormat) Started SQL Database Setup"
    remove-create-database
    Write-Output "$(Get-Date -f $timeStampFormat) Finished SQL Database Setup"
}

Function remove-create-database
{
    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: setting credentials"
    $servercredential = new-object System.Management.Automation.PSCredential("$db_serverlogin", ("$db_serverpassword"  | ConvertTo-SecureString -asPlainText -Force)) 

    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: setting db context"
    $ctx = New-AzureSqlDatabaseServerContext -ServerName $db_servername -Credential $serverCredential 

    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: checking if database exists"
    $db = Get-AzureSqlDatabase $ctx -DatabaseName "$db_databasename" -ErrorAction SilentlyContinue

	$isnewdb = $false # tracks if we created new db

    if ($db -ne $null) # remove existing database if one already exists to start from scratch
    {
		if($db_recreate)  # if we running complete recreate, then remove the database
		{
			Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: database exists, removing ..."
			Remove-AzureSqlDatabase $ctx -DatabaseName "$db_databasename" -Force
			$isnewdb = $true
		}
    }
	else
	{
		$isnewdb = $true
	}

	<# no need to create db, it is created by database scripts cmdlets
    else # no database, create new one
    {
        Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: no database found, creating new"
        $db = New-AzureSqlDatabase $ctx -DatabaseName $db_databasename
    }
	#>

	# run db fresh install scripts
	if($isnewdb)
	{
		run-database-scripts
	}

	run-database-custom-scripts -db_created $isnewdb
}

function run-database-custom-scripts
{
    Param(
  	    [parameter(Mandatory=$true)]
		$db_created
     )  

    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: running database custom scripts"
	. ".\db-custom.ps1" -db_created $db_created -scriptsFolder $db_customFolder -db_servername "tcp:$db_servername.database.windows.net,1433" -db_serverlogin $db_serverlogin -db_serverpassword $db_serverpassword -db_databasename $db_databasename
}

Function run-database-scripts
{
    $db_connectionstring = "Server=tcp:$db_servername.database.windows.net,1433;Database=$db_databasename;User ID=$db_serverlogin@$db_servername;Password=$db_serverpassword;Trusted_Connection=False;Encrypt=True;Connection Timeout=420;MultipleActiveResultSets=True"
    Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: running database scripts"
    . ".\setup-database.ps1" -dbconnection $db_connectionstring -moduleFile $setupModulePath -useSample $db_sampledata
}

#Write-Output "Running Azure Imports"
Import-AzurePublishSettingsFile $publishSettingsFile

#Set-AzureSubscription -DefaultSubscription $selectedSubscription

Set-AzureSubscription -SubscriptionName $selectedSubscription

# Clear out any previous Windows Azure subscription details in the current context (just to be safe).
# Select-AzureSubscription -Clear

# Select (by friendly name entered in the 'Set-AzureSubscription' cmdlet) the Windows Azure subscription to use.
Select-AzureSubscription $selectedSubscription

setup-database
# See all servers in the subscription 
# Get-AzureSqlDatabaseServer


