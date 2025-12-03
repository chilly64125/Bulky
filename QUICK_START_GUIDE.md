# Quick Start Guide - Vue Frontend Development

## 🚀 Getting Started (5 minutes)

### Prerequisites

- Node.js 18+ and npm 9+
- Visual Studio Code (recommended)
- Git

### Installation

```bash
# Navigate to frontend directory
cd e:\WITS\Udemy\Lessons\Vue\Bulky\vue-frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

Your app will be available at: **http://localhost:5173**

---

## 🔐 Login Credentials (Demo)

**Default Test Account:**

```
Email: test@example.com
Password: Test@123
```

---

## 📁 Project Structure

```
src/
├── views/           # Page components (organized by module)
├── components/      # Reusable UI components
├── stores/          # Pinia state management
├── services/        # API integration layer
├── composables/     # Reusable logic hooks
├── utils/           # Helper functions
├── types/           # TypeScript interfaces
├── router/          # Vue Router configuration
├── App.vue          # Root component
└── main.ts          # Entry point
```

---

## 🛠️ Common Development Tasks

### Add a New Page View

1. Create component in `src/views/mymodule/MyView.vue`
2. Import in `src/router/index.ts`
3. Add route configuration
4. Add to navigation menu (Navbar.vue or Sidebar.vue)

### Add a New API Service

1. Create `src/services/myService.ts`
2. Use `apiClient` from `api.ts`
3. Export functions for API calls
4. Import and use in components

### Add Form Validation

```typescript
import * as yup from "yup";

const schema = yup.object({
  name: yup.string().required("Name is required"),
  email: yup.string().email().required("Email is required"),
});

// In component
const { validate, errors } = await useFormValidation(initialValues, schema);
```

### Display Toast Notification

```typescript
import { useNotificationStore } from "@/stores/notificationStore";

const notificationStore = useNotificationStore();

// Usage
notificationStore.success("Action completed!");
notificationStore.error("Something went wrong");
notificationStore.info("Here is some information");
notificationStore.warning("Please be careful");
```

### Check User Permissions

```typescript
import { useAuthorization } from "@/composables/useAuthorization";

const { isAdmin, hasRole, canAccessModule } = useAuthorization();

if (isAdmin.value) {
  // Admin-only code
}
```

---

## 📦 Available Scripts

```bash
npm run dev          # Start dev server
npm run build        # Production build
npm run preview      # Preview production build
npm run lint         # Run ESLint
npm run test         # Run Vitest
npm run test:ui      # Run tests with UI
npm run test:coverage # Generate coverage report
```

---

## 🔗 API Integration

### Update API Base URL

Edit `src/services/api.ts`:

```typescript
const apiClient = axios.create({
  baseURL: "https://your-api-domain.com/api",
});
```

### Add Authentication Header

The Axios interceptor automatically adds JWT token:

```typescript
// Request interceptor adds:
Authorization: `Bearer ${token.value.accessToken}`;
```

---

## 🎨 Styling Guide

### Bootstrap Classes

The app uses Bootstrap 5. Use standard Bootstrap classes:

```vue
<div class="container py-4">
  <div class="row">
    <div class="col-md-6">
      <button class="btn btn-primary">Click me</button>
    </div>
  </div>
</div>
```

### Custom Colors

Edit `src/assets/css/main.css` for custom variables:

```css
:root {
  --primary: #1976d2;
  --success: #388e3c;
  --danger: #d32f2f;
}
```

---

## 🧪 Testing Components

### Run Tests

```bash
npm run test
```

### Create a Test File

New test files go in `tests/` directory with `.spec.ts` extension:

```typescript
import { describe, it, expect, vi, beforeEach } from "vitest";
import { setActivePinia, createPinia } from "pinia";
import { useAuthStore } from "@/stores/authStore";

