<template>
  <div class="customer-dashboard">
    <!-- Page Header -->
    <div class="page-header mb-4 pb-3 border-bottom">
      <div class="row align-items-center">
        <div class="col">
          <h1 class="h3 mb-1">
            <i class="bi bi-person-circle me-2"></i>我的儀表板
          </h1>
          <p class="text-muted mb-0">歡迎, <strong>{{ currentUsername }}</strong></p>
        </div>
        <div class="col-auto">
          <router-link to="/customer/profile" class="btn btn-sm btn-outline-primary">
            <i class="bi bi-person-gear me-1"></i>個人設定
          </router-link>
        </div>
      </div>
    </div>

    <!-- Quick Stats -->
    <div class="row mb-4">
      <div class="col-md-6 col-lg-3 mb-3">
        <div class="stat-card card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <h6 class="card-title text-muted mb-2">祖先牌位</h6>
                <h3 class="mb-0">{{ stats.ancestralCount }}</h3>
              </div>
              <i class="bi bi-person-vcard" style="font-size: 24px; color: #0d6efd; opacity: 0.5;"></i>
            </div>
            <small class="text-muted">
              <router-link to="/ancestral" class="text-decoration-none">檢視全部 →</router-link>
            </small>
          </div>
        </div>
      </div>

      <div class="col-md-6 col-lg-3 mb-3">
        <div class="stat-card card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <h6 class="card-title text-muted mb-2">墓園塔位</h6>
                <h3 class="mb-0">{{ stats.kindnessCount }}</h3>
              </div>
              <i class="bi bi-geo-alt" style="font-size: 24px; color: #198754; opacity: 0.5;"></i>
            </div>
            <small class="text-muted">
              <router-link to="/kindness" class="text-decoration-none">檢視全部 →</router-link>
            </small>
          </div>
        </div>
      </div>

      <div class="col-md-6 col-lg-3 mb-3">
        <div class="stat-card card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <h6 class="card-title text-muted mb-2">待處理訂單</h6>
                <h3 class="mb-0">{{ stats.pendingOrders }}</h3>
              </div>
              <i class="bi bi-bag" style="font-size: 24px; color: #fd7e14; opacity: 0.5;"></i>
            </div>
            <small class="text-muted">
              <router-link to="/order" class="text-decoration-none">查看訂單 →</router-link>
            </small>
          </div>
        </div>
      </div>

      <div class="col-md-6 col-lg-3 mb-3">
        <div class="stat-card card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <h6 class="card-title text-muted mb-2">帳號狀態</h6>
                <h3 class="mb-0">
                  <span class="badge bg-success">正常</span>
                </h3>
              </div>
              <i class="bi bi-shield-check" style="font-size: 24px; color: #198754; opacity: 0.5;"></i>
            </div>
            <small class="text-muted">通過驗證</small>
          </div>
        </div>
      </div>
    </div>

    <!-- Main Content -->
    <div class="row">
      <!-- Quick Actions -->
      <div class="col-lg-8 mb-4">
        <div class="card border-0 shadow-sm">
          <div class="card-header bg-white border-bottom">
            <h5 class="card-title mb-0">
              <i class="bi bi-lightning-fill me-2"></i>快速操作
            </h5>
          </div>
          <div class="card-body">
            <div class="row g-2">
              <div class="col-sm-6 col-md-4">
                <router-link to="/ancestral/create" class="btn btn-outline-primary w-100 py-3">
                  <i class="bi bi-plus-lg"></i>
                  <div class="small mt-2">新增祖先牌位</div>
                </router-link>
              </div>
              <div class="col-sm-6 col-md-4">
                <router-link to="/kindness/create" class="btn btn-outline-success w-100 py-3">
                  <i class="bi bi-plus-lg"></i>
                  <div class="small mt-2">新增塔位紀錄</div>
                </router-link>
              </div>
              <div class="col-sm-6 col-md-4">
                <router-link to="/order/create" class="btn btn-outline-warning w-100 py-3">
                  <i class="bi bi-cart-plus"></i>
                  <div class="small mt-2">建立新訂單</div>
                </router-link>
              </div>
              <div class="col-sm-6 col-md-4">
                <router-link to="/ancestral" class="btn btn-outline-secondary w-100 py-3">
                  <i class="bi bi-search"></i>
                  <div class="small mt-2">查詢牌位</div>
                </router-link>
              </div>
              <div class="col-sm-6 col-md-4">
                <router-link to="/kindness" class="btn btn-outline-secondary w-100 py-3">
                  <i class="bi bi-map"></i>
                  <div class="small mt-2">查看塔位</div>
                </router-link>
              </div>
              <div class="col-sm-6 col-md-4">
                <button class="btn btn-outline-info w-100 py-3" @click="downloadReport">
                  <i class="bi bi-download"></i>
                  <div class="small mt-2">下載報表</div>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Recent Activity -->
      <div class="col-lg-4 mb-4">
        <div class="card border-0 shadow-sm h-100">
          <div class="card-header bg-white border-bottom">
            <h5 class="card-title mb-0">
              <i class="bi bi-clock-history me-2"></i>最近活動
            </h5>
          </div>
          <div class="card-body">
            <div class="activity-list">
              <div class="activity-item pb-3 border-bottom" v-for="(activity, index) in recentActivities" :key="index">
                <div class="d-flex gap-2">
                  <div>
                    <i :class="activity.icon" :style="{ color: activity.color }"></i>
                  </div>
                  <div class="flex-grow-1">
                    <p class="mb-1 small">{{ activity.description }}</p>
                    <small class="text-muted">{{ activity.date }}</small>
                  </div>
                </div>
              </div>
              <div v-if="recentActivities.length === 0" class="text-center text-muted py-3">
                <small>暫無活動記錄</small>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Recent Orders -->
    <div class="row mb-4">
      <div class="col-12">
        <div class="card border-0 shadow-sm">
          <div class="card-header bg-white border-bottom d-flex justify-content-between align-items-center">
            <h5 class="card-title mb-0">
              <i class="bi bi-receipt me-2"></i>最近訂單
            </h5>
            <router-link to="/order" class="btn btn-sm btn-outline-secondary">查看全部</router-link>
          </div>
          <div class="table-responsive">
            <table class="table table-hover mb-0">
              <thead class="table-light">
                <tr>
                  <th>訂單編號</th>
                  <th>金額</th>
                  <th>狀態</th>
                  <th>日期</th>
                  <th>操作</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="order in recentOrders" :key="order.id">
                  <td><strong>#{{ order.id }}</strong></td>
                  <td>{{ formatCurrency(order.amount) }}</td>
                  <td>
                    <span :class="['badge', order.statusClass]">
                      {{ order.status }}
                    </span>
                  </td>
                  <td>{{ formatDate(order.date) }}</td>
                  <td>
                    <button class="btn btn-sm btn-outline-primary" @click="viewOrderDetails(order.id)">
                      詳情
                    </button>
                  </td>
                </tr>
                <tr v-if="recentOrders.length === 0">
                  <td colspan="5" class="text-center text-muted py-3">
                    <small>暫無訂單</small>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../../stores/authStore";

