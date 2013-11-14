# variables
$installutil = "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe"
$datafolder = Split-Path -Parent $MyInvocation.MyCommand.Path
$datafolder = Split-Path -Parent $datafolder
$datafolder = Split-Path -Parent $datafolder
$binaryPath = "..\..\..\Presentation\Services\ScheduleService\bin\Debug\ScheduleService.exe"
$servicePath = Join-Path $datafolder $binaryPath
$serviceName = "VirtoCommerce.ScheduleService"

# init
if (Get-Service $serviceName -ErrorAction SilentlyContinue)
{
	# using WMI to remove Windows service because PowerShell does not have CmdLet for this
    $serviceToRemove = Get-WmiObject -Class Win32_Service -Filter "name='$serviceName'"
    $serviceToRemove.delete()
    "service removed"
}
else
{
	# just do nothing
    "service does not exists"
}

"installing service"
# creating widnows service
New-Service -name $serviceName -binaryPathName $servicePath 
"installation completed"

##################################

Write-Host "Press any key to continue ..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

