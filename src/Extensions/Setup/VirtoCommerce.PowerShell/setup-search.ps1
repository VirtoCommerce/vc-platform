# variables
Param( 
	$dbconnection = "Server=(local);Database=VirtoCommerce;Integrated Security=True;MultipleActiveResultSets=True",
	$searchconnection = "server=virtocommerce;scope=default;key=128EE67AC838DF328B3BEC97ADB1A1B1;provider=azuresearch",
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

#add console listener
$listener = new-object "system.diagnostics.consoletracelistener"
[System.Diagnostics.Debug]::Listeners.Clear();
[System.Diagnostics.Debug]::Listeners.Add($listener)

#index catalog
echo "Update-Virto-Search-Index ***** ***** ***** ***** ***** ***** "
Update-Virto-Search-Index -connection $searchconnection -dbconnection $dbconnection -rebuild -verbose

#set ok
##################################