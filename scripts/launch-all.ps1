# Master Launch Script: Stop all processes, then start backend and frontend independently
# Usage: .\scripts\launch-all.ps1

Write-Host "`n===== VueChenClan - Full Stack Development Environment =====" -ForegroundColor Cyan
Write-Host "" -ForegroundColor Cyan

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Split-Path -Parent $scriptDir

# Step 1: Stop all processes
Write-Host "STEP 1: Stopping all running processes..." -ForegroundColor Yellow
Write-Host "============================================================" -ForegroundColor Gray
& "$scriptDir\stop-all-processes.ps1"

# Wait a bit for processes to fully terminate
Write-Host "`nWaiting 2 seconds for processes to fully terminate..." -ForegroundColor Gray
Start-Sleep -Seconds 2

# Step 2: Start backend
Write-Host "`nSTEP 2: Starting Backend..." -ForegroundColor Yellow
Write-Host "============================================================" -ForegroundColor Gray
& "$scriptDir\start-backend.ps1"

# Step 3: Start frontend
Write-Host "`nSTEP 3: Starting Frontend..." -ForegroundColor Yellow
Write-Host "============================================================" -ForegroundColor Gray
Start-Sleep -Seconds 2
& "$scriptDir\start-frontend.ps1"

Write-Host "`n===== Environment Ready! =====" -ForegroundColor Green
Write-Host "Backend:  http://localhost:5064" -ForegroundColor Green
Write-Host "Frontend: http://localhost:5173" -ForegroundColor Green
Write-Host "=========================================`n" -ForegroundColor Green
