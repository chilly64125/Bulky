<template>
    <div class="guest-welcome">
        <!-- Hero Section -->
        <section class="hero-section bg-gradient py-5">
            <div class="container">
                <div class="row align-items-center">
                    <div class="col-lg-6 text-center text-lg-start mb-4 mb-lg-0">
                        <h1 class="display-4 fw-bold mb-3">
                            <span class="text-primary">{{ appName }}</span>
                        </h1>
                        <p class="lead text-muted mb-4">
                            陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台
                        </p>
                        <p class="mb-4 fs-5">
                            一個簡潔、安全且專業的祖先牌位與墓園塔位管理系統，致力於幫助家族成員輕鬆追悼先人並保存家族記憶。
                        </p>
                        <div
                            class="d-flex flex-column flex-sm-row gap-3 justify-content-center justify-content-lg-start">
                            <router-link to="/login" class="btn btn-primary btn-lg">
                                <i class="bi bi-box-arrow-in-right me-2"></i>登入帳號
                            </router-link>
                            <router-link to="/register" class="btn btn-outline-secondary btn-lg">
                                <i class="bi bi-person-plus me-2"></i>建立新帳號
                            </router-link>
                        </div>
                    </div>
                    <div class="col-lg-6 text-center">
                        <div class="illustration-placeholder">
                            <i class="bi bi-house-heart display-1 text-primary opacity-50"></i>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Features Section -->
        <section class="features-section py-5 bg-light">
            <div class="container">
                <div class="text-center mb-5">
                    <h2 class="fw-bold mb-2">主要功能</h2>
                    <p class="text-muted">為您提供全面的祖先牌位與墓園塔位管理服務</p>
                </div>

                <div class="row g-4">
                    <!-- Feature 1 -->
                    <div class="col-md-6 col-lg-4">
                        <div class="feature-card p-4 bg-white rounded-3 shadow-sm h-100">
                            <div class="feature-icon mb-3 text-primary">
                                <i class="bi bi-card-text display-5"></i>
                            </div>
                            <h5 class="fw-bold mb-2">祖先牌位管理</h5>
                            <p class="text-muted small">
                                詳細記錄祖先資訊、紀念日期與生平故事，讓家族記憶代代相傳。
                            </p>
                        </div>
                    </div>

                    <!-- Feature 2 -->
                    <div class="col-md-6 col-lg-4">
                        <div class="feature-card p-4 bg-white rounded-3 shadow-sm h-100">
                            <div class="feature-icon mb-3 text-primary">
                                <i class="bi bi-building display-5"></i>
                            </div>
                            <h5 class="fw-bold mb-2">墓園塔位管理</h5>
                            <p class="text-muted small">
                                管理懷恩塔與其他墓園的塔位資訊，追蹤位置與維護狀態。
                            </p>
                        </div>
                    </div>

                    <!-- Feature 3 -->
                    <div class="col-md-6 col-lg-4">
                        <div class="feature-card p-4 bg-white rounded-3 shadow-sm h-100">
                            <div class="feature-icon mb-3 text-primary">
                                <i class="bi bi-shield-check display-5"></i>
                            </div>
                            <h5 class="fw-bold mb-2">安全與隱私</h5>
                            <p class="text-muted small">
                                企業級的資料安全保護，確保家族資訊只有授權成員可以存取。
                            </p>
                        </div>
                    </div>

                    <!-- Feature 4 -->
                    <div class="col-md-6 col-lg-4">
                        <div class="feature-card p-4 bg-white rounded-3 shadow-sm h-100">
                            <div class="feature-icon mb-3 text-primary">
                                <i class="bi bi-people display-5"></i>
                            </div>
                            <h5 class="fw-bold mb-2">多角色支持</h5>
                            <p class="text-muted small">
                                管理員、客戶與訪客角色，靈活控制不同使用者的權限與存取範圍。
                            </p>
                        </div>
                    </div>

                    <!-- Feature 5 -->
                    <div class="col-md-6 col-lg-4">
                        <div class="feature-card p-4 bg-white rounded-3 shadow-sm h-100">
                            <div class="feature-icon mb-3 text-primary">
                                <i class="bi bi-search display-5"></i>
                            </div>
                            <h5 class="fw-bold mb-2">快速查詢</h5>
                            <p class="text-muted small">
                                強大的搜尋與篩選功能，讓您輕鬆找到所需的祖先與塔位資訊。
                            </p>
                        </div>
                    </div>

                    <!-- Feature 6 -->
                    <div class="col-md-6 col-lg-4">
                        <div class="feature-card p-4 bg-white rounded-3 shadow-sm h-100">
                            <div class="feature-icon mb-3 text-primary">
                                <i class="bi bi-cloud-check display-5"></i>
                            </div>
                            <h5 class="fw-bold mb-2">雲端備份</h5>
                            <p class="text-muted small">
                                自動雲端備份，確保您的重要資料不會遺失，隨時可以存取。
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- CTA Section -->
        <section class="cta-section py-5 bg-primary text-white">
            <div class="container text-center">
                <h2 class="fw-bold mb-3">開始使用</h2>
                <p class="lead mb-4">加入我們的平台，開始管理您的家族記憶</p>
                <div class="d-flex flex-column flex-sm-row gap-3 justify-content-center">
                    <router-link to="/register" class="btn btn-light btn-lg">
                        <i class="bi bi-person-plus me-2"></i>立即註冊
                    </router-link>
                    <router-link to="/login" class="btn btn-outline-light btn-lg">
                        <i class="bi bi-box-arrow-in-right me-2"></i>已有帳號？登入
                    </router-link>
                </div>
            </div>
        </section>

        <!-- Footer -->
        <footer class="footer bg-dark text-white-50 py-4 mt-5">
            <div class="container text-center">
                <p class="mb-2">© 2025 {{ appName }}. 保留所有權利。</p>
                <p class="small mb-0">
                    <a href="#" class="text-white-50 text-decoration-none">隱私政策</a> |
                    <a href="#" class="text-white-50 text-decoration-none">服務條款</a> |
                    <a href="#" class="text-white-50 text-decoration-none">聯絡我們</a>
                </p>
            </div>
        </footer>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const appName = computed(() => {
    return import.meta.env.VITE_APP_SHORT_NAME || '陳氏宗祠';
});
</script>

<style scoped>
.guest-welcome {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
}

.hero-section {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    min-height: 500px;
    display: flex;
    align-items: center;
    position: relative;
    overflow: hidden;
}

.hero-section::before {
    content: '';
    position: absolute;
    top: 0;
    right: 0;
    width: 500px;
    height: 500px;
    background: rgba(255, 255, 255, 0.1);
    border-radius: 50%;
    transform: translate(100px, -100px);
}

.hero-section h1,
.hero-section .lead {
    position: relative;
    z-index: 1;
}

.illustration-placeholder {
    background: rgba(255, 255, 255, 0.1);
    border-radius: 20px;
    padding: 60px;
    position: relative;
    z-index: 1;
}

.features-section {
    flex: 1;
}

.feature-card {
    border: none;
    transition: all 0.3s ease;
}

.feature-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1) !important;
}

.feature-icon {
    display: inline-block;
}

.cta-section {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.footer {
    margin-top: auto;
}

@media (max-width: 768px) {
    .hero-section {
        min-height: auto;
        padding: 40px 0;
    }

    .hero-section h1 {
        font-size: 2rem;
    }

    .illustration-placeholder {
        padding: 40px;
    }
}
</style>
