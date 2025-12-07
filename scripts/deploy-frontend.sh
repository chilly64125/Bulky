#!/bin/bash
# Deploy Frontend to Azure Web App
# Prerequisites: azure-setup.sh has been run

set -e

FRONTEND_APP_NAME="chenclan-ui"
RESOURCE_GROUP="chenclan-rg"
FRONTEND_DIR="vue-frontend"

echo "=========================================="
echo "Deploying Frontend to Azure"
echo "=========================================="

# Step 1: Install dependencies
echo -e "\n[1] Installing Node dependencies..."
cd "$FRONTEND_DIR" || exit
npm install

# Step 2: Build Vue frontend
echo -e "\n[2] Building Vue frontend for production..."
VITE_API_URL="https://chenclan-api.azurewebsites.net" npm run build

# Step 3: Prepare ZIP for deployment
echo -e "\n[3] Preparing deployment package..."
ZIP_FILE="../frontend-deploy.zip"
cd dist || exit
zip -r "$ZIP_FILE" . -q
cd - > /dev/null || exit

# Step 4: Deploy to Azure
echo -e "\n[4] Deploying to Azure Web App..."
az webapp deployment source config-zip \
  --resource-group "$RESOURCE_GROUP" \
  --name "$FRONTEND_APP_NAME" \
  --src "frontend-deploy.zip"

# Step 5: Verify deployment
echo -e "\n[5] Verifying deployment..."
sleep 10
FRONTEND_URL="https://$FRONTEND_APP_NAME.azurewebsites.net"
if curl -s "$FRONTEND_URL" | grep -q "<!DOCTYPE" > /dev/null 2>&1; then
    echo "✓ Frontend is running!"
    echo "   URL: $FRONTEND_URL"
else
    echo "⚠ Frontend may still be loading. Check:"
    echo "   $FRONTEND_URL"
fi

echo -e "\n=========================================="
echo "✓ Frontend deployment complete!"
echo "=========================================="
