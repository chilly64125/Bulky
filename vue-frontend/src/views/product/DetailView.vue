<template>
    <div class="product-detail-container py-5 bg-light min-vh-100">
        <div class="container">
            <!-- Back Button -->
            <div class="row mb-3">
                <div class="col-12">
                    <router-link to="/" class="btn btn-outline-secondary btn-sm">
                        <i class="bi bi-chevron-left"></i> 返回首頁
                    </router-link>
                </div>
            </div>

            <!-- Loading State -->
            <div v-if="loading" class="row">
                <div class="col-12 text-center">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                    <p class="mt-3">正在載入活動詳細資訊...</p>
                </div>
            </div>

            <!-- Error State -->
            <div v-else-if="error" class="row">
                <div class="col-12">
                    <div class="alert alert-danger">
                        <h4 class="alert-heading">載入失敗</h4>
                        <p>{{ error }}</p>
                    </div>
                </div>
            </div>

            <!-- Product Detail -->
            <div v-else-if="product" class="row">
                <!-- Main Content -->
                <div class="col-lg-8">
                    <!-- Product Images Grid (each image in its own column, rotated) -->
                    <div v-if="product.productImages && product.productImages.length > 0"
                        class="card shadow-sm mb-4 border-0 product-images-card">
                        <div class="card-body">
                            <div class="row g-3">
                                <div v-for="image in product.productImages" :key="image.id"
                                    class="col-12 col-md-6 col-lg-4">
                                    <div class="image-wrapper position-relative"
                                        @wheel.prevent="onWheel($event, image.id)"
                                        @mousedown.prevent="startPan($event, image.id)"
                                        @mousemove.prevent="onPanMove($event, image.id)"
                                        @mouseup.prevent="endPan($event, image.id)"
                                        @mouseleave.prevent="endPan($event, image.id)"
                                        @touchstart.passive="startPanTouch($event, image.id)"
                                        @touchmove.prevent="onPanMoveTouch($event, image.id)"
                                        @touchend.prevent="endPan($event, image.id)">
                                        <img :src="normalizeImageUrl(image.imageUrl)" :style="imageStyle(image.id)"
                                            alt="活動圖片" class="img-fluid rotated-image" />

                                        <div class="image-overlay btn-group" role="group" aria-label="image actions">
                                            <button type="button" class="btn btn-sm btn-light"
                                                @click.prevent="zoomOut(image.id)" title="Zoom Out">
                                                <i class="bi bi-zoom-out"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-light"
                                                @click.prevent="zoomIn(image.id)" title="Zoom In">
                                                <i class="bi bi-zoom-in"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-light"
                                                @click.prevent="rotateImage(image.id)" title="Rotate 90">
                                                <i class="bi bi-arrow-clockwise"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-light"
                                                @click.prevent="toggleFlip(image.id)" title="Flip Horizontal">
                                                <i class="bi bi-arrow-left-right"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-light"
                                                @click.prevent="toggleFilter(image.id, 'grayscale(100%)')"
                                                title="Toggle Grayscale">
                                                <i class="bi bi-circle-half"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-light"
                                                @click.prevent="resetImage(image.id)" title="Reset">
                                                <i class="bi bi-arrow-counterclockwise"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div v-else class="card shadow-sm mb-4 border-0">
                        <img src="https://placehold.co/800x400/png" class="card-img-top rotated-image" alt="預設圖片" />
                    </div>

                    <!-- Product Description -->
                    <div class="card shadow-sm border-0 mb-4">
                        <div class="card-body">
                            <h3 class="card-title text-primary fw-bold mb-3">{{ product.title }}</h3>
                            <div class="card-text">
                                <h5 class="text-secondary fw-bold mb-3">活動說明</h5>
                                <p class="text-dark" style="line-height: 1.8;">
                                    {{ product.description || '暫無說明' }}
                                </p>
                            </div>
                        </div>
                    </div>

                    <!-- Additional Info -->
                    <div class="card shadow-sm border-0">
                        <div class="card-body">
                            <h5 class="card-title fw-bold mb-3">更多資訊</h5>
                            <table class="table table-borderless">
                                <tbody>
                                    <tr v-if="product.hDate">
                                        <td class="fw-bold text-secondary">舉辦日期:</td>
                                        <td>{{ formatDate(product.hDate) }}</td>
                                    </tr>
                                    <tr v-if="product.heldYN">
                                        <td class="fw-bold text-secondary">已舉辦:</td>
                                        <td>
                                            <span v-if="product.heldYN === 'Y'" class="badge bg-success">是</span>
                                            <span v-else class="badge bg-danger">否</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fw-bold text-secondary">上架日期:</td>
                                        <td>{{ formatDate(product.publishedDate) }}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Sidebar -->
                <div class="col-lg-4">
                    <!-- Company Info -->
                    <div class="card shadow-sm border-0 mb-4">
                        <div class="card-body">
                            <h5 class="card-title fw-bold mb-3">主辦單位</h5>
                            <div class="d-flex align-items-center mb-3">
                                <div>
                                    <p class="mb-1">
                                        <strong>{{ product.company?.name || product.company }}</strong>
                                    </p>
                                    <p v-if="product.company?.city" class="mb-1 text-muted small">
                                        {{ product.company.city }}
                                    </p>
                                </div>
                            </div>
                            <div v-if="product.company?.phoneNumber" class="mb-2">
                                <small class="text-muted">電話:</small>
                                <p class="mb-0">{{ product.company.phoneNumber }}</p>
                            </div>
                        </div>
                    </div>

                    <!-- Price Info -->
                    <div class="card shadow-sm border-0 mb-4">
                        <div class="card-body">
                            <h5 class="card-title fw-bold mb-3">報名費用</h5>
                            <p v-if="product.price" class="h4 text-danger fw-bold mb-0">
                                {{ product.price }}元
                            </p>
                            <p v-else class="text-muted">依主辦單位公佈為主</p>
                        </div>
                    </div>

                    <!-- Category -->
                    <div v-if="product.category" class="card shadow-sm border-0">
                        <div class="card-body">
                            <h5 class="card-title fw-bold mb-3">活動類別</h5>
                            <p class="mb-0">
                                <span class="badge bg-info">{{ product.category.name }}</span>
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Not Found -->
            <div v-else class="row">
                <div class="col-12">
                    <div class="alert alert-warning">
                        <h4 class="alert-heading">活動未找到</h4>
                        <p>所請求的活動項目不存在。</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';

