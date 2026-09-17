[CmdletBinding()]
param(
    [string]$Urls = 'http://0.0.0.0:5000'
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$toolRoot = Join-Path $repoRoot '.tools'

$env:DOTNET_CLI_HOME = Join-Path $toolRoot 'dotnet-home'
$env:NUGET_PACKAGES = Join-Path $toolRoot 'nuget-packages'
$env:NUGET_HTTP_CACHE_PATH = Join-Path $toolRoot 'nuget-http-cache'
$env:TEMP = Join-Path $toolRoot 'temp'
$env:TMP = Join-Path $toolRoot 'temp'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:ASPNETCORE_ENVIRONMENT = 'Development'

New-Item -ItemType Directory -Force -Path `
    $env:DOTNET_CLI_HOME, `
    $env:NUGET_PACKAGES, `
    $env:NUGET_HTTP_CACHE_PATH, `
    $env:TEMP | Out-Null

dotnet run `
    --project (Join-Path $repoRoot 'backend\FarmSim.Api\FarmSim.Api.csproj') `
    --urls $Urls
