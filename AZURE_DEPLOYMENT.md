# Azure Web App Deployment Guide (F1 Free Tier + SQLite)

**Cost: FREE! 🎉** Deploy your entire full-stack application to Azure at zero cost with the F1 Free Tier and SQLite database.

This guide explains how to deploy the VueChenClan application (Frontend + Backend) to Azure Web Apps using the F1 Free Tier.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                   Azure Cloud (F1 Free Tier)                │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌──────────────────┐        ┌──────────────────────────┐  │
│  │  Azure Web App   │◄─────►│  SQLite Database         │  │
│  │  (Backend)       │        │  (Local File in App)     │  │
│  │  chenclan-api    │        │  /home/site/wwwroot/data│  │
│  │  :5064           │        └──────────────────────────┘  │
│  └──────────────────┘                                       │
│         ▲                                                    │
│         │                                                    │
│         │ API Calls                                         │
│         │                                                    │
│  ┌──────────────────┐                                       │
│  │  Azure Web App   │                                       │
│  │  (Frontend)      │                                       │
│  │  chenclan-ui     │                                       │
│  │  :80/:443        │                                       │
│  └──────────────────┘                                       │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ Azure App Service Plan (F1 - Free)                   │  │
│  │ • $0.00/month (always free)                          │  │
│  │ • 1 GB storage                                       │  │
│  │ • 60 minutes compute time per day                    │  │
│  │ • Shared infrastructure                             │  │
│  └──────────────────────────────────────────────────────┘  │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

## Prerequisites

### Software

