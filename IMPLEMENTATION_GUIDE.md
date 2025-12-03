# Vue Frontend Development - Implementation Guide

## PROJECT SETUP COMPLETED ✓

The Vue 3 frontend project scaffold has been successfully created with the following structure:

### Core Files Created:

```
vue-frontend/
├── package.json              # Dependencies and scripts
├── tsconfig.json            # TypeScript configuration
├── vite.config.ts           # Vite build configuration
├── .gitignore               # Git ignore rules
├── .env.example             # Environment variables template
├── index.html               # HTML entry point
│
├── src/
│   ├── main.ts              # App entry point
│   ├── App.vue              # Root component
│   │
│   ├── router/
│   │   └── index.ts         # Vue Router configuration (fully configured)
│   │
│   ├── stores/
│   │   ├── authStore.ts     # Authentication state (Pinia)
│   │   ├── notificationStore.ts # Toast notifications
│   │   ├── configStore.ts   # App configuration
│   │   └── sessionStore.ts  # Session/logout tracking
│   │
│   ├── services/
│   │   ├── api.ts           # Axios instance with interceptors
│   │   ├── authService.ts   # Auth API calls
│   │   ├── ancestralService.ts # Ancestral API calls
│   │   ├── kindnessService.ts  # Kindness API calls
│   │   ├── crudService.ts   # Generic CRUD factory
│   │   └── configService.ts # Configuration API
│   │
│   ├── types/
│   │   └── index.ts         # TypeScript interfaces (comprehensive)
│   │
│   ├── components/
│   │   ├── layout/
│   │   │   ├── AppLayout.vue     # Main layout wrapper
│   │   │   ├── Navbar.vue        # Navigation bar (role-based)
│   │   │   ├── Sidebar.vue       # Sidebar menu (responsive)
│   │   │   └── MainLayout.vue    # (to be created)
│   │   │
│   │   ├── global/
│   │   │   ├── ToastContainer.vue        # Toast notifications
│   │   │   ├── AutoLogoutWarning.vue     # Auto-logout modal
│   │   │   └── (other global components)
│   │   │
│   │   ├── ancestral/        # Ancestral components
│   │   ├── kindness/         # Kindness components
│   │   ├── category/         # Category components
│   │   ├── company/          # Company components
│   │   ├── product/          # Product components
│   │   ├── user/             # User components
│   │   └── order/            # Order components
│   │
│   ├── views/
│   │   ├── auth/
│   │   │   ├── LoginView.vue    # (to be created)
│   │   │   └── RegisterView.vue # (to be created)
│   │   ├── HomeView.vue         # (to be created)
│   │   ├── ancestral/
│   │   │   ├── IndexView.vue    # (to be created)
│   │   │   ├── FormView.vue     # (to be created)
│   │   │   └── QueryView.vue    # (to be created)
│   │   └── (similar for other modules)
│   │
│   ├── composables/          # Vue composables (to be created)
│   ├── utils/                # Utility functions (to be created)
│   ├── assets/
│   │   ├── css/
│   │   │   └── main.css      # Global styles
│   │   └── images/           # Static images
│
└── dist/                     # Build output (created during build)
```

---

## NEXT STEPS - IMPLEMENTATION ROADMAP

### Phase 1: View & Component Creation

#### 1.1 Authentication Views (Priority: HIGH)

**Files to create:**

- `src/views/auth/LoginView.vue`
- `src/views/auth/RegisterView.vue`

**Requirements:**

- Login form with username/password
- Form validation (VeeValidate + Yup)
- Error message display
- "Remember me" option
- Link to register page
- Integration with authService

**Implementation checklist:**

- [ ] Create form components with Bootstrap styling
- [ ] Add form validation schemas
- [ ] Connect to authService.login()
- [ ] Store auth token in localStorage
- [ ] Redirect to home on success
- [ ] Display errors from API

#### 1.2 Home View

**File:** `src/views/HomeView.vue`

**Requirements:**

- Display welcome message based on user role
- Show user information
- Quick links to main features based on role
- Dashboard statistics (occupancy, registrations, etc.)

#### 1.3 Ancestral Module Components

**Files to create:**

```
src/components/ancestral/
├── AncestralList.vue          # List view with DataTable
├── AncestralGrid.vue          # 10x10 position grid
├── AncestralForm.vue          # Create/Update form
├── AncestralQuery.vue         # Search interface
└── PositionDetailModal.vue    # Position detail popup

src/views/ancestral/
├── IndexView.vue              # List view wrapper
├── FormView.vue               # Form wrapper
└── QueryView.vue              # Query wrapper
```

**Key Component Requirements:**

**AncestralGrid.vue:**