describe("authStore", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it("should login successfully", async () => {
    // Test code
  });
});
```

---

## 🔍 Debugging

### Vue DevTools

1. Install Vue DevTools extension for Chrome/Firefox
2. Open DevTools (F12)
3. Go to "Vue" tab to inspect components
4. Check "Pinia" tab for state management

### Console Logging

```typescript
console.log("Debug message", variable);
console.table(arrayOfObjects);
console.error("Error message", error);
```

### Network Debugging

1. Open DevTools
2. Go to "Network" tab
3. Check API requests and responses
4. Look for status codes and response data

---

## 🚨 Troubleshooting

### "Cannot find module" Error

```bash
# Clear node_modules and reinstall
rm -r node_modules
npm install
```

### Port Already in Use

```bash
# Change port in vite.config.ts
# Or kill process on port 5173
npx kill-port 5173
```

### TypeScript Errors

```bash
# Restart TypeScript server
# In VS Code: Ctrl+Shift+P → "TypeScript: Restart TS Server"
```

### Hot Module Replacement Not Working

```bash
# Clear browser cache and restart dev server
npm run dev
```

---

## 📚 Module Overview

### Ancestral (陳氏宗祠)

- **Routes:** `/ancestral`, `/ancestral/add`, `/ancestral/edit/:id`, `/ancestral/query`
- **Features:** 10×10 grid, section filtering, position details
- **Permissions:** Admin only (index, form), Customer (query)

### Kindness (懷恩塔)

- **Routes:** `/kindness`, `/kindness/add`, `/kindness/edit/:id`, `/kindness/query`
- **Features:** Multi-floor grid (1F/2F/3F), dynamic sizing, floor filtering
- **Permissions:** Admin only (index, form), Customer (query)

### Category (活動類別)

- **Routes:** `/category`, `/category/add`, `/category/edit/:id`
- **Features:** Category listing, status toggle, display order
- **Permissions:** Admin only

### Company (宗親會)

- **Routes:** `/company`, `/company/add`, `/company/edit/:id`
- **Features:** Company info management
- **Permissions:** Admin only

### Product (活動)

- **Routes:** `/product`, `/product/add`, `/product/edit/:id`
- **Features:** Product listing, pricing, images
- **Permissions:** Admin only

### User (會員)

- **Routes:** `/user`, `/user/add`, `/user/edit/:id`
- **Features:** User management, role assignment
- **Permissions:** Admin only

### Order (訂單)

- **Routes:** `/order`
- **Features:** Order listing, status tracking, payment status
- **Permissions:** Admin only

---

## 🔐 Authentication Flow

1. User enters credentials on LoginView
2. authService.login() calls `/api/auth/login`
3. Response contains `user` and `token` (access + refresh)
4. Store tokens in localStorage and Pinia store
5. Axios interceptor adds token to all requests
6. On 401 response, automatically refresh token
7. Route guard checks `authStore.isAuthenticated`
8. Role check validates user.roles

---

## 💾 State Management (Pinia)

### authStore

```typescript
// Properties
user, token, loading, error;

// Computed
isAuthenticated, isAdmin, isCustomer;

// Methods
login(), register(), logout(), initializeAuth(), hasRole();
```

### notificationStore

```typescript
// Methods
success(), error(), info(), warning(), clear();
```

### configStore

```typescript
// Methods
loadConfig(), getConfig();
// Auto-caches for 1 hour
```

### sessionStore

```typescript
// Properties
lastActivityTime, isLogoutWarningVisible;

// Methods
updateActivity(), showLogoutWarning(), executeLogout();
```

---

## 🚀 Deployment Checklist

Before deploying to production:

- [ ] Run `npm run build` and verify no errors
- [ ] Test production build: `npm run preview`
- [ ] Update API base URL for production
- [ ] Configure environment variables (.env.production)
- [ ] Run test suite: `npm run test`
- [ ] Check browser compatibility
- [ ] Verify CORS settings on backend
- [ ] Enable HTTPS
- [ ] Setup error logging/monitoring
- [ ] Configure CDN for static assets
- [ ] Setup auto-logout timing in configStore
- [ ] Test token refresh logic

---

## 📖 Additional Resources

- Vue 3 Docs: https://vuejs.org
- Pinia: https://pinia.vuejs.org
- Vue Router: https://router.vuejs.org
- Bootstrap 5: https://getbootstrap.com
- Axios: https://axios-http.com
- Yup Validation: https://github.com/jquense/yup

---

## 💬 Getting Help

1. Check existing documentation files in project root
2. Review component comments and inline documentation
3. Check Git history: `git log --oneline`
4. Run tests to verify functionality
5. Use browser DevTools for debugging

---

## 📝 Development Guidelines

1. **Always use TypeScript** — No `any` types without explanation
2. **Write clear commit messages** — Format: "feat/fix/docs: description"
3. **Create feature branches** — From `master`, prefix with `feature/` or `fix/`
4. **Test before pushing** — Run `npm run test` and `npm run lint`
5. **Keep components small** — Max 300 lines per component
6. **Use composables for logic** — Not computed properties for complex logic
7. **Document complex functions** — Add JSDoc comments
8. **Handle errors gracefully** — Try-catch and user feedback
9. **Validate all inputs** — Frontend and backend validation
10. **Follow naming conventions** — PascalCase for components, camelCase for functions

---

**Last Updated:** December 1, 2025  
**Version:** 1.0.0  
**Status:** Ready for Development 🎉
