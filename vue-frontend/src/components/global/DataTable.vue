<template>
  <div class="table-wrapper">
    <div v-if="loading" class="alert alert-info">加載中...</div>
    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>
    <div v-else class="table-responsive">
      <table class="table table-hover" :class="tableClass">
        <thead v-if="columns.length > 0">
          <tr>
            <th v-for="col in columns" :key="col.key">
              {{ col.label }}
            </th>
            <th v-if="hasActions">操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(row, idx) in rows" :key="idx">
            <td v-for="col in columns" :key="col.key">
              {{ row[col.key] }}
            </td>
            <td v-if="hasActions" class="btn-group-cell">
              <button v-if="onEdit" class="btn btn-warning btn-sm" @click="onEdit(row)">編輯</button>
              <button v-if="onDelete" class="btn btn-danger btn-sm" @click="onDelete(row)">刪除</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'

interface Column {
  key: string
  label: string
}

interface Props {
  columns: Column[]
  rows: any[]
  loading?: boolean
  error?: string | null
  tableClass?: string
  onEdit?: (row: any) => void
  onDelete?: (row: any) => void
}

const props = withDefaults(defineProps<Props>(), {
  loading: false,
  error: null,
  tableClass: 'table-striped'
})

const hasActions = computed(() => !!props.onEdit || !!props.onDelete)
</script>

<style scoped>
.table-wrapper {
  width: 100%;
}

.btn-group-cell {
  display: flex;
  gap: 4px;
}

.btn-group-cell .btn {
  white-space: nowrap;
}
</style>