- Azure CLI (`az` command)
- .NET 8 SDK
- Node.js 18+
- Git
- Azure subscription (free tier doesn't require billing!)

### Azure Resources

- Azure Resource Group
- Azure App Service Plan (F1 - Free)
- Two Azure Web Apps (backend + frontend)
- SQLite database (embedded, no setup required)

---

## Step-by-Step Deployment

### Phase 1: Azure Setup (One-time)

#### 1.1 Login to Azure

```powershell
az login
# Browser window opens for authentication
# Select your subscription
az account show
```

#### 1.2 Create Resource Group

```powershell
# Create a resource group (regional container for all resources)
az group create --name chenclan-rg --location eastus

# Verify
az group show --name chenclan-rg
```

#### 1.3 Create App Service Plan (F1 Free Tier)

```powershell
# Create an App Service Plan with F1 Free Tier
# F1 = Free ($0.00/month, 60 min/day, 1 GB storage, shared infrastructure)
# B1 = Basic ($7/month, unlimited compute, dedicated) - for when you need more

az appservice plan create \
  --name chenclan-plan \
  --resource-group chenclan-rg \
  --sku F1 \
  --is-linux

# Verify creation
az appservice plan show --name chenclan-plan --resource-group chenclan-rg
```

#### 1.4 About SQLite with Azure Web Apps

SQLite is **perfect for Azure Web Apps** because:

- **Embedded**: Database file stored in `/home/site/wwwroot/data/chenclan.db`
- **Zero cost**: No separate database service needed
- **Simple**: No connection strings, firewall rules, or complicated setup
- **Local**: Faster than cloud database, no latency
- **Persistent**: Data persists across restarts (stored in App Service storage)

⚠️ **Important Limitation**: F1 Free Tier sleeps after 60 minutes of inactivity. When it wakes up, the application restarts fresh. SQLite file persists, so data is not lost.

**No setup required!** The connection string is simply:

```
Data Source=/home/site/wwwroot/data/chenclan.db;Cache=Shared
```

Skip Step 1.4 (SQL Server creation) - you don't need it!

---

### Phase 2: Deploy Backend Web App

#### 2.1 Create Backend Web App

```powershell
az webapp create \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --plan chenclan-plan \
  --runtime "dotnet:8"
```

#### 2.2 Configure App Settings (SQLite)

```powershell
# Set environment and SQLite connection string
az webapp config appsettings set \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__DefaultConnection="Data Source=/home/site/wwwroot/data/chenclan.db;Cache=Shared"

# Verify settings
az webapp config appsettings list --name chenclan-api --resource-group chenclan-rg
```

**What's happening:**

- `ASPNETCORE_ENVIRONMENT=Production` → Release mode, no debug info, optimized performance
- `ConnectionStrings__DefaultConnection` → Tells your backend where the SQLite database file is
- `/home/site/wwwroot/data/chenclan.db` → Standard Azure Web Apps persistent storage path

#### 2.3 Enable Managed Identity (for secure Key Vault access)

```powershell
az webapp identity assign \
  --name chenclan-api \
  --resource-group chenclan-rg
```

#### 2.4 Configure CORS for Frontend

```powershell
az webapp config appsettings set \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --settings \
    AllowedOrigins="https://chenclan-ui.azurewebsites.net,https://yourdomain.com"
```

#### 2.5 Deploy Backend Code

```powershell
# Clone and build
cd D:\Git\VueChenClan
dotnet publish BulkyWeb/BulkyBookWeb.csproj --configuration Release --output ./bin/Release/publish

# Deploy using ZIP
cd ./bin/Release/publish
Compress-Archive -Path * -DestinationPath ../../../backend.zip -Force
az webapp deployment source config-zip \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --src ../../../backend.zip
```

Or use **Git deployment**:

```powershell
# Configure Git deployment
az webapp deployment user set --user-name <username> --password <password>

# Add Azure remote
git remote add azure https://<username>@chenclan-api.scm.azurewebsites.net/chenclan-api.git

# Deploy (pushes only BulkyWeb folder)
git push azure master
```

---

### Phase 3: Deploy Frontend Web App

#### 3.1 Create Frontend Web App

```powershell
az webapp create \
  --name chenclan-ui \
  --resource-group chenclan-rg \
  --plan chenclan-plan \
  --runtime "node:18-lts"
```

#### 3.2 Build Frontend for Production

```powershell
cd D:\Git\VueChenClan\vue-frontend

# Install dependencies
npm install

# Build for production (creates dist/ folder)
npm run build

# Set API endpoint for production
$env:VITE_API_URL = "https://chenclan-api.azurewebsites.net"
npm run build
```

#### 3.3 Configure Frontend App Settings

```powershell
az webapp config appsettings set \
  --name chenclan-ui \
  --resource-group chenclan-rg \
  --settings \
    SCM_DO_BUILD_DURING_DEPLOYMENT=true \
    WEBSITE_NODE_DEFAULT_VERSION=18 \
    VITE_API_URL="https://chenclan-api.azurewebsites.net"
```

#### 3.4 Deploy Frontend

```powershell
# Option A: Deploy dist folder directly
cd D:\Git\VueChenClan\vue-frontend\dist
Compress-Archive -Path * -DestinationPath ../../../frontend.zip -Force
az webapp deployment source config-zip \
  --name chenclan-ui \
  --resource-group chenclan-rg \
  --src ../../../frontend.zip

# Option B: Deploy source code (Azure builds it)
cd D:\Git\VueChenClan\vue-frontend
az webapp up \
  --name chenclan-ui \
  --resource-group chenclan-rg \
  --plan chenclan-plan \
  --runtime "node:18-lts"
```

---

### Phase 4: Configure Custom Domain (Optional)

#### 4.1 Point Domain to Azure

```powershell
# Get Azure IP
az webapp show --name chenclan-api --resource-group chenclan-rg --query defaultHostName

# Create DNS CNAME records pointing to:
# api.yourdomain.com  -> chenclan-api.azurewebsites.net
# www.yourdomain.com  -> chenclan-ui.azurewebsites.net
```

#### 4.2 Add Custom Domain to Web Apps

```powershell
# Backend
az webapp config hostname add \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --hostname api.yourdomain.com

# Frontend
az webapp config hostname add \
  --name chenclan-ui \
  --resource-group chenclan-rg \
  --hostname www.yourdomain.com
```

---

### Phase 5: Enable HTTPS/SSL

#### 5.1 Add Free SSL Certificate

```powershell
# Azure provides free SSL certificates for Web Apps
# Just add custom domain and certificate is auto-created

az webapp config ssl bind \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --certificate-thumbprint <thumbprint>
```

#### 5.2 Enforce HTTPS

```powershell
az webapp update \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --https-only true

az webapp update \
  --name chenclan-ui \
  --resource-group chenclan-rg \
  --https-only true
```

---

## Monitoring & Troubleshooting

### View Logs

```powershell
# Stream application logs (real-time)
az webapp log tail --name chenclan-api --resource-group chenclan-rg

# View deployment logs
az webapp deployment log show --name chenclan-api --resource-group chenclan-rg
```

### Check Application Health

```powershell
# Backend health check
curl https://chenclan-api.azurewebsites.net/health

# Frontend
curl https://chenclan-ui.azurewebsites.net
```

### Restart Web Apps

```powershell
az webapp restart --name chenclan-api --resource-group chenclan-rg
az webapp restart --name chenclan-ui --resource-group chenclan-rg
```

---

## Cost Estimation

| Resource                 | SKU              | Monthly Cost    |
| ------------------------ | ---------------- | --------------- |
| App Service Plan (F1)    | Free             | **$0.00**       |
| Azure Web App (Backend)  | Included in plan | Included        |
| Azure Web App (Frontend) | Included in plan | Included        |
| SQLite Database          | Embedded         | Included        |
| **Total**                | -                | **$0.00/month** |

🎉 **Your deployment is completely free!**

### F1 Limitations

The F1 Free Tier has these trade-offs:

| Feature              | F1 Free        | B1 Basic    |
| -------------------- | -------------- | ----------- |
| **Cost**             | $0.00/month    | $7.00/month |
| **Compute Time/Day** | 60 minutes     | Unlimited   |
| **Storage**          | 1 GB           | 10 GB       |
| **Always On**        | ❌ No (sleeps) | ✅ Yes      |
| **SSL Certificate**  | ✅ Free        | ✅ Free     |
| **Custom Domain**    | ✅ Yes         | ✅ Yes      |
| **Deployment Slots** | ❌ No          | ✅ 1 slot   |

**When to Upgrade:**

- Your application needs to run 24/7 without sleeping
- You hit the 60-minute daily compute limit
- You need more than 1 GB storage
- You want faster performance (dedicated vs shared)

**How to Upgrade:**

```powershell
# Scale up from F1 to B1 (takes 2-3 minutes)
az appservice plan update \
  --name chenclan-plan \
  --resource-group chenclan-rg \
  --sku B1
```

### SQLite vs Azure SQL Database

| Feature        | SQLite (F1)       | Azure SQL (B1)    |
| -------------- | ----------------- | ----------------- |
| **Cost**       | $0 (embedded)     | ~$15/month        |
| **Setup Time** | 0 seconds         | 10+ minutes       |
| **Backup**     | Automatic (daily) | Automatic (daily) |
| **Scaling**    | Single file       | Full cloud power  |
| **Use Case**   | Dev, test, small  | Production, large |

For learning and small projects, **SQLite is perfect!** When you're ready for production scale, just:

1. Create Azure SQL Database (15 minutes)
2. Update connection string (1 minute)
3. Run migrations (2 minutes)
4. Done! Upgrade complete

---

## Continuous Deployment (CI/CD)

Use GitHub Actions to auto-deploy on git push:

```yaml
# .github/workflows/azure-deploy.yml
name: Deploy to Azure

on:
  push:
    branches: [master]

env:
  AZURE_WEBAPP_BACKEND: chenclan-api
  AZURE_WEBAPP_FRONTEND: chenclan-ui
  AZURE_RG: chenclan-rg

jobs:
  deploy-backend:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: "8.0"
      - run: dotnet publish BulkyWeb -c Release -o ./backend
      - uses: azure/webapps-deploy@v2
        with:
          app-name: ${{ env.AZURE_WEBAPP_BACKEND }}
          package: ./backend
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE_BACKEND }}

  deploy-frontend:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
        with:
          node-version: "18"
      - run: |
          cd vue-frontend
          npm install
          npm run build
      - uses: azure/webapps-deploy@v2
        with:
          app-name: ${{ env.AZURE_WEBAPP_FRONTEND }}
          package: ./vue-frontend/dist
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE_FRONTEND }}
```

---

## Quick Reference Commands

```powershell
# List all resources in resource group
az resource list --resource-group chenclan-rg

# Get Web App URLs
az webapp list --resource-group chenclan-rg --query "[].{name:name, url:defaultHostName}"

# Scale up (change SKU)
az appservice plan update --name chenclan-plan --resource-group chenclan-rg --sku B2

# Delete all resources (cleanup)
az group delete --name chenclan-rg --yes --no-wait
```

---

## Troubleshooting Common Issues

### Issue: Frontend can't reach backend API

**Solution:** Check CORS settings in backend

```powershell
az webapp config appsettings set \
  --name chenclan-api \
  --resource-group chenclan-rg \
  --settings AllowedOrigins="https://chenclan-ui.azurewebsites.net"
```

### Issue: Database connection string not working

**Solution:** Verify connection string format and firewall rules

```powershell
# Allow local IP to test
az sql server firewall-rule create \
  --name AllowLocalIP \
  --server chenclan-sqlserver \
  --resource-group chenclan-rg \
  --start-ip-address <your-ip> \
  --end-ip-address <your-ip>
```

### Issue: Deployment fails with "file locked"

**Solution:** Restart the web app

```powershell
az webapp restart --name chenclan-api --resource-group chenclan-rg
```

---

## Next Steps

1. ✅ Set up Azure account and subscription
2. ✅ Create resource group and app service plan
3. ✅ Deploy backend web app
4. ✅ Deploy frontend web app
5. ✅ Test connectivity between frontend and backend
6. ✅ Configure custom domain (optional)
7. ✅ Set up monitoring and alerts
8. ✅ Configure CI/CD pipeline

## Additional Resources

- [Azure Web Apps Documentation](https://docs.microsoft.com/azure/app-service/)
- [Azure CLI Reference](https://docs.microsoft.com/cli/azure/)
- [App Service Pricing](https://azure.microsoft.com/pricing/details/app-service/)
- [Azure SQL Database Pricing](https://azure.microsoft.com/pricing/details/sql-database/)
