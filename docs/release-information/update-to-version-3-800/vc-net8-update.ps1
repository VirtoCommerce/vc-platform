# Define the target framework
$targetFramework = "net8.0"
$versionPrefix = "3.800.0"
$platformVersion = "3.800.0"

# Read the predefined versions from vc-references.json or create an empty object
$predefinedVersions =  @{
	"AutoFixture" = "4.18.1"
	"Avalara.AvaTax" = "23.11.0"
	"BenchmarkDotNet" = "0.13.11"
	"coverlet.collector" = "6.0.0"
	"CsvHelper" = "30.0.1"
	"DotLiquid" = "2.2.692"
	"FluentAssertions" = "6.12.0"
	"FluentValidation" = "11.8.1"
	"Google.Apis.YouTube.v3" = "1.64.0.3205"
	"GraphQL" = "4.6.0"
	"GraphQL.Authorization" = "4.0.0"
	"GraphQL.Relay" = "0.6.2"
	"GraphQL.Server.Transports.AspNetCore" = "5.0.2"
	"GraphQL.Server.Transports.AspNetCore.NewtonsoftJson" = "5.0.2"
	"Hangfire" = "1.8.6"
	"MailKit" = "4.3.0"
	"Microsoft.ApplicationInsights.AspNetCore" = "2.22.0"
	"Microsoft.ApplicationInsights.PerfCounterCollector" = "2.22.0"
	"Microsoft.AspNetCore.Authentication.OpenIdConnect" = "8.0.0"
	"Microsoft.AspNetCore.Mvc.NewtonsoftJson" = "8.0.0"
	"Microsoft.EntityFrameworkCore" = "8.0.0"
	"Microsoft.EntityFrameworkCore.Design" = "8.0.0"
	"Microsoft.EntityFrameworkCore.InMemory" = "8.0.0"
 	"Microsoft.EntityFrameworkCore.Relational" = "8.0.0"
	"Microsoft.EntityFrameworkCore.SqlServer" = "8.0.0"
	"Microsoft.EntityFrameworkCore.Tools" = "8.0.0"
	"Microsoft.Extensions.Configuration.UserSecrets" = "8.0.0"
	"Microsoft.Extensions.DependencyModel" = "8.0.0"
	"Microsoft.Extensions.Logging.Abstractions" = "8.0.0"
	"Microsoft.NET.Test.Sdk" = "17.8.0"
	"Microsoft.SourceLink.GitHub" = "8.0.0"
	"MockQueryable.Moq" = "7.0.0"
	"Moq" = "4.20.70"
	"MSTest.TestAdapter" = "3.1.1"
	"MSTest.TestFramework" = "3.1.1"
	"Npgsql" = "8.0.1"
	"Npgsql.EntityFrameworkCore.PostgreSQL" = "8.0.0"
	"OpenIddict.AspNetCore" = "4.10.1"
	"OpenIddict.EntityFrameworkCore" = "4.10.1"
	"Polly" = "8.2.0"
	"Pomelo.EntityFrameworkCore.MySql" = "8.0.0-beta.2"
	"Scriban" = "5.9.0"
	"Sendgrid" = "9.28.1"
	"Serilog.Sinks.ApplicationInsights" = "4.0.0"
	"Swashbuckle.AspNetCore.SwaggerGen" = "6.5.0"
	"System.Linq.Async" = "6.0.1"
	"Twilio" = "6.15.2"
	"xunit" = "2.6.2"
	"xunit.runner.console" = "2.6.2"
	"xunit.runner.visualstudio" = "2.5.4"
	"YamlDotNet" = "13.7.1"
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
		$newVersion = "3.800.0"
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
