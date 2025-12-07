# Deploy Frontend to Azure Web App (Windows)
# Prerequisites: azure-setup.ps1 has been run

param(
    [string]$FrontendAppName = "chenclan-ui",
    [string]$ResourceGroup = "chenclan-rg",
    [string]$FrontendDir = "vue-frontend"
)

Write-Host "===========================================" -ForegroundColor Cyan
Write-Host "Deploying Frontend to Azure" -ForegroundColor Cyan
Write-Host "===========================================" -ForegroundColor Cyan

$currentDir = Get-Location

# Step 1: Install dependencies
Write-Host "`n[1] Installing Node dependencies..." -ForegroundColor Yellow
Set-Location $FrontendDir
npm install
if ($LASTEXITCODE -ne 0) { 
    Write-Host "npm install failed!" -ForegroundColor Red
    exit $LASTEXITCODE 
}

# Step 2: Build Vue frontend
Write-Host "`n[2] Building Vue frontend for production..." -ForegroundColor Yellow
$env:VITE_API_URL = "https://chenclan-api.azurewebsites.net"
npm run build
if ($LASTEXITCODE -ne 0) { 
    Write-Host "Build failed!" -ForegroundColor Red
    exit $LASTEXITCODE 
}

# Step 3: Prepare ZIP for deployment
Write-Host "`n[3] Preparing deployment package..." -ForegroundColor Yellow
Set-Location "dist"
$zipFile = "$currentDir\frontend-deploy.zip"
if (Test-Path $zipFile) { Remove-Item $zipFile }
Compress-Archive -Path * -DestinationPath $zipFile -Force
Set-Location $currentDir

# Step 4: Deploy to Azure
Write-Host "`n[4] Deploying to Azure Web App..." -ForegroundColor Yellow
az webapp deployment source config-zip `
  --resource-group $ResourceGroup `
  --name $FrontendAppName `
  --src $zipFile

if ($LASTEXITCODE -ne 0) { 
    Write-Host "Deployment failed!" -ForegroundColor Red
    exit $LASTEXITCODE 
}

# Step 5: Verify deployment
Write-Host "`n[5] Verifying deployment (waiting 10 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

try {
    $response = Invoke-WebRequest -Uri "https://$FrontendAppName.azurewebsites.net" -UseBasicParsing -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Frontend is running!" -ForegroundColor Green
        Write-Host "   URL: https://$FrontendAppName.azurewebsites.net" -ForegroundColor Yellow
    }
} catch {
    Write-Host "⚠ Frontend may still be loading. Check:" -ForegroundColor Yellow
    Write-Host "   https://$FrontendAppName.azurewebsites.net" -ForegroundColor Gray
}

Write-Host "`n===========================================" -ForegroundColor Green
Write-Host "✓ Frontend deployment complete!" -ForegroundColor Green
Write-Host "===========================================" -ForegroundColor Green