interface ProductImage {
    id: number;
    imageUrl: string;
}

interface Company {
    id: number;
    name: string;
    city?: string;
    phoneNumber?: string;
}

interface Category {
    id: number;
    name: string;
}

interface Product {
    id: number;
    title: string;
    description?: string;
    price?: number;
    hDate?: string;
    heldYN?: string;
    publishedDate?: string;
    productImages?: ProductImage[];
    company?: Company;
    category?: Category;
}

const route = useRoute();
const product = ref<Product | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

const formatDate = (dateString: string | undefined) => {
    if (!dateString) return '-';
    try {
        const date = new Date(dateString);
        return date.toLocaleDateString('zh-TW', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
        });
    } catch {
        return dateString;
    }
};

onMounted(async () => {
    loading.value = true;
    error.value = null;

    try {
        const productId = route.params.id;
        const response = await fetch(`/api/product/${productId}`);

        if (!response.ok) {
            throw new Error(`無法載入活動詳細資訊 (${response.status})`);
        }

        const data = await response.json();
        // API sometimes returns a wrapper { success, data }, so unwrap if needed
        product.value = data && data.data ? data.data : data;
    } catch (err) {
        error.value = err instanceof Error ? err.message : '載入失敗，請稍後重試';
        console.error('Error loading product:', err);
    } finally {
        loading.value = false;
    }
});

