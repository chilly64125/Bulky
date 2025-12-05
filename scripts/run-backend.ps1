param()
$RepoRoot = Split-Path -Parent $PSScriptRoot
$backendPath = Join-Path $RepoRoot 'BulkyWeb'
$logDir = Join-Path $PSScriptRoot 'logs'
if (-not (Test-Path $logDir)) { New-Item -ItemType Directory -Path $logDir | Out-Null }
Set-Location -LiteralPath $backendPath
Write-Host "Running backend in: $backendPath"
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet watch run *>&1 | Tee-Object -FilePath (Join-Path $logDir 'backend.log')
