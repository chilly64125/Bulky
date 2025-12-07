# Visual Studio & Azure Deployment Guide

This guide explains how to run the VueChenClan project in Visual Studio and deploy it to Azure using Docker.

## Table of Contents

1. [Local Development in Visual Studio](#local-development-in-visual-studio)
2. [Docker Local Development](#docker-local-development)
3. [Azure Deployment](#azure-deployment)
4. [GitHub Actions CI/CD](#github-actions-cicd)

---

## Local Development in Visual Studio

### Prerequisites

- Visual Studio 2022 (Community, Professional, or Enterprise)
- .NET 8 SDK
- Node.js 18+
- SQL Server or SQL Server Express (optional; SQLite is default)

### Steps

1. **Open the Solution**

   ```powershell
   # Navigate to the project directory
   cd D:\Git\VueChenClan

   # Open in Visual Studio
   start Bulky.sln
   ```

2. **Configure appsettings**

   - Edit `BulkyWeb/appsettings.json` for local database settings
   - Default uses SQLite: `Data Source=data/chenclan.db`

3. **Install Frontend Dependencies**

   ```powershell
   cd vue-frontend
   npm install
   ```

4. **Apply Database Migrations**

   ```powershell
   # In Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
   Update-Database
   ```

5. **Run in Visual Studio**

   - Set `BulkyWeb` as the Startup Project
   - Press `F5` or click "Run"
   - Backend will start on `https://localhost:5001` (IIS Express) or `http://localhost:5064` (Kestrel)

6. **Run Frontend Dev Server (Optional - for HMR)**
   ```powershell
   cd vue-frontend
   npm run dev
   # Frontend runs on http://localhost:5173
   ```

> **Note:** Backend serves the Vue frontend from `/wwwroot/vue-dist` by default. For development with hot reload, run the Vite dev server separately and configure proxy in `vite.config.ts`.

---

## Docker Local Development

### Prerequisites

- Docker Desktop installed and running
- Docker Compose (included with Docker Desktop)

### Quick Start

1. **Clone the Repository**

   ```powershell
   git clone https://github.com/chilly64125/VueChenClan.git
   cd VueChenClan
   ```

2. **Create Environment File**

   ```powershell
   Copy-Item .env.example .env
   ```

   Edit `.env` with your credentials:

   ```env
   DB_SA_PASSWORD=YourStrong@Password123
   STRIPE_SECRET_KEY=sk_test_xxxxx
   STRIPE_PUBLISHABLE_KEY=pk_test_xxxxx
   EMAIL_SMTP_SERVER=smtp.gmail.com
   EMAIL_SMTP_PORT=587
   EMAIL_SENDER=your-email@gmail.com
   EMAIL_PASSWORD=your-app-password
   ```

3. **Build and Run with Docker Compose**

   ```powershell
   # Build all images
   docker-compose build

   # Start all services (development mode)
   docker-compose up -d
   ```

4. **Access the Application**

   - Backend API: `http://localhost:5064`
   - Frontend: `http://localhost:5064` (served from backend)
   - Database (SQL Server): `localhost:1433`
     - SA Username: `sa`
     - SA Password: `YourStrong@Password123`

5. **View Logs**

   ```powershell
   # All services
   docker-compose logs -f

   # Specific service
   docker-compose logs -f api
   ```

6. **Stop Services**

   ```powershell
   docker-compose down

   # Also remove volumes
   docker-compose down -v
   ```

### Using SQLite Instead of SQL Server

Edit `docker-compose.yml` and:

1. Comment out the `db` service section
2. In the `api` service, set: `ConnectionStrings__DefaultConnection=Data Source=/app/data/chenclan.db`
3. Remove `depends_on: - db`

---

## Azure Deployment

### Prerequisites

- Azure Subscription
- Azure Container Registry
- Azure Container Instances (ACI) or App Service
- Azure SQL Database (optional)

### Option 1: Deploy via Azure Portal

1. **Create Container Registry**

   ```powershell
   # Create resource group
   az group create --name chenclan-rg --location eastus

   # Create container registry
   az acr create --resource-group chenclan-rg `
                 --name yourregistry --sku Basic
   ```

2. **Build and Push Image**

   ```powershell
   # Build locally
   docker build -t yourregistry.azurecr.io/chenclan-web:latest .

   # Login to ACR
   az acr login --name yourregistry

   # Push image
   docker push yourregistry.azurecr.io/chenclan-web:latest
   ```

3. **Deploy to Container Instances**
   ```powershell
   az container create --resource-group chenclan-rg `
                       --name chenclan-web `
                       --image yourregistry.azurecr.io/chenclan-web:latest `
                       --registry-login-server yourregistry.azurecr.io `
                       --registry-username <username> `
                       --registry-password <password> `
                       --environment-variables `
                         ASPNETCORE_ENVIRONMENT=Production `
                         ConnectionStrings__DefaultConnection="YourAzureSQLConnectionString" `
                       --port 80 5064
   ```

### Option 2: Deploy via GitHub Actions (Recommended)

1. **Set Up Azure Resources**

   ```powershell
   # Create resource group
   az group create --name chenclan-rg --location eastus

   # Create container registry
   az acr create --resource-group chenclan-rg `
                 --name yourregistry --sku Basic

   # Create App Service Plan
   az appservice plan create --name chenclan-plan `
                             --resource-group chenclan-rg `
                             --sku B1 --is-linux

   # Create Web App
   az webapp create --name chenclan-web `
                    --resource-group chenclan-rg `
                    --plan chenclan-plan `
                    --deployment-container-image-name yourregistry.azurecr.io/chenclan-web:latest
   ```

2. **Create Service Principal for GitHub**

   ```powershell
   # Create service principal
   $sp = az ad sp create-for-rbac --name "github-chenclan" `
                                  --role contributor `
                                  --scopes "/subscriptions/{subscriptionId}/resourceGroups/chenclan-rg"

   # Output the JSON (copy this for GitHub secrets)
   $sp | ConvertTo-Json
   ```

3. **Add GitHub Secrets**

   - Go to: **Settings > Secrets and variables > Actions**
   - Add the following secrets:
     - `AZURE_CREDENTIALS`: Service principal JSON
     - `AZURE_RESOURCE_GROUP`: `chenclan-rg`
     - `AZURE_REGISTRY_USERNAME`: Your ACR username
     - `AZURE_REGISTRY_PASSWORD`: Your ACR password
     - `AZURE_DB_CONNECTION_STRING`: Azure SQL connection string
     - `STRIPE_SECRET_KEY`: Your Stripe secret
     - `STRIPE_PUBLISHABLE_KEY`: Your Stripe publishable key

4. **Push to Master Branch**

   ```powershell
   git push origin master
   ```

   This automatically triggers the GitHub Actions workflow to:

   - Build the .NET backend
   - Build the Vue frontend
   - Create Docker image
   - Push to Azure Container Registry
   - Deploy to Azure App Service

---

## GitHub Actions CI/CD

The workflow (`.github/workflows/build-deploy.yml`) automatically:

1. **On Pull Request:**

   - Builds .NET solution
   - Runs unit tests
   - Lints frontend code
   - Builds Vue frontend

2. **On Push to Master:**
   - Same as above, plus:
   - Builds Docker image
   - Pushes to Azure Container Registry
   - Deploys to production

### Monitoring Deployments

- Check GitHub Actions tab in your repository
- View deployment logs and status

---

## Troubleshooting

### Docker Issues

**Port already in use:**

```powershell
docker-compose down
docker system prune -a
docker-compose up
```

**Database connection fails:**

```powershell
# Check database logs
docker-compose logs db

# Restart database
docker-compose restart db
```

### Azure Deployment Issues

**ACR Login fails:**

```powershell
az acr login --name yourregistry
```

**Web app not starting:**

```powershell
# Check logs
az webapp log tail --name chenclan-web --resource-group chenclan-rg
```

---

## Configuration Reference

### Environment Variables

**Backend (appsettings.json):**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=chenclan.db"
  },
  "Stripe": {
    "SecretKey": "sk_test_...",
    "PublishableKey": "pk_test_..."
  }
}
```

**Docker (docker-compose.yml):**

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ConnectionStrings__DefaultConnection=...
  - Stripe__SecretKey=...
```

---

## Next Steps

- [ ] Set up Azure SQL Database
- [ ] Configure custom domain
- [ ] Enable HTTPS/SSL certificates
- [ ] Set up Azure Application Insights monitoring
- [ ] Configure backup and disaster recovery
- [ ] Set up CI/CD pipeline with GitHub Actions

---

For more information, visit:

- [Azure App Service Documentation](https://docs.microsoft.com/azure/app-service/)
- [Docker Documentation](https://docs.docker.com/)
- [GitHub Actions Documentation](https://docs.github.com/actions)
