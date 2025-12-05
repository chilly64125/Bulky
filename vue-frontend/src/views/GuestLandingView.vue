<template>
  <div class="guest-landing">
    <!-- Ancestral & Kindness Quick Access -->
    <section class="quick-access bg-secondary bg-gradient py-4">
      <div class="container">
        <div class="row g-3">
          <div class="col-md-6">
            <router-link to="/app/ancestral/query" class="text-decoration-none d-block">
              <div class="p-4 rounded text-center fw-bold text-white"
                style="background: #495057; font-size: 24px; cursor: pointer; transition: all 0.3s;">
                <i class="bi bi-people-fill me-2"></i>陳氏宗祠-牌位查詢
              </div>
            </router-link>
          </div>
          <div class="col-md-6">
            <router-link to="/app/kindness/query" class="text-decoration-none d-block">
              <div class="p-4 rounded text-center fw-bold text-white"
                style="background: #495057; font-size: 24px; cursor: pointer; transition: all 0.3s;">
                <i class="bi bi-tree-fill me-2"></i>懷恩塔-塔位查詢
              </div>
            </router-link>
          </div>
        </div>
      </div>
    </section>

    <!-- Welcome Section -->
    <section class="welcome-section bg-secondary bg-gradient text-white py-5">
      <div class="container">
        <div class="row mb-4">
          <div class="col-12 text-center">
            <h1 class="display-5 fw-bold mb-3">歡迎來到台中市銀同碧湖陳氏宗親會</h1>
            <p class="lead opacity-75 fw-bold">我們致力於弘揚[穎川陳氏 祖訓]，奉行[先祖不怕艱苦、勤儉打拼的精神與美德]。</p>
          </div>
        </div>
        <div class="row g-3 justify-content-center">
          <div class="col-auto">
            <button @click="showVideoModal = true" class="btn btn-info btn-lg fw-bold text-white">
              <i class="bi bi-camera-video-fill me-2"></i>
              台中市銀同碧湖陳氏宗親會-影片
              <i class="bi bi-camera-video-fill ms-2"></i>
            </button>
          </div>
          <div class="col-auto">
            <router-link to="/login" class="btn btn-primary btn-lg fw-bold text-white">
              <i class="bi bi-box-arrow-in-right me-2"></i>
              登入
            </router-link>
          </div>
        </div>
      </div>
    </section>

    <!-- Video Modal -->
    <div v-if="showVideoModal" class="video-modal" @click="showVideoModal = false">
      <div
        style="background: white; padding: 40px; border-radius: 8px; text-align: center; color: #333; max-width: 90%; max-height: 90%; overflow: auto;">
        <h3 class="mb-3">台中市銀同碧湖陳氏宗親會-影片</h3>
        <video width="100%" height="auto" style="max-width: 800px; border-radius: 8px;" controls>
          <source src="/images/Films/ChenClanOpening.mp4" type="video/mp4">
          <source src="/images/Films/ChenClanOpening.webm" type="video/webm">
          您的瀏覽器不支援視頻播放。
        </video>
        <p class="text-muted small mt-3">(點擊視頻外的地方關閉)</p>
      </div>
    </div> <!-- Activities/Events Section Header -->
    <section class="activities-header bg-light py-4">
      <div class="container">
        <div class="row">
          <div class="col-12 text-center">
            <h2 class="fw-bold">本會年度祭祖活動一覽表</h2>
          </div>
        </div>
      </div>
    </section>

    <!-- Activity Cards -->
    <section class="activities-section py-5 bg-light">
      <div class="container">
        <div class="row" v-if="products.length > 0">
          <div v-for="product of products" :key="product.id" class="col-lg-3 col-sm-6 mb-4">
            <div class="card border-0 p-3 shadow border-top border-5 rounded h-100"
              style="border-top: 5px solid #dc3545;">
              <div class="card-img-top p-1">
                <img v-if="product.productImages && product.productImages.length > 0"
                  :src="product.productImages[0].imageUrl" class="card-img-top rounded"
                  style="height: 200px; object-fit: cover; width: 100%;" alt="活動圖片" />
                <img v-else src="https://placehold.co/500x600/png" class="card-img-top rounded"
                  style="height: 200px; object-fit: cover; width: 100%;" alt="預設圖片" />
              </div>
              <div class="card-body pb-0">
                <div class="pl-1">
                  <p class="card-title h5 text-dark opacity-75 text-uppercase text-center fw-bold">{{ product.title }}
                  </p>
                  <p class="card-title text-warning text-center fw-bold">主辦單位: <b>{{ product.company?.name ||
                    product.company }}</b></p>
                </div>
                <div class="pl-1">
                  <p class="text-dark text-opacity-75 text-center mb-0 fw-bold">
                    報名費用: 依主辦單位公佈為主
                  </p>
                </div>
              </div>
              <div class="card-footer bg-dark">
                <router-link :to="`/app/customer/product/${product.id}`" class="btn btn-primary w-100">
                  詳細內容
                </router-link>
              </div>
            </div>
          </div>
        </div>
        <div v-else class="row">
          <div class="col-12 text-center text-muted py-5">
            <p>目前沒有活動項目</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Back to Top Button -->
    <button v-if="showBackToTop" @click="scrollToTop" class="back-to-top btn btn-danger rounded-circle"
      style="position: fixed; bottom: 30px; right: 30px; width: 50px; height: 50px; z-index: 99;">
      <i class="bi bi-arrow-up"></i>
    </button>

    <!-- Footer -->
    <footer class="footer text-center text-white bg-secondary bg-gradient py-4 mt-5">
      <div class="container">
        <p class="mb-2">© 2025 財團法人台中市私立銀同碧湖陳氏社會福利基金會. All rights reserved.</p>
        <p class="mb-2">資料及照片來源:財團法人台中市私立銀同碧湖陳氏社會福利基金會</p>
        <p class="mb-0">Powered by Vue 3 + ASP.NET Core</p>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useAuthStore } from '@/stores/authStore';

const authStore = useAuthStore();
const showVideoModal = ref(false);
const showBackToTop = ref(false);

// Get products from server data or fallback
const products = computed(() => {
  return authStore.serverProducts || [];
});

// Handle scroll for back-to-top button
const handleScroll = () => {
  showBackToTop.value = window.scrollY > 200;
};

const scrollToTop = () => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
});

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll);
});
</script>

<style scoped>
.guest-landing {
  min-height: 100vh;
}

.video-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.9);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  cursor: pointer;
}

.video-modal video {
  cursor: auto;
}

.back-to-top {
  opacity: 0.7;
  transition: opacity 0.3s;
}

.back-to-top:hover {
  opacity: 1;
}
</style>