// Normalize image path returned from backend (convert backslashes to forward slashes
// and ensure leading slash so Vite proxy can resolve /images)
function normalizeImageUrl(url: string | undefined | null) {
    if (!url) return '';
    // Replace backslashes with forward slashes
    let u = url.replace(/\\/g, '/');
    // Ensure it begins with a slash
    if (!u.startsWith('/')) u = '/' + u;
    return u;
}
// Image transform state and helpers for augmentation controls

const imageTransforms = ref<Record<number, { rotate: number; flipH: boolean; filter: string; scale: number; panX?: number; panY?: number; originX?: number; originY?: number }>>({});

function ensureTransform(id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    if (!imageTransforms.value[key]) {
        imageTransforms.value[key] = { rotate: 0, flipH: false, filter: '', scale: 1 };
    }
}

function rotateImage(id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    ensureTransform(key);
    imageTransforms.value[key].rotate = (imageTransforms.value[key].rotate + 90) % 360;
}

function toggleFlip(id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    ensureTransform(key);
    imageTransforms.value[key].flipH = !imageTransforms.value[key].flipH;
}

function toggleFilter(id: number | string | undefined, filter: string) {
    if (id == null) return;
    const key = Number(id);
    ensureTransform(key);
    imageTransforms.value[key].filter = imageTransforms.value[key].filter === filter ? '' : filter;
}

function resetImage(id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    imageTransforms.value[key] = { rotate: 0, flipH: false, filter: '', scale: 1 };
}

// Zoom helpers: adjust scale between 0.2 and 3.0
function clampScale(s: number) {
    return Math.min(3, Math.max(0.2, s));
}

function zoomIn(id: number | string | undefined, step = 0.2) {
    if (id == null) return;
    const key = Number(id);
    ensureTransform(key);
    const cur = imageTransforms.value[key].scale ?? 1;
    imageTransforms.value[key].scale = clampScale(cur + step);
}

function zoomOut(id: number | string | undefined, step = 0.2) {
    if (id == null) return;
    const key = Number(id);
    ensureTransform(key);
    const cur = imageTransforms.value[key].scale ?? 1;
    imageTransforms.value[key].scale = clampScale(cur - step);
}

function imageStyle(id: number | string | undefined) {
    if (id == null) return {};
    const key = Number(id);
    const t = imageTransforms.value[key] || { rotate: 0, flipH: false, filter: '' };
    const transforms = [];
    // Apply pan/translate first, then rotate/flip/scale
    const panX = t.panX || 0;
    const panY = t.panY || 0;
    transforms.push(`translate(${panX}px, ${panY}px)`);
    transforms.push(`rotate(${t.rotate}deg)`);
    const scale = t.scale ?? 1;
    if (t.flipH) transforms.push('scaleX(-1)');
    if (scale !== 1) transforms.push(`scale(${scale})`);
    const styleObj: Record<string, string> = {
        transform: transforms.join(' '),
        filter: t.filter || 'none',
        transition: 'transform 0.12s ease, filter 0.12s ease',
    };
    if (typeof t.originX === 'number' && typeof t.originY === 'number') {
        styleObj.transformOrigin = `${t.originX}px ${t.originY}px`;
    }
    return styleObj;
}

// Pan/zoom interaction state
const panState = ref<Record<number, { panning: boolean; lastX: number; lastY: number }>>({});

function startPan(evt: MouseEvent, id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    ensureTransform(key);
    panState.value[key] = { panning: true, lastX: evt.clientX, lastY: evt.clientY };
}

function startPanTouch(evt: TouchEvent, id: number | string | undefined) {
    if (id == null) return;
    const t = evt.touches[0];
    if (!t) return;
    const key = Number(id);
    ensureTransform(key);
    panState.value[key] = { panning: true, lastX: t.clientX, lastY: t.clientY };
}

function onPanMove(evt: MouseEvent, id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    const state = panState.value[key];
    if (!state || !state.panning) return;
    const dx = evt.clientX - state.lastX;
    const dy = evt.clientY - state.lastY;
    imageTransforms.value[key].panX = (imageTransforms.value[key].panX || 0) + dx;
    imageTransforms.value[key].panY = (imageTransforms.value[key].panY || 0) + dy;
    state.lastX = evt.clientX;
    state.lastY = evt.clientY;
    scheduleSaveTransform(key);
}

