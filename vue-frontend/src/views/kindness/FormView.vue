<template>
  <div class="container py-4" style="max-width:600px">
    <h2>{{ isEdit ? '編輯塔位' : '新增塔位' }}</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">樓層</label>
        <select v-model.number="values.floor" class="form-control" :class="{ 'is-invalid': errors.floor }">
          <option value="">選擇樓層</option>
          <option v-for="f in floors" :key="f" :value="f">{{ f }}樓</option>
        </select>
        <div v-if="errors.floor" class="invalid-feedback">{{ errors.floor }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">區域</label>
        <select v-model="values.section" class="form-control" :class="{ 'is-invalid': errors.section }">
          <option value="">選擇區域</option>
          <option v-for="s in sections" :key="s" :value="s">{{ s }}</option>
        </select>
        <div v-if="errors.section" class="invalid-feedback">{{ errors.section }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">列</label>
        <input v-model.number="values.row" type="number" class="form-control" :class="{ 'is-invalid': errors.row }" />
        <div v-if="errors.row" class="invalid-feedback">{{ errors.row }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">欄</label>
        <input v-model.number="values.column" type="number" class="form-control"
          :class="{ 'is-invalid': errors.column }" />
        <div v-if="errors.column" class="invalid-feedback">{{ errors.column }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">佔用者名稱</label>
        <input v-model="values.occupantName" type="text" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">登記日期</label>
        <input v-model="values.dateRegistered" type="date" class="form-control" />
      </div>

      <div v-if="error" class="alert alert-danger">{{ error }}</div>

      <div class="d-flex gap-2">
        <button class="btn btn-primary" :disabled="loading">
          <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
          {{ isEdit ? '更新' : '新增' }}
        </button>
        <router-link to="/app/kindness" class="btn btn-secondary">返回</router-link>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import * as yup from 'yup'
import { kindnessService } from '@/services/kindnessService'
import { useNotificationStore } from '@/stores/notificationStore'

const router = useRouter()
const route = useRoute()
const notificationStore = useNotificationStore()

const loading = ref(false)
const error = ref<string | null>(null)
const isEdit = ref(!!route.params.id)

const values = reactive({
  kindnessPositionId: 0,
  positionId: '',
  floor: 1,
  section: '',
  row: 0,
  column: 0,
  occupantName: '',
  dateRegistered: new Date().toISOString().split('T')[0]
})

const errors = reactive<any>({})
const floors = ref([1, 2, 3])
const sections = ref(['甲區', '乙區', '丙區', '丁區', '戊區', '己區'])

const schema = yup.object({
  floor: yup.number().required('樓層為必填'),
  section: yup.string().required('區域為必填'),
  row: yup.number().required('列為必填').min(1),
  column: yup.number().required('欄為必填').min(1)
})

async function validate() {
  try {
    await schema.validate(values, { abortEarly: false })
    Object.keys(errors).forEach(k => delete (errors as any)[k])
    return true
  } catch (err: any) {
    Object.keys(errors).forEach(k => delete (errors as any)[k])
    if (err.inner && err.inner.length) {
      for (const e of err.inner) {
        if (e.path && e.message) (errors as any)[e.path] = e.message
      }
    }
    return false
  }
}

async function onSubmit() {
  error.value = null
  const ok = await validate()
  if (!ok) return

  loading.value = true
  try {
    if (isEdit.value) {
      await kindnessService.update(values.kindnessPositionId, values as any)
      notificationStore.success('塔位已更新')
    } else {
      await kindnessService.create(values as any)
      notificationStore.success('塔位已新增')
    }
    router.push('/kindness')
  } catch (e: any) {
    error.value = e?.message || '操作失敗'
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  if (isEdit.value) {
    try {
      const { data } = await kindnessService.getById(Number(route.params.id))
      const pos = data.data
      Object.assign(values, pos)
    } catch (e: any) {
      error.value = e?.message || '加載失敗'
    }
  }
})
</script>

<style scoped>
.container {
  margin-top: 1.5rem
}
</style>
