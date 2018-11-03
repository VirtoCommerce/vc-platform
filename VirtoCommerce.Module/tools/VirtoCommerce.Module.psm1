﻿function Compress-Module
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

    if (-not $OutputDir) {
        $OutputDir = Split-Path $project.FullName -Parent
    }

    if (-not $msbuild -or -not (Test-Path $msbuild)) {
        $vswhere="${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
        if (Test-Path $vswhere) {
            $vspath = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
            if ($vspath) {
                $msbuild = Join-Path $vspath 'MSBuild\15.0\Bin\MSBuild.exe'
            }
        }
    }
    if (-not $msbuild -or -not (Test-Path $msbuild)) {
        $msbuild = "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
    }
    if (-not (Test-Path $msbuild)) {
        $msbuild = "${env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
    }
    if (-not (Test-Path $msbuild)) {
        $msbuild = "${env:windir}\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
    }

    $newGuid = [Guid]::NewGuid()
    $tempDirName = "_vc_deploy\$newGuid"
    $tempPath = [System.IO.Path]::GetTempPath()
    $tempDir = Join-Path $tempPath $tempDirName
    $modulesDir = "$tempDir\_PublishedWebsites"
    $packagesDir = $OutputDir

    & $msbuild $project.FullName /nologo /verbosity:m /t:PackModule /p:Configuration=Release /p:Platform=AnyCPU /p:DebugType=none /p:AllowedReferenceRelatedFileExtensions=.xml "/p:OutputPath=$tempDir" "/p:VCModulesOutputDir=$modulesDir" "/p:VCModulesZipDir=$packagesDir"
}

Export-ModuleMember Compress-Module
