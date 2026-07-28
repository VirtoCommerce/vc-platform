<#
.SYNOPSIS
    Grabs the EF Core migrations that a VirtoCommerce platform folder would apply on startup
    (platform + security + all installed modules) and writes them to .sql files, without applying anything.

.DESCRIPTION
    Thin wrapper around the VirtoCommerce.Platform.MigrationScripter companion tool. It optionally
    overrides the database provider and connection string via environment variables (so appsettings.json
    is left untouched and the values do not leak into your shell), runs the tool against the deployed
    platform folder, and can zip the output.

    The tool builds the real platform host up to Build() only: no web server starts and no migrations
    are applied. Per-context .sql files plus a combined _combined.sql are produced. By default, contexts
    that are already up to date produce no file (use -IncludeEmpty to write them too).

.PARAMETER PlatformPath
    Path to the deployed platform folder (contains appsettings.json, ./modules, ./app_data).

.PARAMETER DatabaseProvider
    Optional override for DatabaseProvider (SqlServer | PostgreSql | MySql). Sets the DatabaseProvider env var.

.PARAMETER ConnectionString
    Optional override for the VirtoCommerce connection string. Sets the ConnectionStrings__VirtoCommerce env var.

.PARAMETER OutputPath
    Output folder for the generated .sql files. Defaults to <PlatformPath>/migration-scripts.

.PARAMETER ToolPath
    Path to the built companion tool (VirtoCommerce.Platform.MigrationScripter.dll or .exe).
    Defaults to the most recently built output next to this script.

.PARAMETER IncludeEmpty
    Also write files for contexts that have no pending migrations.

.PARAMETER Build
    Build the companion tool before running (useful on a fresh checkout).

.PARAMETER Zip
    If set, compresses the output folder into <OutputPath>.zip.

.EXAMPLE
    ./Grab-Migrations.ps1 -PlatformPath 'C:\vc-platform-3-demo\platform-net10'

.EXAMPLE
    ./Grab-Migrations.ps1 -PlatformPath 'C:\vc\platform' -DatabaseProvider SqlServer `
        -ConnectionString 'Data Source=.;Initial Catalog=VC;Integrated Security=True;TrustServerCertificate=True' `
        -OutputPath 'C:\vc\out' -Zip
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $PlatformPath,

    [ValidateSet('SqlServer', 'PostgreSql', 'MySql')]
    [string] $DatabaseProvider,

    [string] $ConnectionString,

    [string] $OutputPath,

    [string] $ToolPath,

    [switch] $IncludeEmpty,

    [switch] $Build,

    [switch] $Zip
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $PlatformPath)) {
    throw "Platform path not found: $PlatformPath"
}
$PlatformPath = (Resolve-Path -LiteralPath $PlatformPath).Path

if (-not $OutputPath) {
    $OutputPath = Join-Path $PlatformPath 'migration-scripts'
}

# This script lives inside the tool project folder.
$toolProject = $PSScriptRoot

if ($Build) {
    Write-Host "Building companion tool..."
    & dotnet build $toolProject -c Debug --nologo
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to build the companion tool (exit $LASTEXITCODE)."
    }
}

# Resolve the companion tool. Prefer an explicit -ToolPath; otherwise pick the MOST RECENTLY BUILT output
# (newest by timestamp) so a stale Release/Debug artifact can never shadow a fresh build.
if (-not $ToolPath) {
    $candidates = @(
        Join-Path $toolProject 'bin/Release/net10.0/VirtoCommerce.Platform.MigrationScripter.dll'
        Join-Path $toolProject 'bin/Debug/net10.0/VirtoCommerce.Platform.MigrationScripter.dll'
    )
    $ToolPath = $candidates |
        Where-Object { Test-Path -LiteralPath $_ } |
        Get-Item |
        Sort-Object LastWriteTimeUtc -Descending |
        Select-Object -First 1 -ExpandProperty FullName
}

if (-not $ToolPath -or -not (Test-Path -LiteralPath $ToolPath)) {
    throw "Companion tool not found. Build it first (re-run with -Build, or 'dotnet build $toolProject'), or pass -ToolPath."
}
$ToolPath = (Resolve-Path -LiteralPath $ToolPath).Path

# Build the tool argument list.
$toolArgs = @('--platform-path', $PlatformPath, '--output', $OutputPath)
if ($IncludeEmpty) {
    $toolArgs += '--include-empty'
}

# Apply optional overrides via environment variables, then restore them afterwards so they do not leak
# into the caller's session.
$hadProvider = Test-Path Env:\DatabaseProvider
$oldProvider = if ($hadProvider) { $env:DatabaseProvider } else { $null }
$hadConnection = Test-Path Env:\ConnectionStrings__VirtoCommerce
$oldConnection = if ($hadConnection) { $env:ConnectionStrings__VirtoCommerce } else { $null }

try {
    if ($DatabaseProvider) {
        Write-Host "Overriding DatabaseProvider = $DatabaseProvider"
        $env:DatabaseProvider = $DatabaseProvider
    }
    if ($ConnectionString) {
        Write-Host "Overriding ConnectionStrings__VirtoCommerce"
        $env:ConnectionStrings__VirtoCommerce = $ConnectionString
    }

    Write-Host "Running migration scripter against: $PlatformPath"
    Write-Host "Output: $OutputPath"

    if ($ToolPath.EndsWith('.dll')) {
        & dotnet $ToolPath @toolArgs
    }
    else {
        & $ToolPath @toolArgs
    }

    if ($LASTEXITCODE -ne 0) {
        throw "Migration scripter exited with code $LASTEXITCODE"
    }
}
finally {
    if ($DatabaseProvider) {
        if ($hadProvider) { $env:DatabaseProvider = $oldProvider } else { Remove-Item Env:\DatabaseProvider -ErrorAction SilentlyContinue }
    }
    if ($ConnectionString) {
        if ($hadConnection) { $env:ConnectionStrings__VirtoCommerce = $oldConnection } else { Remove-Item Env:\ConnectionStrings__VirtoCommerce -ErrorAction SilentlyContinue }
    }
}

$sqlFiles = @(Get-ChildItem -LiteralPath $OutputPath -Filter '*.sql' -ErrorAction SilentlyContinue)
Write-Host "Generated $($sqlFiles.Count) .sql file(s) in $OutputPath"
$sqlFiles | Sort-Object Name | Format-Table Name, Length -AutoSize | Out-Host

if ($Zip) {
    $zipPath = "$OutputPath.zip"
    if (Test-Path -LiteralPath $zipPath) {
        Remove-Item -LiteralPath $zipPath -Force
    }
    Compress-Archive -Path (Join-Path $OutputPath '*') -DestinationPath $zipPath
    Write-Host "Zipped output to $zipPath"
}
