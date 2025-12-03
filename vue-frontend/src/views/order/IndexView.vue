<template>
  <div class="container-fluid py-4">
    <h2>訂單管理</h2>

    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else-if="orders.length === 0" class="alert alert-warning">無訂單</div>
    <div v-else class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>訂單ID</th>
            <th>用戶ID</th>
            <th>訂單日期</th>
            <th>總價</th>
            <th>狀態</th>
            <th>付款狀態</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="order in orders" :key="order.orderId">
            <td>{{ order.orderId }}</td>
            <td>{{ order.userId }}</td>
            <td>{{ formatDate(order.orderDate) }}</td>
            <td>${{ order.totalPrice }}</td>
            <td><span class="badge" :class="getStatusClass(order.status)">{{ order.status }}</span></td>
            <td><span class="badge" :class="getPaymentStatusClass(order.paymentStatus)">{{ order.paymentStatus }}</span></td>
            <td>
              <button class="btn btn-info btn-sm" @click="viewOrder(order.orderId)">查看</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Order } from '@/types'
import { crudService } from '@/services/crudService'

const loading = ref(false)
const error = ref<string | null>(null)
const orders = ref<Order[]>([])

const orderService = crudService<Order>('/order')

function formatDate(date: string | Date) {
  if (!date) return '-'
  const d = new Date(date)
  return d.toLocaleDateString('zh-TW')
}

function getStatusClass(status: string) {
  const map: Record<string, string> = {
    pending: 'bg-warning',
    processing: 'bg-info',
    completed: 'bg-success',
    cancelled: 'bg-danger'
  }
  return map[status] || 'bg-secondary'
}

function getPaymentStatusClass(status: string) {
  const map: Record<string, string> = {
    pending: 'bg-warning',
    paid: 'bg-success',
    failed: 'bg-danger'
  }
  return map[status] || 'bg-secondary'
}

function viewOrder(id: number) {
  // Navigate to order detail if needed
  console.log('Viewing order:', id)
}

async function loadOrders() {
  loading.value = true
  error.value = null
  try {
    const { data } = await orderService.list()
    orders.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '加載失敗'
  } finally {
    loading.value = false
  }
}

onMounted(() => loadOrders())
</script>
