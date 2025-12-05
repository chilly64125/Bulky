<template>
  <div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>宗祠牌位管理</h2>
      <router-link to="/app/ancestral/add" class="btn btn-primary">新增牌位</router-link>
    </div>

    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else>
      <div class="mb-3">
        <label class="form-label">選擇區域</label>
        <select v-model="selectedSection" class="form-select" @change="loadPositions">
          <option v-for="s in sections" :key="s" :value="s">{{ s }}</option>
        </select>
      </div>

      <div class="grid-container">
        <div v-for="pos in positions" :key="pos.ancestralPositionId" class="grid-cell"
          :class="{ occupied: pos.occupantName }" @click="selectPosition(pos)">
          <div class="cell-content">
            <small>{{ pos.section }}-{{ pos.level }}-{{ pos.position }}</small>
            <div v-if="pos.occupantName" class="occupant">{{ pos.occupantName }}</div>
          </div>
        </div>
      </div>

      <div v-if="selectedPos" class="mt-4 card">
        <div class="card-header">位置詳情</div>
        <div class="card-body">
          <p><strong>位置ID:</strong> {{ selectedPos.ancestralPositionId }}</p>
          <p><strong>區域:</strong> {{ selectedPos.section }}</p>
          <p><strong>層級:</strong> {{ selectedPos.level }}</p>
          <p><strong>位置:</strong> {{ selectedPos.position }}</p>
          <p><strong>佔用者:</strong> {{ selectedPos.occupantName || '未佔用' }}</p>
          <p><strong>電話:</strong> {{ selectedPos.phone || '-' }}</p>
          <div class="btn-group">
            <router-link :to="`/ancestral/edit/${selectedPos.ancestralPositionId}`"
              class="btn btn-warning btn-sm">編輯</router-link>
            <button class="btn btn-danger btn-sm" @click="deletePosition">刪除</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { AncestralPosition } from '@/types'
import { ancestralService } from '@/services/ancestralService'
import { useNotificationStore } from '@/stores/notificationStore'

const notificationStore = useNotificationStore()

const loading = ref(false)
const error = ref<string | null>(null)
const positions = ref<AncestralPosition[]>([])
const selectedPos = ref<AncestralPosition | null>(null)
const selectedSection = ref('甲區')
const sections = ref(['甲區', '乙區', '丙區', '丁區', '中區'])

async function loadPositions() {
  loading.value = true
  error.value = null
  try {
    const { data } = await ancestralService.query({ section: selectedSection.value })
    positions.value = data.data || []
  } catch (e: any) {
    error.value = e?.message || '加載失敗'
  } finally {
    loading.value = false
  }
}

function selectPosition(pos: AncestralPosition) {
  selectedPos.value = pos
}

async function deletePosition() {
  if (!selectedPos.value) return
  if (!confirm('確定要刪除此牌位嗎？')) return

  try {
    await ancestralService.delete(selectedPos.value.ancestralPositionId)
    notificationStore.success('牌位已刪除')
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
  background-color: #e3f2fd;
  border-color: #1976d2;
}

.cell-content {
  font-size: 0.75rem;
}

.occupant {
  font-weight: bold;
  margin-top: 4px;
}
</style>
