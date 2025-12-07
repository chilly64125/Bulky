# Start Frontend in an independent PowerShell window
# Usage: .\scripts\start-frontend.ps1

$frontendPath = "D:\Git\VueChenClan\vue-frontend"

Write-Host "=== Starting Frontend ===" -ForegroundColor Cyan
Write-Host "Project: $frontendPath" -ForegroundColor Yellow
Write-Host "Endpoint: http://localhost:5173" -ForegroundColor Yellow

# Start in new PowerShell window
$psScript = @"
cd '$frontendPath'
Write-Host "Installing dependencies if needed..." -ForegroundColor Yellow
npm install --silent 2>$null
Write-Host "Starting frontend dev server..." -ForegroundColor Green
npm run dev
"@

$psScriptPath = "$env:TEMP\start-frontend-temp.ps1"
$psScript | Out-File -FilePath $psScriptPath -Encoding UTF8 -Force

# Launch in new independent window
Start-Process powershell.exe -ArgumentList "-NoExit -ExecutionPolicy Bypass -File `"$psScriptPath`"" -WindowStyle Normal

Write-Host "Frontend window opened in new PowerShell window!" -ForegroundColor Green
Write-Host "Waiting 3 seconds before closing this window..." -ForegroundColor Gray
Start-Sleep -Seconds 3
