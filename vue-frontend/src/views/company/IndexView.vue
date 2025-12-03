<template>
  <div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>宗親會管理</h2>
      <router-link to="/company/add" class="btn btn-primary">新增宗親會</router-link>
    </div>

    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else-if="companies.length === 0" class="alert alert-warning">無宗親會</div>
    <div v-else class="table-responsive">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>ID</th>
            <th>名稱</th>
            <th>城市</th>
            <th>電話</th>
            <th>電郵</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="comp in companies" :key="comp.companyId">
            <td>{{ comp.companyId }}</td>
            <td>{{ comp.name }}</td>
            <td>{{ comp.city }}</td>
            <td>{{ comp.phone }}</td>
            <td>{{ comp.email }}</td>
            <td>
              <router-link :to="`/company/edit/${comp.companyId}`" class="btn btn-warning btn-sm">編輯</router-link>
              <button class="btn btn-danger btn-sm" @click="deleteCompany(comp.companyId)">刪除</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Company } from '@/types'
import { crudService } from '@/services/crudService'
import { useNotificationStore } from '@/stores/notificationStore'

const notificationStore = useNotificationStore()
const loading = ref(false)
const error = ref<string | null>(null)
const companies = ref<Company[]>([])

const companyService = crudService<Company>('/company')

async function loadCompanies() {
  loading.value = true
  error.value = null
  try {
    const { data } = await companyService.list()
    companies.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '加載失敗'
  } finally {
    loading.value = false
  }
}

async function deleteCompany(id: number) {
  if (!confirm('確定要刪除此宗親會嗎？')) return
  try {
    await companyService.delete(id)
    notificationStore.success('宗親會已刪除')
    await loadCompanies()
  } catch (e: any) {
    notificationStore.error(e?.message || '刪除失敗')
  }
}

onMounted(() => loadCompanies())
</script>
