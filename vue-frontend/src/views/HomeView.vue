<template>
  <div class="container py-4">
    <h1>首頁</h1>
    <p class="lead">歡迎使用 陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台。</p>

    <div class="row">
      <div class="col-md-4 mb-3" v-for="card in cards" :key="card.title">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{ card.title }}</h5>
            <p class="card-text">{{ card.description }}</p>
            <router-link :to="card.link" class="btn btn-sm btn-outline-primary">前往</router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const cards = ref([
  { title: '陳氏宗祠-牌位管理', description: '管理與查詢祖先牌位', link: '/ancestral' },
  { title: '懷恩塔-塔位管理', description: '管理與查詢懷恩塔塔位', link: '/kindness' },
  { title: '活動管理', description: '活動、分類、報名管理', link: '/category' }
])
</script>

<style scoped>
.container {
  margin-top: 1.5rem
}
</style>
