<template>
  <div class="form-layout">
    <div class="form-header mb-3">
      <h3>{{ title }}</h3>
    </div>

    <form @submit.prevent="onSubmit" novalidate class="form-body">
      <slot></slot>

      <div v-if="error" class="alert alert-danger mt-3">{{ error }}</div>

      <div class="form-footer mt-3">
        <button class="btn btn-primary" type="submit" :disabled="loading">
          <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
          {{ submitText }}
        </button>
        <button class="btn btn-secondary" type="reset">重置</button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
interface Props {
  title?: string
  submitText?: string
  error?: string | null
  loading?: boolean
}

const emit = defineEmits<{
  submit: []
}>()

withDefaults(defineProps<Props>(), {
  title: '表單',
  submitText: '提交',
  error: null,
  loading: false
})

function onSubmit() {
  emit('submit')
}
</script>

<style scoped>
.form-layout {
  max-width: 600px;
}

.form-header {
  border-bottom: 1px solid #ddd;
  padding-bottom: 1rem;
}

.form-footer {
  display: flex;
  gap: 0.5rem;
}
</style>
