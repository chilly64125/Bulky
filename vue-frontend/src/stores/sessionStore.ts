import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useSessionStore = defineStore('session', () => {
  const lastActivityTime = ref<number>(Date.now())
  const showLogoutWarning = ref(false)
  const remainingTime = ref<number>(0)

  const updateActivity = () => {
    lastActivityTime.value = Date.now()
    showLogoutWarning.value = false
    remainingTime.value = 0
  }

  const showWarning = (timeRemaining: number) => {
    showLogoutWarning.value = true
    remainingTime.value = timeRemaining
  }

  const hideWarning = () => {
    showLogoutWarning.value = false
  }

  return {
    lastActivityTime,
    showLogoutWarning,
    remainingTime,
    updateActivity,
    showWarning,
    hideWarning
  }
})
