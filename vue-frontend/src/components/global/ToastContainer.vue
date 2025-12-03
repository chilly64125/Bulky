<template>
  <div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 9999">
    <transition-group name="slide" tag="div">
      <div
        v-for="toast in notificationStore.toasts"
        :key="toast.id"
        class="toast show mb-2"
        :class="`toast-${toast.type}`"
        role="alert"
      >
        <div class="toast-header">
          <strong class="me-auto">
            <i :class="getToastIcon(toast.type)" class="me-2"></i>
            {{ getToastTitle(toast.type) }}
          </strong>
          <button type="button" class="btn-close" @click="notificationStore.removeToast(toast.id)"></button>
        </div>
        <div class="toast-body">
          {{ toast.message }}
        </div>
      </div>
    </transition-group>
  </div>
</template>

<script setup lang="ts">
import { useNotificationStore } from '@/stores/notificationStore'
import type { Toast } from '@/types'

const notificationStore = useNotificationStore()

const getToastIcon = (type: Toast['type']) => {
  const icons = {
    success: 'bi bi-check-circle-fill text-success',
    error: 'bi bi-exclamation-circle-fill text-danger',
    info: 'bi bi-info-circle-fill text-info',
    warning: 'bi bi-exclamation-triangle-fill text-warning'
  }
  return icons[type]
}

const getToastTitle = (type: Toast['type']) => {
  const titles = {
    success: '成功',
    error: '錯誤',
    info: '訊息',
    warning: '警告'
  }
  return titles[type]
}
</script>

<style scoped>
.toast {
  background-color: #fff;
  border: 1px solid #dee2e6;
  border-radius: 0.25rem;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
  min-width: 300px;
}

.toast-header {
  background-color: #f8f9fa;
  border-bottom: 1px solid #dee2e6;
  border-radius: 0.25rem 0.25rem 0 0;
  padding: 0.75rem;
}

.toast-body {
  padding: 0.75rem;
}

.slide-enter-active,
.slide-leave-active {
  transition: all 0.3s ease;
}

.slide-enter-from {
  transform: translateX(100%);
  opacity: 0;
}

.slide-leave-to {
  transform: translateX(100%);
  opacity: 0;
}
</style>
