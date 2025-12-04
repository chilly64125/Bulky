<template>
    <div class="container-fluid py-5">
        <!-- Header -->
        <div class="row mb-5">
            <div class="col-12">
                <h1 class="mb-2">管理者儀表板</h1>
                <p class="text-muted">歡迎, {{ currentUsername }}</p>
            </div>
        </div>

        <!-- Stats Cards -->
        <div class="row mb-4">
            <div class="col-md-3 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h6 class="card-title text-muted">會員總數</h6>
                        <h3 class="mb-0">{{ stats.totalUsers }}</h3>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h6 class="card-title text-muted">管理員</h6>
                        <h3 class="mb-0">{{ stats.adminCount }}</h3>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h6 class="card-title text-muted">客戶</h6>
                        <h3 class="mb-0">{{ stats.customerCount }}</h3>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h6 class="card-title text-muted">其他角色</h6>
                        <h3 class="mb-0">{{ stats.otherRolesCount }}</h3>
                    </div>
                </div>
            </div>
        </div>

        <!-- Main Admin Functions -->
        <div class="row">
            <!-- User Management -->
            <div class="col-lg-4 mb-4">
                <div class="card">
                    <div class="card-body text-center">
                        <i class="bi bi-people fs-1 text-primary mb-3" style="display: block"></i>
                        <h5 class="card-title">會員管理</h5>
                        <p class="card-text text-muted small">
                            管理系統使用者、分配角色和權限
                        </p>
                        <router-link to="/app/admin/users" class="btn btn-primary btn-sm">
                            進入會員管理
                        </router-link>
                    </div>
                </div>
            </div>

            <!-- Category Management -->
            <div class="col-lg-4 mb-4">
                <div class="card">
                    <div class="card-body text-center">
                        <i class="bi bi-tags fs-1 text-info mb-3" style="display: block"></i>
                        <h5 class="card-title">活動類別</h5>
                        <p class="card-text text-muted small">
                            新增、編輯和管理活動類別
                        </p>
                        <router-link to="/app/category" class="btn btn-info btn-sm">
                            進入類別管理
                        </router-link>
                    </div>
                </div>
            </div>

            <!-- Company Management -->
            <div class="col-lg-4 mb-4">
                <div class="card">
                    <div class="card-body text-center">
                        <i class="bi bi-building fs-1 text-success mb-3" style="display: block"></i>
                        <h5 class="card-title">宗親會基本檔</h5>
                        <p class="card-text text-muted small">
                            管理宗親會組織資訊
                        </p>
                        <router-link to="/app/company" class="btn btn-success btn-sm">
                            進入宗親會管理
                        </router-link>
                    </div>
                </div>
            </div>

            <!-- Product Management -->
            <div class="col-lg-4 mb-4">
                <div class="card">
                    <div class="card-body text-center">
                        <i class="bi bi-box fs-1 text-warning mb-3" style="display: block"></i>
                        <h5 class="card-title">活動基本檔</h5>
                        <p class="card-text text-muted small">
                            新增、編輯和管理活動商品
                        </p>
                        <router-link to="/app/product" class="btn btn-warning btn-sm">
                            進入活動管理
                        </router-link>
                    </div>
                </div>
            </div>

            <!-- Kindness Position Management -->
            <div class="col-lg-4 mb-4">
                <div class="card">
                    <div class="card-body text-center">
                        <i class="bi bi-building-check fs-1 text-danger mb-3" style="display: block"></i>
                        <h5 class="card-title">懷恩塔-塔位管理</h5>
                        <p class="card-text text-muted small">
                            管理懷恩塔塔位資訊和位置
                        </p>
                        <router-link to="/app/kindness" class="btn btn-danger btn-sm">
                            進入塔位管理
                        </router-link>
                    </div>
                </div>
            </div>

            <!-- Ancestral Position Management -->
            <div class="col-lg-4 mb-4">
                <div class="card">
                    <div class="card-body text-center">
                        <i class="bi bi-houses fs-1 text-secondary mb-3" style="display: block"></i>
                        <h5 class="card-title">陳氏宗祠-牌位管理</h5>
                        <p class="card-text text-muted small">
                            管理陳氏宗祠祖先牌位資訊
                        </p>
                        <router-link to="/app/ancestral" class="btn btn-secondary btn-sm">
                            進入牌位管理
                        </router-link>
                    </div>
                </div>
            </div>
        </div>

        <!-- Recent Activity (Optional) -->
        <div class="row mt-5">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h6 class="mb-0">系統資訊</h6>
                    </div>
                    <div class="card-body">
                        <p class="mb-1">
                            <strong>環境:</strong>
                            <span class="badge bg-primary">{{ environment }}</span>
                        </p>
                        <p class="mb-0">
                            <strong>您的角色:</strong>
                            <span v-for="role in currentUserRoles" :key="role" class="badge bg-info me-1">
                                {{ getRoleDisplayName(role) }}
                            </span>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()

interface Stats {
    totalUsers: number
    adminCount: number
    customerCount: number
    otherRolesCount: number
}

const stats = ref<Stats>({
    totalUsers: 0,
    adminCount: 0,
    customerCount: 0,
    otherRolesCount: 0,
})

const currentUsername = computed(() => authStore.user?.email || 'Unknown')
const currentUserRoles = computed(() => authStore.user?.roles || [])
const environment = computed(() => (import.meta.env.DEV ? 'Development' : 'Production'))

function getRoleDisplayName(role: string): string {
    const roleMap: { [key: string]: string } = {
        Admin: '管理員',
        Customer: '客戶',
        Employee: '員工',
        Company: '公司',
    }
    return roleMap[role] || role
}

async function loadStats() {
    try {
        const response = await fetch('/api/admin/users', {
            credentials: 'include',
        })
        if (response.ok) {
            const users = await response.json()
            stats.value.totalUsers = users.length
            stats.value.adminCount = users.filter((u: any) => u.roles?.includes('Admin')).length
            stats.value.customerCount = users.filter((u: any) => u.roles?.includes('Customer')).length
            stats.value.otherRolesCount =
                users.filter((u: any) => u.roles?.some((r: string) => r !== 'Admin' && r !== 'Customer')).length -
                stats.value.customerCount
        }
    } catch (error) {
        console.error('Failed to load stats:', error)
    }
}

onMounted(() => loadStats())
</script>

<style scoped>
.card {
    border-radius: 0.5rem;
    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
    transition: transform 0.2s, box-shadow 0.2s;
}

.card:hover {
    transform: translateY(-2px);
    box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
}

.bi {
    color: inherit;
}
</style>
