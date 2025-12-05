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
            $procIds = $conns | Select-Object -ExpandProperty OwningProcess -Unique
            foreach($procId in $procIds){
                Write-Host "Stopping process $procId listening on port $port..."
                Stop-Process -Id $procId -Force -ErrorAction SilentlyContinue
            }
        } else {
            Write-Host "No process found listening on port $port"
        }
    } catch {
        Write-Warning "Could not query connections for port $port using Get-NetTCPConnection. Attempting netstat fallback..."
        $lines = netstat -ano | Select-String ":$port "
        foreach($line in $lines){
            $cols = ($line -split '\s+') | Where-Object {$_ -ne ''}
            $pidToKill = $cols[-1]
            if ($pidToKill -and $pidToKill -ne '0'){
                Write-Host "Killing PID $pidToKill (netstat) for port $port"
                taskkill /PID $pidToKill /F | Out-Null
            }
        }
    }
}

function Stop-ProcessesByCommand([string]$pattern){
    try{
        Write-Host "Searching for processes with commandline matching: $pattern"
        $oldWarning = $WarningPreference
        $WarningPreference = 'SilentlyContinue'
        $allProcs = Get-CimInstance Win32_Process -ErrorAction Stop
        $WarningPreference = $oldWarning
        $found = $false
        foreach ($proc in $allProcs){
            $cmd = $proc.CommandLine
            if ($cmd -and ($cmd -like "*$pattern*")){
                $found = $true
                $pid = $proc.ProcessId
                if ($pid -and $pid -ne 0){
                    Write-Host "Stopping process $pid (matched '$pattern')..."
                    try{ Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue } catch { Write-Warning ("Failed to stop PID {0}: {1}" -f $pid, $_) }
                }
            }
        }
        if (-not $found){ Write-Host "No processes found matching pattern: $pattern" }
    } catch {
        Write-Warning "Could not query processes by commandline: $($_.Exception.Message)"
    }
}

Write-Host "Stopping common dev ports (backend: $BackendPort, frontend: $FrontendPort)..."
Stop-PortProcess -port $BackendPort
Stop-PortProcess -port $FrontendPort

# Paths (define before searching processes)
$backendFull = Join-Path $RepoRoot $BackendPath
$frontendFull = Join-Path $RepoRoot $FrontendPath

# Also stop any previously-started dev processes that reference these folders or common commands
Write-Host "Stopping any existing dev processes by commandline (backend/frontend folder, dotnet watch, npm run dev)..."
Stop-ProcessesByCommand -pattern $backendFull
Stop-ProcessesByCommand -pattern $frontendFull
Stop-ProcessesByCommand -pattern 'dotnet watch'
Stop-ProcessesByCommand -pattern 'npm run dev'

Write-Host "Opening backend window: $backendFull"
Start-Process -FilePath "powershell.exe" -ArgumentList @(
    '-NoExit',
    '-File',
    (Join-Path $RepoRoot 'scripts\run-backend.ps1')
)

Start-Sleep -Milliseconds 500

Write-Host "Opening frontend window: $frontendFull"
Start-Process -FilePath "powershell.exe" -ArgumentList @(
    '-NoExit',
    '-File',
    (Join-Path $RepoRoot 'scripts\run-frontend.ps1')
)

Write-Host "Script finished. Two PowerShell windows were opened (backend & frontend). Check their consoles for startup logs." 
Start-Sleep -Seconds 2
# Print quick tails of the logs (first 40 lines) so user can see startup status
$backendLog = Join-Path $RepoRoot 'scripts\logs\backend.log'
$frontendLog = Join-Path $RepoRoot 'scripts\logs\frontend.log'
Write-Host "\n--- Backend log (first 40 lines) ---"
if (Test-Path $backendLog) { Get-Content -Path $backendLog -TotalCount 40 | ForEach-Object { Write-Host $_ } } else { Write-Host "(no backend log yet)" }
Write-Host "\n--- Frontend log (first 40 lines) ---"
if (Test-Path $frontendLog) { Get-Content -Path $frontendLog -TotalCount 40 | ForEach-Object { Write-Host $_ } } else { Write-Host "(no frontend log yet)" }
