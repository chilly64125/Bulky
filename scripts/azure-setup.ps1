# Azure deployment setup script for Windows
# Creates all necessary Azure resources for VueChenClan application

param(
    [string]$ResourceGroup = "chenclan-rg",
    [string]$Location = "eastus",
    [string]$AppServicePlan = "chenclan-plan",
    [string]$BackendAppName = "chenclan-api",
    [string]$FrontendAppName = "chenclan-ui"
)

Write-Host "===========================================" -ForegroundColor Cyan
Write-Host "VueChenClan Azure Setup - F1 Free Tier" -ForegroundColor Cyan
Write-Host "Cost: $0.00/month with SQLite" -ForegroundColor Green
Write-Host "===========================================" -ForegroundColor Cyan

# Step 1: Create Resource Group
Write-Host "`n[1] Creating resource group: $ResourceGroup" -ForegroundColor Yellow
az group create --name $ResourceGroup --location $Location
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 2: Create App Service Plan (B1 Basic Tier - $7/month)
Write-Host "`n[2] Creating App Service Plan: $AppServicePlan (B1 Basic - $7/month)" -ForegroundColor Yellow
az appservice plan create `
  --name $AppServicePlan `
  --resource-group $ResourceGroup `
  --sku B1 `
  --is-linux
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 3: Create Backend Web App
Write-Host "`n[3] Creating Backend Web App: $BackendAppName" -ForegroundColor Yellow
az webapp create `
  --name $BackendAppName `
  --resource-group $ResourceGroup `
  --plan $AppServicePlan `
  --runtime "dotnet:8"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 4: Create Frontend Web App
Write-Host "`n[4] Creating Frontend Web App: $FrontendAppName" -ForegroundColor Yellow
az webapp create `
  --name $FrontendAppName `
  --resource-group $ResourceGroup `
  --plan $AppServicePlan `
  --runtime "node:18-lts"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 5: Configure Backend App Settings (SQLite - embedded)
Write-Host "`n[5] Configuring Backend App Settings (SQLite)" -ForegroundColor Yellow
az webapp config appsettings set `
  --name $BackendAppName `
  --resource-group $ResourceGroup `
  --settings `
    ASPNETCORE_ENVIRONMENT=Production `
    ConnectionStrings__DefaultConnection="Data Source=/home/site/wwwroot/data/chenclan.db;Cache=Shared"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 6: Configure Frontend App Settings
Write-Host "`n[6] Configuring Frontend App Settings" -ForegroundColor Yellow
az webapp config appsettings set `
  --name $FrontendAppName `
  --resource-group $ResourceGroup `
  --settings `
    VITE_API_URL="https://$BackendAppName.azurewebsites.net" `
    SCM_DO_BUILD_DURING_DEPLOYMENT=true `
    WEBSITE_NODE_DEFAULT_VERSION=18
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 7: Enable CORS
Write-Host "`n[7] Enabling CORS for Backend" -ForegroundColor Yellow
az webapp config appsettings set `
  --name $BackendAppName `
  --resource-group $ResourceGroup `
  --settings `
    AllowedOrigins="https://$FrontendAppName.azurewebsites.net"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 8: Enable HTTPS only
Write-Host "`n[8] Enforcing HTTPS" -ForegroundColor Yellow
az webapp update --name $BackendAppName --resource-group $ResourceGroup --https-only true
az webapp update --name $FrontendAppName --resource-group $ResourceGroup --https-only true

Write-Host "`n===========================================" -ForegroundColor Green
Write-Host "✓ Azure Resources Created Successfully!" -ForegroundColor Green
Write-Host "✓ Cost: $7/month (B1 Basic Tier)" -ForegroundColor Green
Write-Host "===========================================" -ForegroundColor Green

Write-Host "`nEndpoints:" -ForegroundColor Cyan
Write-Host "Backend:  https://$BackendAppName.azurewebsites.net" -ForegroundColor Yellow
Write-Host "Frontend: https://$FrontendAppName.azurewebsites.net" -ForegroundColor Yellow

Write-Host "`nDatabase:" -ForegroundColor Cyan
Write-Host "Type:     SQLite (embedded in backend)" -ForegroundColor Yellow
Write-Host "Location: /home/site/wwwroot/data/chenclan.db" -ForegroundColor Yellow

Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "1. Run .\deploy-backend.ps1 to deploy the backend" -ForegroundColor Gray
Write-Host "2. Run .\deploy-frontend.ps1 to deploy the frontend" -ForegroundColor Gray
Write-Host "3. Visit https://$FrontendAppName.azurewebsites.net in your browser" -ForegroundColor Gray
