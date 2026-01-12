# Define the target framework
$targetFramework = "net10.0"
$versionPrefix = "3.1000.0"
$platformVersion = "3.1000.0"

# Read the predefined versions from vc-references.json or create an empty object
$predefinedVersions =  @{
	"AutoFixture" = "4.18.1"
	"coverlet.collector" = "6.0.4"
	"FluentAssertions" = "8.8.0"
	"FluentValidation" = "12.1.1"
	"Hangfire" = "1.8.22"
	"Microsoft.AspNetCore.Authentication.OpenIdConnect" = "10.0.1"
	"Microsoft.AspNetCore.Mvc.NewtonsoftJson" = "10.0.1"
	"Microsoft.EntityFrameworkCore" = "10.0.1"
	"Microsoft.EntityFrameworkCore.Design" = "10.0.1"
	"Microsoft.EntityFrameworkCore.InMemory" = "10.0.1"
 	"Microsoft.EntityFrameworkCore.Relational" = "10.0.1"
	"Microsoft.EntityFrameworkCore.SqlServer" = "10.0.1"
	"Microsoft.EntityFrameworkCore.Tools" = "10.0.1"
	"Microsoft.Extensions.Configuration.UserSecrets" = "10.0.1"
	"Microsoft.Extensions.DependencyModel" = "10.0.1"
	"Microsoft.Extensions.Logging.Abstractions" = "10.0.1"
	"Microsoft.NET.Test.Sdk" = "18.0.1"
	"Microsoft.SourceLink.GitHub" = "8.0.0"
	"MockQueryable.Moq" = "10.0.1"
	"Moq" = "4.20.72"
	"Npgsql" = "10.0.0"
	"Npgsql.EntityFrameworkCore.PostgreSQL" = "10.0.0"
	"OpenIddict.AspNetCore" = "7.2.0"
	"OpenIddict.EntityFrameworkCore" = "7.2.0"
	"Polly" = "9.0.0"
	"Pomelo.EntityFrameworkCore.MySql" = "9.0.0"
	"Swashbuckle.AspNetCore.SwaggerGen" = "10.1.0"
	"xunit" = "2.9.3"
	"xunit.runner.visualstudio" = "3.1.5"
}

function Save-File ($xml, $filePath) {
	$utf8WithoutBom = New-Object System.Text.UTF8Encoding($false)
	Write-Host ("filename: " + $filePath)
	$xmlWriterSettings = New-Object System.Xml.XmlWriterSettings
	$xmlWriterSettings.Indent = $true
	$xmlWriterSettings.Encoding = $utf8WithoutBom
	$xmlWriterSettings.OmitXmlDeclaration = $filePath.EndsWith(".csproj")

	$xmlWriter = [System.Xml.XmlWriter]::Create($filePath, $xmlWriterSettings)
	$xml.Save($xmlWriter)
	$xmlWriter.Close()
	# Add-Content -Path $filePath -Value "`n"
}

function Load-File($filePath) {
	$xmlDoc = New-Object System.Xml.XmlDocument
	$xmlDoc.PreserveWhitespace = $true
	$xmlDoc.Load($filePath)
	return $xmlDoc
}

function Display-Version-Change ($moduleName, $oldVersion, $newVersion) {
	Write-Host "$(($moduleName + ":").PadRight(60, ' ')) $($oldVersion.PadLeft(30, ' ')) ==> $($newVersion)"
}

# Function to update PackageReference version
function Update-PackageReference ($projectFile, $packageName, $version) {
	$xml = [xml](Load-File $projectFile)
	$packageReference = $xml.Project.ItemGroup.PackageReference | Where-Object { $_.Include -eq $packageName }

	if ($packageReference -ne $null) {
		$packageReference.Version = $version
		Save-File $xml $projectFile
	}
}

# Function to update TargetFramework
function Update-TargetFramework ($projectFile, $targetFramework) {
	$xml = [xml](Load-File $projectFile)

	# Find the first PropertyGroup containing TargetFramework or create one
	$propertyGroup = $xml.Project.PropertyGroup | Where-Object { $_.TargetFramework -ne $null } | Select-Object -First 1

	# Update or create TargetFramework element
	$targetFrameworkElement = $propertyGroup.SelectSingleNode("TargetFramework")

	# Update the InnerText of TargetFramework
	$targetFrameworkElement.InnerText = $targetFramework
	Save-File $xml $projectFile
}

