#!/bin/bash
# Azure deployment setup script
# Creates all necessary Azure resources for VueChenClan application

set -e

# Configuration
RESOURCE_GROUP="chenclan-rg"
LOCATION="eastus"
APP_SERVICE_PLAN="chenclan-plan"
BACKEND_APP_NAME="chenclan-api"
FRONTEND_APP_NAME="chenclan-ui"
SQL_SERVER="chenclan-sqlserver"
SQL_DB="chenclan-db"
SQL_ADMIN_USER="sqladmin"
SQL_ADMIN_PASSWORD="YourStrong@Password123"  # Change this!

echo "=========================================="
echo "VueChenClan Azure Deployment Setup"
echo "=========================================="

# Step 1: Create Resource Group
echo -e "\n[1] Creating resource group: $RESOURCE_GROUP"
az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

# Step 2: Create App Service Plan
echo -e "\n[2] Creating App Service Plan: $APP_SERVICE_PLAN"
az appservice plan create \
  --name "$APP_SERVICE_PLAN" \
  --resource-group "$RESOURCE_GROUP" \
  --sku B1 \
  --is-linux

# Step 3: Create Backend Web App
echo -e "\n[3] Creating Backend Web App: $BACKEND_APP_NAME"
az webapp create \
  --name "$BACKEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --plan "$APP_SERVICE_PLAN" \
  --runtime "dotnet:8"

# Step 4: Create Frontend Web App
echo -e "\n[4] Creating Frontend Web App: $FRONTEND_APP_NAME"
az webapp create \
  --name "$FRONTEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --plan "$APP_SERVICE_PLAN" \
  --runtime "node:18-lts"

# Step 5: Create SQL Server (optional)
echo -e "\n[5] Creating Azure SQL Server and Database"
az sql server create \
  --name "$SQL_SERVER" \
  --resource-group "$RESOURCE_GROUP" \
  --admin-user "$SQL_ADMIN_USER" \
  --admin-password "$SQL_ADMIN_PASSWORD" \
  --location "$LOCATION"

az sql db create \
  --server "$SQL_SERVER" \
  --name "$SQL_DB" \
  --resource-group "$RESOURCE_GROUP" \
  --sku Standard

# Allow Azure services to access SQL
az sql server firewall-rule create \
  --name "AllowAzureServices" \
  --server "$SQL_SERVER" \
  --resource-group "$RESOURCE_GROUP" \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Step 6: Configure Backend App Settings
echo -e "\n[6] Configuring Backend App Settings"
az webapp config appsettings set \
  --name "$BACKEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__DefaultConnection="Data Source=/home/site/wwwroot/data/chenclan.db"

# Step 7: Configure Frontend App Settings
echo -e "\n[7] Configuring Frontend App Settings"
az webapp config appsettings set \
  --name "$FRONTEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --settings \
    VITE_API_URL="https://$BACKEND_APP_NAME.azurewebsites.net" \
    SCM_DO_BUILD_DURING_DEPLOYMENT=true \
    WEBSITE_NODE_DEFAULT_VERSION=18

# Step 8: Enable CORS
echo -e "\n[8] Enabling CORS for Backend"
az webapp config appsettings set \
  --name "$BACKEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --settings \
    AllowedOrigins="https://$FRONTEND_APP_NAME.azurewebsites.net"

# Step 9: Enable HTTPS only
echo -e "\n[9] Enforcing HTTPS"
az webapp update --name "$BACKEND_APP_NAME" --resource-group "$RESOURCE_GROUP" --https-only true
az webapp update --name "$FRONTEND_APP_NAME" --resource-group "$RESOURCE_GROUP" --https-only true

echo -e "\n=========================================="
echo "✓ Azure Resources Created Successfully!"
echo "=========================================="
echo -e "\nEndpoints:"
echo "Backend:  https://$BACKEND_APP_NAME.azurewebsites.net"
echo "Frontend: https://$FRONTEND_APP_NAME.azurewebsites.net"
echo -e "\nSQL Database:"
echo "Server:   $SQL_SERVER.database.windows.net"
echo "Database: $SQL_DB"
echo "User:     $SQL_ADMIN_USER"
echo -e "\nNext steps:"
echo "1. Update SQL_ADMIN_PASSWORD in this script (for security)"
echo "2. Run deploy-backend.sh to deploy the backend"
echo "3. Run deploy-frontend.sh to deploy the frontend"
