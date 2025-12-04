import {
  createRouter,
  createWebHistory,
  type RouteRecordRaw,
} from "vue-router";
import { useAuthStore } from "@/stores/authStore";

// Layout
const AppLayout = () => import("@/components/layout/AppLayout.vue");

// Auth Views
const LoginView = () => import("@/views/auth/LoginView.vue");
const RegisterView = () => import("@/views/auth/RegisterView.vue");
const GuestWelcomeView = () => import("@/views/GuestWelcomeView.vue");
const GuestLandingView = () => import("@/views/GuestLandingView.vue");

// Home
const HomeView = () => import("@/views/HomeView.vue");

// Ancestral Views
const AncestralIndexView = () => import("@/views/ancestral/IndexView.vue");
const AncestralFormView = () => import("@/views/ancestral/FormView.vue");
const AncestralQueryView = () => import("@/views/ancestral/QueryView.vue");

// Kindness Views
const KindnessIndexView = () => import("@/views/kindness/IndexView.vue");
const KindnessFormView = () => import("@/views/kindness/FormView.vue");
const KindnessQueryView = () => import("@/views/kindness/QueryView.vue");

// Category Views
const CategoryIndexView = () => import("@/views/category/IndexView.vue");
const CategoryFormView = () => import("@/views/category/FormView.vue");

// Company Views
const CompanyIndexView = () => import("@/views/company/IndexView.vue");
const CompanyFormView = () => import("@/views/company/FormView.vue");

// Product Views
const ProductIndexView = () => import("@/views/product/IndexView.vue");
const ProductFormView = () => import("@/views/product/FormView.vue");

// User Views
const UserIndexView = () => import("@/views/user/IndexView.vue");
const UserFormView = () => import("@/views/user/FormView.vue");

// Admin Views
const AdminDashboardView = () => import("@/views/admin/DashboardView.vue");
const AdminUsersManageView = () => import("@/views/admin/UsersManageView.vue");

// Customer Views
const CustomerDashboardView = () =>
  import("@/views/customer/DashboardView.vue");

// Order Views
const OrderIndexView = () => import("@/views/order/IndexView.vue");

const routes: RouteRecordRaw[] = [
  {
    path: "/",
    component: GuestLandingView,
    meta: { requiresAuth: false, layout: false, title: "首頁" },
  },
  {
    path: "/welcome",
    component: GuestWelcomeView,
    meta: { requiresAuth: false, layout: false, title: "歡迎" },
  },
  {
    path: "/login",
    component: LoginView,
    meta: { requiresAuth: false, layout: false },
  },
  {
    path: "/register",
    component: RegisterView,
    meta: { requiresAuth: false, layout: false },
  },
  {
    path: "/app",
    component: AppLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: "",
        component: HomeView,
        meta: { title: "首頁" },
      },
      {
        path: "admin",
        meta: { title: "管理", requiresRole: "Admin" },
        children: [
          {
            path: "",
            component: AdminDashboardView,
            meta: { title: "管理者儀表板" },
          },
          {
            path: "users",
            component: AdminUsersManageView,
            meta: { title: "會員管理與角色分配" },
          },
        ],
      },
      {
        path: "customer",
        meta: { title: "客戶中心" },
        children: [
          {
            path: "",
            component: CustomerDashboardView,
            meta: { title: "客戶儀表板" },
          },
        ],
      },
      {
        path: "ancestral",
        meta: { title: "陳氏宗祠-祖先牌位查詢" },
        children: [
          {
            path: "",
            component: AncestralIndexView,
            meta: { title: "牌位清單", requiresRole: "Admin" },
          },
          {
            path: "add",
            component: AncestralFormView,
            meta: { title: "新增牌位", requiresRole: "Admin" },
          },
          {
            path: "edit/:id",
            component: AncestralFormView,
            meta: { title: "編輯牌位", requiresRole: "Admin" },
          },
          {
            path: "query",
            component: AncestralQueryView,
            meta: { title: "查詢牌位" },
          },
        ],
      },
      {
        path: "kindness",
        meta: { title: "懷恩塔-塔位查詢" },
        children: [
          {
            path: "",
            component: KindnessIndexView,
            meta: { title: "塔位清單", requiresRole: "Admin" },
          },
          {
            path: "add",
            component: KindnessFormView,
            meta: { title: "新增塔位", requiresRole: "Admin" },
          },
          {
            path: "edit/:id",
            component: KindnessFormView,
            meta: { title: "編輯塔位", requiresRole: "Admin" },
          },
          {
            path: "query",
            component: KindnessQueryView,
            meta: { title: "查詢塔位" },
          },
        ],
      },
      {
        path: "category",
        meta: { title: "活動類別", requiresRole: "Admin" },
        children: [
          {
            path: "",
            component: CategoryIndexView,
            meta: { title: "類別清單" },
          },
          {
            path: "add",
            component: CategoryFormView,
            meta: { title: "新增類別" },
          },
          {
            path: "edit/:id",
            component: CategoryFormView,
            meta: { title: "編輯類別" },
          },
        ],
      },
      {
        path: "company",
        meta: { title: "宗親會基本檔", requiresRole: "Admin" },
        children: [
          {
            path: "",
            component: CompanyIndexView,
            meta: { title: "宗親會清單" },
          },
          {
            path: "add",
            component: CompanyFormView,
            meta: { title: "新增宗親會" },
          },
          {
            path: "edit/:id",
            component: CompanyFormView,
            meta: { title: "編輯宗親會" },
          },
        ],
      },
      {
        path: "product",
        meta: { title: "活動基本檔", requiresRole: "Admin" },
        children: [
          {
            path: "",
            component: ProductIndexView,
            meta: { title: "活動清單" },
          },
          {
            path: "add",
            component: ProductFormView,
            meta: { title: "新增活動" },
          },
          {
            path: "edit/:id",
            component: ProductFormView,
            meta: { title: "編輯活動" },
          },
        ],
      },
      {
        path: "user",
        meta: { title: "會員管理", requiresRole: "Admin" },
        children: [
          {
            path: "",
            component: UserIndexView,
            meta: { title: "會員清單" },
          },
          {
            path: "add",
            component: UserFormView,
            meta: { title: "新增會員" },
          },
          {
            path: "edit/:id",
            component: UserFormView,
            meta: { title: "編輯會員" },
          },
        ],
      },
      {
        path: "order",
        meta: { title: "訂單管理", requiresRole: "Admin" },
        children: [
          {
            path: "",
            component: OrderIndexView,
            meta: { title: "訂單清單" },
          },
        ],
      },
    ],
  },
  {
    path: "/:pathMatch(.*)*",
    redirect: "/",
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

// Initialize auth from localStorage on app start
const authStore = useAuthStore();
authStore.initializeAuth();

// Route guard
router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore();

  const requiresAuth = to.meta.requiresAuth !== false;
  const requiredRole = to.meta.requiresRole as string | undefined;

  // Check authentication
  if (requiresAuth && !authStore.isAuthenticated) {
    next({ path: "/login", query: { redirect: to.fullPath } });
    return;
  }

  // Check role
  if (requiredRole && !authStore.hasRole(requiredRole)) {
    next("/");
    return;
  }

  // Update page title
  document.title = `${to.meta.title || "Home"} - 陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台`;

  next();
});

export default router;
