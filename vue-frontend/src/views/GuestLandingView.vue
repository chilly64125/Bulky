<template>
  <div class="guest-landing">
    <!-- Hero Section -->
    <section class="hero-section bg-primary text-white py-5">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-lg-6 mb-4 mb-lg-0">
            <h1 class="display-4 fw-bold mb-4">陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台</h1>
            <p class="lead mb-4">一個專為陳氏家族設計的祖先牌位和墓園塔位綜合管理系統</p>
            <p class="mb-4">輕鬆管理家族祖先牌位資訊、墓園塔位詳情、以及重要的家族紀錄</p>
            <div class="d-flex gap-3 flex-wrap">
              <router-link to="/login" class="btn btn-light btn-lg fw-bold">
                <i class="bi bi-box-arrow-in-right me-2"></i>登入系統
              </router-link>
              <router-link to="/register" class="btn btn-outline-light btn-lg">
                <i class="bi bi-person-plus me-2"></i>註冊帳號
              </router-link>
            </div>
          </div>
          <div class="col-lg-6 text-center">
            <i class="bi bi-building" style="font-size: 120px; opacity: 0.3;"></i>
          </div>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center mb-5">核心功能</h2>
        <div class="row">
          <div class="col-md-6 col-lg-3 mb-4">
            <div class="feature-card card h-100">
              <div class="card-body text-center">
                <i class="bi bi-person-vcard" style="font-size: 40px; color: #0d6efd;"></i>
                <h5 class="card-title mt-3">祖先牌位管理</h5>
                <p class="card-text">完整記錄祖先牌位資訊，建檔歷史紀錄，便於家族查詢</p>
              </div>
            </div>
          </div>
          <div class="col-md-6 col-lg-3 mb-4">
            <div class="feature-card card h-100">
              <div class="card-body text-center">
                <i class="bi bi-geo-alt" style="font-size: 40px; color: #198754;"></i>
                <h5 class="card-title mt-3">墓園塔位查詢</h5>
                <p class="card-text">快速查詢懷恩塔家族墓園的塔位詳情和位置</p>
              </div>
            </div>
          </div>
          <div class="col-md-6 col-lg-3 mb-4">
            <div class="feature-card card h-100">
              <div class="card-body text-center">
                <i class="bi bi-file-earmark-text" style="font-size: 40px; color: #fd7e14;"></i>
                <h5 class="card-title mt-3">文件管理</h5>
                <p class="card-text">上傳和管理相關文件、照片和紀錄</p>
              </div>
            </div>
          </div>
          <div class="col-md-6 col-lg-3 mb-4">
            <div class="feature-card card h-100">
              <div class="card-body text-center">
                <i class="bi bi-shield-check" style="font-size: 40px; color: #6f42c1;"></i>
                <h5 class="card-title mt-3">安全隱私</h5>
                <p class="card-text">使用最新的安全技術保護家族資訊</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Quick Links Section -->
    <section class="quick-links-section py-5">
      <div class="container">
        <h2 class="text-center mb-5">快速開始</h2>
        <div class="row">
          <div class="col-md-6 mb-3">
            <div class="quick-link-card d-flex align-items-center p-4 border rounded">
              <div class="me-3">
                <i class="bi bi-question-circle" style="font-size: 30px; color: #0d6efd;"></i>
              </div>
              <div>
                <h5 class="mb-1">常見問題</h5>
                <p class="mb-0 text-muted">了解如何使用系統功能</p>
              </div>
            </div>
          </div>
          <div class="col-md-6 mb-3">
            <div class="quick-link-card d-flex align-items-center p-4 border rounded">
              <div class="me-3">
                <i class="bi bi-telephone" style="font-size: 30px; color: #198754;"></i>
              </div>
              <div>
                <h5 class="mb-1">聯絡客服</h5>
                <p class="mb-0 text-muted">遇到問題？立即聯絡我們</p>
              </div>
            </div>
          </div>
          <div class="col-md-6 mb-3">
            <div class="quick-link-card d-flex align-items-center p-4 border rounded">
              <div class="me-3">
                <i class="bi bi-shield-lock" style="font-size: 30px; color: #fd7e14;"></i>
              </div>
              <div>
                <h5 class="mb-1">隱私政策</h5>
                <p class="mb-0 text-muted">查看我們如何保護您的隱私</p>
              </div>
            </div>
          </div>
          <div class="col-md-6 mb-3">
            <div class="quick-link-card d-flex align-items-center p-4 border rounded">
              <div class="me-3">
                <i class="bi bi-file-text" style="font-size: 30px; color: #6f42c1;"></i>
              </div>
              <div>
                <h5 class="mb-1">使用條款</h5>
                <p class="mb-0 text-muted">了解我們的服務條款</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="cta-section bg-primary text-white py-5">
      <div class="container text-center">
        <h2 class="mb-4">準備好開始了嗎？</h2>
        <p class="lead mb-4">加入陳氏家族管理平台，輕鬆管理祖先牌位和墓園資訊</p>
        <div class="d-flex gap-3 justify-content-center flex-wrap">
          <router-link to="/register" class="btn btn-light btn-lg fw-bold">
            <i class="bi bi-person-plus me-2"></i>立即註冊
          </router-link>
          <router-link to="/login" class="btn btn-outline-light btn-lg">
            <i class="bi bi-box-arrow-in-right me-2"></i>已有帳號？登入
          </router-link>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer class="bg-dark text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-md-6">
            <h5>陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台</h5>
            <p class="text-muted">為陳氏家族提供專業的祖先牌位和墓園塔位管理服務</p>
          </div>
          <div class="col-md-6 text-md-end">
            <p class="text-muted mb-0">&copy; 2025 版權所有。保留所有權利。</p>
            <small class="text-muted">聯絡我們: contact@chenclan.com</small>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
// Guest landing page - no auth required
</script>

<style scoped>
.guest-landing {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.hero-section {
  background: linear-gradient(135deg, #0d6efd 0%, #0dcaf0 100%);
  min-height: 500px;
  display: flex;
  align-items: center;
}

.features-section {
  min-height: 400px;
}

.feature-card {
  transition: transform 0.3s, box-shadow 0.3s;
  border: none;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.feature-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
}

.quick-links-section {
  min-height: 400px;
}

.quick-link-card {
  transition: background-color 0.3s, transform 0.3s;
  background-color: #f8f9fa;
}

.quick-link-card:hover {
  background-color: #e9ecef;
  transform: translateX(5px);
}

.cta-section {
  background: linear-gradient(135deg, #0d6efd 0%, #0dcaf0 100%);
  min-height: 300px;
  display: flex;
  align-items: center;
}

footer {
  margin-top: auto;
}
</style>
