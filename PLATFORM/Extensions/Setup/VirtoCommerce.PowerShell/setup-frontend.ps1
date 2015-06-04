# variables
Param( 
	$dbconnection = "Server=(local);Database=VirtoCommerce;Integrated Security=True;",
	$location = "..\..\..\Presentation\FrontEnd\StoreWebApp\",
    $moduleFile
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

#initialize configs
echo "Initialize-VirtoFrontEndConfigs ***** ***** ***** ***** ***** ***** "
Initialize-VirtoFrontEndConfigs -location $location -dbconnection $dbconnection -indexes localhost:9200

#set ok

##################################


Write-Host "Press any key to continue ..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")