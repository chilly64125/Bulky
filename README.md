# 陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台

## Chen Family Ancestral Temple & Cemetery Position Management Platform

## Project Overview

This project represents a comprehensive transformation of a .NET Core 8.0 MVC application into a modern Vue 3 TypeScript single-page application (SPA). The system manages Chinese ancestral memorial positions, cemetery tower positions, and cultural heritage operations for the Chen Family ancestral temple.

## Key Documents

### 📋 Analysis Documents

- **[PROJECT_ANALYSIS.md](./PROJECT_ANALYSIS.md)** - Comprehensive analysis of the MVC project structure, permission system, database configuration, and transformation roadmap
- **[IMPLEMENTATION_GUIDE.md](./IMPLEMENTATION_GUIDE.md)** - Step-by-step implementation guide for developers

### 🎯 Project Status

| Phase                         | Status             | Notes                                                                 |
| ----------------------------- | ------------------ | --------------------------------------------------------------------- |
| Step 1: Analysis              | ✅ **COMPLETED**   | MVC structure, permissions, DB configs documented in PDF-ready format |
| Step 2: Vue Scaffold          | ✅ **COMPLETED**   | Core project structure, stores, services, routing configured          |
| Step 3: Component Development | ⏳ **IN PROGRESS** | View files and components to be created                               |
| Step 4: Testing               | 📋 **PENDING**     | Unit and E2E tests                                                    |
| Step 5: Docker & Azure        | 📋 **PENDING**     | Containerization and cloud deployment                                 |

## Directory Structure

```
Bulky/
├── PROJECT_ANALYSIS.md           # Comprehensive system analysis
├── IMPLEMENTATION_GUIDE.md        # Developer implementation guide
├── README.md                      # This file
│
├── BulkyWeb/                      # .NET MVC Backend (existing)
│   ├── Areas/Admin/
│   ├── Areas/Customer/
│   └── Areas/Identity/
│
├── Bulky.Models/                  # Data models
├── Bulky.DataAccess/              # Repository pattern
├── Bulky.Utility/                 # Shared utilities
│
└── vue-frontend/                  # NEW: Vue 3 Frontend
    ├── src/
    │   ├── components/            # Vue components (organized by feature)
    │   ├── views/                 # Page-level components
    │   ├── stores/                # Pinia state management
    │   ├── services/              # API integration layer
    │   ├── types/                 # TypeScript interfaces
    │   ├── router/                # Vue Router configuration
    │   ├── composables/           # Vue composables
    │   ├── utils/                 # Utility functions
    │   ├── assets/                # CSS, images, etc.
    │   ├── App.vue                # Root component
    │   └── main.ts                # Entry point
    ├── package.json               # Dependencies
    ├── tsconfig.json              # TypeScript config
    ├── vite.config.ts             # Vite build config
    └── Dockerfile                 # Docker configuration
```

## Technology Stack

### Backend (Existing)

```
ASP.NET Core 8.0
Entity Framework Core
SQL Server
Identity Framework
Stripe Payments
SendGrid Email
```

### Frontend (New)

```
Vue 3 + TypeScript
Vite (Build tool)
Pinia (State management)
Vue Router 4 (Routing)
Axios (HTTP client)
Bootstrap 5 (UI Framework)
Vitest (Testing)
```

## Key Features Implemented

### ✅ Core Infrastructure

- [x] Vite build configuration
- [x] TypeScript configuration
- [x] Pinia stores (auth, config, notifications, session)
- [x] Axios HTTP client with interceptors
- [x] Vue Router with protected routes
- [x] Type definitions for all entities

### ✅ Layout & Navigation

- [x] Navbar with role-based menus
- [x] Responsive sidebar
- [x] Toast notification system
- [x] Auto-logout warning modal

### ✅ Authentication

- [x] Auth store with login/register/logout
- [x] JWT token handling
- [x] Automatic token refresh
- [x] Session timeout detection

### ✅ API Integration

- [x] Auth service
- [x] Ancestral service
- [x] Kindness service
- [x] Generic CRUD service factory
- [x] Config service

### ✅ Permission System

