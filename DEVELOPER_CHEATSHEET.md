# Developer Cheat Sheet - Vue Frontend

## 🎯 Common Code Snippets

### Form with Validation

```vue
<template>
  <form @submit.prevent="onSubmit" novalidate>
    <div class="mb-3">
      <label class="form-label">Name</label>
      <input
        v-model="values.name"
        type="text"
        class="form-control"
        :class="{ 'is-invalid': errors.name }"
      />
      <div v-if="errors.name" class="invalid-feedback">{{ errors.name }}</div>
    </div>
    <button :disabled="loading">Submit</button>
  </form>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import * as yup from "yup";

const loading = ref(false);
const values = reactive({ name: "" });
const errors = reactive<any>({});

const schema = yup.object({ name: yup.string().required("Required") });

async function validate() {
  try {
    await schema.validate(values, { abortEarly: false });
    Object.keys(errors).forEach((k) => delete (errors as any)[k]);
    return true;
  } catch (err: any) {
    Object.keys(errors).forEach((k) => delete (errors as any)[k]);
    err.inner?.forEach((e: any) => {
      if (e.path) (errors as any)[e.path] = e.message;
    });
    return false;
  }
}

async function onSubmit() {
  if (await validate()) {
    loading.value = true;
    // API call here
  }
}
</script>
```

### Data Listing with Delete

```vue
<template>
  <div v-if="loading" class="alert alert-info">Loading...</div>
  <table v-else class="table">
    <tbody>
      <tr v-for="item in items" :key="item.id">
        <td>{{ item.name }}</td>
        <td>
          <button @click="deleteItem(item.id)" class="btn btn-danger btn-sm">
            Delete
          </button>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { crudService } from "@/services/crudService";
import { useNotificationStore } from "@/stores/notificationStore";

const notificationStore = useNotificationStore();
const loading = ref(false);
const items = ref<any[]>([]);

const service = crudService("/items");

async function loadItems() {
  loading.value = true;
  try {
    const { data } = await service.list();
    items.value = data.data || [];
  } finally {
    loading.value = false;
  }
}

async function deleteItem(id: number) {
  if (!confirm("Delete?")) return;
  try {
    await service.delete(id);
    notificationStore.success("Deleted!");
    await loadItems();
  } catch (e: any) {
    notificationStore.error(e.message);
  }
}

onMounted(() => loadItems());
</script>
```

### Using Pinia Store

```typescript
// Use in component
import { useAuthStore } from "@/stores/authStore";

const authStore = useAuthStore();

// Access properties
console.log(authStore.user);
console.log(authStore.isAuthenticated);
console.log(authStore.isAdmin);

// Call methods
await authStore.login({ email: "test@test.com", password: "pass" });
authStore.logout();
```

### Conditional Rendering Based on Role

```vue
<template>
  <!-- Admin only -->
  <router-link v-if="authStore.isAdmin" to="/ancestral">Ancestral</router-link>

  <!-- Any authenticated user -->
  <button v-if="authStore.isAuthenticated" @click="logout">Logout</button>

  <!-- Using composable -->
  <div v-if="canAccessModule('ancestral')">Only admins</div>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/stores/authStore";
import { useAuthorization } from "@/composables/useAuthorization";

const authStore = useAuthStore();
const { canAccessModule } = useAuthorization();
</script>
```

### API Call Pattern

```typescript
// Option 1: Using service
import { ancestralService } from "@/services/ancestralService";

try {
  const { data } = await ancestralService.query({ section: "A" });
  console.log(data.data);
} catch (error) {
  console.error(error);
}

// Option 2: Using crudService factory
import { crudService } from "@/services/crudService";
import type { MyEntity } from "@/types";

const myService = crudService<MyEntity>("/myentity");
const { data } = await myService.list();
const item = await myService.getById(1);
await myService.create(newItem);
await myService.update(1, updatedItem);
await myService.delete(1);
```

### Pagination Example

```vue
<script setup lang="ts">
import { useDataFetch } from "@/composables/useDataFetch";
import { crudService } from "@/services/crudService";

const service = crudService("/items");
const { data, page, totalPages, nextPage, prevPage } = useDataFetch(() =>
  service.list()
);
</script>

<template>
  <div>
    <div v-for="item in data" :key="item.id">{{ item.name }}</div>
    <nav>
      <button @click="prevPage" :disabled="page === 1">Previous</button>
      <span>Page {{ page }} of {{ totalPages }}</span>
      <button @click="nextPage" :disabled="page === totalPages">Next</button>
    </nav>
  </div>
</template>
```

### Show Loading Spinner

```vue
<template>
  <LoadingSpinner :visible="loading" message="Loading data..." />
</template>

<script setup lang="ts">
import LoadingSpinner from "@/components/global/LoadingSpinner.vue";
import { ref, onMounted } from "vue";

const loading = ref(true);

onMounted(() => {
  setTimeout(() => {
    loading.value = false;
  }, 2000);
});
</script>
```

### Confirmation Dialog

```vue
<template>
  <button @click="showDialog = true">Delete</button>
  <ConfirmDialog
    v-model:visible="showDialog"
    title="Are you sure?"
    message="This cannot be undone"
    @confirm="handleDelete"
  />
</template>

<script setup lang="ts">
import ConfirmDialog from "@/components/global/ConfirmDialog.vue";
import { ref } from "vue";

const showDialog = ref(false);

function handleDelete() {
  // Delete logic
  showDialog.value = false;
}
</script>
```

