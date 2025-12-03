<template>
  <div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>懷恩塔-塔位清單</h2>
      <router-link to="/kindness/add" class="btn btn-primary">新增塔位</router-link>
    </div>

    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else>
      <div class="row mb-3">
        <div class="col-md-3">
          <label class="form-label">選擇樓層</label>
          <select v-model="selectedFloor" class="form-select" @change="loadPositions">
            <option v-for="f in floors" :key="f" :value="f">{{ f }}樓</option>
          </select>
        </div>
        <div class="col-md-3">
          <label class="form-label">選擇區域</label>
          <select v-model="selectedSection" class="form-select" @change="loadPositions">
            <option v-for="s in sections" :key="s" :value="s">{{ s }}</option>
          </select>
        </div>
      </div>

      <div class="grid-container">
        <div
          v-for="pos in positions"
          :key="pos.kindnessPositionId"
          class="grid-cell"
          :class="{ occupied: pos.occupantName }"
          @click="selectPosition(pos)"
        >
          <div class="cell-content">
            <small>{{ pos.floor }}F-{{ pos.row }}-{{ pos.column }}</small>
            <div v-if="pos.occupantName" class="occupant">{{ pos.occupantName }}</div>
          </div>
        </div>
      </div>

      <div v-if="selectedPos" class="mt-4 card">
        <div class="card-header">位置詳情</div>
        <div class="card-body">
          <p><strong>位置ID:</strong> {{ selectedPos.kindnessPositionId }}</p>
          <p><strong>樓層:</strong> {{ selectedPos.floor }}樓</p>
          <p><strong>區域:</strong> {{ selectedPos.section }}</p>
          <p><strong>列:</strong> {{ selectedPos.row }}</p>
          <p><strong>欄:</strong> {{ selectedPos.column }}</p>
          <p><strong>佔用者:</strong> {{ selectedPos.occupantName || '未佔用' }}</p>
          <div class="btn-group">
            <router-link :to="`/kindness/edit/${selectedPos.kindnessPositionId}`" class="btn btn-warning btn-sm">編輯</router-link>
            <button class="btn btn-danger btn-sm" @click="deletePosition">刪除</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { KindnessPosition } from '@/types'
import { kindnessService } from '@/services/kindnessService'
import { useNotificationStore } from '@/stores/notificationStore'

const notificationStore = useNotificationStore()

const loading = ref(false)
const error = ref<string | null>(null)
const positions = ref<KindnessPosition[]>([])
const selectedPos = ref<KindnessPosition | null>(null)
const selectedFloor = ref(1)
const selectedSection = ref('甲區')
const floors = ref([1, 2, 3])
const sections = ref(['甲區', '乙區', '丙區', '丁區', '戊區', '己區'])

async function loadPositions() {
  loading.value = true
  error.value = null
  try {
    const { data } = await kindnessService.query({ floor: selectedFloor.value, section: selectedSection.value })
    positions.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '加載失敗'
  } finally {
    loading.value = false
  }
}

function selectPosition(pos: KindnessPosition) {
  selectedPos.value = pos
}

async function deletePosition() {
  if (!selectedPos.value) return
  if (!confirm('確定要刪除此塔位嗎？')) return

  try {
    await kindnessService.delete(selectedPos.value.kindnessPositionId)
    notificationStore.success('塔位已刪除')
    await loadPositions()
    selectedPos.value = null
  } catch (e: any) {
    notificationStore.error(e?.message || '刪除失敗')
  }
}

onMounted(() => loadPositions())
</script>

<style scoped>
.grid-container {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(80px, 1fr));
  gap: 8px;
  margin-bottom: 2rem;
}

.grid-cell {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: center;
  cursor: pointer;
  border-radius: 4px;
  transition: all 0.2s;
  min-height: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.grid-cell:hover {
  background-color: #f0f0f0;
}

.grid-cell.occupied {
  background-color: #fff3cd;
  border-color: #ffc107;
}

.cell-content {
  font-size: 0.75rem;
}

.occupant {
  font-weight: bold;
  margin-top: 4px;
}
</style>
