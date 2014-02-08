<#
    Script runs custom db scripts depending on if db was created or simply updated, scripts are ran from the specified folder

    The scripts are run according to these rules:
    1. Scripts run in alphabetical order.
    2. Script containing keyword ".oncreate." run only when db is created.
    3. Script containing keyword ".always." run always, meaning on create and update.
    4. Script containing keyword ".update." run only on db update.
    5. Script containing keyword ".master." run against master database, all other script run against passed db name.
    6. Script split by taking into account GO statements into batches.
    7. Create login scripts must not include ";" at the end of the line.
    8. Script containing keyword ".silentfail." will fail silently without generating any exceptions.
    9. Multiple Folders can be specified using comma.

    Name examples:

    100.drop_sql_logins.silentfail.master.oncreate.sql - will fail silently and will run on master database on db creation only
    300.grant_sql_permissions_in_virtodb.always.sql - will always run, after the script before on default db


#>

Param(  
	[parameter(Mandatory=$false)]
    $db_created = $false, 
    [parameter(Mandatory=$true)]
	$scriptsFolder,
  	[parameter(Mandatory=$true)]
	$db_servername,
    [parameter(Mandatory=$true)]
	$db_serverlogin,
	[parameter(Mandatory=$true)]
	$db_serverpassword,
	[parameter(Mandatory=$true)]
    $db_databasename
)

function run
{
    $folders = $scriptsFolder -split ","

    # check if any folders contain sql files, if not skip execution
    $found = $false
    foreach($folder in $folders){
        $path = "$folder\*.sql"
        Write-Output $path
        if (Test-Path $path)
        {
            $found = $true
        }
    }

    if(!$found)
    {
        Write-Output "$(Get-Date -f $timeStampFormat) ""$scriptsFolder"" doesn't exist or doesn't contain any sql file, so we skipping running custom update scripts"
    }
    else
    {
		# must have the module setup, included with sql 2012
		Push-Location; Import-Module sqlps; Pop-Location

        if($db_created)
        {
           run-oncreate -folders $folders
        }
        else
        {
           run-onupdate -folders $folders
        }
    }
}

function run-oncreate($folders)
{
    Write-Output "$(Get-Date -f $timeStampFormat) Running on db create sql scripts"
    $folders | Get-ChildItem -File -Recurse -i "*.oncreate.*", "*.always.*sql" | sort -property Name | sqlfile -db_servername $db_servername -db_serverlogin $db_serverlogin -db_serverpassword $db_serverpassword -db_databasename $db_databasename
}

function run-onupdate($folders)
{
    Write-Output "$(Get-Date -f $timeStampFormat) Running on db update sql scripts"
    $folders | Get-ChildItem -File -Recurse -i "*.update.*", "*.always.*sql" | sort -property Name | sqlfile -db_servername $db_servername -db_serverlogin $db_serverlogin -db_serverpassword $db_serverpassword -db_databasename $db_databasename
}

function sqlfile
{
    Param(
        [parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [PSObject[]]$filename,
  	    [parameter(Mandatory=$true)]
		$db_servername,
        [parameter(Mandatory=$true)]
		$db_serverlogin,
		[parameter(Mandatory=$true)]
		$db_serverpassword,
		[parameter(Mandatory=$true)]
        $db_databasename
     )    

    process {   
        
        # in this script we only need to check which db we execute the script against
        if($filename.Name.Contains('.master.'))
        {
            Write-Output "$(Get-Date -f $timeStampFormat) Running ""$filename"" script on ""master"" db"
            if($filename.Name.Contains('.silentfail.'))
            {
                Get-SqlBatchesFromFile $filename | ForEach-Object { Invoke-Sqlcmd -query "$_" -ServerInstance $db_servername -Database 'master' -Username $db_serverlogin -Password $db_serverpassword -Verbose -ErrorAction SilentlyContinue }
            }
            else
            {
                Get-SqlBatchesFromFile $filename | ForEach-Object { Invoke-Sqlcmd -query "$_" -ServerInstance $db_servername -Database 'master' -Username $db_serverlogin -Password $db_serverpassword -Verbose }
            }
        }
        else
        {
            Write-Output "$(Get-Date -f $timeStampFormat) Running script ""$filename"" on ""$db_databasename"" db"
            if($filename.Name.Contains('.silentfail.'))
            {
                Get-SqlBatchesFromFile $filename | ForEach-Object { Invoke-Sqlcmd -query "$_" -ServerInstance $db_servername -Database $db_databasename -Username $db_serverlogin -Password $db_serverpassword -Verbose -ErrorAction SilentlyContinue }
            }
            else
            {
                Get-SqlBatchesFromFile $filename | ForEach-Object { Invoke-Sqlcmd -query "$_" -ServerInstance $db_servername -Database $db_databasename -Username $db_serverlogin -Password $db_serverpassword -Verbose }
            }
        }
    }
}

filter Invoke-ExecuteBatch {
    Write-Host "Batch part ==== start"
    Write-Host $_
    Write-Host "Batch part ==== end"   
}

function Get-SqlBatchesFromFile {
    param($file)

    $accumulate = @()

    $containsGO = $false
    foreach($line in (get-content $file)){
      
        if($line -eq "GO") {        
            $accumulate -join "`r`n"
            $accumulate = @()
            $containsGO = $true
        } else {
            if($line.Trim().Length -gt 0) # ignore empty strings
            {
                $accumulate+=$line.TrimEnd(";")
            }
        }
    }

    if(!$containsGO)
    {
        if($accumulate.Length -gt 0) # handle empty arrays, don't return them
        {
            $accumulate -join "`r`n"
        }
    }
}

# run the script
run