# variables
Param( 
	$dbconnection = "Server=(local);Database=VirtoCommerce;Integrated Security=True;",
    $datafolder,
    $moduleFile,
    $useSample = $true,
	$reducedSample = $false
)

if (!$datafolder) {
	$datafolder = Split-Path -Parent $MyInvocation.MyCommand.Path
}


if (!$moduleFile) {
	$moduleFile = ${datafolder} + "\bin\Debug\VirtoCommerce.PowerShell.dll"
}

echo $moduleFile

#$datafolder = Split-Path -Parent $MyInvocation.MyCommand.Path
Import-Module -DisableNameChecking $moduleFile

##################################

echo $dbconnection

#create security
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Security-Database"
Publish-Virto-Security-Database -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create customer
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Customer-Database"
Publish-Virto-Customer-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create global settings
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-AppConfig-Database"
Publish-Virto-AppConfig-Database -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create catalog
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Catalog-Database"
Publish-Virto-Catalog-Database -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create importing
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Import-Database"
Publish-Virto-Import-Database -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create Review
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Review-Database"
Publish-Virto-Review-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create store
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Store-Database"
Publish-Virto-Store-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create marketing
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Marketing-Database"
Publish-Virto-Marketing-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create inventory
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Inventory-Database"
Publish-Virto-Inventory-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create log
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Log-Database"
Publish-Virto-Log-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create orders
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Order-Database"
Publish-Virto-Order-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create search
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-Virto-Search-Database"
Publish-Virto-Search-Database -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#set ok

#create sqldependency
#Write-Host "Create the ASPNETDB SQL Server database for SqlDependency"
#$frameworkDir = $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory())
#Set-Alias aspnet_regsql (Join-Path $frameworkDir "aspnet_regsql.exe")
#aspnet_regsql -C $dbconnection -ed -et -t DynamicContentItemProperty
#aspnet_regsql -C $dbconnection -ed -et -t PromotionReward



##################################

# cleanup

# Write-Host "Press any key to continue ..."
# $x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")