param (
  [Parameter(Position=0, Mandatory=$true)]
  [string]$SHA
)

Xml]$buildPropsXml = Get-Content -Path 'Directory.Build.Props'
      
$versionPrefixNode = $buildPropsXml.SelectNodes('Project/PropertyGroup/VersionPrefix') | Select-Object -First 1
$versionSuffixNode = $buildPropsXml.SelectNodes('Project/PropertyGroup/VersionSuffix') | Select-Object -First 1
$versionPrefix = "$($versionPrefixNode.InnerText.Trim())"
if (-not ([string]::IsNullOrEmpty($versionSuffixNode.InnerText))) 
{ 
  $versionSuffix = "-$($versionSuffixNode.InnerText.Trim())" 
}
  
$version = "$($versionPrefix)$versionSuffix-$($SHA.Substring(0, 8))"

Write-Output "::set-output name=tag::$version"