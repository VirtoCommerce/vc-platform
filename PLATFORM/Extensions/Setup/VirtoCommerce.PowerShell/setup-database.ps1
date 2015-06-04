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
Import-Module $moduleFile

##################################

echo $dbconnection

#create security
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoSecurityDatabase"
Publish-VirtoSecurityDatabase -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create customer
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoCustomerDatabase"
Publish-VirtoCustomerDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create global settings
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoAppConfigDatabase"
Publish-VirtoAppConfigDatabase -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create catalog
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoCatalogDatabase"
Publish-VirtoCatalogDatabase -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create importing
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoImportDatabase"
Publish-VirtoImportDatabase -c $dbconnection -data $datafolder -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create Review
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoReviewDatabase"
Publish-VirtoReviewDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create store
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoStoreDatabase"
Publish-VirtoStoreDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create marketing
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoMarketingDatabase"
Publish-VirtoMarketingDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create inventory
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoInventoryDatabase"
Publish-VirtoInventoryDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create log
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoLogDatabase"
Publish-VirtoLogDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create orders
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoOrderDatabase"
Publish-VirtoOrderDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
if(! $?)
{
	throw "Deployment failed"
}

#create search
Write-Output "$(Get-Date –f $timeStampFormat) - SQL Database Deployment: Publish-VirtoSearchDatabase"
Publish-VirtoSearchDatabase -c $dbconnection -sample:$useSample -reduced:$reducedSample -verbose
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