function onPanMoveTouch(evt: TouchEvent, id: number | string | undefined) {
    if (id == null) return;
    const t = evt.touches[0];
    if (!t) return;
    const key = Number(id);
    const state = panState.value[key];
    if (!state || !state.panning) return;
    const dx = t.clientX - state.lastX;
    const dy = t.clientY - state.lastY;
    imageTransforms.value[key].panX = (imageTransforms.value[key].panX || 0) + dx;
    imageTransforms.value[key].panY = (imageTransforms.value[key].panY || 0) + dy;
    state.lastX = t.clientX;
    state.lastY = t.clientY;
    scheduleSaveTransform(key);
}

function endPan(_evt: Event | null, id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    if (panState.value[key]) panState.value[key].panning = false;
}

// Wheel zoom centered at pointer
function onWheel(evt: WheelEvent, id: number | string | undefined) {
    if (id == null) return;
    const key = Number(id);
    ensureTransform(key);
    const wrapper = (evt.currentTarget as HTMLElement);
    const rect = wrapper.getBoundingClientRect();
    const px = evt.clientX - rect.left;
    const py = evt.clientY - rect.top;
    imageTransforms.value[key].originX = px;
    imageTransforms.value[key].originY = py;
    const step = evt.deltaY > 0 ? -0.1 : 0.1;
    const cur = imageTransforms.value[key].scale ?? 1;
    imageTransforms.value[key].scale = clampScale(cur + step);
    scheduleSaveTransform(key);
}

// Debounced save to server/localStorage
const saveTimers: Record<number, number> = {};
function scheduleSaveTransform(key: number) {
    if (saveTimers[key]) window.clearTimeout(saveTimers[key]);
    saveTimers[key] = window.setTimeout(() => saveTransform(key), 700);
}

async function saveTransform(imageId: number) {
    try {
        const prodId = product.value?.id;
        if (!prodId) return;
        const t = imageTransforms.value[imageId];
        if (!t) return;
        // Attempt to POST to an endpoint; backend may not have this route — fail silently
        await fetch(`/api/product/${prodId}/image/${imageId}/transform`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(t),
        });
    } catch (err) {
        // fallback: persist transforms to localStorage
        try {
            const prodId = product.value?.id as any;
            const key = `product_${prodId}_imageTransforms`;
            const existing = JSON.parse(localStorage.getItem(key) || '{}');
            existing[imageId] = imageTransforms.value[imageId];
            localStorage.setItem(key, JSON.stringify(existing));
        } catch (e) {
            console.error('Failed to persist transform', e);
        }
    }
}
</script>

<style scoped>
.product-detail-container {
    min-height: 100vh;
}

.carousel {
    border-radius: 8px;
    overflow: hidden;
}

.card {
    border-radius: 8px;
    transition: box-shadow 0.3s ease;
}

.card:hover {
    box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15) !important;
}

table td {
    padding: 0.75rem 0;
    border-bottom: 1px solid #e9ecef;
}

table tbody tr:last-child td {
    border-bottom: none;
}

.text-secondary {
    color: #6c757d;
}

/* Rotate product images and keep them contained in columns */
.product-images-card .image-wrapper {
    height: 300px;
    display: flex;
    align-items: center;
    justify-content: center;
    overflow: hidden;
    background: #f8f9fa;
    border-radius: 6px;
}

.product-images-card .rotated-image {
    /* no rotation: restore normal orientation */
    transform: none;
    transform-origin: center;
    height: 100%;
    width: auto;
    object-fit: cover;
    display: block;
}

/* Placeholder image: normal orientation */
.card-img-top.rotated-image {
    transform: none;
    transform-origin: center;
    height: 400px;
    width: auto;
    object-fit: cover;
}

/* Overlay controls for image augmentation */
.image-overlay {
    position: absolute;
    top: 8px;
    right: 8px;
    z-index: 10;
}

.image-overlay .btn {
    opacity: 0.92;
}
</style>
