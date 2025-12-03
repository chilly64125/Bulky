<template>
    <div class="container-fluid py-4">
        <div class="row mb-4">
            <div class="col-md-8">
                <h2 class="mb-0">會員管理與角色分配</h2>
                <p class="text-muted">管理系統使用者和分配角色權限</p>
            </div>
            <div class="col-md-4 text-end">
                <button class="btn btn-success" @click="refreshUsers">
                    <i class="bi bi-arrow-clockwise"></i> 刷新
                </button>
            </div>
        </div>

        <!-- Loading State -->
        <div v-if="loading" class="alert alert-info">
            <div class="spinner-border spinner-border-sm me-2" role="status">
                <span class="visually-hidden">加載中...</span>
            </div>
            加載會員清單中...
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="alert alert-danger alert-dismissible fade show" role="alert">
            <strong>錯誤</strong> {{ error }}
            <button type="button" class="btn-close" @click="error = null"></button>
        </div>

        <!-- No Users -->
        <div v-else-if="filteredUsers.length === 0" class="alert alert-warning">
            <i class="bi bi-info-circle"></i> 無會員記錄
        </div>

        <!-- Users Table -->
        <div v-else>
            <!-- Filter Bar -->
            <div class="card mb-3">
                <div class="card-body">
                    <div class="row g-3">
                        <div class="col-md-6">
                            <input v-model="searchQuery" type="text" class="form-control" placeholder="搜尋用戶名或電郵..." />
                        </div>
                        <div class="col-md-6">
                            <select v-model="roleFilter" class="form-select">
                                <option value="">所有角色</option>
                                <option value="Admin">管理員</option>
                                <option value="Customer">客戶</option>
                                <option value="Employee">員工</option>
                                <option value="Company">公司</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Users Table -->
            <div class="table-responsive">
                <table class="table table-hover align-middle">
                    <thead class="table-light">
                        <tr>
                            <th style="width: 5%">ID</th>
                            <th style="width: 15%">用戶名</th>
                            <th style="width: 20%">電郵</th>
                            <th style="width: 35%">角色</th>
                            <th style="width: 25%">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="user in filteredUsers" :key="user.id">
                            <td>
                                <code class="text-muted">{{ user.id.substring(0, 8) }}...</code>
                            </td>
                            <td>
                                <strong>{{ user.userName }}</strong>
                            </td>
                            <td>{{ user.email }}</td>
                            <td>
                                <div v-if="user.roles && user.roles.length > 0">
                                    <span v-for="role in user.roles" :key="role" class="badge"
                                        :class="getRoleBadgeClass(role)">
                                        {{ getRoleDisplayName(role) }}
                                    </span>
                                </div>
                                <div v-else>
                                    <span class="text-muted">無角色</span>
                                </div>
                            </td>
                            <td>
                                <button class="btn btn-sm btn-primary" @click="editUserRoles(user)" title="編輯角色">
                                    <i class="bi bi-pencil"></i> 編輯角色
                                </button>
                                <button v-if="user.id !== currentUserId" class="btn btn-sm btn-danger ms-2"
                                    @click="confirmDeleteUser(user)" title="刪除用戶">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <!-- Pagination Info -->
            <div class="text-muted mt-3">
                <small>共 {{ filteredUsers.length }} 名會員</small>
            </div>
        </div>

        <!-- Role Edit Modal -->
        <div v-if="showRoleModal" class="modal fade show d-block" style="background-color: rgba(0, 0, 0, 0.5)"
            tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">編輯角色 - {{ selectedUser?.userName }}</h5>
                        <button type="button" class="btn-close" @click="showRoleModal = false"></button>
                    </div>
                    <div class="modal-body">
                        <div class="form-check" v-for="role in availableRoles" :key="role.value">
                            <input class="form-check-input" type="checkbox" :id="`role-${role.value}`"
                                :value="role.value" v-model="editingRoles" />
                            <label class="form-check-label" :for="`role-${role.value}`">
                                {{ role.label }}
                            </label>
                            <small class="text-muted d-block ms-4">{{ role.description }}</small>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" @click="showRoleModal = false">
                            取消
                        </button>
                        <button type="button" class="btn btn-primary" @click="saveUserRoles" :disabled="savingRoles">
                            <span v-if="savingRoles" class="spinner-border spinner-border-sm me-2" role="status">
                                <span class="visually-hidden">保存中...</span>
                            </span>
                            保存
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Delete Confirmation Modal -->
        <div v-if="showDeleteModal" class="modal fade show d-block" style="background-color: rgba(0, 0, 0, 0.5)"
            tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h5 class="modal-title">確認刪除</h5>
                        <button type="button" class="btn-close btn-close-white"
                            @click="showDeleteModal = false"></button>
                    </div>
                    <div class="modal-body">
                        <p>確定要刪除用戶 <strong>{{ selectedUser?.userName }}</strong> ({{ selectedUser?.email }}) 嗎？</p>
                        <p class="text-danger"><small>此操作無法復原。</small></p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" @click="showDeleteModal = false">
                            取消
                        </button>
                        <button type="button" class="btn btn-danger" @click="deleteUser" :disabled="deletingUser">
                            <span v-if="deletingUser" class="spinner-border spinner-border-sm me-2" role="status">
                                <span class="visually-hidden">刪除中...</span>
                            </span>
                            確認刪除
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore'

