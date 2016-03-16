param(
  [string] [Parameter(Mandatory=$true)] $ResourceGroupLocation,
  [string] $ResourceGroupName,  
  [string] $SubscriptionName, 
  [string] $azureAccount,
  [string] $azureAccountPassword,
  [switch] $UploadArtifacts,
  [string] $StorageAccountName,
  [string] $StorageAccountResourceGroupName,
  [string] $StorageContainerName = $ResourceGroupName.ToLowerInvariant() + '-stageartifacts',
  [string] $TemplateFile = 'Templates\dev.json',
  [string] $TemplateParametersFile = 'Templates\dev.params.json',
  [string] $ArtifactStagingDirectory = '..\..\artifacts\staging',
  [string] $AzCopyPath = 'Tools\AzCopy.exe',
  [string] $DSCSourceFolder = '..\..\artifacts\wwwroot\'
)

. "$PSScriptRoot\Modules.ps1"

Import-Module Azure -ErrorAction SilentlyContinue

try {

} catch { }

Set-StrictMode -Version 3

$OptionalParameters = New-Object -TypeName Hashtable
$TemplateFile = [System.IO.Path]::Combine($PSScriptRoot, $TemplateFile)
$TemplateParametersFile = [System.IO.Path]::Combine($PSScriptRoot, $TemplateParametersFile)

$templateParams = Get-Settings $TemplateParametersFile

$ResourceGroupLocation = $templateParams.parameters.siteLocation.value

# Create new azure resource group.
#The AzureResourceManager module requires Add-AzureAccount. A Publish Settings file is not sufficient.
$secpassword = ConvertTo-SecureString $AzureAccountPassword -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential ($AzureAccount, $secpassword)
Add-AzureRmAccount -Credential $credentials 

# To select a default subscription for your current session
Get-AzureRmSubscription –SubscriptionName $SubscriptionName | Select-AzureRmSubscription

#Creating new AzureResourceGroup
Write-Host "Creating '$ResourceGroupName' Azure Resource group..."

# Create or update the resource group using the specified template file and template parameters file
New-AzureRMResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation -Verbose -Force -ErrorAction Stop

if ($UploadArtifacts) {
    # Create storage account if one doesn't exist

    $artifactStorageAccountExists = Find-AzureRmResource -ResourceType Microsoft.Storage/storageAccounts -ResourceGroupNameContains $ResourceGroupName | where {$_.ResourceName -eq $StorageAccountName}

    if ($artifactStorageAccountExists -eq $null)
    {
       # create the storage account.
       New-AzureRmStorageAccount -ResourceGroupName $ResourceGroupName -AccountName $StorageAccountName -Location $ResourceGroupLocation -Type "Standard_GRS"
    }

    # Convert relative paths to absolute paths if needed
    $AzCopyPath = [System.IO.Path]::Combine($PSScriptRoot, $AzCopyPath)
    $ArtifactStagingDirectory = [System.IO.Path]::Combine($PSScriptRoot, $ArtifactStagingDirectory)
    $DSCSourceFolder = [System.IO.Path]::Combine($PSScriptRoot, $DSCSourceFolder)

    if (!(Test-Path $ArtifactStagingDirectory)) {
        New-Item -ItemType Directory -Force -Path $ArtifactStagingDirectory
    }

    Set-Variable ArtifactsLocationName '_artifactsLocation' -Option ReadOnly
    Set-Variable ArtifactsLocationSasTokenName '_artifactsLocationSasToken' -Option ReadOnly

    $OptionalParameters.Add($ArtifactsLocationName, $null)
    $OptionalParameters.Add($ArtifactsLocationSasTokenName, $null)

    # Parse the parameter file and update the values of artifacts location and artifacts location SAS token if they are present
    $JsonContent = Get-Content $TemplateParametersFile -Raw | ConvertFrom-Json
    $JsonParameters = $JsonContent | Get-Member -Type NoteProperty | Where-Object {$_.Name -eq "parameters"}

    if ($JsonParameters -eq $null) {
        $JsonParameters = $JsonContent
    }
    else {
        $JsonParameters = $JsonContent.parameters
    }

    $JsonParameters | Get-Member -Type NoteProperty | ForEach-Object {
        $ParameterValue = $JsonParameters | Select-Object -ExpandProperty $_.Name

        if ($_.Name -eq $ArtifactsLocationName -or $_.Name -eq $ArtifactsLocationSasTokenName) {
            $OptionalParameters[$_.Name] = $ParameterValue.value
        }
    }

    $StorageAccountKey = (Get-AzureRMStorageAccountKey -ResourceGroupName $ResourceGroupName -Name $StorageAccountName).Key1

    $StorageAccountContext = (Get-AzureRmStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName).Context

    # Create DSC configuration archive
    if (Test-Path $DSCSourceFolder) {
    Add-Type -Assembly System.IO.Compression.FileSystem
        $ArchiveFile = Join-Path $ArtifactStagingDirectory "package.zip"
        Remove-Item -Path $ArchiveFile -ErrorAction SilentlyContinue
        [System.IO.Compression.ZipFile]::CreateFromDirectory($DSCSourceFolder, $ArchiveFile)
    }

    # Generate the value for artifacts location if it is not provided in the parameter file
    $ArtifactsLocation = $OptionalParameters[$ArtifactsLocationName]
    if ($ArtifactsLocation -eq $null) {
        $ArtifactsLocation = $StorageAccountContext.BlobEndPoint + $StorageContainerName
        $OptionalParameters[$ArtifactsLocationName] = $ArtifactsLocation
    }

    # Use AzCopy to copy files from the local storage drop path to the storage account container   
    & $AzCopyPath """$ArtifactStagingDirectory""", $ArtifactsLocation, "/DestKey:$StorageAccountKey", "/S", "/Y", "/V", "/Z:$env:LocalAppData\Microsoft\Azure\AzCopy\$ResourceGroupName"
    if ($LASTEXITCODE -ne 0) { return }

    # Generate the value for artifacts location SAS token if it is not provided in the parameter file
    $ArtifactsLocationSasToken = $OptionalParameters[$ArtifactsLocationSasTokenName]
    if ($ArtifactsLocationSasToken -eq $null) {
       # Create a SAS token for the storage container - this gives temporary read-only access to the container
       $ArtifactsLocationSasToken = New-AzureStorageContainerSASToken -Container $StorageContainerName -Context $StorageAccountContext -Permission r -ExpiryTime (Get-Date).AddHours(4)
       $ArtifactsLocationSasToken = ConvertTo-SecureString $ArtifactsLocationSasToken -AsPlainText -Force
       $OptionalParameters[$ArtifactsLocationSasTokenName] = $ArtifactsLocationSasToken
    }
}

Test-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile $TemplateFile -TemplateParameterFile $TemplateParametersFile @OptionalParameters -ErrorAction Stop

# Create or update the resource group using the specified template file and template parameters file
New-AzureRMResourceGroupDeployment -Name ((Get-ChildItem $TemplateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile $TemplateFile `
    -TemplateParameterFile $TemplateParametersFile `
    @OptionalParameters `
    -Force -Verbose

Write-Host "'$ResourceGroupName' Azure Resource group has been successfully created."