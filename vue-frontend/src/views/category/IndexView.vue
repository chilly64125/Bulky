<template>
  <div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>活動類別</h2>
      <router-link to="/app/category/add" class="btn btn-primary">新增類別</router-link>
    </div>

    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else-if="categories.length === 0" class="alert alert-warning">無類別</div>
    <div v-else class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>ID</th>
            <th>名稱</th>
            <th>排序</th>
            <th>狀態</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cat in categories" :key="cat.categoryId">
            <td>{{ cat.categoryId }}</td>
            <td>{{ cat.name }}</td>
            <td>{{ cat.displayOrder }}</td>
            <td>
              <span v-if="cat.isActive" class="badge bg-success">啟用</span>
              <span v-else class="badge bg-secondary">禁用</span>
            </td>
            <td>
              <router-link :to="`/category/edit/${cat.categoryId}`" class="btn btn-warning btn-sm">編輯</router-link>
              <button class="btn btn-danger btn-sm" @click="deleteCategory(cat.categoryId)">刪除</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Category } from '@/types'
import { crudService } from '@/services/crudService'
import { useNotificationStore } from '@/stores/notificationStore'

const notificationStore = useNotificationStore()
const loading = ref(false)
const error = ref<string | null>(null)
const categories = ref<Category[]>([])

const categoryService = crudService<Category>('/category')

async function loadCategories() {
  loading.value = true
  error.value = null
  try {
    const { data } = await categoryService.list()
    categories.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '加載失敗'
  } finally {
    loading.value = false
  }
}

async function deleteCategory(id: number) {
  if (!confirm('確定要刪除此類別嗎？')) return
  try {
    await categoryService.delete(id)
    notificationStore.success('類別已刪除')
    await loadCategories()
  } catch (e: any) {
    notificationStore.error(e?.message || '刪除失敗')
  }
}

onMounted(() => loadCategories())
</script>
