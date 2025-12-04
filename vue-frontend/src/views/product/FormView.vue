<template>
  <div class="card shadow border-0 my-4 fw-bold">
    <div class="card-header bg-secondary bg-gradient ml-0 py-3">
      <div class="row">
        <div class="col-12 text-center">
          <h5 class="text-white py-2">{{ isEdit ? '修改' : '新增' }}活動</h5>
        </div>
      </div>
    </div>
    <div class="card-body p-4 fw-bold">
      <form @submit.prevent="onSubmit" enctype="multipart/form-data" novalidate class="row">
        <div class="row">
          <div class="col-10">
            <div class="border p-3">
              <!-- Title -->
              <div class="form-floating py-2 col-12 fw-bold">
                <input v-model="values.title" type="text" class="form-control border-0 shadow text-bg-info fw-bold"
                  placeholder="活動名稱" :class="{ 'is-invalid': errors.title }" />
                <label class="ms-2 fw-bold">活動名稱 *</label>
                <div v-if="errors.title" class="invalid-feedback">{{ errors.title }}</div>
              </div>

              <!-- ISBN (簡介) -->
              <div class="form-floating py-2 col-12 fw-bold">
                <input v-model="values.isbn" type="text" class="form-control border-0 shadow fw-bold" placeholder="活動簡介"
                  :class="{ 'is-invalid': errors.isbn }" />
                <label class="ms-2 fw-bold">活動簡介 *</label>
                <div v-if="errors.isbn" class="invalid-feedback">{{ errors.isbn }}</div>
              </div>

              <!-- Event Date -->
              <div class="form-floating py-2 col-12">
                <input v-model="values.hDate" type="text" class="form-control border-0 shadow" placeholder="活動日期" />
                <label class="ms-2">活動日期</label>
              </div>

              <!-- Price -->
              <div class="form-floating py-2 col-12">
                <input v-model.number="values.listPrice" type="number" step="0.01" class="form-control border-0 shadow"
                  placeholder="費用" :class="{ 'is-invalid': errors.listPrice }" />
                <label class="ms-2">一般費用 *</label>
                <div v-if="errors.listPrice" class="invalid-feedback">{{ errors.listPrice }}</div>
              </div>

              <!-- Category Select -->
              <div class="form-floating py-2 col-12">
                <select v-model="values.categoryId" class="form-select border-0 shadow"
                  :class="{ 'is-invalid': errors.categoryId }">
                  <option :value="null" disabled>--選活動類別--</option>
                  <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
                    {{ cat.name }}
                  </option>
                </select>
                <label class="ms-2 fw-bold">活動類別 *</label>
                <div v-if="errors.categoryId" class="invalid-feedback">{{ errors.categoryId }}</div>
              </div>

              <!-- Company Select -->
              <div class="form-floating py-2 col-12">
                <select v-model="values.companyId" class="form-select border-0 shadow text-bg-info fw-bold"
                  :class="{ 'is-invalid': errors.companyId }">
                  <option :value="null" disabled>--選主辦單位--</option>
                  <option v-for="comp in companies" :key="comp.companyId" :value="comp.companyId">
                    {{ comp.name }}
                  </option>
                </select>
                <label class="ms-2 fw-bold">主辦單位 *</label>
                <div v-if="errors.companyId" class="invalid-feedback">{{ errors.companyId }}</div>
              </div>

              <!-- Held YN Select -->
              <div class="form-floating py-2 col-12">
                <select v-model="values.heldYN" class="form-select border-0 text-bg-warning shadow">
                  <option value="Y">是</option>
                  <option value="N">否</option>
                </select>
                <label class="ms-2 fw-bold">是否舉辦(是Y/否N) *</label>
              </div>

              <!-- Description -->
              <div class="py-2 col-12">
                <label class="ms-2 fw-bold">說明</label>
                <textarea v-model="values.description" class="form-control border-0 shadow text-bg-info fw-bold"
                  rows="14" cols="150"></textarea>
              </div>

              <!-- File Upload -->
              <div class="form-floating py-2 col-12">
                <input type="file" name="files" multiple class="form-control border-0 shadow fw-bold"
                  @change="handleFileChange" />
                <label class="ms-2 fw-bold">上傳DM照片</label>
              </div>

              <!-- Submit Buttons -->
              <div class="row pt-2 fw-bold">
                <div class="col-6 col-md-3">
                  <button type="submit" class="btn btn-primary form-control fw-bold" :disabled="loading">
                    <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                    {{ isEdit ? '修改' : '新增' }}
                  </button>
                </div>
                <div class="col-6 col-md-3">
                  <router-link to="/app/product" class="btn btn-outline-primary border form-control fw-bold">
                    回清單
                  </router-link>
                </div>
              </div>

              <!-- Error Message -->
              <div v-if="error" class="alert alert-danger mt-3 fw-bold">{{ error }}</div>
            </div>
          </div>

          <!-- Image Preview Section -->
          <div class="col-2">
            <div v-if="values.productImages && values.productImages.length > 0">
              <div v-for="image in values.productImages" :key="image.id" class="border p-1 m-2 text-center">
                <img :src="image.imageUrl" width="100%" style="border-radius: 5px; border: 1px solid #bbb9b9"
                  alt="活動圖片" />
                <button type="button" class="btn btn-danger btn-sm mt-2 fw-bold" @click="deleteImage(image.id)">
                  <i class="bi bi-trash-fill"></i> 刪除
                </button>
              </div>
            </div>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import * as yup from 'yup'
