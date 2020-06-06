[Xml]$buildPropsXml = Get-Content -Path 'Directory.Build.Props'
      
$versionPrefixNode = $buildPropsXml.SelectNodes('Project/PropertyGroup/VersionPrefix') | Select-Object -First 1
$versionSuffixNode = $buildPropsXml.SelectNodes('Project/PropertyGroup/VersionSuffix') | Select-Object -First 1
$versionPrefix = "$($versionPrefixNode.InnerText.Trim())"
if (-not ([string]::IsNullOrEmpty($versionSuffixNode.InnerText))) 
{ 
  $versionSuffix = "-$($versionSuffixNode.InnerText.Trim())" 
}  

$sha = (git rev-parse HHEAD).Substring(0, 8)
  
$version = "$($versionPrefix)$versionSuffix-$($sha)"

Write-Output "::set-output name=tag::$version"