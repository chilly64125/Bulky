param()
$RepoRoot = Split-Path -Parent $PSScriptRoot
$frontendPath = Join-Path $RepoRoot 'vue-frontend'
$logDir = Join-Path $PSScriptRoot 'logs'
if (-not (Test-Path $logDir)) { New-Item -ItemType Directory -Path $logDir | Out-Null }
Set-Location -LiteralPath $frontendPath
Write-Host "Running frontend in: $frontendPath"
npm run dev *>&1 | Tee-Object -FilePath (Join-Path $logDir 'frontend.log')
