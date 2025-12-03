<template>
  <div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>活動管理</h2>
      <router-link to="/product/add" class="btn btn-primary">新增活動</router-link>
    </div>

    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else-if="products.length === 0" class="alert alert-warning">無活動</div>
    <div v-else class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>ID</th>
            <th>標題</th>
            <th>類別ID</th>
            <th>價格</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="prod in products" :key="prod.productId">
            <td>{{ prod.productId }}</td>
            <td>{{ prod.title }}</td>
            <td>{{ prod.categoryId }}</td>
            <td>${{ prod.price }}</td>
            <td>
              <router-link :to="`/product/edit/${prod.productId}`" class="btn btn-warning btn-sm">編輯</router-link>
              <button class="btn btn-danger btn-sm" @click="deleteProduct(prod.productId)">刪除</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Product } from '@/types'
import { crudService } from '@/services/crudService'
import { useNotificationStore } from '@/stores/notificationStore'

const notificationStore = useNotificationStore()
const loading = ref(false)
const error = ref<string | null>(null)
const products = ref<Product[]>([])

const productService = crudService<Product>('/product')

async function loadProducts() {
  loading.value = true
  error.value = null
  try {
    const { data } = await productService.list()
    products.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '加載失敗'
  } finally {
    loading.value = false
  }
}

async function deleteProduct(id: number) {
  if (!confirm('確定要刪除此活動嗎？')) return
  try {
    await productService.delete(id)
    notificationStore.success('活動已刪除')
    await loadProducts()
  } catch (e: any) {
    notificationStore.error(e?.message || '刪除失敗')
  }
}

onMounted(() => loadProducts())
</script>