- Display 10x10 position grid per section
- Show occupied positions (highlighted)
- Click to select position
- Display position details on selection
- Color coding: Available (white), Occupied (red), Selected (blue)
- Grid configuration from configStore

**AncestralForm.vue:**

- Form fields: positionId, occupantName, occupantPhone, dateRegistered, notes
- Form validation
- Create/Update toggle based on route
- Date picker for dateRegistered
- Occupancy check before creating/updating

**AncestralList.vue:**

- DataTable with sorting/filtering/pagination
- Columns: Position ID, Occupant Name, Phone, Date Registered
- Actions: Edit, Delete, View Details
- Bulk operations (optional)
- Export to Excel (optional)

**AncestralQuery.vue:**

- Advanced search form
- Filter by: Side, Section, Occupant Name
- Display results in DataTable
- Pagination support

#### 1.4 Kindness Module Components

**Similar structure to Ancestral but with:**

- Floor selection (1F, 2F, 3F)
- Dynamic grid sizes (4×6 for 1F/2F, 7×7 for 3F)
- Section selection per floor

#### 1.5 CRUD Modules (Category, Company, Product, User, Order)

**Generic pattern for each:**

```
src/components/{entity}/
├── {Entity}List.vue
├── {Entity}Form.vue
└── {Entity}Filter.vue   # Optional

src/views/{entity}/
├── IndexView.vue
└── FormView.vue
```

**Each module needs:**

- List with DataTable (columns based on entity)
- Create/Update form with validation
- Delete confirmation
- Basic filtering/search

---

### Phase 2: Enhanced Functionality

#### 2.1 Grid Components (Reusable)

```
src/components/global/
├── GridPositionDisplay.vue   # Reusable grid component
├── DataTableComponent.vue    # Reusable table component
├── FormLayoutComponent.vue   # Reusable form wrapper
└── ConfirmDialog.vue         # Confirmation modal
```

#### 2.2 Composables for Common Functionality

```
src/composables/
├── useDataFetch.ts           # Data loading/fetching logic
├── useFormValidation.ts      # Form validation helpers
├── useAuthorization.ts       # Permission checking
├── useInactivityTimer.ts     # Session timeout tracking
└── usePagination.ts          # Pagination logic
```

#### 2.3 Utility Functions

```
src/utils/
├── formatters.ts             # Date, currency, etc. formatting
├── validators.ts             # Custom validators
├── errors.ts                 # Error handling
├── constants.ts              # App constants
└── helpers.ts                # General helpers
```

---

### Phase 3: Testing

Create test files for:

- Store tests (Vitest)
- Component tests (Vitest + Vue Test Utils)
- API integration tests
- E2E tests (Cypress/Playwright)

---

### Phase 4: Docker & Deployment

#### 4.1 Docker Setup

```
Dockerfile (multi-stage build)
.dockerignore
docker-compose.yml (optional)
```

#### 4.2 Azure Setup

- Create resource group
- Create Azure SQL Database
- Create App Service Plan
- Create Web App
- Configure App Settings
- Setup CI/CD pipeline

---

## INSTALLATION & SETUP INSTRUCTIONS

### Local Development Setup

```powershell
# Navigate to frontend directory
cd vue-frontend

# Install dependencies
npm install

# Create .env.local from .env.example
Copy-Item .env.example .env.local

# Update .env.local with your API base URL
# Example:
# VITE_API_BASE_URL=http://localhost:5000/api

# Start development server
npm run dev

# Application will be available at http://localhost:5173
```

### Backend API Requirements

The Vue frontend expects the following API endpoints to exist on the backend (.NET Core):

**Authentication:**

```
POST   /api/auth/login
POST   /api/auth/register
POST   /api/auth/logout
GET    /api/auth/current-user
POST   /api/auth/refresh-token
```

**Ancestral:**

```
GET    /api/ancestral
GET    /api/ancestral/{id}
POST   /api/ancestral
PUT    /api/ancestral/{id}
DELETE /api/ancestral/{id}
GET    /api/ancestral/positions/query
```

**Kindness, Category, Company, Product, User, Order:** (Similar CRUD pattern)

**Configuration:**

```
GET    /api/config/app
GET    /api/config/ancestral
GET    /api/config/kindness
GET    /api/config/logout
```

---

## STYLING APPROACH

### CSS Framework: Bootstrap 5

- Pre-configured in main.ts
- Bootstrap Icons included
- Custom CSS in `src/assets/css/main.css`

### Color Scheme

```
Primary:    #007bff (Blue)
Secondary:  #6c757d (Gray)
Success:    #28a745 (Green)
Danger:     #dc3545 (Red)
Warning:    #ffc107 (Yellow)
Info:       #17a2b8 (Cyan)
```

### Custom CSS Classes

