<template>
  <div class="modal-overlay" v-if="visible" @click.self="close">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">{{ title }}</h5>
          <button class="btn-close" @click="close"></button>
        </div>
        <div class="modal-body">
          {{ message }}
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary" @click="close">{{ cancelText }}</button>
          <button class="btn btn-danger" @click="confirm">{{ confirmText }}</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  visible?: boolean
  title?: string
  message?: string
  confirmText?: string
  cancelText?: string
}

const emit = defineEmits<{
  confirm: []
  cancel: []
}>()

withDefaults(defineProps<Props>(), {
  visible: false,
  title: '確認',
  message: '',
  confirmText: '確認',
  cancelText: '取消'
})

function confirm() {
  emit('confirm')
}

function close() {
  emit('cancel')
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1050;
}

.modal-dialog {
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.modal-content {
  padding: 1rem;
}
</style>
