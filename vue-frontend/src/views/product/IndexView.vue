<template>
  <div class="card shadow border-0 my-4">
    <div class="card-header bg-secondary bg-gradient ml-0 py-3">
      <div class="row">
        <div class="col-12 text-center">
          <h2 class="text-white py-2 fw-bold">活動清單</h2>
        </div>
      </div>
    </div>
    <div class="card-body p-4">
      <!-- Search & Add Button -->
      <div class="row pb-3">
        <div class="col-6"></div>
        <div class="col-6 text-end">
          <router-link to="/app/product/add" class="btn btn-primary fw-bold">
            <i class="bi bi-plus-circle fw-bold"></i> 新增活動
          </router-link>
        </div>
      </div>

      <!-- Loading, Error, or Empty State -->
      <div v-if="loading" class="alert alert-info fw-bold">加載中...</div>
      <div v-else-if="error" class="alert alert-danger fw-bold">{{ error }}</div>
      <div v-else-if="products.length === 0" class="alert alert-warning fw-bold">無活動項目</div>

      <!-- DataTable -->
      <div v-else class="table-responsive">
        <table class="table table-bordered table-striped fw-bold" style="width: 100%">
          <thead class="table-light">
            <tr>
              <th>名稱</th>
              <th>舉辦(是Y/否N)</th>
              <th>日期</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="product in products" :key="product.id">
              <td>{{ product.title }}</td>
              <td class="text-center">
                <span v-if="product.heldYN === 'Y'" class="badge bg-success fw-bold">是</span>
                <span v-else class="badge bg-danger fw-bold">否</span>
              </td>
              <td>{{ product.hDate || '未設定' }}</td>
              <td class="text-center">
                <router-link :to="`/app/product/edit/${product.id}`" class="btn btn-warning btn-sm fw-bold me-1">
                  <i class="bi bi-pencil-fill"></i> 編輯
                </router-link>
                <button class="btn btn-danger btn-sm fw-bold" @click="deleteProduct(product.id)">
                  <i class="bi bi-trash-fill"></i> 刪除
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
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
    if (data?.data) {
      // Handle both paginated and direct array responses
      products.value = Array.isArray(data.data)
        ? data.data
        : (data.data as any)?.items || []
    } else {
      products.value = []
    }
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
