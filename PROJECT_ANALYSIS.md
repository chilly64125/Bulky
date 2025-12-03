# MVC TO VUE TRANSFORMATION ANALYSIS

## Bulky Project - Comprehensive Architecture & Planning Document

---

## TABLE OF CONTENTS

1. [Project Overview](#project-overview)
2. [Technology Stack](#technology-stack)
3. [Permission System](#permission-system)
4. [Database Layout Configuration](#database-layout-configuration)
5. [MVC Project Structure](#mvc-project-structure)
6. [Key Features](#key-features)
7. [Transformation Roadmap](#transformation-roadmap)
8. [Backend API Endpoints](#backend-api-endpoints)
9. [Recommendations](#recommendations)
10. [Deployment Strategy](#deployment-strategy)

---

## PROJECT OVERVIEW

This is a comprehensive .NET Core 8.0 MVC application designed for managing Chinese ancestral memorial systems. The system supports:

- **Ancestral Hall (陳氏宗祠)**: Traditional tablet management for ancestors
- **Kindness Tower (懷恩塔)**: Modern cremation position management
- **Event Management**: Activity registration and tracking
- **User Management**: Multi-role permission system
- **Survey System**: Durability and usage questionnaires

### Current Statistics

- **6-7 Main View Sections** (Views directories)
- **6-7 Controllers** (one per section)
- **3 Main Roles**: Admin, Customer, Guest
- **Complex Position Grids**: Configurable via appsettings

---

## TECHNOLOGY STACK

### Backend

```
Framework: ASP.NET Core 8.0
Pattern: MVC (Model-View-Controller)
Database: SQL Server / LocalDB
ORM: Entity Framework Core
Authentication: ASP.NET Core Identity
```

### Frontend (Current - Razor-based)

```
View Engine: Razor (.cshtml)
CSS Framework: Bootstrap 5
JavaScript: Vanilla JS + jQuery
DataTables: 2.3.2 (for tables)
Modals: SweetAlert2
Rich Text: TinyMCE 7
Icons: Bootstrap Icons 1.10.3
Notifications: Toastr
```

### External Services

```
Email: SendGrid
Payments: Stripe
```

---

## PERMISSION SYSTEM (3-TIER ROLE MODEL)

### Role Hierarchy

#### 1. ADMIN ROLE (`SD.Role_Admin`)

**Access Level**: Full system administration

**Menu Sections**:

```
一. 活動管理 (Event Management)
├── 活動類別檔 (Category Controller)
├── 宗親會基本檔 (Company Controller)
├── 活動基本檔 (Product Controller)
├── 活動報名 (EventRegistration)
└── 會員管理 (User Controller)

二. 懷恩塔-祖先塔位管理 (Kindness Tower)
├── 懐恩塔-塔位-新增 (Add Position)
├── 懐恩塔-塔位-查詢 (Query Position)
└── 會員管理 (User Management)

三. 陳氏宗祠-祖先牌位管理 (Ancestral Hall)
├── 陳氏宗祠-牌位-新增 (Add Tablet)
├── 陳氏宗祠-牌位-查詢 (Query Tablet)
└── 會員管理 (User Management)
```

#### 2. CUSTOMER ROLE (`SD.Role_Customer`)

**Access Level**: Limited user functions

**Menu Items**:

```
首頁 (Home - limited view)
懐恩塔-塔位查詢 (Kindness Tower - Query)
陳氏宗祠-牌位查詢 (Ancestral Hall - Query)
問卷調查 (Survey System)
├── [耐用性]問卷調查 (Durability Questionnaire)
└── [耐用性]調查結果 (Survey Results)
```

#### 3. GUEST ROLE (Anonymous/Unauthenticated)

**Access Level**: Read-only, limited views

```
首頁 (Home - public view)
```

---

## DATABASE LAYOUT CONFIGURATION

### Configuration Files

All layouts are defined in environment-specific appsettings files:

- `appsettings.json` (Base configuration)
- `appsettings.Development.json` (Development overrides)
- `appsettings.Production.json` (Production overrides)

### A) Kindness Tower (懷恩塔) Layout

#### Structure

```
Floors: 3
├── 1F (1樓)
├── 2F (2樓)
└── 3F (3樓)

Sections per Floor: 6
├── 甲區 (Section A)
├── 乙區 (Section B)
├── 丙區 (Section C)
├── 丁區 (Section D)
├── 戊區 (Section E)
└── 己區 (Section F)

Positions:
├── 1F-2F: 4 rows × 6 columns = 24 positions per section
└── 3F: 7 rows × 7 columns = 49 positions per section

Total Positions:
- 1F: 6 × 24 = 144 positions
- 2F: 6 × 24 = 144 positions
- 3F: 6 × 49 = 294 positions
- Grand Total: 582 positions
```

#### Configuration Keys

```json
"Kindness": {
  "Layout_1F": "1樓-甲區,1樓-乙區,1樓-丙區,1樓-丁區,1樓-戊區,1樓-己區",
  "Layout_2F": "2樓-甲區,2樓-乙區,2樓-丙區,2樓-丁區,2樓-戊區,2樓-己區",
  "Layout_3F": "3樓-甲區,3樓-乙區,3樓-丙區,3樓-丁區,3樓-戊區,3樓-己區",
  "Floor": 3,
  "Section": 6,
  "Level_1f_2f": 4,
  "Level_3f": 7,
  "Position": 7,
  "f1a": { "row": 4, "col": 6 },
  "f1b": { "row": 4, "col": 6 },
  // ... more section configs
}
```

### B) Ancestral Hall (陳氏宗祠) Layout

#### Structure

```
Sides: 2 (Left & Right)
├── Left Side (左側)
│   └── Sections: 甲區, 丙區, 戊區, 庚區, 中區 (middle)
│
└── Right Side (右側)
    └── Sections: 辛區, 己區, 丁區, 乙區, 中區 (middle)

Levels (Rows): 10
Positions per Level (Columns): 10

Position Grid per Section: 10 × 10 = 100 positions
Total Positions per Side: 5 × 100 = 500 positions
Grand Total: 1,000 positions
```

#### Configuration Keys

```json
"Ancestral": {
  "Layout_L": "辛區,己區,丁區,乙區,中區",
  "Layout_R": "甲區,丙區,戊區,庚區,中區",
  "Layout": "辛區,己區,丁區,乙區,中區,甲區,丙區,戊區,庚區",
  "Side": 2,
  "Section": 4,
  "Level": 10,
  "Position": 10,
  "la": { "row": 10, "col": 10 },
  "lb": { "row": 10, "col": 10 },
  // ... more section configs
}
```

#### Position Numbering (Example for section Kf3a)

```
Row 7: 268,269,270,271,272,273,275
Row 6: 221,222,223,225,226,227,228
Row 5: 179,180,181,000,182,183,185
Row 4: 135,136,137,138,139,140,141
Row 3: 090,091,092,093,095,096,097
Row 2: 046,047,048,049,050,051,052
Row 1: 001,002,003,005,006,007,008
```

_Note: "000" indicates reserved/maintenance positions_

### C) Session & Auto-Logout Settings

```json
"Logout_Duration": {
  "AUTO_LOGOUT_MINUTE": 3.0,           // Logout after 3 minutes inactivity
  "WARNING_BEFORE_LOGOUT_SECOND": 5    // Warning 5-10 seconds before logout
},
"Work_Duration": 3.0,                   // Work session timeout
"Import_Duration": 3.0,                 // Import process timeout
"WORK_WARNING_SECONDS": 5,              // Warning period
"PublishDate": "2025/08/26 10:10"       // System update date
```

---

## MVC PROJECT STRUCTURE

### Directory Organization

```
BulkyWeb/
├── Areas/
│   ├── Admin/
│   │   ├── Controllers/
│   │   │   ├── AncestralController.cs
│   │   │   ├── KindnessController.cs
│   │   │   ├── CategoryController.cs
│   │   │   ├── CompanyController.cs
│   │   │   ├── ProductController.cs
│   │   │   ├── UserController.cs
│   │   │   └── OrderController.cs
│   │   └── Views/
│   │       ├── Ancestral/
│   │       ├── Kindness/
│   │       ├── Category/
│   │       ├── Company/
│   │       ├── Product/
│   │       ├── User/
│   │       └── Order/
│   ├── Customer/
│   │   └── ... (Customer-specific views)
│   └── Identity/
│       └── ... (Authentication pages)
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml (Main navbar)
│   │   ├── _LoginPartial.cshtml
│   │   ├── _Notification.cshtml
│   │   └── ...
│   └── ...
└── wwwroot/
    ├── css/
    ├── js/
    └── images/
```

### Controllers & Views Breakdown

#### 1. Ancestral Controller (陳氏宗祠-祖先牌位)

**Primary Function**: Manage ancestral memorial tablets

| View                     | Purpose                                         |
| ------------------------ | ----------------------------------------------- |
| `Index.cshtml`           | Display all ancestral positions in grid format  |
| `Application.cshtml`     | Application form for new tablet registration    |
| `DisplayPosition.cshtml` | Show detailed information for specific position |
| `PositionQuery.cshtml`   | Search/query ancestral positions                |
| `Upsert.cshtml`          | Create or update ancestral position             |

**Key Methods**:

- `Index()` - List all positions
- `Application(int? AncestralPositionId)` - Show application form
- `DisplayPosition(int? AncestralPositionId)` - View position details
- `PositionQuery()` - Search positions
- `Upsert(int? id)` - Create/update form
- `UpsertAncestral(AncestralPosition obj)` - Save operation

#### 2. Kindness Controller (懷恩塔-塔位)

**Primary Function**: Manage kindness tower positions

| View                     | Purpose                                    |
| ------------------------ | ------------------------------------------ |
| `Index.cshtml`           | Display all tower positions in grid format |
| `Application.cshtml`     | Application form for position purchase     |
| `DisplayPosition.cshtml` | Show position details and occupant info    |
| `PositionQuery.cshtml`   | Search tower positions                     |
| `Upsert.cshtml`          | Create or update position record           |

_Similar structure to Ancestral, adapted for tower layout_

#### 3. Category Controller (活動類別)

**Primary Function**: Manage event categories

| View            | Purpose                   |
| --------------- | ------------------------- |
| `Index.cshtml`  | List all event categories |
| `Upsert.cshtml` | Create or update category |

**Operations**: List, Create, Update, Delete

#### 4. Company Controller (宗親會基本檔)

**Primary Function**: Manage society/organization information

| View            | Purpose                          |
| --------------- | -------------------------------- |
| `Index.cshtml`  | List all companies/organizations |
| `Upsert.cshtml` | Create or update company info    |

**Operations**: List, Create, Update, Delete

#### 5. Product Controller (活動基本檔)

**Primary Function**: Manage activities/events

| View            | Purpose                      |
| --------------- | ---------------------------- |
| `Index.cshtml`  | List all products/activities |
| `Upsert.cshtml` | Create or update activity    |

**Operations**: List, Create, Update, Delete

#### 6. User Controller (會員管理)

**Primary Function**: Manage users and roles

| View            | Purpose                           |
| --------------- | --------------------------------- |
| `Index.cshtml`  | List all users with roles         |
| `Upsert.cshtml` | Manage user roles and permissions |

**Operations**: List users, Assign roles, Update permissions

#### 7. Order Controller (Order Management)

**Primary Function**: Manage orders

| View             | Purpose                        |
| ---------------- | ------------------------------ |
| `Index.cshtml`   | List all orders                |
| `Details.cshtml` | View order details and history |

#### 8. EventRegistration Controller (活動報名)

**Primary Function**: Manage event registrations

| View             | Purpose                   |
| ---------------- | ------------------------- |
| `Index.cshtml`   | List event registrations  |
| `Details.cshtml` | View registration details |

### Shared Views Structure

#### \_Layout.cshtml (Main Template)

**Includes**:

- Navigation bar with brand logo
- Role-based menu items
- Bootstrap and CSS frameworks
- JavaScript libraries (jQuery, DataTables, SweetAlert2, TinyMCE)
- Auto-logout functionality
- Session tracking

**Key Elements**:

```html
<!-- Head Section -->
<meta> tags, Bootstrap CSS, DataTables CSS, custom CSS

<!-- Body Section -->
<header>
  <nav> (Role-based menu)
    <li> (Home)
    <li> (Event Management - Admin only)
    <li> (Kindness Tower - Admin only)
    <li> (Ancestral Hall - Admin only)
    <li> (Queries - Customer)
    <li> (Survey - Customer)
    <partial> (_LoginPartial)

<div class="container">
  <partial> (_Notification) - Toast messages
  @RenderBody()

<script section> - All JS libraries
```

#### \_LoginPartial.cshtml

- User authentication status
- Profile dropdown
- Logout button
- User info display

#### \_Notification.cshtml

- Toast message display area
- Error/warning/success notifications

---

## KEY FEATURES

### 1. Grid Visualization

**Purpose**: Display complex position layouts in interactive grids

**Features**:

- Dynamic grid rendering based on configuration
- Color-coded occupied vs. available positions
- Click to select position
- Occupancy tracking
- Position detail tooltips

**Grids**:

- Ancestral: 10×10 per section (100 cells × 9 sections)
- Kindness: 4×6 or 7×7 per section (24-49 cells × 36 sections)

### 2. CRUD Operations

**Implemented for all 7 entities**:

- **Create**: Form validation, duplicate checking
- **Read**: List views with DataTables, detailed views
- **Update**: Form population, change tracking
- **Delete**: Confirmation dialogs, cascade handling

### 3. Permission-Based Rendering

```csharp
@if (User.IsInRole(SD.Role_Admin)) {
  // Admin-only menu items
}

@if (User.IsInRole(SD.Role_Customer)) {
  // Customer menu items
}
```

### 4. Navigation & Menu System

**Features**:

- Hierarchical dropdown menus
- Role-based visibility
- Active state highlighting
- Mobile responsive toggle

### 5. Auto-Logout System

**Features**:

- Inactivity detection (configurable minutes)
- Warning modal (N seconds before logout)
- Graceful session termination
- Redirect to login page

**Client-side Implementation**: JavaScript timer, AJAX heartbeat

### 6. Data Tables Integration

**Features**:

- Server-side pagination
- Column sorting
- Search/filtering
- Export functionality
- Responsive design

### 7. Form Validation

**Types**:

- Required field validation
- Data type validation (email, number, date)
- Custom validation rules
- Client-side (Razor/HTML5) and server-side (.NET)

### 8. File Upload/Import

**Used for**:

- Product images
- Data imports
- Document uploads

---

## TRANSFORMATION ROADMAP

### Phase 1: Planning & Analysis ✓

- [x] Analyze MVC structure
- [x] Document permission system
- [x] Identify all views and controllers
- [x] Map out data flows
- [x] Create API specifications

### Phase 2: Project Setup

**Deliverables**:

- [ ] Create Vue 3 + TypeScript project
- [ ] Setup build tools (Vite)
- [ ] Configure Axios HTTP client
- [ ] Setup Pinia for state management
- [ ] Configure Vue Router
- [ ] Install UI frameworks (Bootstrap, UI component libraries)
- [ ] Setup development environment

**Tech Stack**:

```
Frontend Framework: Vue 3
Language: TypeScript
Build Tool: Vite
State Management: Pinia
HTTP Client: Axios
Router: Vue Router 4
UI Framework: Bootstrap 5
Component Library: Vue Bootstrap / Tauri UI
Form Validation: VeeValidate
Date Picker: Vue DatePicker
Table: DataTables for Vue (or VueDataTables)
```

### Phase 3: Core Infrastructure

**Deliverables**:

- [ ] API client service (Axios wrapper)
- [ ] Authentication store (login/logout)
- [ ] Permission/authorization composable
- [ ] Configuration store (layout settings)
- [ ] Toast/notification system
- [ ] Error handling middleware
- [ ] Request interceptors (token injection, error handling)
- [ ] Response interceptors (error handling, token refresh)

### Phase 4: Layout & Navigation

**Deliverables**:

- [ ] Main Layout component
- [ ] Navigation/Navbar component (role-based)
- [ ] Sidebar/Menu component
- [ ] Footer component
- [ ] Login page
- [ ] Home/Dashboard page
- [ ] Auto-logout warning modal

**Requirements**:

- Responsive design (mobile-first)
- Dynamic menu based on user role
- Session timeout handling
- Logout functionality

### Phase 5: Entity Components - Ancestral (Priority 1)

**Deliverables**:

- [ ] Ancestral List component
- [ ] Ancestral Grid Display component (10×10 position grid)
- [ ] Ancestral Form component (Create/Update)
- [ ] Ancestral Position Query component
- [ ] Position detail modal/drawer
- [ ] Occupancy tracker
- [ ] Search/filter functionality

**API Integration**:

```typescript
GET / api / ancestral;
GET / api / ancestral / { id };
POST / api / ancestral;
PUT / api / ancestral / { id };
DELETE / api / ancestral / { id };
GET / api / ancestral / positions / query;
```

**Key Features**:

- Interactive grid with hover states
- Click to select position
- Show occupant details
- Form validation
- Real-time position availability check
- Export to Excel

### Phase 6: Entity Components - Kindness (Priority 2)

**Deliverables**:

- [ ] Kindness List component
- [ ] Kindness Grid Display component (floor/section layout)
- [ ] Kindness Form component
- [ ] Position query by floor/section
- [ ] Position detail modal
- [ ] 3D visualization option (optional enhancement)

**API Integration**: Similar to Ancestral

**Key Features**:

- Floor/section navigation
- Dynamic grid sizing (4×6 or 7×7)
- Visual layout preview
- Available positions highlight

### Phase 7: Entity Components - Others (Priority 3)

**Deliverables**:

- [ ] Category List & Form
- [ ] Company List & Form
- [ ] Product List & Form
- [ ] User Management component
- [ ] Order List component
- [ ] EventRegistration component

**Common Features** for all:

- DataTable with sorting/filtering
- Create/Update/Delete operations
- Form validation
- Bulk operations (optional)
- Export functionality

### Phase 8: Cross-Cutting Concerns

**Deliverables**:

- [ ] Permission guard composable
- [ ] Route-level authorization
- [ ] Auto-logout timer
- [ ] Configuration loader
- [ ] Notification system
- [ ] Error boundary component
- [ ] Loading spinner component
- [ ] Confirmation dialog component
- [ ] Toast messages
- [ ] API error handling

### Phase 9: Testing

**Deliverables**:

- [ ] Unit tests (Vitest) for components and stores
- [ ] Integration tests for API calls
- [ ] E2E tests (Cypress/Playwright)
- [ ] Auto-logout functionality tests
- [ ] Permission system tests
- [ ] Form validation tests

### Phase 10: Optimization & Polish

**Deliverables**:

- [ ] Performance optimization
- [ ] Bundle size optimization
- [ ] Lazy loading implementation
- [ ] Caching strategy
- [ ] SEO optimization (if needed)
- [ ] Accessibility audit (WCAG 2.1)
- [ ] Mobile responsiveness verification

### Phase 11: Deployment Preparation

**Deliverables**:

- [ ] Docker configuration
- [ ] Environment configuration
- [ ] Build process optimization
- [ ] CI/CD pipeline setup
- [ ] Pre-deployment checklist

### Phase 12: Azure Deployment

**Deliverables**:

- [ ] Azure Web App Service setup
- [ ] Docker image deployment
- [ ] Environment variables configuration
- [ ] SSL/TLS certificate setup
- [ ] Application Insights setup
- [ ] Monitoring and alerting
- [ ] Backup strategy

---

## BACKEND API ENDPOINTS

### Authentication Endpoints

```
POST   /api/auth/login                  - User login
POST   /api/auth/logout                 - User logout
POST   /api/auth/register               - User registration
GET    /api/auth/current-user           - Get current user info
POST   /api/auth/refresh-token          - Refresh JWT token
POST   /api/auth/change-password        - Change password
```

### Ancestral Endpoints

```
GET    /api/ancestral                   - Get all ancestral positions
GET    /api/ancestral/{id}              - Get position by ID
POST   /api/ancestral                   - Create new position
PUT    /api/ancestral/{id}              - Update position
DELETE /api/ancestral/{id}              - Delete position
GET    /api/ancestral/positions/query   - Query positions with filters
GET    /api/ancestral/occupancy         - Get occupancy statistics
POST   /api/ancestral/import            - Bulk import positions
```

### Kindness Endpoints

```
GET    /api/kindness                    - Get all tower positions
GET    /api/kindness/{id}               - Get position by ID
POST   /api/kindness                    - Create new position
PUT    /api/kindness/{id}               - Update position
DELETE /api/kindness/{id}               - Delete position
GET    /api/kindness/positions/query    - Query positions with filters
GET    /api/kindness/occupancy          - Get occupancy by floor/section
POST   /api/kindness/import             - Bulk import positions
```

### Category Endpoints

```
GET    /api/category                    - Get all categories
GET    /api/category/{id}               - Get category by ID
POST   /api/category                    - Create category
PUT    /api/category/{id}               - Update category
DELETE /api/category/{id}               - Delete category
```

### Company Endpoints

```
GET    /api/company                     - Get all companies
GET    /api/company/{id}                - Get company by ID
POST   /api/company                     - Create company
PUT    /api/company/{id}                - Update company
DELETE /api/company/{id}                - Delete company
```

### Product Endpoints

```
GET    /api/product                     - Get all products
GET    /api/product/{id}                - Get product by ID
POST   /api/product                     - Create product
PUT    /api/product/{id}                - Update product
DELETE /api/product/{id}                - Delete product
POST   /api/product/{id}/upload-image   - Upload product image
```

### User Endpoints

```
GET    /api/user                        - Get all users
GET    /api/user/{id}                   - Get user by ID
POST   /api/user                        - Create user
PUT    /api/user/{id}                   - Update user
DELETE /api/user/{id}                   - Delete user
POST   /api/user/{id}/roles             - Assign roles
GET    /api/user/roles                  - Get available roles
```

### Order Endpoints

```
GET    /api/order                       - Get all orders
GET    /api/order/{id}                  - Get order by ID
POST   /api/order                       - Create order
PUT    /api/order/{id}                  - Update order status
GET    /api/order/{id}/details          - Get order details
```

### Event Registration Endpoints

```
GET    /api/eventregistration           - Get all registrations
GET    /api/eventregistration/{id}      - Get registration by ID
POST   /api/eventregistration           - Create registration
PUT    /api/eventregistration/{id}      - Update registration
DELETE /api/eventregistration/{id}      - Cancel registration
```

### Configuration Endpoints

```
GET    /api/config/kindness             - Get kindness layout config
GET    /api/config/ancestral            - Get ancestral layout config
GET    /api/config/logout               - Get logout duration config
GET    /api/config/app                  - Get general app config
```

---

## RECOMMENDATIONS

### Architecture Enhancements

#### 1. API-First Design

- **Benefit**: Clear contract between frontend and backend
- **Implementation**:
  - Define OpenAPI/Swagger spec upfront
  - Use API versioning (v1, v2, etc.)
  - Document all endpoints with examples
  - Use consistent response format

#### 2. State Management

- **Use Pinia for**:
  - User authentication state
  - User permissions/roles
  - Application configuration
  - Notification queue
  - Toast messages
  - Sidebar collapsed state
- **Keep local component state for**:
  - Form inputs
  - Component-specific UI state
  - Temporary selections

#### 3. Component Architecture

**Reusable Components**:

```
GlobalComponents/
├── GridPositionDisplay.vue      - Reusable grid for positions
├── FormLayout.vue               - Standard form wrapper
├── DataTable.vue                - Reusable data table
├── ConfirmDialog.vue            - Confirmation modal
├── Toast.vue                    - Notification toast
├── LoadingSpinner.vue           - Loading indicator
├── ErrorBoundary.vue            - Error handling
├── PermissionCheck.vue          - Permission guard
└── AutoLogoutWarning.vue        - Session timeout modal

LayoutComponents/
├── Navbar.vue
├── Sidebar.vue
├── Footer.vue
└── MainLayout.vue

EntityComponents/
├── Ancestral/
│   ├── AncestralList.vue
│   ├── AncestralGrid.vue
│   ├── AncestralForm.vue
│   └── AncestralQuery.vue
├── Kindness/
│   ├── KindnessList.vue
│   ├── KindnessGrid.vue
│   ├── KindnessForm.vue
│   └── KindnessQuery.vue
// ... similar for other entities
```

#### 4. Security Enhancements

- **JWT Authentication**:

  - Short-lived access tokens (15-30 minutes)
  - Refresh tokens for long-term sessions
  - Token refresh on 401 response

- **CSRF Protection**:

  - Double-submit cookie pattern
  - SameSite cookie attribute

- **Input Validation**:

  - Client-side validation (user experience)
  - Server-side validation (security)
  - Sanitization of user inputs

- **Authorization**:
  - Role-based access control (RBAC)
  - Permission checks on API level
  - Route guards on frontend

### Performance Optimization

#### 1. Lazy Loading

- **Route-based code splitting**:

  ```typescript
  const AncestralView = () => import("../views/Ancestral/Index.vue");
  ```

- **Component lazy loading**:
  - Defer non-critical components
  - Load on-demand (modals, dropdowns)

#### 2. Data Caching

- **Configuration caching**:

  - Cache appsettings on app startup
  - Cache for session duration

- **List caching**:

  - Cache category/company lists (less frequently changing)
  - Invalidate on create/update/delete

- **Query result caching**:
  - Cache position query results with timeout
  - Allow manual refresh

#### 3. Asset Optimization

- **Code splitting**:

  - Separate vendor code
  - Lazy load routes
  - Tree shaking unused code

- **Image optimization**:

  - Use WebP format
  - Responsive images
  - Compression

- **CSS optimization**:
  - PurgeCSS to remove unused styles
  - Critical CSS inlining

### Testing Strategy

#### 1. Unit Tests (Vitest)

```
Unit tests for:
- Store/composables (Pinia stores)
- Utility functions
- Helper functions
- Custom composables
```

#### 2. Component Tests

```
Component tests for:
- GridPositionDisplay
- FormLayout
- DataTable
- Permission guards
```

#### 3. Integration Tests

```
Integration tests for:
- API calls with mocked backend
- Store interactions with components
- Router navigation
```

#### 4. E2E Tests (Cypress/Playwright)

```
E2E tests for:
- User login workflow
- CRUD operations for each entity
- Permission-based access
- Auto-logout functionality
- Navigation flows
```

### Accessibility (WCAG 2.1 AA)

#### 1. Keyboard Navigation

- All interactive elements accessible via Tab
- Proper focus management
- Keyboard shortcuts for common actions

#### 2. Screen Reader Support

- ARIA labels for interactive elements
- Form labels properly associated
- Skip to content link
- Proper heading hierarchy

#### 3. Visual Accessibility

- Sufficient color contrast (4.5:1 for normal text)
- Don't rely on color alone for information
- Responsive text sizing
- Readable fonts

#### 4. Grid Accessibility

- ARIA table markup for position grids
- Proper cell headers
- Keyboard navigation within grids
- Position information available to screen readers

### Internationalization (i18n)

**Current**: Traditional Chinese (繁體中文)
**Future Support**:

- English (en)
- Simplified Chinese (zh-Hans)

**Implementation**:

```typescript
// Use vue-i18n library
import { useI18n } from "vue-i18n";

const { t, locale } = useI18n();
```

---

## DEPLOYMENT STRATEGY

### Docker Configuration

#### Multi-stage Build Strategy

```dockerfile
# Stage 1: Build Vue frontend
FROM node:18 AS frontend-build
WORKDIR /app/frontend
COPY frontend/package*.json ./
RUN npm install
COPY frontend/ .
RUN npm run build

# Stage 2: Build .NET backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app/backend
COPY BulkyWeb/*.csproj ./
RUN dotnet restore
COPY BulkyWeb/ .
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /app/publish .
COPY --from=frontend-build /app/frontend/dist ./wwwroot/frontend
EXPOSE 5000
ENTRYPOINT ["dotnet", "BulkyBookWeb.dll"]
```

#### Container Strategy Options

**Option A: Monolithic Container**

- Single container with backend + frontend
- Pros: Simpler deployment, better for small teams
- Cons: Harder to scale independently

**Option B: Separate Containers**

- Frontend container (Node.js + Nginx)
- Backend container (.NET)
- Pros: Independent scaling, easier updates
- Cons: More complex orchestration

**Recommendation**: Option A for simplicity, with plan to migrate to Option B for production

### Azure Web App Service Deployment

#### Prerequisites

- Azure subscription
- Docker image in Azure Container Registry
- SQL Server instance (Azure SQL Database)

#### Deployment Steps

**1. Create Azure Resources**

```bash
# Create resource group
az group create --name BulkyResourceGroup --location eastasia

# Create Azure SQL Database
az sql server create --resource-group BulkyResourceGroup \
  --name bulkysqlserver --admin-user bulkyadmin

# Create App Service Plan
az appservice plan create --name BulkyAppServicePlan \
  --resource-group BulkyResourceGroup --sku B1 --is-linux

# Create Web App
az webapp create --resource-group BulkyResourceGroup \
  --plan BulkyAppServicePlan --name bulkyapp \
  --deployment-container-image-name bulkyapp:latest
```

**2. Configure Application Settings**

```bash
az webapp config appsettings set \
  --resource-group BulkyResourceGroup \
  --name bulkyapp \
  --settings \
    ConnectionStrings__DefaultConnection="Server=tcp:bulkysqlserver.database.windows.net..." \
    Stripe__SecretKey="sk_live_..." \
    SendGrid__ApiKey="SG..." \
    WEBSITE_PULL_IMAGE_OVER_HTTPS=true \
    DOCKER_REGISTRY_SERVER_URL=https://bulkyregistry.azurecr.io
```

**3. Deploy Docker Image**

```bash
az webapp deployment container config \
  --name bulkyapp \
  --resource-group BulkyResourceGroup \
  --docker-custom-image-name bulkyregistry.azurecr.io/bulkyapp:latest

az webapp deployment container config \
  --enable-cd true \
  --name bulkyapp \
  --resource-group BulkyResourceGroup
```

**4. Setup CI/CD Pipeline (Optional)**

- GitHub Actions to build and push Docker image
- Automatic deployment on push to main branch

#### Environment Configuration

**Development** (`appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=BulkyDev;..."
  },
  "Logging": { "LogLevel": { "Default": "Debug" } }
}
```

**Production** (`appsettings.Production.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:bulkysqlserver.database.windows.net;Authentication=Active Directory Default;..."
  },
  "Logging": { "LogLevel": { "Default": "Warning" } },
  "AllowedHosts": "bulkyapp.azurewebsites.net"
}
```

### Monitoring & Maintenance

#### Application Insights

- Monitor application performance
- Track user sessions
- Alert on errors
- Custom metrics

#### Azure SQL Database

- Automatic backups (7-35 days retention)
- Geo-redundant replication
- Performance insights
- Query performance analyzer

#### Scaling Strategy

- **Vertical Scaling**: Increase App Service Plan tier
- **Horizontal Scaling**: Multiple instances with load balancer
- **Database Scaling**: DTU auto-scale for Azure SQL

### Security Considerations

#### SSL/TLS Certificates

- Use Azure-managed certificates
- HTTPS enforcement
- HSTS headers

#### Network Security

- Virtual Network integration (optional)
- Firewall rules for database
- DDoS protection (Standard included)

#### Data Protection

- Encryption in transit (HTTPS)
- Encryption at rest (TDE for SQL Database)
- Sensitive data masking

#### Backup & Disaster Recovery

- Regular database backups
- Geo-redundant storage
- Backup retention policy (30 days minimum)

---

## IMPLEMENTATION PRIORITY

### High Priority (Must-Have)

1. **Authentication System** - Foundation for all features
2. **Ancestral Module** - Core business logic
3. **Kindness Module** - Core business logic
4. **Permission System** - Security requirement
5. **API Integration** - Backend communication

### Medium Priority (Should-Have)

1. **Other CRUD Modules** (Category, Company, Product, User, Order)
2. **Auto-logout System** - User experience
3. **Configuration Management** - Flexibility
4. **Form Validation** - Data integrity

### Low Priority (Nice-to-Have)

1. **3D Visualization** - Ancestral/Kindness display
2. **Advanced Export** - Excel/PDF export
3. **Analytics Dashboard** - Occupancy insights
4. **Mobile App** - Native mobile support
5. **Advanced Filtering** - Complex search queries

---

## TIMELINE ESTIMATE

| Phase               | Duration        | Notes                                     |
| ------------------- | --------------- | ----------------------------------------- |
| Setup               | 1-2 weeks       | Vue project, build tools, CI/CD           |
| Core Infrastructure | 1-2 weeks       | API client, auth, state management        |
| Layout & Navigation | 1 week          | Navbar, sidebar, responsive design        |
| Ancestral Module    | 2-3 weeks       | Grid, forms, queries, testing             |
| Kindness Module     | 2-3 weeks       | Similar to Ancestral, adjusted for layout |
| Other Modules       | 2-3 weeks       | CRUD operations, forms, lists             |
| Cross-cutting       | 1-2 weeks       | Permissions, auto-logout, notifications   |
| Testing             | 2-3 weeks       | Unit, integration, E2E tests              |
| Optimization        | 1-2 weeks       | Performance, bundle size, accessibility   |
| Deployment          | 1-2 weeks       | Docker, Azure setup, CI/CD                |
| **Total**           | **16-24 weeks** | ~4-6 months                               |

_Estimates based on experienced team of 2-3 developers_

---

## CONCLUSION

This MVC-to-Vue transformation is a comprehensive undertaking that will modernize the Bulky system with:

- Improved user experience with SPA architecture
- Better performance with client-side rendering
- Easier maintenance and scalability
- Enhanced testing capabilities
- Cloud-ready deployment

The transformation should follow the outlined roadmap prioritizing core functionality first, with enhancement opportunities at each phase.

---

**Document Version**: 1.0
**Last Updated**: 2025-12-01
**Status**: Ready for Implementation
