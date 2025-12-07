# Stop all running backend and frontend processes
# Usage: .\scripts\stop-all-processes.ps1

Write-Host "=== Stopping all Backend and Frontend processes ===" -ForegroundColor Cyan

# Stop .NET processes (backend)
Write-Host "`nStopping .NET backend processes..." -ForegroundColor Yellow
$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue
if ($dotnetProcesses) {
    $dotnetProcesses | ForEach-Object {
        Write-Host "  Stopping dotnet (PID: $($_.Id)) - $($_.ProcessName)"
        Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
    }
    Write-Host "  ✓ Dotnet processes stopped" -ForegroundColor Green
} else {
    Write-Host "  No dotnet processes running" -ForegroundColor Gray
}

# Stop Node.js processes (frontend/npm)
Write-Host "`nStopping Node.js frontend processes..." -ForegroundColor Yellow
$nodeProcesses = Get-Process -Name "node" -ErrorAction SilentlyContinue
if ($nodeProcesses) {
    $nodeProcesses | ForEach-Object {
        Write-Host "  Stopping node (PID: $($_.Id)) - $($_.ProcessName)"
        Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
    }
    Write-Host "  ✓ Node.js processes stopped" -ForegroundColor Green
} else {
    Write-Host "  No node processes running" -ForegroundColor Gray
}

# Stop Docker containers
Write-Host "`nStopping Docker containers..." -ForegroundColor Yellow
$dockerInstalled = Get-Command docker -ErrorAction SilentlyContinue
if ($dockerInstalled) {
    $containers = docker ps -q 2>$null
    if ($containers) {
        Write-Host "  Found running containers: $containers"
        docker-compose down 2>$null
        Write-Host "  ✓ Docker containers stopped" -ForegroundColor Green
    } else {
        Write-Host "  No running Docker containers" -ForegroundColor Gray
    }
} else {
    Write-Host "  Docker not installed, skipping..." -ForegroundColor Gray
}

Write-Host "`n✓ All processes cleaned up" -ForegroundColor Green
Write-Host "Ready to start backend and frontend independently!`n" -ForegroundColor Cyan
