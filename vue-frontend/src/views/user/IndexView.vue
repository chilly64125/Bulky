<template>
  <div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>會員管理</h2>
      <router-link to="/app/user/add" class="btn btn-primary">新增會員</router-link>
    </div>

    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else-if="users.length === 0" class="alert alert-warning">無會員</div>
    <div v-else class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>ID</th>
            <th>用戶名</th>
            <th>電郵</th>
            <th>角色</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.userId">
            <td>{{ user.userId }}</td>
            <td>{{ user.username }}</td>
            <td>{{ user.email }}</td>
            <td>
              <span v-for="role in user.roles" :key="role" class="badge bg-info me-1">{{ role }}</span>
            </td>
            <td>
              <router-link :to="`/user/edit/${user.userId}`" class="btn btn-warning btn-sm">編輯</router-link>
              <button class="btn btn-danger btn-sm" @click="deleteUser(user.userId)">刪除</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { User } from '@/types'
import { crudService } from '@/services/crudService'
import { useNotificationStore } from '@/stores/notificationStore'

const notificationStore = useNotificationStore()
const loading = ref(false)
const error = ref<string | null>(null)
const users = ref<User[]>([])

const userService = crudService<User>('/user')

async function loadUsers() {
  loading.value = true
  error.value = null
  try {
    const { data } = await userService.list()
    users.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '加載失敗'
  } finally {
    loading.value = false
  }
}

async function deleteUser(id: string) {
  if (!confirm('確定要刪除此會員嗎？')) return
  try {
    await userService.delete(id as any)
    notificationStore.success('會員已刪除')
    await loadUsers()
  } catch (e: any) {
    notificationStore.error(e?.message || '刪除失敗')
  }
}

onMounted(() => loadUsers())
</script>
