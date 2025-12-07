# Multi-stage build for .NET and Vue frontend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-backend

WORKDIR /src

# Copy solution and project files
COPY ["Bulky.sln", "."]
COPY ["BulkyWeb/BulkyBookWeb.csproj", "BulkyWeb/"]
COPY ["Bulky.DataAccess/BulkyBook.DataAccess.csproj", "Bulky.DataAccess/"]
COPY ["Bulky.Models/BulkyBook.Models.csproj", "Bulky.Models/"]
COPY ["Bulky.Utility/BulkyBook.Utility.csproj", "Bulky.Utility/"]

# Restore dependencies
RUN dotnet restore "BulkyWeb/BulkyBookWeb.csproj"

# Copy all source code
COPY . .

# Build the backend
RUN dotnet build "BulkyWeb/BulkyBookWeb.csproj" -c Release -o /app/build

# Publish the backend
RUN dotnet publish "BulkyWeb/BulkyBookWeb.csproj" -c Release -o /app/publish

# Build frontend stage
FROM node:18-alpine AS build-frontend

WORKDIR /app/frontend

# Copy frontend files
COPY vue-frontend/package*.json ./
RUN npm ci

COPY vue-frontend/ .

# Build Vue frontend
RUN npm run build

# Final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published backend
COPY --from=build-backend /app/publish .

# Copy built frontend dist folder to wwwroot
COPY --from=build-frontend /app/frontend/dist ./wwwroot/vue-dist

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5064
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 5064

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:5064/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "ChenClanWeb.dll"]
