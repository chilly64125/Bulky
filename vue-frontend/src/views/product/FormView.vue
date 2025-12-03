<template>
  <div class="container py-4" style="max-width:600px">
    <h2>{{ isEdit ? '編輯活動' : '新增活動' }}</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">標題</label>
        <input v-model="values.title" type="text" class="form-control" :class="{'is-invalid': errors.title}" />
        <div v-if="errors.title" class="invalid-feedback">{{ errors.title }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">描述</label>
        <textarea v-model="values.description" class="form-control" rows="3"></textarea>
      </div>

      <div class="mb-3">
        <label class="form-label">類別ID</label>
        <input v-model.number="values.categoryId" type="number" class="form-control" :class="{'is-invalid': errors.categoryId}" />
        <div v-if="errors.categoryId" class="invalid-feedback">{{ errors.categoryId }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">價格</label>
        <input v-model.number="values.price" type="number" step="0.01" class="form-control" :class="{'is-invalid': errors.price}" />
        <div v-if="errors.price" class="invalid-feedback">{{ errors.price }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">圖片URL</label>
        <input v-model="values.imageUrl" type="url" class="form-control" />
      </div>

      <div v-if="error" class="alert alert-danger">{{ error }}</div>

      <div class="d-flex gap-2">
        <button class="btn btn-primary" :disabled="loading">
          <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
          {{ isEdit ? '更新' : '新增' }}
        </button>
        <router-link to="/app/product" class="btn btn-secondary">返回</router-link>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import * as yup from 'yup'
import type { Product } from '@/types'
import { crudService } from '@/services/crudService'
import { useNotificationStore } from '@/stores/notificationStore'

const router = useRouter()
const route = useRoute()
const notificationStore = useNotificationStore()
const productService = crudService<Product>('/product')

const loading = ref(false)
const error = ref<string | null>(null)
const isEdit = ref(!!route.params.id)

const values = reactive<Partial<Product>>({
  title: '',
  description: '',
  categoryId: 0,
  price: 0,
  imageUrl: ''
})

const errors = reactive<any>({})

const schema = yup.object({
  title: yup.string().required('標題為必填'),
  categoryId: yup.number().required('類別ID為必填'),
  price: yup.number().required('價格為必填').positive()
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
    if (isEdit.value && values.productId) {
      await productService.update(values.productId, values)
      notificationStore.success('活動已更新')
    } else {
      await productService.create(values)
      notificationStore.success('活動已新增')
    }
    router.push('/product')
  } catch (e: any) {
    error.value = e?.message || '操作失敗'
  } finally {
    loading.value = false
  }
}

onMounted(async () => {
  if (isEdit.value && route.params.id) {
    try {
      const { data } = await productService.getById(Number(route.params.id))
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
