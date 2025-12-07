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

echo "=========================================="
echo "VueChenClan Azure Setup - F1 Free Tier"
echo "Cost: \$0.00/month with SQLite"
echo "=========================================="

# Step 1: Create Resource Group
echo -e "\n[1] Creating resource group: $RESOURCE_GROUP"
az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

# Step 2: Create App Service Plan (F1 Free Tier)
echo -e "\n[2] Creating App Service Plan: $APP_SERVICE_PLAN (F1 - FREE)"
az appservice plan create \
  --name "$APP_SERVICE_PLAN" \
  --resource-group "$RESOURCE_GROUP" \
  --sku F1 \
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

# Step 5: Configure Backend App Settings (SQLite - embedded)
echo -e "\n[5] Configuring Backend App Settings (SQLite)"
az webapp config appsettings set \
  --name "$BACKEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__DefaultConnection="Data Source=/home/site/wwwroot/data/chenclan.db;Cache=Shared"

# Step 6: Configure Frontend App Settings
echo -e "\n[6] Configuring Frontend App Settings"
az webapp config appsettings set \
  --name "$FRONTEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --settings \
    VITE_API_URL="https://$BACKEND_APP_NAME.azurewebsites.net" \
    SCM_DO_BUILD_DURING_DEPLOYMENT=true \
    WEBSITE_NODE_DEFAULT_VERSION=18

# Step 7: Enable CORS
echo -e "\n[7] Enabling CORS for Backend"
az webapp config appsettings set \
  --name "$BACKEND_APP_NAME" \
  --resource-group "$RESOURCE_GROUP" \
  --settings \
    AllowedOrigins="https://$FRONTEND_APP_NAME.azurewebsites.net"

# Step 8: Enable HTTPS only
echo -e "\n[8] Enforcing HTTPS"
az webapp update --name "$BACKEND_APP_NAME" --resource-group "$RESOURCE_GROUP" --https-only true
az webapp update --name "$FRONTEND_APP_NAME" --resource-group "$RESOURCE_GROUP" --https-only true

echo -e "\n=========================================="
echo "✓ Azure Resources Created Successfully!"
echo "✓ Cost: \$0.00/month (F1 Free Tier)"
echo "=========================================="
echo -e "\nEndpoints:"
echo "Backend:  https://$BACKEND_APP_NAME.azurewebsites.net"
echo "Frontend: https://$FRONTEND_APP_NAME.azurewebsites.net"
echo -e "\nDatabase:"
echo "Type:     SQLite (embedded in backend)"
echo "Location: /home/site/wwwroot/data/chenclan.db"
echo -e "\nNext steps:"
echo "1. Run deploy-backend.sh to deploy the backend"
echo "2. Run deploy-frontend.sh to deploy the frontend"
echo "3. Visit https://$FRONTEND_APP_NAME.azurewebsites.net in your browser"