# Function to update packages to the latest version
function Update-Latest-Packages ($projectFile) {
	$xml = [xml](Load-File $projectFile)
	
	$xml.Project.ItemGroup.PackageReference | Where-Object { $_.Version -ne $null } | ForEach-Object {
		$packageName = $_.Include
		$installedVersion = $_.Version
		$item = $_

		$version = $predefinedVersions.$packageName
	    if (-not $version) {
		    try {
			    $latestVersion = (Find-Package $packageName -Source https://www.nuget.org/api/v2).Version
			
			    if ($packageName.StartsWith("VirtoCommerce") -or ($versionPrefix.Contains("alpha") -or [System.Version]$versionPrefix -gt [System.Version]$latestVersion)) {
				    $latestVersion = $versionPrefix
			    }
			    $predefinedVersions["$packageName"] = $latestVersion
			    $version = $latestVersion
		    } catch {
			    # ignore beta versions
		    }
	    }

		$_.Version = $version

		Display-Version-Change $packageName $installedVersion $version
	}
	
	Save-File $xml $projectFile
}

# Function to find all .csproj files in a folder and its subfolders
function Find-Csproj-Files ($folder) {
	Get-ChildItem -Path $folder -Recurse -Filter *.csproj | Where-Object { $_.Extension -eq '.csproj' }
}

function Find-File($folder, $fileName) {
	$file = Get-ChildItem -Path $folder -Recurse -Filter $fileName | Select-Object -First 1
	return $file 
}

# Function
function Update-Directory-Build-Props($directoryBuildPropsFile)
{
	$xml = [xml](Load-File $directoryBuildPropsFile)

	$versionPrefixPropertyGroup = $xml.Project.PropertyGroup | Where-Object { $_.VersionPrefix -ne $null } | Select-Object -First 1
	$VersionPrefixElement = $versionPrefixPropertyGroup.SelectSingleNode("VersionPrefix")
	$VersionPrefixElement.InnerText = $versionPrefix

	Save-File $xml $directoryBuildPropsFile
}

# Function 
function Update-Module-Manifest($moduleManifestFile) {
	# it's better to load https://raw.githubusercontent.com/VirtoCommerce/vc-modules/master/modules_v3_net8.json and get latest version there
	
	$xml = [xml](Load-File $moduleManifestFile)

	$xml.module.version = $versionPrefix
	$xml.module.platformVersion = $platformVersion

	$dependencies = $xml.GetElementsByTagName("dependency")
	foreach ($dependency in $dependencies) {
		$newVersion = "3.1000.0"
		Display-Version-Change $dependency.id $dependency.version $newVersion
		$dependency.SetAttribute("version", $newVersion)
	}

	Save-File $xml $moduleManifestFile
}

# Main script
$repFolder = "."
$srcFolder = "src"
$csprojFiles = Find-Csproj-Files $repFolder

foreach ($csprojFile in $csprojFiles) {
	Write-Host """$($csprojFile.Name)""" -ForegroundColor Yellow

	# Step 1: Change TargetFramework
	Write-Host "Updating TargetFramework"
	Update-TargetFramework $csprojFile.FullName $targetFramework

	# Step 2: Update packages to the latest version
	Write-Host "Updating other packages to the latest version"
	Update-Latest-Packages $csprojFile.FullName
}

# Step 3: Update packages to the latest version
Write-Host "Updating Directory.Build.props"
$directoryBuildPropsFile = Find-File $repFolder "Directory.Build.props"
Update-Directory-Build-Props $directoryBuildPropsFile.FullName


# Step 3: Update module.manifest to the latest version
Write-Host "Updating module.manifest"
$moduleManifestFile = Find-File $srcFolder "module.manifest"
Update-Module-Manifest $moduleManifestFile.FullName


Write-Host "Script completed successfully."
