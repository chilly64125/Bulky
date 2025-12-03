import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { AppConfig } from '@/types'
import { configService } from '@/services/configService'

export const useConfigStore = defineStore('config', () => {
  const config = ref<AppConfig | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const loadConfig = async () => {
    if (config.value) return config.value

    loading.value = true
    error.value = null

    try {
      const appConfig = await configService.getConfig()
      config.value = appConfig
      return appConfig
    } catch (err: any) {
      error.value = err.message || 'Failed to load configuration'
      throw err
    } finally {
      loading.value = false
    }
  }

  const getKindnessConfig = () => config.value?.Kindness
  const getAncestralConfig = () => config.value?.Ancestral
  const getLogoutConfig = () => config.value?.Logout_Duration

  return {
    config,
    loading,
    error,
    loadConfig,
    getKindnessConfig,
    getAncestralConfig,
    getLogoutConfig
  }
})
