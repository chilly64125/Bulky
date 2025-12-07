# Azure deployment setup script for Windows
# Creates all necessary Azure resources for VueChenClan application

param(
    [string]$ResourceGroup = "chenclan-rg",
    [string]$Location = "eastus",
    [string]$AppServicePlan = "chenclan-plan",
    [string]$BackendAppName = "chenclan-api",
    [string]$FrontendAppName = "chenclan-ui",
    [string]$SqlServer = "chenclan-sqlserver",
    [string]$SqlDb = "chenclan-db",
    [string]$SqlAdminUser = "sqladmin",
    [string]$SqlAdminPassword = "YourStrong@Password123"
)

Write-Host "===========================================" -ForegroundColor Cyan
Write-Host "VueChenClan Azure Deployment Setup (Windows)" -ForegroundColor Cyan
Write-Host "===========================================" -ForegroundColor Cyan

# Step 1: Create Resource Group
Write-Host "`n[1] Creating resource group: $ResourceGroup" -ForegroundColor Yellow
az group create --name $ResourceGroup --location $Location
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 2: Create App Service Plan
Write-Host "`n[2] Creating App Service Plan: $AppServicePlan" -ForegroundColor Yellow
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

# Step 5: Create SQL Server (optional)
Write-Host "`n[5] Creating Azure SQL Server and Database" -ForegroundColor Yellow
az sql server create `
  --name $SqlServer `
  --resource-group $ResourceGroup `
  --admin-user $SqlAdminUser `
  --admin-password $SqlAdminPassword `
  --location $Location
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

az sql db create `
  --server $SqlServer `
  --name $SqlDb `
  --resource-group $ResourceGroup `
  --sku Standard
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Allow Azure services to access SQL
Write-Host "Configuring SQL firewall rules..." -ForegroundColor Gray
az sql server firewall-rule create `
  --name "AllowAzureServices" `
  --server $SqlServer `
  --resource-group $ResourceGroup `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 6: Configure Backend App Settings
Write-Host "`n[6] Configuring Backend App Settings" -ForegroundColor Yellow
az webapp config appsettings set `
  --name $BackendAppName `
  --resource-group $ResourceGroup `
  --settings `
    ASPNETCORE_ENVIRONMENT=Production `
    ConnectionStrings__DefaultConnection="Data Source=/home/site/wwwroot/data/chenclan.db"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 7: Configure Frontend App Settings
Write-Host "`n[7] Configuring Frontend App Settings" -ForegroundColor Yellow
az webapp config appsettings set `
  --name $FrontendAppName `
  --resource-group $ResourceGroup `
  --settings `
    VITE_API_URL="https://$BackendAppName.azurewebsites.net" `
    SCM_DO_BUILD_DURING_DEPLOYMENT=true `
    WEBSITE_NODE_DEFAULT_VERSION=18
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 8: Enable CORS
Write-Host "`n[8] Enabling CORS for Backend" -ForegroundColor Yellow
az webapp config appsettings set `
  --name $BackendAppName `
  --resource-group $ResourceGroup `
  --settings `
    AllowedOrigins="https://$FrontendAppName.azurewebsites.net"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Step 9: Enable HTTPS only
Write-Host "`n[9] Enforcing HTTPS" -ForegroundColor Yellow
az webapp update --name $BackendAppName --resource-group $ResourceGroup --https-only true
az webapp update --name $FrontendAppName --resource-group $ResourceGroup --https-only true

Write-Host "`n===========================================" -ForegroundColor Green
Write-Host "✓ Azure Resources Created Successfully!" -ForegroundColor Green
Write-Host "===========================================" -ForegroundColor Green

Write-Host "`nEndpoints:" -ForegroundColor Cyan
Write-Host "Backend:  https://$BackendAppName.azurewebsites.net" -ForegroundColor Yellow
Write-Host "Frontend: https://$FrontendAppName.azurewebsites.net" -ForegroundColor Yellow

Write-Host "`nSQL Database:" -ForegroundColor Cyan
Write-Host "Server:   $SqlServer.database.windows.net" -ForegroundColor Yellow
Write-Host "Database: $SqlDb" -ForegroundColor Yellow
Write-Host "User:     $SqlAdminUser" -ForegroundColor Yellow

Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "1. Change SQL_AdminPassword in this script (for security)" -ForegroundColor Gray
Write-Host "2. Run .\deploy-backend.ps1 to deploy the backend" -ForegroundColor Gray
Write-Host "3. Run .\deploy-frontend.ps1 to deploy the frontend" -ForegroundColor Gray
