<template>
  <div class="loading-spinner" v-if="visible">
    <div class="spinner-border" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
    <p v-if="message" class="mt-2">{{ message }}</p>
  </div>
</template>

<script setup lang="ts">
interface Props {
  visible?: boolean
  message?: string
}

withDefaults(defineProps<Props>(), {
  visible: true,
  message: ''
})
</script>

<style scoped>
.loading-spinner {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 2rem;
}

.spinner-border {
  width: 3rem;
  height: 3rem;
}
</style>