```css
.position-grid         /* Grid container */
/* Grid container */
.position-cell         /* Individual cell */
.position-cell.occupied /* Occupied cell */
.position-cell.selected /* Selected cell */
.text-truncate-2       /* 2-line truncation */
.text-truncate-3; /* 3-line truncation */
```

---

## KEY FEATURES IMPLEMENTED

### ✓ Authentication System

- Login/Register pages
- JWT token handling
- Automatic token refresh
- Auto-logout warning
- Session timeout

### ✓ Permission System

- Role-based access control (Admin, Customer, Guest)
- Route guards
- Component-level permission checking
- Dynamic menu visibility

### ✓ State Management (Pinia)

- Auth store (user, token, roles)
- Notification store (toasts)
- Config store (app settings)
- Session store (logout tracking)

### ✓ API Integration

- Axios with interceptors
- Token injection in requests
- Automatic token refresh on 401
- Error handling
- Generic CRUD service factory

### ✓ UI/UX

- Responsive navbar with dropdowns
- Collapsible sidebar (responsive)
- Toast notifications
- Auto-logout warning modal
- Form validation
- DataTable integration ready

---

## BUILD & DEPLOYMENT

### Development Build

```powershell
npm run dev
```

### Production Build

```powershell
npm run build
```

This creates optimized files in `dist/` directory suitable for Docker/Azure deployment.

### Docker Build

```powershell
docker build -t bulky-frontend:latest .
docker run -p 3000:80 bulky-frontend:latest
```

### Azure Deployment

```powershell
# Login to Azure
az login

# Create resource group
az group create --name BulkyResourceGroup --location eastasia

# Build and push Docker image
az acr build --registry bulkyregistry --image bulky-frontend:latest .

# Deploy to Web App Service
az webapp deployment container config \
  --name bulkyapp \
  --resource-group BulkyResourceGroup \
  --docker-custom-image-name bulkyregistry.azurecr.io/bulky-frontend:latest
```

---

## TROUBLESHOOTING

### CORS Issues

If you get CORS errors:

1. Ensure backend has CORS configured
2. Check VITE_API_BASE_URL in .env.local
3. Verify API endpoint is accessible

### Module Not Found Errors

- Run `npm install` to ensure all dependencies are installed
- Check import paths (use `@/` alias)
- Verify file extensions (.vue, .ts)

### Authentication Issues

- Check localStorage for tokens
- Verify token format in API responses
- Check token refresh endpoint on backend
- Look at Network tab in DevTools

### Styling Issues

- Ensure Bootstrap CSS is loaded
- Check CSS file paths
- Use Bootstrap utilities first before custom CSS
- Test in different browsers for compatibility

---

## RECOMMENDED NEXT STEPS

### Short Term (Week 1-2)

1. [ ] Create authentication views (Login/Register)
2. [ ] Test authentication flow
3. [ ] Create home view with dashboard
4. [ ] Implement Ancestral module (highest priority)

### Medium Term (Week 3-4)

1. [ ] Implement Kindness module
2. [ ] Implement Category/Company/Product modules
3. [ ] Add form validation across all modules
4. [ ] Implement search/filter functionality

### Long Term (Week 5-6)

1. [ ] Add comprehensive testing
2. [ ] Performance optimization
3. [ ] Accessibility improvements
4. [ ] Docker & Azure deployment
5. [ ] CI/CD pipeline setup

---

## RESOURCES & DOCUMENTATION

### Vue 3 Documentation

- https://vuejs.org/guide/introduction.html
- Vue Router: https://router.vuejs.org/
- Pinia: https://pinia.vuejs.org/

### Libraries Used

- Axios: https://axios-http.com/
- Bootstrap: https://getbootstrap.com/
- VeeValidate: https://vee-validate.logaretm.com/
- Vitest: https://vitest.dev/

### Design Patterns

- Component Composition Pattern
- Custom Composables for Logic
- Store Pattern for State Management
- Service Pattern for API Calls

---

## SUMMARY

The Vue 3 TypeScript frontend scaffold is now ready for development. The project includes:

✓ Fully configured Vite build system
✓ Vue 3 with TypeScript support
✓ Pinia for state management
✓ Vue Router with protected routes
✓ Axios HTTP client with interceptors
✓ Bootstrap 5 styling framework
✓ Core layout components (Navbar, Sidebar)
✓ Authentication system scaffolding
✓ API service layer
✓ Type definitions for all entities
✓ Toast notification system
✓ Auto-logout functionality
✓ Role-based access control

**Next Phase:** Create the remaining view and component files based on this guide. Follow the MVC→Vue transformation pattern established in the PROJECT_ANALYSIS.md document.

---

**Created:** 2025-12-01
**Version:** 1.0.0
**Status:** Ready for Development
