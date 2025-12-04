<script setup lang="ts">
import { onMounted } from "vue";
import { useConfigStore } from "./stores/configStore";

// Auth is initialized in router before any route checks
const configStore = useConfigStore();

onMounted(() => {
  configStore.loadConfig().catch(console.error);
});
</script>

<template>
  <router-view />
</template>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family:
    -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen, Ubuntu,
    Cantarell, "Open Sans", "Helvetica Neue", sans-serif;
  font-size: 14px;
  line-height: 1.5;
  color: #333;
  background-color: #f5f5f5;
}

html,
body,
#app {
  height: 100%;
  width: 100%;
}

#app {
  display: flex;
  flex-direction: column;
}
</style>
