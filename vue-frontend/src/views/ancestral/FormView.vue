<template>
  <div class="container py-4" style="max-width:600px">
    <h2>{{ isEdit ? '編輯牌位' : '新增牌位' }}</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">區域</label>
        <select v-model="values.section" class="form-control" :class="{ 'is-invalid': errors.section }">
          <option value="">選擇區域</option>
          <option v-for="s in sections" :key="s" :value="s">{{ s }}</option>
        </select>
        <div v-if="errors.section" class="invalid-feedback">{{ errors.section }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">層級</label>
        <input v-model.number="values.level" type="number" class="form-control" :class="{ 'is-invalid': errors.level }" />
        <div v-if="errors.level" class="invalid-feedback">{{ errors.level }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">位置</label>
        <input v-model.number="values.position" type="number" class="form-control"
          :class="{ 'is-invalid': errors.position }" />
        <div v-if="errors.position" class="invalid-feedback">{{ errors.position }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">佔用者名稱</label>
        <input v-model="values.occupantName" type="text" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">電話</label>
        <input v-model="values.phone" type="tel" class="form-control" />
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
        <router-link to="/app/ancestral" class="btn btn-secondary">返回</router-link>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import * as yup from 'yup'
import { ancestralService } from '@/services/ancestralService'
import { useNotificationStore } from '@/stores/notificationStore'

const router = useRouter()
const route = useRoute()
const notificationStore = useNotificationStore()

const loading = ref(false)
const error = ref<string | null>(null)
const isEdit = ref(!!route.params.id)

const values = reactive({
  ancestralPositionId: 0,
  positionId: '',
  side: '',
  section: '',
  level: 0,
  position: 0,
  occupantName: '',
  phone: '',
  dateRegistered: new Date().toISOString().split('T')[0]
})

const errors = reactive<any>({})
const sections = ref(['甲區', '乙區', '丙區', '丁區', '中區'])

const schema = yup.object({
  section: yup.string().required('區域為必填'),
  level: yup.number().required('層級為必填').min(1).max(10),
  position: yup.number().required('位置為必填').min(1).max(10)
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
      await ancestralService.update(values.ancestralPositionId, values as any)
      notificationStore.success('牌位已更新')
    } else {
      await ancestralService.create(values as any)
      notificationStore.success('牌位已新增')
    }
    router.push('/ancestral')
  } catch (e: any) {
    error.value = e?.message || '操作失敗'
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  if (isEdit.value) {
    try {
      const { data } = await ancestralService.getById(Number(route.params.id))
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
