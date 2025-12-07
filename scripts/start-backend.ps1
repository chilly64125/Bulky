# Start Backend in an independent PowerShell window
# Usage: .\scripts\start-backend.ps1

$projectPath = "D:\Git\VueChenClan\BulkyWeb"
$projectName = "BulkyBookWeb.csproj"

Write-Host "=== Starting Backend ===" -ForegroundColor Cyan
Write-Host "Project: $projectPath\$projectName" -ForegroundColor Yellow
Write-Host "Endpoint: http://localhost:5064" -ForegroundColor Yellow

# Start in new PowerShell window
$psScript = @"
cd '$projectPath'
Write-Host "Starting backend application..." -ForegroundColor Green
dotnet watch run --project '$projectName'
"@

$psScriptPath = "$env:TEMP\start-backend-temp.ps1"
$psScript | Out-File -FilePath $psScriptPath -Encoding UTF8 -Force

# Launch in new independent window
Start-Process powershell.exe -ArgumentList "-NoExit -ExecutionPolicy Bypass -File `"$psScriptPath`"" -WindowStyle Normal

Write-Host "Backend window opened in new PowerShell window!" -ForegroundColor Green
Write-Host "Waiting 3 seconds before closing this window..." -ForegroundColor Gray
Start-Sleep -Seconds 3