- [x] Role-based access control (Admin, Customer, Guest)
- [x] Route guards
- [x] Dynamic menu visibility
- [x] Component-level permission checks

## Getting Started

### Prerequisites

- Node.js 18+
- npm or yarn
- .NET 8.0 SDK (for backend)
- Visual Studio Code

### Frontend Setup

```powershell
cd vue-frontend

# Install dependencies
npm install

# Create environment file
Copy-Item .env.example .env.local

# Edit .env.local with your backend URL
# VITE_API_BASE_URL=http://localhost:5000/api

# Start development server
npm run dev
```

Frontend will be available at: `http://localhost:5173`

### Backend Setup

```powershell
cd BulkyWeb

# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update

# Run the application
dotnet run
```

Backend API will be available at: `http://localhost:5000`

## Project Modules

### 1. **Ancestral (陳氏宗祠-祖先牌位)**

- 10×10 position grid per section
- Multiple sections per side (Left/Right)
- Total 1,000 positions
- CRUD operations with occupancy tracking

### 2. **Kindness (懷恩塔-塔位)**

- 3-floor tower (1F, 2F, 3F)
- 6 sections per floor
- Dynamic grid sizes (4×6 or 7×7)
- Total 582 positions

### 3. **Event Management (活動管理)**

- Categories, Companies, Products
- Event registration system
- Order management

### 4. **User Management (會員管理)**

- Role-based access control
- User CRUD operations
- Permission assignment

## Role-Based Access

### Admin Role

- Full access to all modules
- Can manage positions (Ancestral & Kindness)
- Can manage users and roles
- Can manage events and categories

### Customer Role

- Query positions (read-only)
- Register for events
- View survey results
- Limited dashboard access

### Guest Role (Unauthenticated)

- Limited public pages only

## API Endpoints

### Authentication

```
POST   /api/auth/login
POST   /api/auth/register
POST   /api/auth/logout
GET    /api/auth/current-user
POST   /api/auth/refresh-token
```

### Ancestral

```
GET    /api/ancestral
GET    /api/ancestral/{id}
POST   /api/ancestral
PUT    /api/ancestral/{id}
DELETE /api/ancestral/{id}
GET    /api/ancestral/positions/query
GET    /api/ancestral/occupancy
```

### Kindness

```
GET    /api/kindness
GET    /api/kindness/{id}
POST   /api/kindness
PUT    /api/kindness/{id}
DELETE /api/kindness/{id}
GET    /api/kindness/positions/query
GET    /api/kindness/occupancy/{floor}
```

### Configuration

```
GET    /api/config/app
GET    /api/config/ancestral
GET    /api/config/kindness
GET    /api/config/logout
```

_See [PROJECT_ANALYSIS.md](./PROJECT_ANALYSIS.md) for complete API documentation_

## Development Workflow

### 1. Create Feature Branches

```powershell
git checkout -b feature/ancestral-module
```

### 2. Implement Component

- Create Vue components in `src/components/`
- Create views in `src/views/`
- Add TypeScript types
- Implement API calls using services

### 3. Test Locally

```powershell
npm run dev
```

### 4. Build for Production

```powershell
npm run build
```

### 5. Commit and Push

```powershell
git add .
git commit -m "feat: add ancestral module"
git push origin feature/ancestral-module
```

## Docker Deployment

### Build Docker Image

```powershell
docker build -t bulky-frontend:latest .
```

### Run Docker Container

```powershell
docker run -p 3000:80 bulky-frontend:latest
```

## Azure Deployment

### Prerequisites

- Azure subscription
- Azure Container Registry
- Azure App Service

### Steps

1. Create resource group
2. Create SQL Database
3. Create App Service Plan
4. Build and push Docker image
5. Configure Web App
6. Setup environment variables
7. Deploy application

See [IMPLEMENTATION_GUIDE.md](./IMPLEMENTATION_GUIDE.md) for detailed instructions.

## Configuration Files

### Development Settings

- `appsettings.Development.json` - Local development config
- `.env.local` - Vue frontend environment

### Production Settings

- `appsettings.Production.json` - Azure production config
- Environment variables in Azure App Service

### Key Configuration Values

