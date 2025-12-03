<template>
  <div class="container-fluid py-4">
    <h2>懷恩塔-塔位查詢</h2>

    <div class="card mb-3">
      <div class="card-body">
        <div class="row">
          <div class="col-md-3">
            <label class="form-label">搜尋名稱</label>
            <input v-model="searchName" type="text" class="form-control" placeholder="輸入佔用者名稱" />
          </div>
          <div class="col-md-3">
            <label class="form-label">樓層</label>
            <select v-model.number="selectedFloor" class="form-select">
              <option value="">全部</option>
              <option v-for="f in floors" :key="f" :value="f">{{ f }}樓</option>
            </select>
          </div>
          <div class="col-md-3">
            <label class="form-label">區域</label>
            <select v-model="selectedSection" class="form-select">
              <option value="">全部</option>
              <option v-for="s in sections" :key="s" :value="s">{{ s }}</option>
            </select>
          </div>
          <div class="col-md-3">
            <label class="form-label">&nbsp;</label>
            <button @click="search" class="btn btn-primary w-100">查詢</button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="loading" class="alert alert-info">查詢中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else-if="results.length === 0" class="alert alert-warning">無符合結果</div>
    <div v-else>
      <div class="table-responsive">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>位置ID</th>
              <th>樓層</th>
              <th>區域</th>
              <th>列</th>
              <th>欄</th>
              <th>佔用者</th>
              <th>登記日期</th>
              <th>操作</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="pos in results" :key="pos.kindnessPositionId">
              <td>{{ pos.kindnessPositionId }}</td>
              <td>{{ pos.floor }}樓</td>
              <td>{{ pos.section }}</td>
              <td>{{ pos.row }}</td>
              <td>{{ pos.column }}</td>
              <td>{{ pos.occupantName || '-' }}</td>
              <td>{{ formatDate(pos.dateRegistered) }}</td>
              <td>
                <router-link :to="`/kindness/edit/${pos.kindnessPositionId}`" class="btn btn-warning btn-sm">編輯</router-link>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { KindnessPosition } from '@/types'
import { kindnessService } from '@/services/kindnessService'

const loading = ref(false)
const error = ref<string | null>(null)
const searchName = ref('')
const selectedFloor = ref<number | ''>('')
const selectedSection = ref('')
const results = ref<KindnessPosition[]>([])
const floors = ref([1, 2, 3])
const sections = ref(['甲區', '乙區', '丙區', '丁區', '戊區', '己區'])

function formatDate(date: string | Date) {
  if (!date) return '-'
  const d = new Date(date)
  return d.toLocaleDateString('zh-TW')
}

async function search() {
  loading.value = true
  error.value = null
  try {
    const { data } = await kindnessService.query({
      occupantName: searchName.value,
      floor: selectedFloor.value || undefined,
      section: selectedSection.value
    })
    results.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '查詢失敗'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.container-fluid {
  margin-top: 1.5rem
}
</style>
