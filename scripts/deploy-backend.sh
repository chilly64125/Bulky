#!/bin/bash
# Deploy Backend to Azure Web App
# Prerequisites: azure-setup.sh has been run

set -e

BACKEND_APP_NAME="chenclan-api"
RESOURCE_GROUP="chenclan-rg"
PROJECT_DIR="BulkyWeb"
OUTPUT_DIR="./bin/Release/publish"

echo "=========================================="
echo "Deploying Backend to Azure"
echo "=========================================="

# Step 1: Build .NET project
echo -e "\n[1] Building .NET backend..."
cd "$PROJECT_DIR" || exit
dotnet publish --configuration Release --output "$OUTPUT_DIR"
cd - > /dev/null || exit

# Step 2: Prepare ZIP for deployment
echo -e "\n[2] Preparing deployment package..."
cd "$PROJECT_DIR/$OUTPUT_DIR" || exit
ZIP_FILE="../../../backend-deploy.zip"
zip -r "$ZIP_FILE" . -q
cd - > /dev/null || exit

# Step 3: Deploy to Azure
echo -e "\n[3] Deploying to Azure Web App..."
az webapp deployment source config-zip \
  --resource-group "$RESOURCE_GROUP" \
  --name "$BACKEND_APP_NAME" \
  --src "backend-deploy.zip"

# Step 4: Verify deployment
echo -e "\n[4] Verifying deployment..."
sleep 10
HEALTH_ENDPOINT="https://$BACKEND_APP_NAME.azurewebsites.net/health"
if curl -s "$HEALTH_ENDPOINT" > /dev/null; then
    echo "✓ Backend is running and healthy!"
    echo "   Endpoint: $HEALTH_ENDPOINT"
else
    echo "⚠ Backend may still be starting. Check logs:"
    echo "   az webapp log tail --name $BACKEND_APP_NAME --resource-group $RESOURCE_GROUP"
fi

echo -e "\n=========================================="
echo "✓ Backend deployment complete!"
echo "=========================================="
