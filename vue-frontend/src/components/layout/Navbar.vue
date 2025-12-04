<template>
  <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
    <div class="container-fluid">
      <!-- Brand -->
      <router-link class="navbar-brand d-flex align-items-center" to="/">
        <i class="bi bi-house-fill me-2"></i>
        <!-- short name on xs, full name on md+ -->
        <span class="d-inline d-md-none">{{ shortName }}</span>
        <span class="d-none d-md-inline">{{ fullName }}</span>
      </router-link>

      <!-- Toggler -->
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
        <span class="navbar-toggler-icon"></span>
      </button>

      <!-- Menu -->
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav ms-auto">
          <!-- Admin Menu (mobile only; sidebar shows on desktop) -->
          <li v-if="authStore.isAdmin" class="nav-item dropdown d-md-none">
            <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
              一.活動管理
            </a>
            <ul class="dropdown-menu">
              <li>
                <router-link class="dropdown-item" to="/app/category">活動類別檔</router-link>
              </li>
              <li>
                <router-link class="dropdown-item" to="/app/company">宗親會基本檔</router-link>
              </li>
              <li>
                <router-link class="dropdown-item" to="/app/product">活動基本檔</router-link>
              </li>
              <li>
                <hr class="dropdown-divider" />
              </li>
              <li>
                <router-link class="dropdown-item" to="/app/user">會員管理</router-link>
              </li>
            </ul>
          </li>

          <!-- Kindness Menu (mobile only) -->
          <li v-if="authStore.isAdmin" class="nav-item dropdown d-md-none">
            <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
              二.懷恩塔-塔位管理
            </a>
            <ul class="dropdown-menu">
              <li>
                <router-link class="dropdown-item" to="/app/kindness">懐恩塔-塔位-新增</router-link>
              </li>
              <li>
                <router-link class="dropdown-item" to="/app/kindness/query">懐恩塔-塔位-查詢</router-link>
              </li>
              <li>
                <hr class="dropdown-divider" />
              </li>
              <li>
                <router-link class="dropdown-item" to="/app/user">會員管理</router-link>
              </li>
            </ul>
          </li>

          <!-- Ancestral Menu (mobile only) -->
          <li v-if="authStore.isAdmin" class="nav-item dropdown d-md-none">
            <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
              三.陳氏宗祠-祖先牌位管理
            </a>
            <ul class="dropdown-menu">
              <li>
                <router-link class="dropdown-item" to="/app/ancestral">陳氏宗祠-牌位-新增</router-link>
              </li>
              <li>
                <router-link class="dropdown-item" to="/app/ancestral/query">陳氏宗祠-牌位-查詢</router-link>
              </li>
              <li>
                <hr class="dropdown-divider" />
              </li>
              <li>
                <router-link class="dropdown-item" to="/app/user">會員管理</router-link>
              </li>
            </ul>
          </li>

          <!-- Customer Menu (mobile only) -->
          <li v-if="authStore.isCustomer" class="nav-item d-md-none">
            <router-link class="nav-link" to="/app/kindness/query">懐恩塔-塔位查詢</router-link>
          </li>
          <li v-if="authStore.isCustomer" class="nav-item d-md-none">
            <router-link class="nav-link" to="/app/ancestral/query">陳氏宗祠-牌位查詢</router-link>
          </li>

          <!-- User Menu -->
          <li class="nav-item dropdown d-flex align-items-center">
            <a class="nav-link dropdown-toggle d-flex align-items-center" href="#" role="button"
              data-bs-toggle="dropdown">
              <i class="bi bi-person-circle me-1"></i>
              <span class="me-2">{{ authStore.user?.username }}</span>
            </a>
            <ul class="dropdown-menu">
              <li>
                <a class="dropdown-item" href="#">個人資料</a>
              </li>
              <li>
                <a class="dropdown-item" href="#">設定</a>
              </li>
              <li>
                <hr class="dropdown-divider" />
              </li>
              <li>
                <a class="dropdown-item cursor-pointer" @click="handleLogout">登出</a>
              </li>
            </ul>
          </li>

          <!-- Quick logout button (visible on all breakpoints) -->
          <li class="nav-item d-flex align-items-center ms-2">
            <button class="btn btn-outline-light btn-sm" @click="handleLogout" title="Logout">
              <i class="bi bi-box-arrow-right"></i>
              <span class="d-none d-md-inline ms-1">登出</span>
            </button>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/stores/authStore";
import { useRouter } from "vue-router";
import { useNotificationStore } from "@/stores/notificationStore";

const authStore = useAuthStore();
const router = useRouter();
const notificationStore = useNotificationStore();

// Display names: short name for compact UI, full name for larger areas
const shortName = (((import.meta as any).env?.VITE_APP_SHORT_NAME) as string) || '陳氏宗祠';
const fullName = '陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台';

const handleLogout = async () => {
  await authStore.logout();
  notificationStore.success("已登出");
  router.push("/login");
};
</script>

<style scoped>
.navbar-brand {
  font-weight: bold;
  font-size: 1.5rem;
}

.nav-link {
  color: rgba(255, 255, 255, 0.8) !important;
  transition: color 0.3s ease;
}

.nav-link:hover {
  color: rgba(255, 255, 255, 1) !important;
}

.dropdown-menu {
  min-width: 200px;
  border: none;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

.dropdown-item {
  transition: all 0.2s ease;
}

.dropdown-item:hover {
  background-color: #f8f9fa;
  padding-left: 1.5rem;
}
</style>
