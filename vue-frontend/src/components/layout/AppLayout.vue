<template>
  <div class="app-layout">
    <Navbar />
    <div class="main-content">
      <Sidebar />
      <div class="content-area">
        <slot></slot>
        <ToastContainer />
      </div>
    </div>
    <AutoLogoutWarning />
  </div>
</template>

<script setup lang="ts">
import Navbar from './Navbar.vue'
import Sidebar from './Sidebar.vue'
import ToastContainer from '../global/ToastContainer.vue'
import AutoLogoutWarning from '../global/AutoLogoutWarning.vue'
</script>

<style scoped>
.app-layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.main-content {
  display: flex;
  flex: 1;
}

.content-area {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
}

@media (max-width: 768px) {
  .content-area {
    padding: 10px;
  }
}
</style>
