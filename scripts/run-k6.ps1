[CmdletBinding()]
param(
    [ValidateSet('smoke', 'load')]
    [string]$Profile = 'load',

    [string]$BaseUrl = 'http://localhost:5000'
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$localK6 = Join-Path $repoRoot '.tools\k6\k6.exe'
$k6Command = Get-Command k6 -ErrorAction SilentlyContinue

if (Test-Path -LiteralPath $localK6) {
    $k6Exe = $localK6
}
elseif ($null -ne $k6Command) {
    $k6Exe = $k6Command.Source
}
else {
    throw 'k6 was not found. Expected .tools\k6\k6.exe or k6 on PATH.'
}

$resultDirectory = Join-Path $repoRoot 'docs\performance-testing\results'
New-Item -ItemType Directory -Force -Path $resultDirectory | Out-Null

$timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$textResult = Join-Path $resultDirectory "k6-$Profile-$timestamp.txt"
$jsonResult = Join-Path $resultDirectory "k6-$Profile-$timestamp.json"
$testScript = Join-Path $repoRoot 'tests\k6\load_test.js'

& $k6Exe run `
    --env "BASE_URL=$BaseUrl" `
    --env "K6_PROFILE=$Profile" `
    --summary-export $jsonResult `
    $testScript 2>&1 | Tee-Object -FilePath $textResult

$k6ExitCode = $LASTEXITCODE
Write-Host "Text results: $textResult"
Write-Host "JSON summary: $jsonResult"
exit $k6ExitCode
