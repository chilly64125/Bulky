# Deploy Backend to Azure Web App (Windows)
# Prerequisites: azure-setup.ps1 has been run

param(
    [string]$BackendAppName = "chenclan-api",
    [string]$ResourceGroup = "chenclan-rg",
    [string]$ProjectDir = "BulkyWeb"
)

Write-Host "===========================================" -ForegroundColor Cyan
Write-Host "Deploying Backend to Azure" -ForegroundColor Cyan
Write-Host "===========================================" -ForegroundColor Cyan

$currentDir = Get-Location

# Step 1: Build .NET project
Write-Host "`n[1] Building .NET backend..." -ForegroundColor Yellow
Set-Location $ProjectDir
dotnet publish --configuration Release --output "bin/Release/publish"
if ($LASTEXITCODE -ne 0) { 
    Write-Host "Build failed!" -ForegroundColor Red
    exit $LASTEXITCODE 
}

# Step 2: Prepare ZIP for deployment
Write-Host "`n[2] Preparing deployment package..." -ForegroundColor Yellow
Set-Location "bin/Release/publish"
$zipFile = "$currentDir\backend-deploy.zip"
if (Test-Path $zipFile) { Remove-Item $zipFile }
Compress-Archive -Path * -DestinationPath $zipFile -Force
Set-Location $currentDir

# Step 3: Deploy to Azure
Write-Host "`n[3] Deploying to Azure Web App..." -ForegroundColor Yellow
az webapp deployment source config-zip `
  --resource-group $ResourceGroup `
  --name $BackendAppName `
  --src $zipFile

if ($LASTEXITCODE -ne 0) { 
    Write-Host "Deployment failed!" -ForegroundColor Red
    exit $LASTEXITCODE 
}

# Step 4: Verify deployment
Write-Host "`n[4] Verifying deployment (waiting 10 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

try {
    $healthResponse = Invoke-WebRequest -Uri "https://$BackendAppName.azurewebsites.net/health" -UseBasicParsing -TimeoutSec 5
    Write-Host "✓ Backend is running and healthy!" -ForegroundColor Green
    Write-Host "   Endpoint: https://$BackendAppName.azurewebsites.net/health" -ForegroundColor Yellow
} catch {
    Write-Host "⚠ Backend may still be starting. Check logs:" -ForegroundColor Yellow
    Write-Host "   az webapp log tail --name $BackendAppName --resource-group $ResourceGroup" -ForegroundColor Gray
}

Write-Host "`n===========================================" -ForegroundColor Green
Write-Host "✓ Backend deployment complete!" -ForegroundColor Green
Write-Host "===========================================" -ForegroundColor Green