const router = useRouter();
const authStore = useAuthStore();

const currentUsername = computed(() => authStore.user?.name || "用戶");

const stats = ref({
  ancestralCount: 5,
  kindnessCount: 3,
  pendingOrders: 2,
});

const recentActivities = ref([
  {
    icon: "bi bi-person-plus",
    color: "#0d6efd",
    description: "新增祖先牌位 - 陳文武先生",
    date: "2 小時前",
  },
  {
    icon: "bi bi-shopping-bag",
    color: "#fd7e14",
    description: "提交訂單 #12345",
    date: "昨天",
  },
  {
    icon: "bi bi-check-circle",
    color: "#198754",
    description: "訂單 #12344 已完成",
    date: "3 天前",
  },
  {
    icon: "bi bi-pencil-square",
    color: "#6f42c1",
    description: "更新牌位資訊",
    date: "5 天前",
  },
]);

const recentOrders = ref([
  {
    id: 12347,
    amount: 5000,
    status: "待確認",
    statusClass: "bg-warning",
    date: new Date(),
  },
  {
    id: 12346,
    amount: 3500,
    status: "進行中",
    statusClass: "bg-info",
    date: new Date(Date.now() - 86400000),
  },
  {
    id: 12345,
    amount: 2000,
    status: "已完成",
    statusClass: "bg-success",
    date: new Date(Date.now() - 172800000),
  },
]);

function formatCurrency(amount: number): string {
  return new Intl.NumberFormat("zh-TW", {
    style: "currency",
    currency: "TWD",
    minimumFractionDigits: 0,
  }).format(amount);
}

function formatDate(date: Date): string {
  return date.toLocaleDateString("zh-TW");
}

function viewOrderDetails(orderId: number) {
  router.push(`/order/${orderId}`);
}

function downloadReport() {
  // TODO: Implement report download
  alert("報表下載功能即將推出");
}

onMounted(() => {
  // Load stats from API
});
</script>

<style scoped>
.stat-card {
  transition: transform 0.3s, box-shadow 0.3s;
}

.stat-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
}

.activity-list {
  max-height: 400px;
  overflow-y: auto;
}

.activity-item:last-child {
  border-bottom: none !important;
  padding-bottom: 0 !important;
}

table tbody tr:hover {
  background-color: #f8f9fa;
}
</style>
