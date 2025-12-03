<template>
  <div class="container py-4" style="max-width:600px">
    <h2>{{ isEdit ? '編輯類別' : '新增類別' }}</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">類別名稱</label>
        <input v-model="values.name" type="text" class="form-control" :class="{'is-invalid': errors.name}" />
        <div v-if="errors.name" class="invalid-feedback">{{ errors.name }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">排序</label>
        <input v-model.number="values.displayOrder" type="number" class="form-control" />
      </div>

      <div class="mb-3">
        <div class="form-check">
          <input v-model="values.isActive" type="checkbox" class="form-check-input" id="isActive" />
          <label class="form-check-label" for="isActive">啟用</label>
        </div>
      </div>

      <div v-if="error" class="alert alert-danger">{{ error }}</div>

      <div class="d-flex gap-2">
        <button class="btn btn-primary" :disabled="loading">
          <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
          {{ isEdit ? '更新' : '新增' }}
        </button>
        <router-link to="/category" class="btn btn-secondary">返回</router-link>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import * as yup from 'yup'
import type { Category } from '@/types'
import { crudService } from '@/services/crudService'
import { useNotificationStore } from '@/stores/notificationStore'

const router = useRouter()
const route = useRoute()
const notificationStore = useNotificationStore()
const categoryService = crudService<Category>('/category')

const loading = ref(false)
const error = ref<string | null>(null)
const isEdit = ref(!!route.params.id)

const values = reactive<Partial<Category>>({
  name: '',
  displayOrder: 0,
  isActive: true
})

const errors = reactive<any>({})

const schema = yup.object({
  name: yup.string().required('類別名稱為必填')
})

async function validate() {
  try {
    await schema.validate(values, { abortEarly: false })
    Object.keys(errors).forEach(k => delete (errors as any)[k])
    return true
  } catch (err: any) {
    Object.keys(errors).forEach(k => delete (errors as any)[k])
    if (err.inner) {
      for (const e of err.inner) {
        if (e.path) (errors as any)[e.path] = e.message
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
    if (isEdit.value && values.categoryId) {
      await categoryService.update(values.categoryId, values)
      notificationStore.success('類別已更新')
    } else {
      await categoryService.create(values)
      notificationStore.success('類別已新增')
    }
    router.push('/category')
  } catch (e: any) {
    error.value = e?.message || '操作失敗'
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  if (isEdit.value && route.params.id) {
    try {
      const { data } = await categoryService.getById(Number(route.params.id))
      Object.assign(values, data.data)
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
