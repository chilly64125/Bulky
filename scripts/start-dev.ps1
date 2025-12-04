# start-dev.ps1
# Stops processes listening on common dev ports and opens two PowerShell windows:
#  - Backend: runs in BulkyWeb (dotnet watch run)
#  - Frontend: runs in vue-frontend (npm run dev)
# Usage (from any PowerShell):
#   powershell -NoProfile -ExecutionPolicy Bypass -File "D:\Git\VueChenClan\scripts\start-dev.ps1"

param(
    [int]$BackendPort = 5064,
    [int]$FrontendPort = 5173,
    [string]$RepoRoot = "D:\Git\VueChenClan",
    [string]$BackendPath = "BulkyWeb",
    [string]$FrontendPath = "vue-frontend"
)

function Stop-PortProcess([int]$port){
    try{
        $conns = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
        if ($conns){
            $pids = $conns | Select-Object -ExpandProperty OwningProcess -Unique
            foreach($pid in $pids){
                Write-Host "Stopping process $pid listening on port $port..."
                Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
            }
        } else {
            Write-Host "No process found listening on port $port"
        }
    } catch {
        Write-Warning "Could not query connections for port $port using Get-NetTCPConnection. Attempting netstat fallback..."
        $lines = netstat -ano | Select-String ":$port "
        foreach($line in $lines){
            $cols = ($line -split '\s+') | Where-Object {$_ -ne ''}
            $pid = $cols[-1]
            if ($pid -and $pid -ne '0'){
                Write-Host "Killing PID $pid (netstat) for port $port"
                taskkill /PID $pid /F | Out-Null
            }
        }
    }
}

Write-Host "Stopping common dev ports (backend: $BackendPort, frontend: $FrontendPort)..."
Stop-PortProcess -port $BackendPort
Stop-PortProcess -port $FrontendPort

# Paths
$backendFull = Join-Path $RepoRoot $BackendPath
$frontendFull = Join-Path $RepoRoot $FrontendPath

Write-Host "Opening backend window: $backendFull"
Start-Process -FilePath "powershell.exe" -ArgumentList @(
    '-NoExit',
    '-Command',
    "Set-Location -LiteralPath '$backendFull'; Write-Host 'Running backend: dotnet watch run'; $env:ASPNETCORE_ENVIRONMENT='Development'; dotnet watch run"
)

Start-Sleep -Milliseconds 500

Write-Host "Opening frontend window: $frontendFull"
Start-Process -FilePath "powershell.exe" -ArgumentList @(
    '-NoExit',
    '-Command',
    "Set-Location -LiteralPath '$frontendFull'; Write-Host 'Running frontend: npm run dev'; npm run dev"
)

Write-Host "Script finished. Two PowerShell windows were opened (backend & frontend). Check their consoles for startup logs." 
