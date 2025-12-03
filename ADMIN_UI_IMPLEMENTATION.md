# 陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台 - Admin UI Implementation

## Project: Chen Family Ancestral Temple & Cemetery Management Platform

## ✅ What Was Created

### 1. Admin Dashboard (`/admin`)

- **Path:** `/admin`
- **Component:** `AdminDashboardView.vue`
- **Features:**
  - Statistics overview (total users, admins, customers, other roles)
  - Quick navigation cards to all admin functions
  - System environment info
  - User role display

### 2. Users Management & Role Assignment (`/admin/users`)

- **Path:** `/admin/users`
- **Component:** `UsersManageView.vue`
- **Features:**
  - List all system users with roles
  - Search users by username or email
  - Filter by role
  - Modal-based role editing (add/remove roles)
  - Delete users with confirmation
  - Real-time role badge display (color-coded by role)

## 🔌 Backend API Requirements

The admin UI expects these API endpoints:

### GET `/api/admin/users`

Returns a list of all users with their roles.

**Response Example:**

```json
[
  {
    "id": "user-id-1",
    "userName": "admin@example.com",
    "email": "admin@example.com",
    "roles": ["Admin"]
  },
  {
    "id": "user-id-2",
    "userName": "customer@example.com",
    "email": "customer@example.com",
    "roles": ["Customer"]
  }
]
```

### PUT `/api/admin/users/{userId}/roles`

Update a user's roles.

**Request Body:**

```json
{
  "roles": ["Admin", "Customer"]
}
```

### DELETE `/api/admin/users/{userId}`

Delete a user.

## 🧭 Navigation

### From the App Navbar

If you add an admin menu item to your navbar, it should link to `/admin`

### Accessing Admin UI

1. **Login as Admin:** Use an account with "Admin" role
2. **Navigate to Dashboard:** Visit `http://localhost:5173/admin`
3. **Access Users Management:** Click "進入會員管理" or visit `http://localhost:5173/admin/users`

## 🧪 Testing Steps

### 1. Verify Backend APIs Exist

```bash
# In PowerShell, test if the API endpoint responds
curl -i http://localhost:5064/api/admin/users
# (This will return 401 Unauthorized if not logged in — that's expected)
```

### 2. Test in Browser

1. Open `http://localhost:5173/`
2. Click Login and sign in with an Admin account
3. (Optional) Click Admin menu or navigate to `http://localhost:5173/admin`
4. You should see:
   - Admin Dashboard with stats
   - Navigation cards to admin functions
   - Users Management page with user list and role editing

### 3. Verify API Calls

Open DevTools (F12) → Network tab:

- When you load `/admin/users`, you should see a GET request to `/api/admin/users`
- The response should contain a list of users with their roles

### 4. Test Role Editing

1. Click "編輯角色" on any user
2. Check/uncheck roles in the modal
3. Click "保存"
4. Should make a PUT request to `/api/admin/users/{id}/roles`
5. The role badges should update

## ⚠️ Prerequisites

### Backend Setup Required

1. **Users API Controller** at `GET /api/admin/users` and `PUT /api/admin/users/{id}/roles`

   - Should require Admin role (use `[Authorize(Roles = "Admin")]`)
   - Return list of users with roles
   - Accept role update requests

2. **Authentication Middleware**
   - Ensure credentials (cookies) are sent with each request
   - Backend should validate session/auth on each API call

### Frontend Already Setup

✅ Routes configured in `router/index.ts`
✅ Auth store (`authStore`) manages user login state
✅ Role checking in route guards
✅ Components with full UI

## 📝 Role Display Names

The components use localized role names:

- `Admin` → "管理員" (Administrator)
- `Customer` → "客戶" (Customer)
- `Employee` → "員工" (Employee)
- `Company` → "公司" (Company)

## 🎨 UI Features

- **Color-coded badges:** Admin (red), Customer (blue), Employee (yellow), Company (green)
- **Responsive table:** Works on mobile/tablet/desktop
- **Real-time feedback:** Loading spinners, error messages, confirmations
- **Accessibility:** Form labels, disabled states, error handling
- **Bootstrap 5 styling:** Matches your existing design

## 🚀 Next Steps

1. **Implement Backend APIs:** Create `/api/admin/users` endpoints in your backend
2. **Test End-to-End:** Login as admin and test the UI
3. **Add Navbar Link:** Add an admin menu item to your AppLayout navbar pointing to `/admin`
4. **Enhance Dashboard:** Add more stats or charts as needed

## 📂 Files Created/Modified

- ✅ `/src/views/admin/DashboardView.vue` — New admin dashboard
- ✅ `/src/views/admin/UsersManageView.vue` — New users management page
- ✅ `/src/router/index.ts` — Updated with admin routes

---

**Status:** ✅ Ready for backend integration and end-to-end testing