### Toast Notifications

```typescript
import { useNotificationStore } from "@/stores/notificationStore";

const notify = useNotificationStore();

notify.success("Operation successful!");
notify.error("Something went wrong");
notify.info("Here is some info");
notify.warning("Please be careful");

// Notifications auto-dismiss after 3 seconds
```

---

## 🔐 Authorization Patterns

### Protect a Route

```typescript
// In router/index.ts
{
  path: '/admin',
  component: AdminView,
  meta: {
    requiresAuth: true,
    requiresRole: 'Admin'  // Only admins
  }
}
```

### Check Permission in Component

```vue
<template>
  <div v-if="canAccessModule('ancestral')">
    <!-- Content only visible to admins -->
  </div>
</template>

<script setup lang="ts">
import { useAuthorization } from "@/composables/useAuthorization";

const { canAccessModule, isAdmin } = useAuthorization();

if (isAdmin.value) {
  // Admin-only logic
}
</script>
```

---

## 📊 Working with Data Types

### Define Entity Type

```typescript
// Add to src/types/index.ts
export interface MyEntity {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
  createdAt: Date;
}
```

### Use in Component

```typescript
import type { MyEntity } from "@/types";

const items = ref<MyEntity[]>([]);
const item = ref<MyEntity | null>(null);

function processItem(entity: MyEntity) {
  console.log(entity.name);
}
```

---

## 🎨 Styling Quick Reference

### Bootstrap Classes

```vue
<!-- Layout -->
<div class="container">        <!-- Fixed width -->
<div class="container-fluid">  <!-- Full width -->

<!-- Grid -->
<div class="row">
  <div class="col-md-6">Half width on medium screens</div>
  <div class="col-md-6">Half width on medium screens</div>
</div>

<!-- Spacing -->
<div class="mb-3">Margin bottom</div>
<div class="p-4">Padding all sides</div>
<div class="mt-2 mb-4">Margin top + bottom</div>

<!-- Buttons -->
<button class="btn btn-primary">Primary</button>
<button class="btn btn-success">Success</button>
<button class="btn btn-danger">Danger</button>
<button class="btn btn-secondary">Secondary</button>

<!-- Alerts -->
<div class="alert alert-info">Info message</div>
<div class="alert alert-success">Success message</div>
<div class="alert alert-warning">Warning message</div>
<div class="alert alert-danger">Danger message</div>

<!-- Forms -->
<input class="form-control" type="text" />
<select class="form-select">
  <option>Option 1</option>
</select>

<!-- Tables -->
<table class="table table-striped table-hover">
```

---

## 🐛 Debugging Tips

### Log Component State

```typescript
console.log("Values:", values);
console.log("Errors:", errors);
console.table(items.value);
```

### Inspect Pinia Store

```typescript
import { useAuthStore } from "@/stores/authStore";

const store = useAuthStore();
console.log("Auth State:", store.$state);
console.log("Auth Getters:", store.isAuthenticated);
```

### Check Network Requests

1. Open DevTools (F12)
2. Go to Network tab
3. Perform action
4. Check request/response details

### Vue DevTools

1. Install Vue DevTools extension
2. Open DevTools (F12)
3. Click "Vue" tab
4. Inspect components and Pinia stores

---

## ⚡ Performance Tips

### Lazy Load Images

```vue
<img loading="lazy" src="image.jpg" alt="Description" />
```

### Memoize Computed Properties

```typescript
const expensiveComputed = computed(() => {
  // This runs only when dependencies change
  return items.value.filter((x) => x.active);
});
```

### Defer Non-Critical Data

```typescript
// Load on mount, not on initial render
onMounted(() => {
  loadAnalytics();
});
```

---

## 📋 Git Workflow

```bash
# Create feature branch
git checkout -b feature/new-feature

# Make changes and commit
git add .
git commit -m "feat: add new feature"

# Push to remote
git push origin feature/new-feature

# Create pull request on GitHub

# After merge, delete branch
git branch -d feature/new-feature
```

---

## 🚀 Build & Deploy

```bash
# Production build
npm run build

# Output: dist/ folder ready for deployment

# Preview production build locally
npm run preview

# Deploy dist/ folder to:
# - Azure Static Web Apps
# - Netlify
# - Vercel
# - AWS S3
# - Docker container
```

---

## 📚 File Naming Conventions

```
Components:     MyComponent.vue     (PascalCase)
Services:       myService.ts        (camelCase)
Stores:         myStore.ts          (camelCase)
Composables:    useMyComposable.ts  (camelCase, 'use' prefix)
Utils:          myUtils.ts          (camelCase)
Types:          types.ts or index.ts (lowercase)
Tests:          myComponent.spec.ts (*.spec.ts)
Styles:         main.css            (lowercase)
Routes:         /path/to/feature    (kebab-case in URLs)
```

---

## 🆘 Common Issues & Solutions

| Issue                       | Solution                                          |
| --------------------------- | ------------------------------------------------- |
| TypeScript errors on import | Run `npm install` again, restart TS server        |
| Blank page in browser       | Check console for errors, verify route exists     |
| API calls returning 401     | Check token in localStorage, verify login         |
| Form validation not showing | Check yup schema matches field names              |
| Styles not applying         | Clear browser cache, rebuild with `npm run build` |
| Hot reload not working      | Restart dev server with `npm run dev`             |

---

**Version:** 1.0.0 | **Last Updated:** December 1, 2025