import type { Product, Category, Company } from '@/types'
import { crudService } from '@/services/crudService'
import { useNotificationStore } from '@/stores/notificationStore'

const router = useRouter()
const route = useRoute()
const notificationStore = useNotificationStore()
const productService = crudService<Product>('/product')
const categoryService = crudService<Category>('/category')
const companyService = crudService<Company>('/company')

const loading = ref(false)
const error = ref<string | null>(null)
const isEdit = ref(!!route.params.id)
const categories = ref<Category[]>([])
const companies = ref<Company[]>([])
const uploadedFiles = ref<File[]>([])

const values = reactive<Partial<Product>>({
  id: undefined,
  title: '',
  description: '',
  isbn: '',
  categoryId: undefined,
  companyId: undefined,
  hDate: '',
  heldYN: 'N',
  listPrice: 0,
  productImages: []
})

const errors = reactive<any>({})

const schema = yup.object({
  title: yup.string().trim().required('活動名稱為必填'),
  isbn: yup.string().trim().required('活動簡介為必填'),
  categoryId: yup.number().positive('類別為必填').required('類別為必填'),
  companyId: yup.number().positive('主辦單位為必填').required('主辦單位為必填'),
  listPrice: yup.number().typeError('費用為必填').required('費用為必填').min(0, '費用不能為負數')
})

async function validate() {
  try {
    // Ensure numeric fields are actually numbers
    const valuesToValidate = {
      ...values,
      categoryId: values.categoryId !== null && values.categoryId !== undefined ? Number(values.categoryId) : null,
      companyId: values.companyId !== null && values.companyId !== undefined ? Number(values.companyId) : null,
      listPrice: values.listPrice !== null && values.listPrice !== undefined ? Number(values.listPrice) : 0
    }

    // Check required fields manually before yup validation
    if (!valuesToValidate.categoryId) {
      (errors as any).categoryId = '類別為必填'
      throw new Error('Validation failed')
    }
    if (!valuesToValidate.companyId) {
      (errors as any).companyId = '主辦單位為必填'
      throw new Error('Validation failed')
    }

    await schema.validate(valuesToValidate, { abortEarly: false })
    Object.keys(errors).forEach(k => delete (errors as any)[k])
    return true
  } catch (err: any) {
    if (err.inner) {
      Object.keys(errors).forEach(k => delete (errors as any)[k])
      for (const e of err.inner) {
        if (e.path) (errors as any)[e.path] = e.message
      }
    }
    return false
  }
}

async function handleFileChange(event: Event) {
  const target = event.target as HTMLInputElement
  const files = target.files
  if (files) {
    uploadedFiles.value = Array.from(files)
  }
}

async function deleteImage(imageId: number) {
  if (!confirm('確定要刪除此圖片嗎？')) return
  try {
    // Call backend to delete image
    await fetch(`/api/product/image/${imageId}`, { method: 'DELETE' })
    notificationStore.success('圖片已刪除')
    // Reload product data
    if (isEdit.value && route.params.id) {
      const { data } = await productService.getById(Number(route.params.id))
      Object.assign(values, data.data)
    }
  } catch (e: any) {
    notificationStore.error(e?.message || '刪除圖片失敗')
  }
}

async function onSubmit() {
  error.value = null
  const ok = await validate()
  if (!ok) return

  loading.value = true
  try {
    const formData = new FormData()
    formData.append('title', values.title || '')
    formData.append('description', values.description || '')
    formData.append('isbn', values.isbn || '')
    formData.append('categoryId', values.categoryId?.toString() || '')
    formData.append('companyId', values.companyId?.toString() || '')
    formData.append('hDate', values.hDate || '')
    formData.append('heldYN', values.heldYN || 'N')
    formData.append('listPrice', values.listPrice?.toString() || '0')

    // Add uploaded files
    for (const file of uploadedFiles.value) {
      formData.append('files', file)
    }

    if (isEdit.value && values.id) {
      await fetch(`/api/product/${values.id}`, {
        method: 'PUT',
        body: formData
      })
      notificationStore.success('活動已更新')
    } else {
      await fetch('/api/product', {
        method: 'POST',
        body: formData
      })
      notificationStore.success('活動已新增')
    }
    router.push('/app/product')
  } catch (e: any) {
    error.value = e?.message || '操作失敗'
  } finally {
    loading.value = false
  }
}

async function loadCategories() {
  try {
    const { data } = await categoryService.list()
    if (data?.data) {
      categories.value = Array.isArray(data.data) ? data.data : (data.data as any)?.items || []
    }
  } catch (e: any) {
    console.error('Failed to load categories:', e)
  }
}

async function loadCompanies() {
  try {
    const { data } = await companyService.list()
    if (data?.data) {
      companies.value = Array.isArray(data.data) ? data.data : (data.data as any)?.items || []
    }
  } catch (e: any) {
    console.error('Failed to load companies:', e)
  }
}

onMounted(async () => {
  await loadCategories()
  await loadCompanies()

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
.card {
  margin-bottom: 2rem;
}

.form-floating {
  position: relative;
}

.table-responsive {
  overflow-x: auto;
}

.border {
  border: 1px solid #dee2e6;
}
</style>