interface User {
    id: string
    userName: string
    email: string
    roles: string[]
}

const authStore = useAuthStore()

const users = ref<User[]>([])
const loading = ref(false)
const error = ref<string | null>(null)
const searchQuery = ref('')
const roleFilter = ref('')
const showRoleModal = ref(false)
const showDeleteModal = ref(false)
const selectedUser = ref<User | null>(null)
const editingRoles = ref<string[]>([])
const savingRoles = ref(false)
const deletingUser = ref(false)

const currentUserId = computed(() => authStore.user?.id || '')

const availableRoles = [
    {
        value: 'Admin',
        label: '管理員',
        description: '擁有系統全部存取權限',
    },
    {
        value: 'Customer',
        label: '客戶',
        description: '可查詢塔位和參加問卷調查',
    },
    {
        value: 'Employee',
        label: '員工',
        description: '系統幹部角色',
    },
    {
        value: 'Company',
        label: '公司',
        description: '宗親會會員角色',
    },
]

const filteredUsers = computed(() => {
    return users.value.filter((user) => {
        const matchesSearch =
            user.userName.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
            user.email.toLowerCase().includes(searchQuery.value.toLowerCase())

        const matchesRole =
            roleFilter.value === '' || (user.roles && user.roles.includes(roleFilter.value))

        return matchesSearch && matchesRole
    })
})

function getRoleDisplayName(role: string): string {
    const roleMap: { [key: string]: string } = {
        Admin: '管理員',
        Customer: '客戶',
        Employee: '員工',
        Company: '公司',
    }
    return roleMap[role] || role
}

function getRoleBadgeClass(role: string): string {
    const roleClassMap: { [key: string]: string } = {
        Admin: 'bg-danger',
        Customer: 'bg-info',
        Employee: 'bg-warning',
        Company: 'bg-success',
    }
    return roleClassMap[role] || 'bg-secondary'
}

async function loadUsers() {
    loading.value = true
    error.value = null
    try {
        const response = await fetch('/api/admin/users', {
            credentials: 'include',
        })
        if (response.status === 401) {
            error.value = '未授權。請先登入。'
            return
        }
        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`)
        }
        users.value = await response.json()
    } catch (e: any) {
        error.value = `加載失敗: ${e.message}`
    } finally {
        loading.value = false
    }
}

function editUserRoles(user: User) {
    selectedUser.value = user
    editingRoles.value = [...(user.roles || [])]
    showRoleModal.value = true
}

async function saveUserRoles() {
    if (!selectedUser.value) return
    savingRoles.value = true
    try {
        const response = await fetch(`/api/admin/users/${selectedUser.value.id}/roles`, {
            method: 'PUT',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ roles: editingRoles.value }),
        })
        if (response.status === 401) {
            error.value = '未授權。請先登入。'
            return
        }
        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`)
        }
        selectedUser.value.roles = editingRoles.value
        showRoleModal.value = false
    } catch (e: any) {
        error.value = `保存失敗: ${e.message}`
    } finally {
        savingRoles.value = false
    }
}

function confirmDeleteUser(user: User) {
    selectedUser.value = user
    showDeleteModal.value = true
}

async function deleteUser() {
    if (!selectedUser.value) return
    deletingUser.value = true
    try {
        const response = await fetch(`/api/admin/users/${selectedUser.value.id}`, {
            method: 'DELETE',
            credentials: 'include',
        })
        if (response.status === 401) {
            error.value = '未授權。請先登入。'
            return
        }
        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`)
        }
        users.value = users.value.filter((u) => u.id !== selectedUser.value?.id)
        showDeleteModal.value = false
        selectedUser.value = null
    } catch (e: any) {
        error.value = `刪除失敗: ${e.message}`
    } finally {
        deletingUser.value = false
    }
}

async function refreshUsers() {
    await loadUsers()
}

onMounted(() => loadUsers())
</script>

<style scoped>
.modal {
    display: block;
}

.form-check-label {
    margin-left: 0.5rem;
    cursor: pointer;
}

.badge {
    font-size: 0.875rem;
    padding: 0.35rem 0.65rem;
}
</style>
