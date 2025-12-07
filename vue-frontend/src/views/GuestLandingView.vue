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
            <button @click="openVideoModal" class="btn btn-info btn-lg fw-bold text-white">
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
        <video ref="landingVideo" width="100%" height="auto" style="max-width: 800px; border-radius: 8px;" controls
          muted playsinline preload="auto">
          <source :src="videoUrl" type="video/mp4">
          您的瀏覽器不支援視頻播放。
        </video>
        <!-- Custom animated toast shown when autoplay blocked (unless user dismissed) -->
        <div v-if="autoplayBlocked && !dontShowHint" class="custom-toast" role="status" aria-live="polite">
          <div class="toast-inner">
            <div class="toast-text">自動播放被瀏覽器阻擋。請按下影片上的播放按鈕或使用下方按鈕啟用聲音並播放。</div>
            <div class="toast-actions">
              <button @click.stop.prevent="playWithSound" @keydown.enter.prevent="playWithSound"
                @keydown.space.prevent="playWithSound" class="btn btn-sm btn-secondary"
                aria-label="Play with sound">啟用聲音並播放</button>
              <button @click.stop.prevent="dismissHint" @keydown.enter.prevent="dismissHint"
                @keydown.space.prevent="dismissHint" class="btn btn-sm btn-outline-dark"
                aria-label="Dismiss autoplay hint">不再顯示</button>
            </div>
          </div>
        </div>
        <!-- Play overlay: visible when video not playing to encourage tap/click -->
        <div v-if="!isPlaying" class="video-overlay" @click.stop.prevent="overlayPlay">
          <div class="overlay-inner text-center">
            <button class="btn btn-light btn-lg rounded-circle play-icon" @click.stop.prevent="overlayPlay">
              <i class="bi bi-play-fill" style="font-size: 28px;"></i>
            </button>
            <div class="mt-2 text-white small">點擊播放</div>
          </div>
        </div>
        <!-- Play overlay: visible when video not playing to encourage tap/click; keyboard accessible -->
        <div v-if="!isPlaying" class="video-overlay" @click.stop.prevent="overlayPlay" tabindex="0"
          @keydown.enter.prevent="overlayPlay" @keydown.space.prevent="overlayPlay" role="button"
          aria-label="Play video">
          <div class="overlay-inner text-center">
            <button class="btn btn-light btn-lg rounded-circle play-icon" @click.stop.prevent="overlayPlay"
              @keydown.enter.prevent="overlayPlay" @keydown.space.prevent="overlayPlay" aria-label="Play video button">
              <i class="bi bi-play-fill" style="font-size: 28px;"></i>
            </button>
            <div class="mt-2 text-white small">點擊播放</div>
          </div>
        </div>

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
        <div class="mt-2 small text-white">
          影片自動播放: <strong>{{ autoplayPreference === 'auto' ? '自動' : '手動' }}</strong>
          <button @click.prevent="toggleAutoplayPreference" class="btn btn-sm btn-light ms-2">切換</button>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, nextTick } from 'vue';
import { useAuthStore } from '@/stores/authStore';

const authStore = useAuthStore();
const showVideoModal = ref(false);
// video element reference so we can call play() programmatically
const landingVideo = ref<HTMLVideoElement | null>(null);
const showBackToTop = ref(false);
// flag to indicate whether autoplay was blocked
const autoplayBlocked = ref(false);
// whether the user chose to dismiss the hint
const dontShowHint = ref(localStorage.getItem('videoAutoplayHintDismissed') === 'true');
// whether video is playing
const isPlaying = ref(false);

// Video URL - served from backend wwwroot via proxy
const videoUrl = computed(() => {
  return '/images/Films/ChenClanOpening.mp4';
});

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

function openVideoModal() {
  showVideoModal.value = true;
  autoplayBlocked.value = false;
  nextTick(() => {
    if (landingVideo.value) {
      // Attempt to play muted video. If the browser blocks autoplay, set flag so we
      // can show a hint and provide a play-with-sound button.
      landingVideo.value.play().catch(() => {
        autoplayBlocked.value = true;
      });
    }
  });
}

function playWithSound() {
  if (!landingVideo.value) return;
  // user interaction — unmute and play with sound
  landingVideo.value.muted = false;
  landingVideo.value.play().then(() => {
    autoplayBlocked.value = false;
    isPlaying.value = true;
  }).catch(() => {
    autoplayBlocked.value = true;
  });
}

function dismissHint() {
  dontShowHint.value = true;
  try { localStorage.setItem('videoAutoplayHintDismissed', 'true'); } catch { }
}

function overlayPlay() {
  if (!landingVideo.value) return;
  // clicking overlay is a user gesture — attempt to play with sound
  landingVideo.value.muted = false;
  landingVideo.value.play().then(() => {
    autoplayBlocked.value = false;
    isPlaying.value = true;
  }).catch(() => {
    autoplayBlocked.value = true;
  });
}

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
  // attach play/pause listeners to update isPlaying if video ref exists
  nextTick(() => {
    if (landingVideo.value) {
      landingVideo.value.addEventListener('play', () => (isPlaying.value = true));
      landingVideo.value.addEventListener('pause', () => (isPlaying.value = false));
      landingVideo.value.addEventListener('ended', () => (isPlaying.value = false));
    }
  });
});

// Autoplay preference for site: 'auto' or 'manual'. Stored in localStorage so it's site-wide.
const autoplayPreference = ref(localStorage.getItem('videoAutoplayPreference') || 'auto');

function toggleAutoplayPreference() {
  autoplayPreference.value = autoplayPreference.value === 'auto' ? 'manual' : 'auto';
  try { localStorage.setItem('videoAutoplayPreference', autoplayPreference.value); } catch { }
}

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

/* Play overlay styles */
.video-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  pointer-events: auto;
}

.video-overlay .overlay-inner {
  background: rgba(0, 0, 0, 0.35);
  padding: 16px;
  border-radius: 8px;
}

.video-overlay .play-icon {
  width: 72px;
  height: 72px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

/* Toast wrapper spacing */
.video-toast .toast-body {
  font-size: 0.95rem;
}

.back-to-top {
  opacity: 0.7;
  transition: opacity 0.3s;
}

.back-to-top:hover {
  opacity: 1;
}
</style>