```json
{
  "Logout_Duration": {
    "AUTO_LOGOUT_MINUTE": 3.0,
    "WARNING_BEFORE_LOGOUT_SECOND": 5
  },
  "Kindness": {
    "Floor": 3,
    "Section": 6,
    "Level_1f_2f": 4,
    "Level_3f": 7
  },
  "Ancestral": {
    "Side": 2,
    "Section": 4,
    "Level": 10,
    "Position": 10
  }
}
```

## Common Commands

### Frontend

```powershell
npm run dev           # Start dev server
npm run build         # Production build
npm run preview       # Preview build
npm run lint          # Lint code
npm run test          # Run tests
npm run test:coverage # Test with coverage
```

### Backend

```powershell
dotnet run            # Run application
dotnet build          # Build project
dotnet test           # Run tests
dotnet ef migrations add [Name]  # Create migration
dotnet ef database update        # Apply migrations
```

## Troubleshooting

### CORS Errors

- Check backend CORS configuration
- Verify `VITE_API_BASE_URL` in `.env.local`
- Ensure both applications are running

### Module Not Found

- Run `npm install`
- Clear node_modules: `rm -r node_modules` then `npm install`
- Check import paths (use `@/` alias)

### Port Already in Use

```powershell
# Find and kill process on port 5173
Get-Process | Where-Object Port -eq 5173 | Stop-Process
```

### Authentication Not Working

- Verify backend auth endpoints return correct JWT format
- Check token storage in browser localStorage
- Look at Network tab in DevTools for token inclusion

## Performance Optimization

### Code Splitting

- Routes are lazy-loaded automatically
- Large components use async imports

### Bundle Optimization

- Bootstrap split into separate chunk
- Vue split into separate chunk
- Tree-shaking enabled for unused imports

### Caching

- Config cached in localStorage (1 hour TTL)
- API responses cached with appropriate headers
- Service worker ready for offline support (future)

## Security Considerations

### Authentication

- JWT tokens stored in localStorage
- Refresh tokens for long-term sessions
- Automatic logout on token expiration

### Authorization

- Role-based access control on routes
- Component-level permission checks
- API-level authorization enforcement

### Data Protection

- HTTPS in production
- Sensitive data never logged
- Input validation on client and server

## Contributing

### Code Style

- Follow Vue 3 Composition API style
- Use TypeScript for type safety
- Use Prettier for formatting
- Use ESLint for linting

### Commit Messages

```
feat: add new feature
fix: fix bug
docs: update documentation
style: code style changes
refactor: code refactoring
test: add/update tests
chore: maintenance tasks
```

## Support & Documentation

- **Vue 3 Docs:** https://vuejs.org
- **Vite Docs:** https://vitejs.dev
- **Pinia Docs:** https://pinia.vuejs.org
- **Vue Router Docs:** https://router.vuejs.org
- **Bootstrap Docs:** https://getbootstrap.com

## Timeline

### Phase 1: Analysis ✅

- **Duration:** 1 week
- **Completed:** ✅ PROJECT_ANALYSIS.md

### Phase 2: Scaffold ✅

- **Duration:** 1 week
- **Completed:** ✅ Vue project structure, stores, services

### Phase 3: Component Development (Current)

- **Duration:** 3-4 weeks
- **Status:** Starting
- **Deliverables:** All view and component files

### Phase 4: Testing

- **Duration:** 2 weeks
- **Status:** Pending

### Phase 5: Deployment

- **Duration:** 2 weeks
- **Status:** Pending

## Total Estimated Timeline: 16-24 weeks (4-6 months)

## License

This project is part of the Bulky system and follows the same license as the main application.

## Contact & Support

For questions or issues with the Vue frontend transformation:

- Review [PROJECT_ANALYSIS.md](./PROJECT_ANALYSIS.md)
- Check [IMPLEMENTATION_GUIDE.md](./IMPLEMENTATION_GUIDE.md)
- Refer to inline code documentation

---

**Project Started:** 2025-12-01  
**Last Updated:** 2025-12-01  
**Status:** Phase 2 Complete - Ready for Phase 3 Development

**Created by:** GitHub Copilot  
**For:** Bulky MVC to Vue Transformation Project
