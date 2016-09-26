function Compress-Module
{
    Param(
        [string] $ProjectName,
        [string] $OutputDir
    )

    if ($ProjectName) {
        $project = Get-Project -Name $ProjectName
    } else {
        $project = Get-Project
    }

    if (-Not $OutputDir) {
        $OutputDir = Split-Path $project.FullName -Parent
    }

    $msbuild = "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
    if (-Not (Test-Path $msbuild)) {
        $msbuild = "${env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
    }
    if (-Not (Test-Path $msbuild)) {
        $msbuild = "${env:windir}\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
    }

    $newGuid = [Guid]::NewGuid()
    $tempDirName = "_vc_module_$newGuid"
    $tempPath = [System.IO.Path]::GetTempPath()
    $tempDir = Join-Path $tempPath $tempDirName
    $modulesDir = "$tempDir\_PublishedWebsites"
    $packagesDir = $OutputDir

    & $msbuild $project.FullName /nologo /verbosity:m /t:PackModule /p:Configuration=Release /p:Platform=AnyCPU /p:DebugType=none /p:AllowedReferenceRelatedFileExtensions=.xml "/p:OutputPath=$tempDir" "/p:VCModulesOutputDir=$modulesDir" "/p:VCModulesZipDir=$packagesDir"
}

Export-ModuleMember Compress-Module
