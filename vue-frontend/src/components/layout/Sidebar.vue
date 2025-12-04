<template>
  <aside class="sidebar">
    <div class="sidebar-header">
      <h5>功能清單</h5>
    </div>
    <nav class="sidebar-nav">
      <ul class="nav flex-column">
        <li class="nav-item">
          <router-link class="nav-link" :to="homeLink">
            <i class="bi bi-house-door-fill me-2"></i>
            首頁
          </router-link>
        </li>

        <!-- Admin Section -->
        <template v-if="authStore.isAdmin">
          <li class="nav-item-group">
            <span class="nav-group-title">活動管理</span>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/category">
              <i class="bi bi-tag-fill me-2"></i>
              活動類別
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/company">
              <i class="bi bi-building me-2"></i>
              宗親會檔案
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/product">
              <i class="bi bi-box-seam me-2"></i>
              活動清單
            </router-link>
          </li>

          <li class="nav-item-group">
            <span class="nav-group-title">位置管理</span>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/kindness">
              <i class="bi bi-door-closed me-2"></i>
              懐恩塔管理
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/ancestral">
              <i class="bi bi-diagram-3 me-2"></i>
              宗祠牌位管理
            </router-link>
          </li>

          <li class="nav-item-group">
            <span class="nav-group-title">系統管理</span>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/user">
              <i class="bi bi-people-fill me-2"></i>
              會員管理
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/order">
              <i class="bi bi-cart-check-fill me-2"></i>
              訂單管理
            </router-link>
          </li>
        </template>

        <!-- Customer Section -->
        <template v-if="authStore.isCustomer">
          <li class="nav-item-group">
            <span class="nav-group-title">查詢</span>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/kindness/query">
              <i class="bi bi-search me-2"></i>
              懐恩塔查詢
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/app/ancestral/query">
              <i class="bi bi-search me-2"></i>
              宗祠牌位查詢
            </router-link>
          </li>
        </template>
        <li class="nav-item mt-2">
          <a class="nav-link cursor-pointer text-danger" @click="handleLogout">
            <i class="bi bi-box-arrow-right me-2"></i>
            登出
          </a>
        </li>
      </ul>
    </nav>
  </aside>
</template>

<script setup lang="ts">
import { useAuthStore } from "@/stores/authStore";
import { useRouter } from "vue-router";
import { useNotificationStore } from "@/stores/notificationStore";

import { computed } from "vue";

const authStore = useAuthStore();
const router = useRouter();
const notificationStore = useNotificationStore();

const homeLink = computed(() => {
  if (authStore.isAdmin) return "/app/admin";
  if (authStore.isCustomer) return "/app/customer";
  // default authenticated home
  return "/app";
});

const handleLogout = async () => {
  await authStore.logout();
  notificationStore.success("已登出");
  router.push("/login");
};
</script>

<style scoped>
.sidebar {
  width: 250px;
  background-color: #f8f9fa;
  border-right: 1px solid #dee2e6;
  overflow-y: auto;
  max-height: calc(100vh - 56px);
}

.sidebar-header {
  padding: 1rem;
  border-bottom: 1px solid #dee2e6;
}

.sidebar-header h5 {
  margin: 0;
  font-weight: 600;
  color: #333;
}

.sidebar-nav {
  padding: 0.5rem 0;
}

.nav-link {
  color: #333;
  padding: 0.75rem 1rem;
  display: flex;
  align-items: center;
  transition: all 0.3s ease;
  border-left: 3px solid transparent;
  margin-bottom: 0.25rem;
}

.nav-link:hover {
  background-color: #e9ecef;
  border-left-color: #007bff;
  color: #007bff;
}

.nav-link.router-link-active {
  background-color: #e7f3ff;
  border-left-color: #007bff;
  color: #007bff;
  font-weight: 600;
}

.nav-item-group {
  padding: 1rem 1rem 0.5rem 1rem;
}

.nav-group-title {
  font-size: 0.75rem;
  font-weight: 700;
  color: #999;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* Icons */
i {
  width: 20px;
  text-align: center;
}

@media (max-width: 768px) {
  .sidebar {
    width: 100%;
    position: relative;
    border-right: none;
    max-height: none;
  }

  .sidebar-header {
    padding: 0.75rem;
    text-align: left;
  }

  .sidebar-header h5 {
    font-size: 1rem;
  }

  .sidebar-nav {
    padding: 0.25rem;
  }

  .nav-link {
    padding: 0.75rem 1rem;
    justify-content: flex-start;
  }

  .nav-item-group {
    padding: 0.5rem 1rem 0.25rem 1rem;
  }
}
</style>
