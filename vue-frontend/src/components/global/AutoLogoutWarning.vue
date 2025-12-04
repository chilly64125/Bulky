<template>
  <div v-if="sessionStore.showLogoutWarning" class="logout-warning-overlay">
    <div class="logout-warning-modal">
      <div class="modal-header">
        <h5 class="modal-title">
          <i class="bi bi-exclamation-circle-fill text-warning me-2"></i>
          即將登出
        </h5>
      </div>
      <div class="modal-body">
        <p>由於長時間未活動，您將在 <strong>{{ sessionStore.remainingTime }} 秒</strong>後自動登出。</p>
        <p>如果您需要繼續使用系統，請點擊下方按鈕。</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" @click="handleStayLoggedIn">保持登入</button>
        <button type="button" class="btn btn-danger" @click="handleLogout">立即登出</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue'
import { useSessionStore } from '@/stores/sessionStore'
import { useAuthStore } from '@/stores/authStore'
import { useRouter } from 'vue-router'
import { useNotificationStore } from '@/stores/notificationStore'

const sessionStore = useSessionStore()
const authStore = useAuthStore()
const router = useRouter()
const notificationStore = useNotificationStore()

let logoutTimer: number | null = null
let countdownTimer: number | null = null

// Toggle this to true to disable the auto-logout timers (temporary, safer for debugging)
// Set to false to enable auto-logout behavior
const DISABLE_AUTO_LOGOUT = true

const AUTO_LOGOUT_MINUTES = 3
const WARNING_SECONDS = 5

onMounted(() => {
  if (!DISABLE_AUTO_LOGOUT) {
    startInactivityTimer()
    addActivityListeners()
  } else {
    // Ensure any existing timers are cleared when disabling
    clearTimers()
  }
})

onUnmounted(() => {
  clearTimers()
  if (!DISABLE_AUTO_LOGOUT) removeActivityListeners()
})

const addActivityListeners = () => {
  if (DISABLE_AUTO_LOGOUT) return
  document.addEventListener('mousedown', handleActivity)
  document.addEventListener('keydown', handleActivity)
  document.addEventListener('touchstart', handleActivity)
}

const removeActivityListeners = () => {
  if (DISABLE_AUTO_LOGOUT) return
  document.removeEventListener('mousedown', handleActivity)
  document.removeEventListener('keydown', handleActivity)
  document.removeEventListener('touchstart', handleActivity)
}

const handleActivity = () => {
  if (DISABLE_AUTO_LOGOUT) return
  sessionStore.updateActivity()
  clearTimers()
  startInactivityTimer()
}

const startInactivityTimer = () => {
  if (DISABLE_AUTO_LOGOUT) return
  const logoutTimeMs = AUTO_LOGOUT_MINUTES * 60 * 1000
  const warningTimeMs = (AUTO_LOGOUT_MINUTES * 60 - WARNING_SECONDS) * 1000

  logoutTimer = window.setTimeout(() => {
    handleLogout()
  }, logoutTimeMs)

  window.setTimeout(() => {
    sessionStore.showWarning(WARNING_SECONDS)
    startCountdown()
  }, warningTimeMs)
}

const startCountdown = () => {
  if (DISABLE_AUTO_LOGOUT) return
  let remaining = WARNING_SECONDS

  countdownTimer = window.setInterval(() => {
    remaining--
    sessionStore.remainingTime = remaining

    if (remaining <= 0) {
      clearInterval(countdownTimer!)
    }
  }, 1000)
}

const handleStayLoggedIn = () => {
  if (DISABLE_AUTO_LOGOUT) {
    // No-op when disabled, but keep UX consistent by hiding warning
    sessionStore.hideWarning()
    return
  }
  sessionStore.hideWarning()
  sessionStore.updateActivity()
  clearTimers()
  startInactivityTimer()
}

const handleLogout = async () => {
  clearTimers()
  if (DISABLE_AUTO_LOGOUT) {
    // When disabled, do not perform automatic logout. Keep function available for manual logout.
    notificationStore.info('自動登出已被暫時停用')
    return
  }
  await authStore.logout()
  notificationStore.warning('由於長時間未活動，已自動登出')
  router.push('/login')
}

const clearTimers = () => {
  if (logoutTimer) clearTimeout(logoutTimer)
  if (countdownTimer) clearInterval(countdownTimer)
}
</script>

<style scoped>
.logout-warning-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10000;
  animation: fadeIn 0.3s ease;
}

.logout-warning-modal {
  background-color: white;
  border-radius: 0.5rem;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  max-width: 400px;
  width: 90%;
  animation: slideUp 0.3s ease;
}

.modal-header {
  padding: 1.5rem;
  border-bottom: 1px solid #dee2e6;
}

.modal-title {
  margin: 0;
  font-weight: 600;
}

.modal-body {
  padding: 1.5rem;
}

.modal-body p {
  margin-bottom: 0.75rem;
}

.modal-body p:last-child {
  margin-bottom: 0;
}

.modal-footer {
  padding: 1.5rem;
  border-top: 1px solid #dee2e6;
  display: flex;
  gap: 0.75rem;
  justify-content: flex-end;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }

  to {
    opacity: 1;
  }
}

@keyframes slideUp {
  from {
    transform: translateY(20px);
    opacity: 0;
  }

  to {
    transform: translateY(0);
    opacity: 1;
  }
}
</style>
